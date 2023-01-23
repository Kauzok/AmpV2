using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using EntityStates;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class FireChargeSlash : BaseSkillState
    {
        [SerializeField]
        public float baseDuration = .3f;
        private float duration;
        public float charge;
        private float surgeBuffCount;
        private bool hasFired;
        public OverlapAttack attack;
        private StackDamageController stackDamageController;
        private float minDamageCoefficient = Modules.StaticValues.minSlashDamageCoefficient;
        private float maxDamageCoefficient = Modules.StaticValues.maxSlashDamageCoefficient;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();

            this.duration = this.baseDuration / this.attackSpeedStat;
            surgeBuffCount = base.GetBuffCount(Buffs.damageGrowth);
            this.Fire();

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && !hasFired) {
                Fire();
            }
            if (base.isAuthority && base.fixedAge >= this.duration && hasFired)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private void PlayFireAnimation()
        {

        }

        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                float calcedDamage = Util.Remap(this.charge, 0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
                //Debug.Log("calced damage is " + calcedDamage);
                //Debug.Log("firing");
                float slashDamage = (StaticValues.growthDamageCoefficient * surgeBuffCount * calcedDamage) + calcedDamage;


            }
        }


    }
}
