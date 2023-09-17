using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Projectile;
using RoR2;
using AmpMod;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    public class NemLightningColorController : MonoBehaviour
    {
        public bool isBlue;
        private CharacterBody body;
        private CharacterModel model;

        [Header("Death/Spawn Effects")]
        public GameObject deathExplosionVFX;
        public Material deathOverlay;
        public GameObject intialSpawnVFX;
        public GameObject spawnRingVFX;

        [Header("Passive Effects")]
        public GameObject buffOnVFX;
        public GameObject buffOffVFX;
        public Material buffOverlayMat;

        [Header("Fulmination Effects")]
        public GameObject streamVFX;
        public GameObject streamMuzzleFlashVFX;
        public GameObject streamMuzzleVFX;
        public GameObject streamChainVFX;
        public GameObject streamImpactVFX;
        public GameObject streamReticlePrefab;

        [Header("Lorentz Blades Effects")]
        public GameObject bladePrepVFX;
        public GameObject bladePrefab;
        public GameObject bladeFireVFX;

        [Header("Furious Spark Effects")]
        public GameObject beamMuzzleVFX;
        public GameObject beamChargeVFX;
        public GameObject beamObject;

        [Header("Static Field Effects")]
        public GameObject fieldPrefab;
        public GameObject fieldMuzzleVFX;
        public GameObject fieldAimVFX;

        [Header("Quicksurge Effects")]
        public GameObject dashEnterExitVFX;
        public GameObject lightningBallPrefab;
        public GameObject lightningBallMuzzleVFX;
        public GameObject dashPrefab;

        [Header("Voltaic Onslaught Effects")]
        public GameObject specialBoltVFX;
        public GameObject specialMuzzleVFX;



        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();


        }

        private void Start()
        {
            isBlue = this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME"; // && !Config.RedSpriteBlueLightning.Value;
            //Debug.Log(isBlue);
            if (isBlue)
            {
                #region Death
                deathExplosionVFX = Assets.deathExplosionEffectBlue;
                deathOverlay = Assets.matDeathOverlayBlue;
                #endregion

                #region Gathering Storm
                buffOffVFX = Assets.maxBuffOffEffectBlue;
                buffOnVFX = Assets.maxBuffFlashEffectBlue;
                buffOverlayMat = Assets.buffOverlayMatBlue;
                #endregion

                #region Fulmination
                streamVFX = Assets.lightningStreamEffectBlue;
                streamMuzzleFlashVFX = Assets.lightningStreamMuzzleFlashBlue;
                streamMuzzleVFX = Assets.lightningStreamMuzzleEffectBlue;
                streamChainVFX = Assets.lightningStreamChainEffectPrefabBlue;
                streamReticlePrefab = Assets.lightningCrosshairBlue;
                streamImpactVFX = Assets.lightningStreamImpactEffectBlue;
                #endregion

                #region Lorentz Blades
                #endregion
                bladePrepVFX = Assets.bladePrepObjectBlue;
                bladeFireVFX = Assets.bladeFireEffectBlue;
                bladePrefab = Projectiles.bladeProjectilePrefab;
                bladePrefab.GetComponent<ProjectileController>().ghostPrefab = Projectiles.bladeProjectileGhostBlue;
                bladePrefab.GetComponent<ProjectileSimple>().lifetimeExpiredEffect = Assets.bladeExpireEffectBlue;

                #region Furious Spark
                beamMuzzleVFX = Assets.beamMuzzleFlashEffectBlue;
                beamChargeVFX = Assets.chargeBeamMuzzleEffectBlue;
                beamObject = Assets.chargeBeamTracerPrefabBlue;
                #endregion

                #region Static Field
                fieldAimVFX = Assets.aimFieldMuzzleEffectBlue;
                fieldMuzzleVFX = Assets.releaseFieldMuzzleEffectBlue;
                fieldPrefab = Projectiles.fieldProjectilePrefabBlue;
                #endregion

                #region Voidsurge
                dashEnterExitVFX = Assets.dashEnterEffectBlue;
                dashPrefab = Assets.dashVFXPrefabBlue;
                lightningBallPrefab = Projectiles.lightningBallPrefab;
                lightningBallPrefab.GetComponent<ProjectileController>().ghostPrefab = Projectiles.lightningBallGhostBlue;
                lightningBallMuzzleVFX = Assets.lightningBallMuzzleFlashEffectBlue;
                lightningBallPrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.lightningBallExplosionEffectBlue;
                #endregion

                #region Voltaic Onslaught
                specialMuzzleVFX = Assets.stormMuzzleFlashEffectBlue;
                specialBoltVFX = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/SimpleLightningStrikeImpact");
                #endregion
            }
            else
            {
                #region Death
                deathExplosionVFX = Assets.deathExplosionEffect;
                deathOverlay = Assets.matDeathOverlay;
                #endregion

                #region Gathering Storm
                buffOffVFX = Assets.maxBuffOffEffect;
                buffOnVFX = Assets.maxBuffFlashEffect;
                buffOverlayMat = Assets.buffOverlayMat;
                #endregion

                #region Fulmination
                streamVFX = Assets.lightningStreamEffect;
                streamMuzzleFlashVFX = Assets.lightningStreamMuzzleFlash;
                streamMuzzleVFX = Assets.lightningStreamMuzzleEffect;
                streamChainVFX = Assets.lightningStreamChainEffectPrefab;
                streamReticlePrefab = Assets.lightningCrosshair;
                streamImpactVFX = Assets.lightningStreamImpactEffect;
                #endregion

                #region Lorentz Blades
                #endregion
                bladePrepVFX = Assets.bladePrepObject;
                bladeFireVFX = Assets.bladeFireEffect;
                bladePrefab = Projectiles.bladeProjectilePrefab;
                bladePrefab.GetComponent<ProjectileController>().ghostPrefab = Projectiles.bladeProjectileGhost;
                bladePrefab.GetComponent<ProjectileSimple>().lifetimeExpiredEffect = Assets.bladeExpireEffect;

                #region Furious Spark
                beamMuzzleVFX = Assets.beamMuzzleFlashEffect;
                beamChargeVFX = Assets.chargeBeamMuzzleEffect;
                beamObject = Assets.chargeBeamTracerPrefab;
                #endregion

                #region Static Field
                fieldAimVFX = Assets.aimFieldMuzzleEffect;
                fieldMuzzleVFX = Assets.releaseFieldMuzzleEffect;
                fieldPrefab = Projectiles.fieldProjectilePrefab;
                #endregion

                #region Voidsurge
                dashEnterExitVFX = Assets.dashEnterEffect;
                dashPrefab = Assets.dashVFXPrefab;
                lightningBallPrefab = Projectiles.lightningBallPrefab;
                lightningBallPrefab.GetComponent<ProjectileController>().ghostPrefab = Projectiles.lightningBallGhost;
                lightningBallMuzzleVFX = Assets.lightningBallMuzzleFlashEffect;
                lightningBallPrefab.GetComponent<ProjectileImpactExplosion>().explosionEffect = Assets.lightningBallExplosionEffect;
                #endregion

                #region Voltaic Onslaught
                specialMuzzleVFX = Assets.stormMuzzleFlashEffect;
                specialBoltVFX = Assets.purpleStormBoltEffect;
                #endregion
            }
        }

    }
}

