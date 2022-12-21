using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using EntityStates;
using System.IO;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using RoR2.UI;
using UnityEngine.Rendering.PostProcessing;

namespace AmpMod.Modules
{
    internal static class Assets
    {
        // the assetbundle to load assets from
        internal static AssetBundle mainAssetBundle;
        internal static string prefix = AmpPlugin.developerPrefix;


        [Header("Materials")]
        internal static Material matRedLightning;
        internal static Material matBlueLightning;
        internal static Material matRedTrail;
        internal static Material matBlueTrail;
        internal static Material matLightningLongRed;
        internal static Material matLightningLongBlue;
        internal static Material matLightningMatrixRed;
        internal static Material matLightningSphereRed;
        internal static Material matDirectionalMatrixRed;
        internal static Material matHitSpark;
        internal static Material matRing;
        internal static Material matTracerBright;
        internal static Texture texRampRedLightning;
        internal static Material matPulseTriRed;
        internal static Material matLightningStrikeRed;
        internal static Color redLightningColor = new Color32(191, 2, 0, 255);
        internal static Color lightRedLightningColor = new Color32(255, 100, 100, 255);
        internal static Color darkRedLightningColor = new Color32(103, 0, 0, 255);

        [Header("Shockblade Effects")]
        internal static GameObject swordSwingEffect;
        internal static GameObject swordHitImpactEffect;
        internal static GameObject swordSwingEffectRed;
        internal static GameObject swordHitImpactEffectRed;

        [Header("Charge Effects")]
        internal static GameObject chargeExplosionEffect;
        internal static GameObject chargeExplosionEffectRed;
        internal static GameObject electrifiedOverlay;
        internal static Material electrifiedMaterial;
        internal static GameObject chargeOrbObject;
        internal static GameObject chargeOrbFullObject;
        internal static GameObject chargeOrbRedObject;
        internal static GameObject chargeOrbFullRedObject;

        [Header("Ferroshot/Lorentz Cannon Effects")]
        internal static GameObject bulletSpawnEffect;
        internal static GameObject bulletPrepItem;
        internal static GameObject bulletImpactEffect;
        internal static GameObject ampBulletMuzzleEffect;

        [Header("Magnetic Vortex Effects")]
        internal static GameObject vortexBlackholePrefab;
        internal static GameObject vortexExplosionEffect;
        internal static GameObject vortexMuzzleEffect;
        internal static GameObject vortexMuzzleEffectRed;

        [Header("Bolt Effects")]
        internal static GameObject boltExitEffect;
        internal static GameObject boltEnterEffect;
        internal static GameObject boltExitEffectRed;
        internal static GameObject boltEnterEffectRed;
        internal static GameObject boltVehicle;

        [Header("Pulse Leap Effects")]
        internal static GameObject pulseBlastEffect;
        internal static GameObject pulseMuzzleEffect;
        internal static GameObject pulseBlastEffectRed;
        internal static GameObject pulseMuzzleEffectRed;

        [Header("Plasma Slash Effects")]
        internal static GameObject heatSwing;
        internal static GameObject heatHit;
        internal static GameObject heatExplosion;
        internal static NetworkSoundEventDef plasmaExplosionSoundEvent;
        internal static GameObject plasmaMuzzle;
        internal static GameObject fireTrail;

        [Header("Fulmination Effects")]
        internal static GameObject electricStreamEffect;
        internal static GameObject electricImpactEffect;
        internal static GameObject electricChainEffect;
        internal static GameObject electricMuzzleEffect;
        internal static GameObject electricStreamEffectRed;
        internal static GameObject electricImpactEffectRed;
        internal static GameObject electricChainEffectRed;

        [Header("VoltaicBombardment Effects")]
        internal static GameObject lightningStrikePrefab;
        internal static GameObject lightningMuzzleChargePrefab;
        internal static GameObject lightningStrikePrefabRed;
        internal static GameObject lightningMuzzleChargePrefabRed;

        [Header("Bulwark of Storms Effects")]
        internal static GameObject wormExplosionEffect;
        internal static GameObject melvinPrefab;
        internal static GameObject melvinBody;

