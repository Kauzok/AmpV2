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
        private bool isRed;
        private CharacterBody body;
        private CharacterModel model;

        public GameObject surgeEnter;
        public GameObject fulminationEffect;
        public GameObject fulminationHitEffect;
        public GameObject lorentzProjectile;
        public GameObject fulminationChainEffect;
        public GameObject surgeEffect;
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

            if ((this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken) == AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY_SKIN_NAME")
            {
                isRed = true;
            }
            else
            {
                isRed = false;
            }
            if (!isRed)
            {
                surgeVehicle = Assets.boltVehicle;
                surgeVehicle.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial = Assets.matBlueLightning;
                Light light = surgeVehicle.GetComponentInChildren<Light>();
                light.color = new Color32(16, 192, 255, 0);
                surgeEnter = Assets.boltEnterEffect;

                lorentzProjectile = Projectiles.ferroshotPrefab;
                lorentzProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<TrailRenderer>().SetMaterial(Assets.matBlueTrail);

                fulminationEffect = Assets.electricStreamEffect;
                chargeExplosion = Assets.chargeExplosionEffect;
                pulseLeapEffect = Assets.pulseBlastEffect;
                pulseMuzzleEffect = Assets.pulseMuzzleEffect;
                swingEffect = Assets.swordSwingEffect;
                swingHitEffect = Assets.swordHitImpactEffect;
                bombardmentMuzzleEffect = Assets.lightningMuzzleChargePrefab;
                fulminationHitEffect = Assets.electricImpactEffect;
                fulminationChainEffect = Assets.electricChainEffect;

            }
            if (isRed)
            {
                surgeVehicle = Assets.boltVehicle;
                surgeVehicle.GetComponentInChildren<ParticleSystemRenderer>().trailMaterial = Assets.matRedLightning;
                Light light = surgeVehicle.GetComponentInChildren<Light>();
                light.color = new Color32(191, 2, 0, 255);
  
                surgeEnter = Assets.boltEnterEffectRed;

                lorentzProjectile = Projectiles.ferroshotPrefab;
                lorentzProjectile.GetComponent<ProjectileController>().ghostPrefab.GetComponentInChildren<TrailRenderer>().SetMaterial(Assets.matRedTrail);
                
                fulminationEffect = Assets.electricStreamEffectRed;
                chargeExplosion = Assets.chargeExplosionEffectRed;
                swingEffect = Assets.swordSwingEffectRed;
                pulseLeapEffect = Assets.pulseBlastEffectRed;
                pulseMuzzleEffect = Assets.pulseMuzzleEffectRed;
                swingHitEffect = Assets.swordHitImpactEffectRed;
                bombardmentMuzzleEffect = Assets.lightningMuzzleChargePrefabRed;
                fulminationHitEffect = Assets.electricImpactEffectRed;
                fulminationChainEffect = Assets.electricChainEffectRed;

            }
            
        }
    }
}
