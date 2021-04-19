using AncientScepter;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace HenryMod.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;
        internal static GameObject ferroshotPrefab;

        internal static void RegisterProjectiles()
        {
            // only separating into separate methods for my sanity
            CreateBomb();
            CreateFerroshot();

            AddProjectile(bombPrefab);
            AddProjectile(ferroshotPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Prefabs.projectilePrefabs.Add(projectileToAdd);
        }
        private static void CreateFerroshot()
        {
            ferroshotPrefab = CloneProjectilePrefab("LunarShardProjectile", "Ferroshot");

            ProjectileDamage ferroshotDamage = ferroshotPrefab.GetComponent<ProjectileDamage>();
            ferroshotPrefab.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            /* ProjectileImpactExplosion ferroshotImpactExplosion = ferroshotPrefab.GetComponent<ProjectileImpactExplosion>();
            ferroshotImpactExplosion.blastRadius = 10f; */

            HenryPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileImpactExplosion>());
            HenryPlugin.Destroy(ferroshotPrefab.GetComponent<ProjectileProximityBeamController>());
            

            ProjectileController ferroshotController = ferroshotPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("SpikeGhost") != null) ferroshotController.ghostPrefab = CreateGhostPrefab("SpikeGhost");

            ferroshotPrefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 0f;


            ProjectileSingleTargetImpact ferroshotContact = ferroshotPrefab.AddComponent<ProjectileSingleTargetImpact>();

            ferroshotPrefab.GetComponent<Rigidbody>().useGravity = false;

            InitializeFerroshotContact(ferroshotContact);
            ferroshotContact.destroyOnWorld = true;
            ferroshotContact.impactEffect = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab;

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