        [Header("Lobby Effects")]
        internal static GameObject lobbyEntranceEffect;

        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEvent;
        internal static NetworkSoundEventDef vortexLoopSoundEvent;
        internal static NetworkSoundEventDef vortexFlightLoopSoundEvent;
        internal static NetworkSoundEventDef stormbladeHitSoundEvent;
        internal static NetworkSoundEventDef vortexSpawnSoundEvent;
        internal static NetworkSoundEventDef heatShockSwingSoundEvent;
        internal static NetworkSoundEventDef heatShockHitSoundEvent;

        // lists of assets to add to contentpack
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        // cache these and use to create our own materials
        internal static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");
        internal static Material commandoMat;
        private static string[] assetNames = new string[0];
        internal static ItemDef wormHealth;


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
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AmpMod." + assetbundleName))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            assetNames = mainAssetBundle.GetAllAssetNames();
        }

        internal static void LoadSoundbank()
        {
 

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("AmpMod.AmpSoundbank.bnk"))
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


            ItemDisplayRule[] nullDisplay = new ItemDisplayRule[] { };
            wormHealth = ScriptableObject.CreateInstance<ItemDef>();
            wormHealth.name = "wormHealth";

            //wormHealth.tier = ItemTier.NoTier;
            wormHealth.descriptionToken = "bruh";
            wormHealth.loreToken = "bruh";
            wormHealth.AutoPopulateTokens();

            wormHealth.deprecatedTier = ItemTier.NoTier;

                
            
            // wormHealth.pickupToken = "bruh";

            ItemAPI.Add(new CustomItem(wormHealth, new ItemDisplayRule[0]));


            //shockblade hit
            stormbladeHitSoundEvent = CreateNetworkSoundEventDef(StaticValues.stormbladeHit4String);

            //create magnetic vortex projectile flight loop event
            vortexFlightLoopSoundEvent = CreateNetworkSoundEventDef(StaticValues.vortexFlightLoopString);

            //magnetic vortex loop sound effect
            vortexLoopSoundEvent = CreateNetworkSoundEventDef(StaticValues.vortexLoopString);

            //magnetic vortex spawn sound effect
            vortexSpawnSoundEvent = CreateNetworkSoundEventDef(StaticValues.vortexSpawnString);

            //heatshock hit & swing sound events
            heatShockHitSoundEvent = CreateNetworkSoundEventDef(StaticValues.heatHitString);
            heatShockSwingSoundEvent = CreateNetworkSoundEventDef(StaticValues.heatSwingString);

            //plasma slash explosion sound events
            plasmaExplosionSoundEvent = CreateNetworkSoundEventDef(StaticValues.plasmaExplosionString);

            //materials
            CreateMaterials();

            //on voltaic bombardment aim
            CreateLightningCharge();

            //on charge proc
            CreateChargeOrb();

            //on charge explosion when 3 procs are reached
            CreateChargePrefab();
            CreateElectrified();

            //on fulmination skill chain
            //electricChainEffect = mainAssetBundle.LoadAsset<GameObject>("ElectricityChain");

            //on fulmination skill use
            CreateStreamPrefab();
            CreateChainPrefab();

            //on heatshock use
            heatHit = Assets.LoadEffect("heatHit");
            heatSwing = Assets.LoadEffect("heatSwing 1", true);
            CreateHeatExplosion();

            //on boltvehicle exit/state/enter
            CreateBoltExitPrefab();
            CreateBoltVehicle();
            CreateBoltEnterPrefab();

            //on ferroshot/Lorentz Cannon skill prep
            CreateBulletPrep();
            bulletSpawnEffect = LoadEffect("Spike Spawn");
            //CreateBulletMuzzle();

            //on ferroshot/Lorentz Cannon spike collision
            bulletImpactEffect = LoadEffect("SpikeImpact");

            //on pulse leap use
            CreatePulseBlastPrefab();  

            //on magnetic vortex blackhole spawn
            CreateVortexBlackhole();

            //magnetic vortex muzzle
            CreateVortexMuzzle();

            //on bulwark of storms use
            CreateWormEffects();
            CreateMelvin();

            //functions for prefabs that require adjustments made at runtime
            CreateLightningPrefab();

