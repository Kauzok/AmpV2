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

namespace AmpMod.SkillStates.Nemesis_Amp
{
     class LightningStream : BaseSkillState
    {

        private float lightningTickDamage = Modules.StaticValues.lightningStreamTickDamageCoefficient;
        private StackDamageController stackDamageController;
        private float charge;
        private NemAmpLightningTracker tracker;
        private HurtBox targetHurtbox;
        private Animator animator;
        private ChildLocator childLocator;
        private float baseTickTime = .25f;
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

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
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

            lightningOrb.AddModdedDamageType(Modules.DamageTypes.controlledChargeProc);
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
            this.FireLightning();
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
                damageValue = lightningTickDamage * damageStat,
                isCrit = base.characterBody.RollCrit(),
                bouncesRemaining = 1,
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = gameObject,
                procCoefficient = 1f,
                lightningType = LightningOrb.LightningType.Loader,
                damageColorIndex = DamageColorIndex.Default,
                range = 10,
                speed = -1,
                target = targetHurtbox,
            };
        }

        public override void OnExit()
        {
            base.OnExit();

            this.FireLightning();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.targetHurtbox));
        }

        // Token: 0x06000E60 RID: 3680 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
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
