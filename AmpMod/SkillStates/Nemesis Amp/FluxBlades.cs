using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;
using RoR2.Projectile;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FluxBlades : BaseSkillState
    {
        private float damageCoefficient = Modules.StaticValues.bladeDamageCoefficient;
        public GameObject bladePrefab = Modules.Projectiles.bladeProjectilePrefab;
        private Animator animator;
        private float baseChargeTime = .6f;
        private float baseDuration = .8f;
        private float chargeTime;
        private float duration;
        private float surgeBuffCount;
        private ChildLocator childLocator;
        private bool hasFired;
        private String soundString = StaticValues.fluxBladesFireString;
        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();

            stackDamageController = base.GetComponent<StackDamageController>();

            surgeBuffCount = base.GetBuffCount(Buffs.damageGrowth);

            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            animator = base.GetModelAnimator();
            duration = baseDuration / this.attackSpeedStat;
            chargeTime = baseChargeTime / this.attackSpeedStat;
            base.PlayAnimation("Gesture, Override", "LaunchVortex", "BaseSkill.playbackRate", duration);
            animator.SetBool("isUsingIndependentSkill", true);

            Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);

            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                //this.leftMuzzleTransform = this.childLocator.FindChild("HandL");

            }

        }
        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {

        }


        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                if (bladePrefab != null)
                {

                    float baseDamage = (StaticValues.growthDamageCoefficient * surgeBuffCount * this.damageCoefficient) + this.damageCoefficient;
                    // Debug.Log(base.gameObject);
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = bladePrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = base.characterBody.damage * baseDamage,
                        force = 120f,
                        crit = base.RollCrit()
                    };
                    
                    ModifyProjectile(ref fireProjectileInfo);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }

                //play shoot sound
                //Util.PlaySound(ShootString, base.gameObject);

            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && !hasFired)
            {
                Fire();
                //Debug.Log("firing");
                hasFired = true;
            }

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();


            if (fixedAge >= duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }
}
