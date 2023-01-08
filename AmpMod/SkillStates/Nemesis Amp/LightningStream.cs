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
using AmpMod.SkillStates.Nemesis_Amp.Orbs;

namespace AmpMod.SkillStates.Nemesis_Amp
{
     class LightningStream : BaseSkillState
    {

        private float lightningTickDamage = Modules.StaticValues.lightningStreamPerSecondDamageCoefficient / 3f;
        private StackDamageController stackDamageController;
        private float charge;
        private NemAmpLightningEffectController lightningEffectController;
        private float procCoefficient = Modules.StaticValues.lightningStreamProcCoefficient;
        private NemAmpLightningTracker tracker;
        private HurtBox targetHurtbox;
        private Animator animator;
        private ChildLocator childLocator;
        private float baseTickTime = Modules.StaticValues.lightningStreamBaseTickTime;
        private float tickTime;
        private bool isCrit;
        private bool lightningTetherActive;
        private float healthCheck;
        private float tickTimer;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            lightningEffectController = base.GetComponent<NemAmpLightningEffectController>();
            lightningEffectController.isAttacking = true;
            
            tracker = base.GetComponent<NemAmpLightningTracker>();
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.animator = modelTransform.GetComponent<Animator>();
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
            NemAmpLightningLockOrb lightningOrb = createDmgOrb();

            if (Util.CheckRoll(procCoefficient * 100f, base.characterBody.master))
            {
                lightningOrb.AddModdedDamageType(DamageTypes.controlledChargeProc);
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


            if (this.tracker && base.isAuthority)
            {
                this.targetHurtbox = this.tracker.GetTrackingTarget();
                if (targetHurtbox && !lightningTetherActive)
                {
                    lightningEffectController.CreateLightningTether(base.gameObject);
                    lightningTetherActive = true;
                }

            }

            this.FireLightning();
            
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();

            
            if ((base.isAuthority && !base.inputBank.skill1.down) || (base.isAuthority && this.targetHurtbox && this.targetHurtbox.healthComponent.health <= 0) || (base.isAuthority && !this.targetHurtbox))
            {
                this.outer.SetNextStateToMain();
            }
        }

        private NemAmpLightningLockOrb createDmgOrb()
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
                lightningType = LightningOrb.LightningType.Loader,
                damageColorIndex = DamageColorIndex.Default,
                target = targetHurtbox,
            };
        }

        public override void OnExit()
        {
            base.OnExit();
            lightningEffectController.isAttacking = false;
            //Debug.Log("Exiting");
            this.FireLightning();
            lightningEffectController.DestroyLightningTether();
            lightningTetherActive = true;
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
