using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Projectile;
using RoR2;
using AmpMod.Modules;

namespace AmpMod.Modules
{
    internal class AmpLightningController : MonoBehaviour
    {
        public bool isRed;
        private CharacterBody body;
        private CharacterModel model;

        public GameObject chargeOrb;
        public GameObject chargeOrbFull;
        public GameObject surgeEnter;
        public GameObject fulminationEffect;
        public GameObject fulminationHitEffect;
        public GameObject lorentzProjectile;
        public GameObject fulminationChainEffect;
        public GameObject vortexMuzzle;
        public GameObject vortexProjectile;
        public GameObject chargeExplosion;
        public GameObject surgeVehicle;
        public GameObject swingEffect;
        public GameObject swingHitEffect;
        public GameObject surgeExitEffect;
        public GameObject bombardmentEffect;
        public GameObject bombardmentMuzzleEffect;
        public GameObject pulseLeapEffect;
        public GameObject pulseMuzzleEffect;
        public GameObject swordSparks;



        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();
           

        }
        private void Start()
        {
            isRed = this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken == AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY_SKIN_NAME" && !Config.RedSpriteBlueLightning.Value;

            if (isRed) {
                surgeVehicle = Assets.boltVehicle;
                surgeVehicle.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial = Assets.matRedLightning;
                Light light = surgeVehicle.GetComponentInChildren<Light>();
                light.color = new Color32(191, 2, 0, 255);
  
                surgeEnter = Assets.boltEnterEffectRed;
                surgeExitEffect = Assets.boltExitEffectRed;

                //vortexMuzzle = Assets.vortexMuzzleEffectRed;
                vortexMuzzle = Assets.vortexMuzzleEffect;

                lorentzProjectile = Projectiles.ferroshotPrefab;
                lorentzProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<TrailRenderer>().SetMaterial(Assets.matRedTrail);
                
                fulminationEffect = Assets.electricStreamEffectRed;
                fulminationHitEffect = Assets.electricImpactEffectRed;
                fulminationChainEffect = Assets.electricChainEffectRed;

                chargeOrb = Assets.chargeOrbRedObject;
                chargeOrbFull = Assets.chargeOrbFullRedObject;
                chargeExplosion = Assets.chargeExplosionEffectRed;
                swingEffect = Assets.swordSwingEffectRed;
                swingHitEffect = Assets.swordHitImpactEffectRed;

                pulseLeapEffect = Assets.pulseBlastEffectRed;
                pulseMuzzleEffect = Assets.pulseMuzzleEffectRed;

                bombardmentMuzzleEffect = Assets.lightningMuzzleChargePrefabRed;
                bombardmentEffect = Assets.lightningStrikePrefabRed;


            } 
            else
            {
                surgeVehicle = Assets.boltVehicle;
                surgeVehicle.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial = Assets.matBlueLightning;
                Light light = surgeVehicle.GetComponentInChildren<Light>();
                light.color = new Color32(16, 192, 255, 0);
                surgeEnter = Assets.boltEnterEffect;
                surgeExitEffect = Assets.boltExitEffect;

                lorentzProjectile = Projectiles.ferroshotPrefab;
                lorentzProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<TrailRenderer>().SetMaterial(Assets.matBlueTrail);

                vortexMuzzle = Assets.vortexMuzzleEffect;

                fulminationEffect = Assets.electricStreamEffect;
                fulminationHitEffect = Assets.electricImpactEffect;
                fulminationChainEffect = Assets.electricChainEffect;

                chargeOrb = Assets.chargeOrbObject;
                chargeOrbFull = Assets.chargeOrbFullObject;
                chargeExplosion = Assets.chargeExplosionEffect;
                swingEffect = Assets.swordSwingEffect;
                swingHitEffect = Assets.swordHitImpactEffect;

                pulseLeapEffect = Assets.pulseBlastEffect;
                pulseMuzzleEffect = Assets.pulseMuzzleEffect;

                bombardmentMuzzleEffect = Assets.lightningMuzzleChargePrefab;
                bombardmentEffect = Assets.lightningStrikePrefab;


            }

        }
    }
}
