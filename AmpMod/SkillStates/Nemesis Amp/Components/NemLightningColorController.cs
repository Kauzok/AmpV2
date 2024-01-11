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
        public GameObject lightningStakePrefab;
        public GameObject lightningStakeMuzzleVFX;
        public GameObject chargeLightningStakeVFX;
        public GameObject lightningStakeFlashVFX;
        public GameObject dashPrefab;
        public GameObject dashPrimaryVFX;

        [Header("Voltaic Onslaught Effects")]
        public GameObject specialBoltVFX;
        public GameObject specialMuzzleVFX;

        [Header("Photon Shot Effects")]
        public GameObject specialBeamTracer;
        public GameObject specialBeamImpact;
        public GameObject specialBeamImpactDetonate;
        public GameObject specialBeamMuzzleFlash;


        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();


        }

        private void Start()
        {
            isBlue = this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME" && !Config.NemOriginPurpleLightning.Value;
            
            

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
                bladePrepVFX = Assets.bladePrepObjectBlue;
                bladeFireVFX = Assets.bladeFireEffectBlue;
                bladePrefab = Projectiles.bladeProjectilePrefabBlue;
                #endregion

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
                lightningStakePrefab = Projectiles.lightningStakePrefabBlue;
                lightningStakeMuzzleVFX = Assets.lightningBallMuzzleFlashEffectBlue;
                lightningStakeFlashVFX = Assets.lightningStakeFlashEffectBlue;
                chargeLightningStakeVFX = Assets.lightningStakeMuzzleObjectBlue;
                dashPrimaryVFX = Assets.plasmaActiveVFXBlue;
                #endregion

                #region Voltaic Onslaught
                specialMuzzleVFX = Assets.stormMuzzleFlashEffectBlue;
                specialBoltVFX = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/SimpleLightningStrikeImpact");
                #endregion

                #region Photon Shot 
                specialBeamImpact = Assets.photonImpactBlue;
                specialBeamImpactDetonate = Assets.photonImpactDetonateBlue;
                specialBeamMuzzleFlash = Assets.photonMuzzleFlashBlue;
                specialBeamTracer = Assets.photonTracerBlue;
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
                bladePrepVFX = Assets.bladePrepObject;
                bladeFireVFX = Assets.bladeFireEffect;
                bladePrefab = Projectiles.bladeProjectilePrefab;
                #endregion

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
                lightningStakePrefab = Projectiles.lightningStakePrefab;
                lightningStakeMuzzleVFX = Assets.lightningBallMuzzleFlashEffect;
                lightningStakeFlashVFX = Assets.lightningStakeFlashEffect;
                chargeLightningStakeVFX = Assets.lightningStakeMuzzleObject;
                dashPrimaryVFX = Assets.plasmaActiveVFX;
                #endregion

                #region Voltaic Onslaught
                specialMuzzleVFX = Assets.stormMuzzleFlashEffect;
                specialBoltVFX = Assets.purpleStormBoltEffect;
                #endregion

                #region Photon Shot 
                specialBeamImpact = Assets.photonImpact;
                specialBeamImpactDetonate = Assets.photonImpactDetonate;
                specialBeamMuzzleFlash = Assets.photonMuzzleFlash;
                specialBeamTracer = Assets.photonTracer;
                #endregion
            }
        }

    }
}

