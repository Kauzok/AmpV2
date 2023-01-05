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
        private float radius = Modules.StaticValues.chargeBeamRadius;

        private float minDamageCoefficient = Modules.StaticValues.chargeBeamMinDamageCoefficient;
        private float maxDamageCoefficient = Modules.StaticValues.chargeBeamMaxDamageCoefficient;
        private float additionalPierceDamageCoefficient = StaticValues.additionalPierceDamageCoefficient;
        public float charge;
        private float maxForce = 2000f;
        private float baseDamage;

        private GameObject beamPrefab = Assets.chargeBeamTracerPrefab;

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

        private void ModifyBullet(BulletAttack bulletAttack)
        {
            //bulletAttack.sniper = true;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;

            bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
            {
                _bulletAttack.damage += (baseDamage * this.additionalPierceDamageCoefficient); 
               
            };
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
                    tracerEffectPrefab = Assets.chargeBeamTracerPrefab,
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
                baseDamage = calcedDamage * base.characterBody.damage;
                ModifyBullet(beamAttack);
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
