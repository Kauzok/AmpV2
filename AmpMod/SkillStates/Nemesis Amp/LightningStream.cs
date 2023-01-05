using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;
using R2API;
using RoR2.Orbs;
using UnityEngine.Networking;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
     class LightningStream : BaseSkillState
    {

        private float lightningTickDamage = Modules.StaticValues.lightningStreamPerSecondDamageCoefficient / 3f;
        private StackDamageController stackDamageController;
        private float charge;
        private float procCoefficient = Modules.StaticValues.lightningStreamProcCoefficient;
        private NemAmpLightningTracker tracker;
        private HurtBox targetHurtbox;
        private Animator animator;
        private ChildLocator childLocator;
        private float baseTickTime = Modules.StaticValues.lightningStreamBaseTickTime;
        private float tickTime;
        private bool isCrit;
        private float tickTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();

            tracker = base.GetComponent<NemAmpLightningTracker>();
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.animator = modelTransform.GetComponent<Animator>();
            }
            if (this.tracker && base.isAuthority)
            {
                this.targetHurtbox = this.tracker.GetTrackingTarget();
            }

            //stackDamageController.newSkillUsed = this;
            //stackDamageController.resetComboTimer();

            this.tickTime= this.baseTickTime / this.attackSpeedStat;
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.baseTickTime + 1f);
            }

        }

        private void FireLightning()
        {
            if (!NetworkServer.active || tickTimer > 0f)
            {
                return;
            }
            this.tickTimer = this.tickTime;
            NemAmpLightningLockOrb lightningOrb = createOrb();

            if (Util.CheckRoll(procCoefficient * 100f, base.characterBody.master))
            {
                lightningOrb.AddModdedDamageType(Modules.DamageTypes.controlledChargeProc);
            }
            
            if (targetHurtbox)
            {
                //Transform transform = this.childLocator.FindChild(this.muzzleString);
                OrbManager.instance.AddOrb(lightningOrb);
            }

            
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.tickTimer -= Time.fixedDeltaTime;
            if (NetworkServer.active)
            {
                this.FireLightning();
            }
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
            if (base.isAuthority && base.fixedAge > this.tickTime)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private NemAmpLightningLockOrb createOrb()
        {
            return new NemAmpLightningLockOrb
            {
                origin = base.gameObject.transform.position,
                damageValue = lightningTickDamage * damageStat + (StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth)),
                isCrit = base.characterBody.RollCrit(),
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = .2f,
                lightningType = LightningOrb.LightningType.MageLightning,
                damageColorIndex = DamageColorIndex.Default,
                target = targetHurtbox,
            };
        }

        public override void OnExit()
        {
            base.OnExit();
            //Debug.Log("Exiting");
            this.FireLightning();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.targetHurtbox));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            this.targetHurtbox = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

   
    }
}
