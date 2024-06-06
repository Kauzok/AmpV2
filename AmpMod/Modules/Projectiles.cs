using AncientScepter;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Audio;
using static R2API.DamageAPI;

namespace AmpMod.Modules
{
    internal static class Projectiles
    {

        internal static GameObject ferroshotPrefab;
        internal static GameObject vortexPrefab;
        internal static GameObject lightningPrefab;
        internal static GameObject fireBeamPrefab;
        internal static GameObject fieldProjectilePrefab;
        internal static GameObject bladeProjectilePrefab;
        internal static GameObject bladeProjectilePrefabBlue;
        internal static GameObject lightningStakePrefab;
        internal static GameObject lightningStakePrefabBlue;
        internal static GameObject fieldProjectilePrefabBlue;
        internal static GameObject bladeProjectileGhostBlue;
        internal static GameObject bladeProjectileGhost;
        internal static GameObject lightningStakeGhostBlue;
        internal static GameObject lightningStakeGhost;

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            CreateFerroshot();
            CreateVortex();
            CreateFireBeam();
            //CreateLightning();
            CreateStaticField();
            CreateFluxBlade();
            CreateLightningBall();

            AddProjectile(ferroshotPrefab);
            AddProjectile(vortexPrefab);
            AddProjectile(fireBeamPrefab);
            AddProjectile(fieldProjectilePrefab);
            AddProjectile(fieldProjectilePrefabBlue);
            AddProjectile(bladeProjectilePrefab);
            AddProjectile(lightningStakePrefab);
            AddProjectile(lightningStakePrefabBlue);
            AddProjectile(bladeProjectilePrefabBlue);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        private static void CreateFluxBlade()
        {
            bladeProjectilePrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("BladeProjectilePrefab");
            bladeProjectilePrefab.layer = LayerIndex.projectile.intVal;

   
            bladeProjectileGhost = CreateGhostPrefab("BladeGhostPrefab");

            bladeProjectileGhostBlue = CreateGhostPrefab("BladeGhostPrefabBlue");

            ProjectileSingleTargetImpact bladeContactController = bladeProjectilePrefab.GetComponent<ProjectileSingleTargetImpact>();
            bladeContactController.impactEffect = Assets.bulletImpactEffect;

            var dmgTypeHolder = bladeProjectilePrefab.AddComponent<ModdedDamageTypeHolderComponent>();
            dmgTypeHolder.Add(DamageTypes.controlledChargeProcProjectile);

            #region Blue
            bladeProjectilePrefabBlue = Assets.mainAssetBundle.LoadAsset<GameObject>("BladeProjectilePrefabBlue");
            bladeProjectilePrefabBlue.layer = LayerIndex.projectile.intVal;

            ProjectileSingleTargetImpact bladeContactControllerBlue = bladeProjectilePrefabBlue.GetComponent<ProjectileSingleTargetImpact>();
            bladeContactControllerBlue.impactEffect = Assets.bulletImpactEffect;

            var dmgTypeHolderBlue = bladeProjectilePrefabBlue.AddComponent<ModdedDamageTypeHolderComponent>();
            dmgTypeHolderBlue.Add(DamageTypes.controlledChargeProcProjectile);

            var bladeProjectileControllerBlue = bladeProjectilePrefabBlue.GetComponent<ProjectileController>();
            bladeProjectileControllerBlue.procCoefficient = StaticValues.bladeProcCoefficient;
            #endregion


            var bladeProjectileController = bladeProjectilePrefab.GetComponent<ProjectileController>();
            bladeProjectileController.procCoefficient = StaticValues.bladeProcCoefficient;

            PrefabAPI.RegisterNetworkPrefab(bladeProjectilePrefab);
            PrefabAPI.RegisterNetworkPrefab(bladeProjectilePrefabBlue);


        }