            CreateShockbladeEffects();
        


        }


        private static void CreateShockbladeEffects()
        {
            swordSwingEffect = Assets.LoadEffect("StormbladeSwing", true);
            swordHitImpactEffect = Assets.LoadEffect("StormbladeHit");
            swordSwingEffectRed = Assets.LoadEffect("StormbladeSwingRed", true);
            swordHitImpactEffectRed = Assets.LoadEffect("StormbladeHitRed");

        }

        private static void CreateMaterials()
        {

            List<Material> matList = new List<Material>();

            matRedLightning = mainAssetBundle.LoadAsset<Material>("LightningEffectRed");
            matBlueLightning = mainAssetBundle.LoadAsset<Material>("LightningEffect");

            matBlueTrail = CreateVFXMaterial("matLorentzTrail");
            matRedTrail = CreateVFXMaterial("matLorentzTrailRed");
            CreateVFXMaterial("matLightningSphereBlue");
            CreateVFXMaterial("matLightningOrbRed");
            matLightningMatrixRed = CreateVFXMaterial("matLightningMatrixRed");
            matLightningLongRed = CreateVFXMaterial("matLightningLongRed");
            matPulseTriRed = CreateVFXMaterial("matPulseMuzzleTriRed");
            matDirectionalMatrixRed = CreateVFXMaterial("matDirectionalMatrixRed");
            matTracerBright = CreateVFXMaterial("matTracerBright");
            matRing = CreateVFXMaterial("matOmniRing2");
            matHitSpark = CreateVFXMaterial("matOmniHitspark3");
            matLightningStrikeRed = CreateVFXMaterial("matLightningStrikeRed");
            matLightningLongBlue = CreateVFXMaterial("matLightningLongBlue");

            matLightningSphereRed = CreateIntersectMaterial("matLightningSphereRed");

            texRampRedLightning = mainAssetBundle.LoadAsset<Texture>("texRampLightningRed");


    
            //matRedTrail.SetTexture("_RemapTex", mainAssetBundle.LoadAsset<Texture>("texRampGolem"));



        }

        private static void CreateChargeOrb()
        {
            chargeOrbObject = mainAssetBundle.LoadAsset<GameObject>("ChargeBall");
            chargeOrbFullObject = mainAssetBundle.LoadAsset<GameObject>("ChargeBallFull");
            chargeOrbRedObject = mainAssetBundle.LoadAsset<GameObject>("ChargeBallRed");
            chargeOrbFullRedObject = mainAssetBundle.LoadAsset<GameObject>("ChargeBallFullRed");

            PrefabAPI.RegisterNetworkPrefab(chargeOrbObject);
            PrefabAPI.RegisterNetworkPrefab(chargeOrbFullObject);
            PrefabAPI.RegisterNetworkPrefab(chargeOrbRedObject);
            PrefabAPI.RegisterNetworkPrefab(chargeOrbFullRedObject);
        }

        private static void CreateBoltVehicle()
        {
            boltVehicle = mainAssetBundle.LoadAsset<GameObject>("BoltVehicle");
            //boltVehicle = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle");

            //adds boltvehicle to the bolt prefab finalizing the gameobject that will act as the primary enactor of the surge skill
            boltVehicle.AddComponent<SkillStates.BoltVehicle>();
            
            
            PrefabAPI.RegisterNetworkPrefab(boltVehicle);
        }


        private static void CreateElectrified()
        {
            electrifiedMaterial = CreateVFXMaterial("matIsElectrified");

        }
        private static void CreateMelvin()
        {
            melvinPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricWormMaster.prefab").WaitForCompletion(), "MelvinPrefab", true);
            
            CharacterMaster melvinMaster = melvinPrefab.GetComponent<CharacterMaster>();
        
            melvinBody = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion(), "MelvinBody", true);
            //var healthTracker = melvinBody.AddComponent<SkillStates.SkillComponents.WormHealthTracker>();

            var melvinCharBody = melvinBody.GetComponent<CharacterBody>();
            
            melvinCharBody.baseNameToken = prefix + "_AMP_BODY_SPECIAL_WORM_DISPLAY_NAME";
            //melvinCharBody.baseMaxHealth = healthTracker.summonerHealth * 3f; //melvinCharBody.baseMaxHealth;//melvinMinionOwner.ownerMaster.GetBody().baseMaxHealth * 3f;
            //melvinCharBody.statsDirty = true;

            melvinMaster.bodyPrefab = melvinBody;

            

            melvinBody.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(SkillStates.BaseStates.MelvinDeathState));

            

           PrefabAPI.RegisterNetworkPrefab(melvinPrefab);

        }


        private static void CreateVortexBlackhole()
        {
           vortexBlackholePrefab = mainAssetBundle.LoadAsset<GameObject>("VortexSphere");
           vortexBlackholePrefab.AddComponent<SkillStates.RadialDamage>();
            //vortexBlackholePrefab.GetComponent<ProjectileDamage>().damage = 2f;

            vortexExplosionEffect = mainAssetBundle.LoadAsset<GameObject>("VortexExplosion");
            
            AddNewEffectDef(vortexExplosionEffect, Modules.StaticValues.vortexExplosionString);
            AddNewEffectDef(mainAssetBundle.LoadAsset<GameObject>("VortexSpawnExplosion"));

            ChildLocator childLocator = vortexBlackholePrefab.GetComponent<ChildLocator>();
            Transform vortexTransform = childLocator.FindChild("VortexHitbox");
            Modules.Prefabs.SetupHitbox(vortexBlackholePrefab, vortexTransform, "VortexHitbox"); 
            //mainAssetBundle.LoadAsset<GameObject>("VortexHitbox").gameObject.layer = LayerIndex.projectile.intVal;

            PrefabAPI.RegisterNetworkPrefab(vortexBlackholePrefab);
             
        }

        private static void CreateVortexMuzzle()
        {
            vortexMuzzleEffect = mainAssetBundle.LoadAsset<GameObject>("VortexMuzzleObject");
            vortexMuzzleEffectRed = mainAssetBundle.LoadAsset<GameObject>("VortexMuzzleObjectRed");
        }

        private static void CreateBulletPrep()
        {
            bulletPrepItem = mainAssetBundle.LoadAsset<GameObject>("Spike");
            //bulletPrepItem.AddComponent<NetworkIdentity>();
            

            PrefabAPI.RegisterNetworkPrefab(bulletPrepItem);
        }
     
        private static void CreateHeatExplosion()
        {
            AddNewEffectDef(mainAssetBundle.LoadAsset<GameObject>("HeatExplosionEffect"), Modules.StaticValues.plasmaExplosionString);
            plasmaMuzzle = mainAssetBundle.LoadAsset<GameObject>("PlasmaMuzzleEffect");
            
        }
        private static void CreateBulletMuzzle()
        {
            ampBulletMuzzleEffect = mainAssetBundle.LoadAsset<GameObject>("FerroshotMuzzleObject");

            PrefabAPI.RegisterNetworkPrefab(ampBulletMuzzleEffect);
        }

        private static void CreateChargePrefab()
        {
            chargeExplosionEffect = LoadEffect("ChargeExplosion", StaticValues.chargeExplosionString);
            chargeExplosionEffectRed = LoadEffect("ChargeExplosionRed", StaticValues.chargeExplosionString);


        }

        private static void CreateLightningCharge()
        {
           
            lightningMuzzleChargePrefab = mainAssetBundle.LoadAsset<GameObject>("HandSpark");
            lightningMuzzleChargePrefabRed = mainAssetBundle.LoadAsset<GameObject>("HandSparkRed");
            //AddNewEffectDef(lightningMuzzleChargePrefab);
        }

        private static void CreateWormEffects()
        {
            wormExplosionEffect = mainAssetBundle.LoadAsset<GameObject>("WormExplosionEffect");
            AddNewEffectDef(wormExplosionEffect);
        }
        
        private static void CreateStreamPrefab()
        {

            electricStreamEffect = mainAssetBundle.LoadAsset<GameObject>("ElectricityStream");
            electricStreamEffectRed = mainAssetBundle.LoadAsset<GameObject>("ElectricityStreamRed");

            electricMuzzleEffect = mainAssetBundle.LoadAsset<GameObject>("FulminationMuzzleObject");

            //CreateVFXMaterial("lightningEffect");

            //on fulmination skill contact
            electricImpactEffect = LoadEffect("ElectricitySphere 1", null);
            electricImpactEffectRed = LoadEffect("ElectricitySphereRed", null);

            electricStreamEffectRed.AddComponent<NetworkIdentity>();
            electricStreamEffect.AddComponent<NetworkIdentity>();

        }

        private static void CreateBoltExitPrefab()
        {
            //electricExplosionEffect = LoadEffect("ElectricExplosion", "HenryBombExplosion");
            boltExitEffect = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MageLightningBombExplosion"), "boltExitEffect", true);
            boltExitEffect.AddComponent<NetworkIdentity>();

            boltExitEffectRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MageLightningBombExplosion"), "boltExitEffectRed", true);
            boltExitEffectRed.AddComponent<NetworkIdentity>();

            AddNewEffectDef(boltExitEffect);
            AddNewEffectDef(boltExitEffectRed);

            Transform boltExitTransform = boltExitEffectRed.transform;
            foreach(Transform child in boltExitTransform)
            {
                switch (child.name)
                {
                    case "Matrix, Dynamic":
                        var main = child.GetComponent<ParticleSystem>().main;
                        main.startColor = redLightningColor;
                        child.GetComponent<ParticleSystemRenderer>().trailMaterial = matLightningLongRed;
                        break;
                    case "Matrix, Billboard":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningMatrixRed);
                        break;
                    case "Point light":
                        child.GetComponent<Light>().color = redLightningColor;
                        break;
                    case "Flash, Directional":
                        var main2 = child.GetComponent<ParticleSystem>().main;
                        main2.startColor = redLightningColor;
                        break;
                    case "Matrix, Directional":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matDirectionalMatrixRed);
                        break;
                    case "Flash, Blue":
                        var main3 = child.GetComponent<ParticleSystem>().main;
                        main3.startColor = darkRedLightningColor;
                        break;
                }
            }
        }
        
        private static void CreatePulseBlastPrefab()
        {
            pulseBlastEffect = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/omnieffect/OmniImpactVFXLightningMage"), "pulseBlastEffect", true);
            pulseBlastEffect.AddComponent<NetworkIdentity>();

            pulseBlastEffectRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/omnieffect/OmniImpactVFXLightningMage"), "pulseBlastEffectRed", true);
            //pulseBlastEffectRed = mainAssetBundle.LoadAsset<GameObject>("pulseBlastRed");
            pulseBlastEffectRed.AddComponent<NetworkIdentity>();

            

            pulseMuzzleEffect = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/muzzleflashes/MuzzleflashMageLightningLargeWithTrail"), "pulseMuzzleEffect", true);
            pulseMuzzleEffect.AddComponent<NetworkIdentity>();

            pulseMuzzleEffectRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/muzzleflashes/MuzzleflashMageLightningLargeWithTrail"), "pulseMuzzleEffectRed", true);
            pulseMuzzleEffectRed.AddComponent<NetworkIdentity>();

          //  Material triMaterial = UnityEngine.Material.Instantiate(LegacyResourcesAPI.Load<Material>("matMageMatrixTriLightning"));
          //  triMaterial.SetTexture("_RemapTex", texRampRedLightning);


            Transform muzzleParticleTransform = pulseMuzzleEffectRed.transform.GetChild(0);
            foreach (Transform child in muzzleParticleTransform)
            {
                switch (child.name)
                {
                    case "Matrix, Billboard":
                        child.GetComponent<ParticleSystemRenderer>().material = matLightningMatrixRed;
                        break;
                    case "Flash":
                        var main = child.GetComponent<ParticleSystem>().main;
                        main.startColor = redLightningColor;
                        break;
                    case "Point light":
                        child.GetComponent<Light>().color = redLightningColor;
                        break;
                    case "Smoke":
                        break;
                    case "Matrix, Mesh":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matPulseTriRed);
                        break;
                    case "Spinner":
                        child.GetComponentInChildren<TrailRenderer>().SetMaterial(matLightningLongRed);
                        break;
                }
            }

            Transform blastEffectTransformRed = pulseBlastEffectRed.transform;
            Vector3 scaleChange = new Vector3(0.3f, 03f, 0.3f);    
            blastEffectTransformRed.localScale = scaleChange;
            Debug.Log(blastEffectTransformRed.localScale);
            foreach (Transform child in blastEffectTransformRed)
            {
                switch (child.name)
                {
                    case "Flash, Blue":
                        var main = child.GetComponent<ParticleSystem>().main;
                        main.startColor = redLightningColor;
                        break;
                    case "Scaled Hitspark 3 (Random Color)":
                        var main2 = child.GetComponent<ParticleSystem>().main;
                        main2.startColor = redLightningColor;
                        break;
                    case "Impact Shockwave":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matRing);
                        var main3 = child.GetComponent<ParticleSystem>().main;
                        main3.startColor = redLightningColor;
                        break;
                    case "Matrix, Directional":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matDirectionalMatrixRed);
                        break;
                    case "Matrix, Dynamic":
                        var main4 = child.GetComponent<ParticleSystem>().main;
                        main4.startColor = redLightningColor;
                        child.GetComponent<ParticleSystemRenderer>().trailMaterial = matLightningLongRed;
                        break;
                    case "Point light":
                        child.GetComponent<Light>().color = redLightningColor;
                        break;
                    case "Sphere, Expanding":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningSphereRed);
                        break;
                    case "Matrix, Billboard": 
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningMatrixRed);
                        break;
                    case "Scaled Hitspark 1 (Random Color)":
                        var main5 = child.GetComponent<ParticleSystem>().main;
                        main5.startColor = redLightningColor;
                        break;
                    case "Flash, Directional":
                        GameObject.Destroy(child.gameObject);
                        break;
                    case "Dash, Bright":
                        var main6 = child.GetComponent<ParticleSystem>().main;
                        main6.startColor = redLightningColor;
                        break;
                   
                }
                
           /*     if (child.name == "Flash, Blue")
                {
                    var main = child.GetComponent<ParticleSystem>().main;
                    main.startColor = redLightningColor;
                }

                else if (child.name == "Matrix, Directional")
                {
                    child.GetComponent<ParticleSystemRenderer>().material = mainAssetBundle.LoadAsset<Material>("matDirectionalMatrixRed");
                }

                else if (child.name == "Matrix, Dynamic")
                {
                    var main = child.GetComponent<ParticleSystem>().main;
                    main.startColor = redLightningColor;
                    child.GetComponent<ParticleSystemRenderer>().trailMaterial = matLightningLongRed;
                }

                else if (child.name == "Point light")
                {
                    child.GetComponent<Light>().color = redLightningColor;
                }

                else if (child.name == "Sphere, Expanding")
                {
                    child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningSphereRed);
                }

                else if (child.name == "Matrix, Billboard")
                {
                    child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningMatrixRed);
                } */
            }


            AmpPlugin.Destroy(pulseMuzzleEffect.GetComponent<EffectComponent>());
            AmpPlugin.Destroy(pulseMuzzleEffectRed.GetComponent<EffectComponent>());


              AddNewEffectDef(pulseBlastEffect, null, true);
              //AddNewEffectDef(pulseMuzzleEffect); 

              AddNewEffectDef(pulseBlastEffectRed, null, true);
              //AddNewEffectDef(pulseMuzzleEffectRed);
        }

        private static void CreateBoltEnterPrefab()
        {
            boltEnterEffect = mainAssetBundle.LoadAsset<GameObject>("BoltEnter");
            boltEnterEffect.AddComponent<NetworkIdentity>();
            boltEnterEffect.AddComponent<VFXAttributes>();
            boltEnterEffect.AddComponent<EffectComponent>();

            boltEnterEffectRed = mainAssetBundle.LoadAsset<GameObject>("BoltEnterRed");
            boltEnterEffectRed.AddComponent<NetworkIdentity>();
            boltEnterEffectRed.AddComponent<VFXAttributes>();
            boltEnterEffectRed.AddComponent<EffectComponent>();


            AddNewEffectDef(boltEnterEffect);
            AddNewEffectDef(boltEnterEffectRed);

        }
         


        //instantiate voltaic bombardment main effect as copy of royal capacitor's effect
        private static void CreateLightningPrefab()
        {
            lightningStrikePrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), "lightningStrike", true);
            lightningStrikePrefab.AddComponent<NetworkIdentity>();
            //lightningStrikePrefab.GetComponent<EffectComponent>().soundName = "Play_item_use_lighningArm";

            lightningStrikePrefabRed = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), "lightningStrikeRed", true);
            lightningStrikePrefabRed.AddComponent<NetworkIdentity>();
            //lightningStrikePrefabRed.GetComponent<EffectComponent>().soundName = "Play_item_use_lighningArm";

            Transform redLightningTransform = lightningStrikePrefabRed.transform;
            foreach (Transform child in redLightningTransform)
            {
                switch (child.name)
                {
                    case "PostProcess":
                        child.GetComponent<PostProcessVolume>().sharedProfile = mainAssetBundle.LoadAsset<PostProcessProfile>("ppLocalLightningRed");//Addressables.LoadAssetAsync<PostProcessProfile>("RoR2/Base/title/ppLocalTPActivation.asset").WaitForCompletion();
                        break;
                    case "Ring":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningLongRed);
                        break;
                    case "Sphere":
                        child.GetComponent<ParticleSystemRenderer>().SetMaterial(matLightningSphereRed);
                        break;
                    case "Point light":
                        child.GetComponent<Light>().color = redLightningColor;
                        break;
                    case "LightningRibbon":
                        child.GetComponent<ParticleSystemRenderer>().trailMaterial = matLightningStrikeRed;
                        break;
                    case "Flash Lines":
                        var main = child.GetComponent<ParticleSystem>().main;
                        main.startColor = redLightningColor;
                        break;
                    case "Flash":
                        var main2 = child.GetComponent<ParticleSystem>().main;
                        main2.startColor = darkRedLightningColor;
                        break;

                }
            }
            // lightningStrikePrefab.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Hierarchy;

            AddNewEffectDef(lightningStrikePrefab);
            AddNewEffectDef(lightningStrikePrefabRed);

        }

        //instantiates chain lightning effect 
        private static void CreateChainPrefab()
        {

            electricChainEffect = mainAssetBundle.LoadAsset<GameObject>("ChainLightningEffect");
            electricChainEffect.AddComponent<NetworkIdentity>();

            electricChainEffectRed = mainAssetBundle.LoadAsset<GameObject>("ChainLightningEffectRed");
            electricChainEffectRed.AddComponent<NetworkIdentity>();

            AddNewEffectDef(electricChainEffect);
            AddNewEffectDef(electricChainEffectRed);




        }

        private static GameObject CreateTracer(string originalTracerName, string newTracerName)
        {
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName) == null) return null;

            GameObject newTracer = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/" + originalTracerName), newTracerName, true);

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
            if (RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair") == null) return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/StandardCrosshair");
            return RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/" + crosshairName + "Crosshair");
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
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            //newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
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

        private static void AddNewEffectDef(GameObject effectPrefab, string soundName, bool removeSound)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            if (removeSound)
            {
                newEffectDef.spawnSoundEventName = soundName;
            }


            effectDefs.Add(newEffectDef);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
         /*   if (!commandoMat) commandoMat = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

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
            //mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetTexture("_EmTex", mainAssetBundle.LoadAsset<Texture>("texEmission"));
            
            if (!mainAssetBundle.LoadAsset<Texture>("texEmission"))
            {
                Debug.Log("No Emission Map");
            }
            mat.SetFloat("_NormalStrength", normalStrength);
            //mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/deferred/hgstandard"); */
            Material mat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/deferred/hgstandard");
            return mat;
        }

        public static Material CreateVFXMaterial(string materialName)
        {
            Material mat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/fx/hgcloudremap");
            return mat;
        }

        public static Material CreateStandardMaterial(string materialName)
        {
            Material mat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/deferred/standard");
            return mat;
        }
        public static Material CreateIntersectMaterial(string materialName)
        {
            Material mat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);

            mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/fx/hgintersectioncloudremap");
            return mat;
        }


        public static Material CreateVFXMaterial(Material mat)
        {
            mat.shader = LegacyResourcesAPI.Load<Shader>("shaders/fx/hgcloudremap");
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