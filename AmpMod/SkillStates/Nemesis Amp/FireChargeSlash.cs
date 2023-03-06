using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using R2API;
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
        private float procCoefficient = 1f;
        private StackDamageController stackDamageController;
        private float minDamageCoefficient = Modules.StaticValues.minSlashDamageCoefficient;
        private float maxDamageCoefficient = Modules.StaticValues.maxSlashDamageCoefficient;
        private ChildLocator childLocator;
        private HitBoxGroup hitBoxGroup;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();

            this.duration = this.baseDuration / this.attackSpeedStat;
            surgeBuffCount = base.GetBuffCount(Buffs.damageGrowth);
            Transform modelTransform = base.GetModelTransform();

            childLocator = modelTransform.GetComponent<ChildLocator>();
            if (modelTransform)
            {
                //hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                //this.swordMuzzle = this.childLocator.FindChild("SwordPlace");
            }

            base.PlayAnimation("Gesture, Override", "FireSlash", "BaseSkill.playbackRate", this.duration);

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
                float calcedRange = Util.Remap(this.charge, 0f, 1f, 0f, 1f);
                //Debug.Log("calced damage is " + calcedDamage);
                //Debug.Log("firing");
                float slashDamage = (StaticValues.growthDamageCoefficient * surgeBuffCount * calcedDamage) + calcedDamage;

                this.attack = new OverlapAttack();
                this.attack.damageType = DamageType.Stun1s;
                this.attack.attacker = base.gameObject;
                this.attack.inflictor = base.gameObject;
                this.attack.teamIndex = base.GetTeam();
                this.attack.damage = slashDamage * base.characterBody.damage;
                this.attack.procCoefficient = this.procCoefficient;
                //this.attack.hitEffectPrefab = this.hitEffectPrefab;
                //this.attack.forceVector = 0f;
                this.attack.pushAwayForce = 0f;
                this.attack.hitBoxGroup = hitBoxGroup;
                this.attack.isCrit = base.RollCrit();
                //this.attack.impactSound = this.impactSound;
                attack.AddModdedDamageType(Modules.DamageTypes.controlledChargeProc);




            }
        }


    }
}