        private static void CreateLightningBall()
        {

            Assets.CreateIntersectMaterial("matTransparentLightningPurple");
            lightningStakePrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("LightningBallProjectilePrefab");
            lightningStakePrefab.layer = LayerIndex.projectile.intVal;

            ProjectileController lightningBallController = lightningStakePrefab.GetComponent<ProjectileController>();


            lightningStakeGhostBlue = CreateGhostPrefab("PlasmaShotGhostBlue");
            lightningStakeGhost = CreateGhostPrefab("PlasmaShotGhost");

            lightningBallController.allowPrediction = true;

            var damageHolder = lightningStakePrefab.AddComponent<ModdedDamageTypeHolderComponent>();

            damageHolder.Add(DamageTypes.controlledChargeProc);

            #region Blue
            lightningStakePrefabBlue = Assets.mainAssetBundle.LoadAsset<GameObject>("LightningBallProjectilePrefabBlue");
            lightningStakePrefabBlue.layer = LayerIndex.projectile.intVal;

            ProjectileController lightningBallControllerBlue = lightningStakePrefabBlue.GetComponent<ProjectileController>();


            lightningBallControllerBlue.allowPrediction = true;

            var damageHolderBlue = lightningStakePrefabBlue.AddComponent<ModdedDamageTypeHolderComponent>();

            damageHolderBlue.Add(DamageTypes.controlledChargeProc);
            #endregion

            PrefabAPI.RegisterNetworkPrefab(lightningStakePrefab);
            PrefabAPI.RegisterNetworkPrefab(lightningStakePrefabBlue);
        }

        private static void CreateStaticField()
        {
            fieldProjectilePrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("StaticFieldDOT");

            //use blast attacks instead of DOT Zones
            var damageComponent = fieldProjectilePrefab.AddComponent<SkillStates.Nemesis_Amp.Components.StaticFieldDamage>();
            damageComponent.procCoefficient = StaticValues.staticFieldTickProcCoefficient;
      
            //add the attack speed buff
            var buffWard = fieldProjectilePrefab.GetComponent<BuffWard>();
            buffWard.buffDef = Buffs.nemAmpAtkSpeed;

            //make the field play sound
            var stopLoop = fieldProjectilePrefab.AddComponent<SkillStates.BaseStates.StopLoop>();
            stopLoop.SoundEventToPlay = StaticValues.fieldLoopString;
            stopLoop.SoundId = 2520482775;

            PrefabAPI.RegisterNetworkPrefab(fieldProjectilePrefab);

            #region Blue
            fieldProjectilePrefabBlue = Assets.mainAssetBundle.LoadAsset<GameObject>("StaticFieldDOTBlue");

            var blueDamageComponent = fieldProjectilePrefabBlue.AddComponent<SkillStates.Nemesis_Amp.Components.StaticFieldDamage>();
            blueDamageComponent.procCoefficient = StaticValues.staticFieldTickProcCoefficient;

            var buffWardBlue = fieldProjectilePrefabBlue.GetComponent<BuffWard>();
            buffWardBlue.buffDef = Buffs.nemAmpAtkSpeed;

            var stopLoopBlue = fieldProjectilePrefabBlue.AddComponent<SkillStates.BaseStates.StopLoop>();
            stopLoopBlue.SoundEventToPlay = StaticValues.fieldLoopString;
            stopLoopBlue.SoundId = 2520482775;

            PrefabAPI.RegisterNetworkPrefab(fieldProjectilePrefabBlue);
            #endregion
        }

        //instantiates ferroshot/Lorentz Cannon projectile
        private static void CreateFerroshot()
        {
            //ferroshotPrefab = CloneProjectilePrefab("LunarShardProjectile", "Ferroshot");

            ferroshotPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("SpikeProjectile");
            //ferroshotPrefab.layer = LayerIndex.projectile.intVal;
            //change damagetype of ferroshot to generic
            //ProjectileDamage ferroshotDamage = ferroshotPrefab.GetComponent<ProjectileDamage>();
            //.damageType = DamageType.Generic;

            //remove/nullify components from lunarshard that are unnecessary, such as the tracker and on impact explosion
            /* AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileImpactExplosion>());
             AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileProximityBeamController>());
             AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileSteerTowardTarget>());
             AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileDirectionalTargetFinder>());
             AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileTargetComponent>());

             ferroshotPrefab.GetComponent<Rigidbody>().useGravity = false;
             AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ParticleSystem>()); */
            ferroshotPrefab.GetComponent<ProjectileSimple>().lifetime = 1f;


            ferroshotPrefab.AddComponent<DestroyOnTimer>().duration = .5f;
           ferroshotPrefab.AddComponent<SkillStates.SkillComponents.ChargedTargeting>();
            ferroshotPrefab.AddComponent<SkillStates.SkillComponents.ChargedHoming>();
           ferroshotPrefab.AddComponent<SkillStates.SkillComponents.ChargedDirectionFinder>();

            ProjectileController ferroshotController = ferroshotPrefab.GetComponent<ProjectileController>();
         
            //instantiates the projectile model and associates it with the prefab
            if (Assets.mainAssetBundle.LoadAsset<GameObject>("SpikeGhost") != null) ferroshotController.ghostPrefab = CreateGhostPrefab("SpikeGhost");


            ferroshotController.procCoefficient = .7f;
            
            //ferroshotController.allowPrediction = true;
            //makes ferroshot destroy itself on contact with other entities, + adds impact effect
            ProjectileSingleTargetImpact ferroshotContact = ferroshotPrefab.AddComponent<ProjectileSingleTargetImpact>();
            InitializeFerroshotContact(ferroshotContact);
            ferroshotContact.destroyOnWorld = true;
            ferroshotContact.impactEffect = Assets.bulletImpactEffect;


            PrefabAPI.RegisterNetworkPrefab(ferroshotPrefab);

        }

