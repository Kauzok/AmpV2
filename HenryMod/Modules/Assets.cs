using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Orbs;
using System.IO;
using System.Collections.Generic;
using RoR2.UI;


namespace HenryMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;

        // particle effects
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;
        internal static GameObject bombExplosionEffect;

        [Header("Charge Effects")]
        internal static GameObject chargeExplosionEffect;

        [Header("Ferroshot/Lorentz Cannon Effects")]
        internal static GameObject bulletSpawnEffect;
        internal static GameObject bulletImpactEffect;

        [Header("Bolt Effects")]
        internal static GameObject boltExitEffect;
        internal static GameObject boltEnterEffect;
        
        [Header("Fulmination Effects")]
        internal static GameObject electricStreamEffect;
        internal static GameObject electricImpactEffect;
        internal static GameObject electricChainEffect;

        [Header("VoltaicBombardment Effects")]
        internal static GameObject lightningStrikePrefab;


        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        internal static NetworkSoundEventDef chargeExplosionSound;
        internal static NetworkSoundEventDef stormbladeHitSoundEvent;

        // lists of assets to add to contentpack
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        // cache these and use to create our own materials
        internal static Shader hotpoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];

        // CHANGE THIS
        private const string assetbundleName = "ampbundle";

        internal static void Initialize()
        {
            if (assetbundleName == "myassetbundle")
            {
                Debug.LogError("AssetBundle name hasn't been changed- not loading any assets to avoid conflicts");
                return;
            }

            LoadAssetBundle();
            LoadSoundbank();
            PopulateAssets();
        }

        internal static void LoadAssetBundle()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HenryMod." + assetbundleName))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
        }

        internal static void LoadSoundbank()
        {
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("HenryMod.HenryBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("HenryMod.AmpSoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        

        internal static void PopulateAssets()
        {
            if (!mainAssetBundle)
            {
                Debug.LogError("There is no AssetBundle to load assets from.");
                return;
            }

            // feel free to delete everything in here and load in your own assets instead
            // it should work fine even if left as is- even if the assets aren't in the bundle
            swordHitSoundEvent = CreateNetworkSoundEventDef("HenrySwordHit");

            stormbladeHitSoundEvent = CreateNetworkSoundEventDef(Modules.StaticValues.stormbladeHit4String);

            bombExplosionEffect = LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            //on fulmination skill contact
            electricImpactEffect = LoadEffect("ElectricitySphere", null);

            //on charge explosion when 3 procs are reached
            CreateChargePrefab();

            //on fulmination skill chain
            electricChainEffect = mainAssetBundle.LoadAsset<GameObject>("ElectricityChain");

            //on fulmination skill use
            CreateStreamPrefab();

            //on boltvehicle exit/enter
            CreateBoltExitPrefab();
            CreateBoltEnterPrefab();

            //on ferroshot/Lorentz Cannon skill prep
            bulletSpawnEffect = LoadEffect("Spike Spawn");

            //on ferroshot/Lorentz Cannon spike collision
            bulletImpactEffect = LoadEffect("SpikeImpact");


            //functions for prefabs that require adjustments made at runtime
            CreateChainPrefab();
            CreateLightningPrefab();
            
          

            if (bombExplosionEffect)
            {
                ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
                shakeEmitter.amplitudeTimeDecay = true;
                shakeEmitter.duration = 0.5f;
                shakeEmitter.radius = 200f;
                shakeEmitter.scaleShakeRadiusWithLocalScale = false;

                shakeEmitter.wave = new Wave
                {
                    amplitude = 1f,
                    frequency = 40f,
                    cycleOffset = 0f
                };
            }

            swordSwingEffect = Assets.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = Assets.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateChargePrefab()
        {
           chargeExplosionEffect = mainAssetBundle.LoadAsset<GameObject>("ChargeExplosion");
           chargeExplosionEffect.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Hierarchy;
            chargeExplosionEffect.AddComponent<VFXAttributes>();
            chargeExplosionEffect.AddComponent<EffectComponent>();
            chargeExplosionEffect.AddComponent<NetworkIdentity>();

            EffectAPI.AddEffect(chargeExplosionEffect);



        }

        private static void CreateStreamPrefab()
        {
            electricStreamEffect = mainAssetBundle.LoadAsset<GameObject>("ElectricityStream");
            electricStreamEffect.AddComponent<NetworkIdentity>();

        }

        private static void CreateBoltExitPrefab()
        {
            //electricExplosionEffect = LoadEffect("ElectricExplosion", "HenryBombExplosion");
            boltExitEffect = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/MageLightningBombExplosion"), "boltExitEffect", true);
            boltExitEffect.AddComponent<NetworkIdentity>();

            EffectAPI.AddEffect(boltExitEffect);
        }


        private static void CreateBoltEnterPrefab()
        {
            boltEnterEffect = mainAssetBundle.LoadAsset<GameObject>("BoltEnter");
            boltEnterEffect.AddComponent<NetworkIdentity>();
            boltEnterEffect.AddComponent<VFXAttributes>();
            boltEnterEffect.AddComponent<EffectComponent>();

            EffectAPI.AddEffect(boltEnterEffect);


        }



        //instantiate voltaic bombardment main effect as copy of royal capacitor's effect
        private static void CreateLightningPrefab()
        {
            lightningStrikePrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), "lightningStrike", true);
            lightningStrikePrefab.AddComponent<NetworkIdentity>();

            // lightningStrikePrefab.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Hierarchy;

            EffectAPI.AddEffect(lightningStrikePrefab);


        }

        //instantiates chain lightning effect 
        private static void CreateChainPrefab()
        {
        /*   electricChainEffect = mainAssetBundle.LoadAsset<GameObject>("chainOrb.prefab");
           electricChainEffect.AddComponent<EffectComponent>();

            var vfxChain = electricChainEffect.AddComponent<VFXAttributes>();
            vfxChain.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            vfxChain.vfxPriority = VFXAttributes.VFXPriority.Always;

            var orbEffect = electricChainEffect.AddComponent<OrbEffect>();
            //orbEffect.startEffect = Resources.Load<GameObject>("Prefabs/Effects/ShieldBreakEffect");
            orbEffect.startEffect = mainAssetBundle.LoadAsset<GameObject>("ElectricityChain.prefab");
            orbEffect.startVelocity1 = new Vector3(-10, 10, -10);
            orbEffect.startVelocity2 = new Vector3(10, 13, 10);
            orbEffect.endVelocity1 = new Vector3(-10, 0, -10);
            orbEffect.endVelocity2 = new Vector3(10, 5, 10);
            orbEffect.movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

            electricChainEffect.AddComponent<NetworkIdentity>();

            if (electricChainEffect) PrefabAPI.RegisterNetworkPrefab(electricChainEffect);
            EffectAPI.AddEffect(electricChainEffect);

            OrbAPI.AddOrb(typeof(FulminationOrb)); */
            

           electricChainEffect = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/MageLightningOrbEffect"), "chainOrb", true);
           electricChainEffect.AddComponent<NetworkIdentity>();

            var orbEffect = electricChainEffect.GetComponent<OrbEffect>();


            var bezier = electricChainEffect.transform.GetChild(0).GetComponent<BezierCurveLine>();

            bezier.endTransform = electricChainEffect.transform;

            EffectAPI.AddEffect(mainAssetBundle.LoadAsset<GameObject>("ElectricityChain"));


            //electricChainEffect.GetComponent<BezierCurveLine>().endTransform = mainAssetBundle.LoadAsset<GameObject>("ElectricityChain").transform;

            EffectAPI.AddEffect(electricChainEffect); 

            


        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

            if (!newTracer.GetComponent<EffectComponent>()) newTracer.AddComponent<EffectComponent>();
            if (!newTracer.GetComponent<VFXAttributes>()) newTracer.AddComponent<VFXAttributes>();
            if (!newTracer.GetComponent<NetworkIdentity>()) newTracer.AddComponent<NetworkIdentity>();

            newTracer.GetComponent<Tracer>().speed = 250f;
            newTracer.GetComponent<Tracer>().length = 50f;

            AddNewEffectDef(newTracer);

            return newTracer;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        internal static void ConvertAllRenderersToHopooShader(GameObject objectToConvert)
        {
            if (!objectToConvert) return;

            foreach (MeshRenderer i in objectToConvert.GetComponentsInChildren<MeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }

            foreach (SkinnedMeshRenderer i in objectToConvert.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (i)
                {
                    if (i.material)
                    {
                        i.material.shader = hotpoo;
                    }
                }
            }
        }

        internal static CharacterModel.RendererInfo[] SetupRendererInfos(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] rendererInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                rendererInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            return rendererInfos;
        }

        internal static Texture LoadCharacterIcon(string characterName)
        {
            return mainAssetBundle.LoadAsset<Texture>("tex" + characterName + "Icon");
        }

        internal static GameObject LoadCrosshair(string crosshairName)
        {
            if (Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return Resources.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return Resources.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
        }

        private static GameObject LoadEffect(string resourceName)
        {
            return LoadEffect(resourceName, "", false);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            return LoadEffect(resourceName, soundName, false);
        }

        private static GameObject LoadEffect(string resourceName, bool parentToTransform)
        {
            return LoadEffect(resourceName, "", parentToTransform);
        }

        private static GameObject LoadEffect(string resourceName, string soundName, bool parentToTransform)
        {
            bool assetExists = false;
            for (int i = 0; i < assetNames.Length; i++)
            {
                if (assetNames[i].Contains(resourceName.ToLower()))
                {
                    assetExists = true;
                    i = assetNames.Length;
                }
            }

            if (!assetExists)
            {
                Debug.LogError("Failed to load effect: " + resourceName + " because it does not exist in the AssetBundle");
                return null;
            }

            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = parentToTransform;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddNewEffectDef(newEffect, soundName);

            return newEffect;
        }

        private static void AddNewEffectDef(GameObject effectPrefab)
        {
            AddNewEffectDef(effectPrefab, "");
        }

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!commandoMat) commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat)
            {
                Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                return commandoMat;
            }

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            return mat;
        }

        public static Material CreateMaterial(string materialName)
        {
            return Assets.CreateMaterial(materialName, 0f);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return Assets.CreateMaterial(materialName, emission, Color.white);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return Assets.CreateMaterial(materialName, emission, emissionColor, 0f);
        }
    }
}