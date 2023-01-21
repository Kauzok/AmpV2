using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2;
using AmpMod.Modules;
using RoR2.Projectile;

namespace AmpMod.SkillStates
{
    class Vortex : BaseSkillState
    {
        public static float tickDamage = Modules.StaticValues.vortexDamageCoefficient;
        public GameObject Vortexprefab = Modules.Projectiles.vortexPrefab;
        public float explodeDamage = Modules.StaticValues.vortexExplosionCoefficient;
        public Vector3 blastPosition;
        private AmpLightningController lightningController;
        private string ShootString = Modules.StaticValues.vortexShootString;
        private Animator animator;
        private GameObject muzzleEffectPrefab;
        private string chargeString = Modules.StaticValues.vortexChargeString;
        private uint stopCharge;
        private ChildLocator childLocator;
        private Transform leftMuzzleTransform;
        private float baseChargeTime = .6f;
        private float baseDuration = 1.2f;
        private float chargeTime;
        private float duration;
        private bool hasMuzzleEffect;
        private bool hasFired;
        private Transform fireMuzzleTransform;

        public override void OnEnter()
        {
            base.OnEnter();

            lightningController = base.GetComponent<AmpLightningController>();

            muzzleEffectPrefab = lightningController.vortexMuzzle;
            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            animator = base.GetModelAnimator();
            duration = baseDuration / this.attackSpeedStat;
            chargeTime = baseChargeTime / this.attackSpeedStat;
            base.PlayAnimation("Gesture, Override", "LaunchVortex", "BaseSkill.playbackRate", duration);
            animator.SetBool("isUsingIndependentSkill", true);
            stopCharge = Util.PlaySound(chargeString, base.gameObject);
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.leftMuzzleTransform = this.childLocator.FindChild("HandL");

            }



        }

       

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasMuzzleEffect)
            {
                hasMuzzleEffect = true;
                if (this.childLocator)
                {
                    {
                         fireMuzzleTransform = UnityEngine.Object.Instantiate<GameObject>(muzzleEffectPrefab, leftMuzzleTransform).transform;
                         //Debug.Log("Spawning Muzzle Effect");

                    }

                }
            }


            if (fixedAge >= chargeTime && base.isAuthority && !hasFired)
            {
                Fire();
                hasFired = true;
            }

            if (fixedAge >= duration)
            {
                this.outer.SetNextStateToMain();
            }
            
        }

        //fire vortex projectile
        private void Fire()
        {
            AkSoundEngine.StopPlayingID(stopCharge, 0);
            if (fireMuzzleTransform)
            {
                EntityState.Destroy(fireMuzzleTransform.gameObject);
            }


            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                if (Vortexprefab != null)
                {
                    
                  
                     //set vars for the radial damage component of the vortex blackhole prefab
                    var vortexDamage = Modules.Assets.vortexBlackholePrefab.GetComponent<RadialDamage>();
                    //vortexDamage.radius = 5f;
                    vortexDamage.duration = Modules.Assets.vortexBlackholePrefab.GetComponent<ProjectileSimple>().lifetime;

                   // Debug.Log(base.gameObject);
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = Vortexprefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = 0f,
                        force = Vortexprefab.GetComponent<ProjectileSimple>().desiredForwardSpeed,
                        crit = base.RollCrit()
                    };
                    ModifyProjectile(ref fireProjectileInfo);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }

                //play shoot sound
                Util.PlaySound(ShootString, base.gameObject);
                
            }
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit()
        {

            if (fireMuzzleTransform)
            {
                EntityState.Destroy(fireMuzzleTransform.gameObject);
            }

            //base.PlayAnimation("Gesture, Override", "BufferEmpty");
            animator.SetBool("isUsingIndependentSkill", false);
            base.OnExit();
        }

    }

}