        private static void CreateVortex()
        {
            vortexPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("VortexProjectile");

            ProjectileController vortexController = vortexPrefab.GetComponent<ProjectileController>();
            //instantiates the  projectile model and associates it with the prefab
            if (Assets.mainAssetBundle.LoadAsset<GameObject>("VortexEffect") != null) vortexController.ghostPrefab = CreateGhostPrefab("VortexEffect");
            vortexController.allowPrediction = true;
            
            //code for giving flight loop sound to vortex projectile and making it stop on contact
            /* LoopSoundDef flightLoop = new LoopSoundDef();
              flightLoop.startSoundName = StaticValues.vortexFlightLoopString;
              vortexPrefab.GetComponent<ProjectileController>().flightSoundLoop = flightLoop; */

            var stopSound = vortexPrefab.AddComponent<SkillStates.BaseStates.StopLoop>();
            stopSound.SoundEventToPlay = StaticValues.vortexFlightLoopStringAlt;
            stopSound.SoundId = 2447326215;
            

            PrefabAPI.RegisterNetworkPrefab(vortexPrefab);






            /*
            vortexPrefab = CloneProjectilePrefab("LunarShardProjectile", "Vortex");
            ProjectileDamage vortexDamage = vortexPrefab.GetComponent<ProjectileDamage>();
            vortexDamage.damageType = DamageType.Generic;

            //remove/nullify components from lunarshard that are unnecessary, such as the tracker and on impact explosion
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileImpactExplosion>());
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileProximityBeamController>());
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileSteerTowardTarget>());
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ParticleSystem>()); */

        }

        private static void CreateFireBeam()
        {
            fireBeamPrefab = Assets.mainAssetBundle.LoadAsset<GameObject>("HeatProjectile");
            //fireBeamPrefab.layer = LayerIndex.projectile.intVal;

            var damageTypeComponent = fireBeamPrefab.AddComponent<ModdedDamageTypeHolderComponent>();
            damageTypeComponent.Add(DamageTypes.strongBurnIfCharged);

            ProjectileController heatController = fireBeamPrefab.GetComponent<ProjectileController>();
            if (Assets.mainAssetBundle.LoadAsset<GameObject>("HeatProjectileGhost") != null) heatController.ghostPrefab = CreateGhostPrefab("HeatProjectileGhost");
            heatController.allowPrediction = true;

            PrefabAPI.RegisterNetworkPrefab(fireBeamPrefab);
            

            //fireBeamPrefab.GetComponent<ProjectileImpactExplosion>().lifetimeExpiredSound = Assets.plasmaExplosionSoundEvent;
           // FireExplosion.blastDamageCoefficient = 3f;
        }
        //projectile to be used for voltaic bombardment
        private static void CreateLightning()
        {
            lightningPrefab = CloneProjectilePrefab("MageLightningBombProjectile", "Lightning");
            lightningPrefab.GetComponent<Rigidbody>().useGravity = false;
            AmpPlugin.Destroy(lightningPrefab.GetComponent<AntiGravityForce>());
            AmpPlugin.Destroy(lightningPrefab.GetComponent<ProjectileProximityBeamController>());
            AmpPlugin.Destroy(lightningPrefab.GetComponent<ProjectileImpactExplosion>());

           lightningPrefab.AddComponent<ProjectileImpactExplosion>();
           ProjectileImpactExplosion lightningExplosion = lightningPrefab.GetComponent<ProjectileImpactExplosion>();

            lightningExplosion.blastRadius = 10f;
            lightningExplosion.destroyOnEnemy = false;
            lightningExplosion.lifetimeAfterImpact = 0f;
            lightningExplosion.impactEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact");



        }

        private static void InitializeFerroshotContact(ProjectileSingleTargetImpact ferroshotContact)
        {

        }

     

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.explosionSoundString = "";
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeExpiredSoundString = "";
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}