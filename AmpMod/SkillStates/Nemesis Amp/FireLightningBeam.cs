using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;
using AmpMod.Modules;
using R2API;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FireLightningBeam : BaseState
    {
        [SerializeField]
        public float baseDuration = .3f;

        private float duration;
        private float radius = 2f;

        public float minDamageCoefficient = Modules.StaticValues.chargeBeamMinDamageCoefficient;
        public float maxDamageCoefficient = Modules.StaticValues.chargeBeamMaxDamageCoefficient;
        public float charge;
        private float maxForce = 2000f;

        [SerializeField]
        public float selfForce;

        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.PlayFireAnimation();
            this.Fire();
        }


        private void PlayFireAnimation()
        {

        }


        public override void FixedUpdate()
        {
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }

        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                float calcedDamage = Util.Remap(this.charge, 0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
                float num2 = this.charge * this.maxForce;
                //Debug.Log("calced damage is " + calcedDamage);
                //Debug.Log("firing");
                BulletAttack beamAttack = new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = calcedDamage * base.characterBody.damage,
                    force = num2,
                    //muzzleName = muzzleString,
                    //hitEffectPrefab = impactEffectPrefab,
                    isCrit = base.characterBody.RollCrit(),
                    radius = this.radius,
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = 1f,
                    maxDistance = 100f,
                    smartCollision = true,
                    damageType = DamageType.Generic
                };
                beamAttack.AddModdedDamageType(DamageTypes.controlledChargeProc);
                
                beamAttack.Fire();

                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-this.selfForce * this.charge), false, false);
                }
            }
        }


        public override void OnExit()
        {
            base.OnExit();
        }


	}
}
