using AncientScepter;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace AmpMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject ferroshotPrefab;
        internal static GameObject lightningPrefab;

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
           //CreateBomb();
            CreateFerroshot();
          //  CreateLightning();

         //  AddProjectile(bombPrefab);
            AddProjectile(ferroshotPrefab);
          //  AddProjectile(lightningPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }

        //instantiates ferroshot/Lorentz Cannon projectile
        private static void CreateFerroshot()
        {
           ferroshotPrefab = CloneProjectilePrefab("LunarShardProjectile", "Ferroshot");


            //change damagetype of ferroshot to generic
            ProjectileDamage ferroshotDamage = ferroshotPrefab.GetComponent<ProjectileDamage>();
            ferroshotDamage.damageType = DamageType.Generic;

            //remove/nullify components from lunarshard that are unnecessary, such as the tracker and on impact explosion
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileImpactExplosion>());
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileProximityBeamController>());
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileSteerTowardTarget>());
            ferroshotPrefab.GetComponent<Rigidbody>().useGravity = false;
            AmpPlugin.Destroy(ferroshotPrefab.GetComponent<ParticleSystem>());


            ProjectileController ferroshotController = ferroshotPrefab.GetComponent<ProjectileController>();
            //instantiates the projectile model and associates it with the prefab
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("SpikeGhost") != null) ferroshotController.ghostPrefab = CreateGhostPrefab("SpikeGhost");
            
  

            //makes ferroshot destroy itself on contact with other entities, + adds impact effect
            ProjectileSingleTargetImpact ferroshotContact = ferroshotPrefab.AddComponent<ProjectileSingleTargetImpact>();
            InitializeFerroshotContact(ferroshotContact);
            ferroshotContact.destroyOnWorld = true;
            ferroshotContact.impactEffect = Modules.Assets.bulletImpactEffect;
            

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
            lightningExplosion.impactEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact");



        }

        private static void InitializeFerroshotContact(ProjectileSingleTargetImpact ferroshotContact)
        {

        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
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
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}