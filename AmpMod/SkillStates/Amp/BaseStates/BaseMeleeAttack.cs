﻿using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Orbs;
using R2API;
using AmpMod.Modules;
using AmpMod.SkillStates.Amp.BaseStates;
using System.Collections.Generic;

namespace AmpMod.SkillStates.BaseStates
{
    public class BaseMeleeAttack : BaseReplenishingSkill
    {
        public int swingIndex;

        protected string hitboxName = "Sword";

        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = 3.5f;
        protected float procCoefficient = 1f;
        protected float chargeProc = Modules.StaticValues.stormbladeChargeProcCoefficient;
        protected float pushForce = 300f;
        protected Vector3 bonusForce = Vector3.zero;
        protected float baseDuration = 1f;
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected float baseEarlyExitTime = 0.4f;
        protected float hitStopDuration = 0.012f;
        protected float attackRecoil = 0.75f;
        protected float hitHopVelocity = 4f;
        private List<HurtBox> results;
        private bool inCombo;
        protected bool cancelled = false;

        private bool hasStruck;

        protected string swingSoundString = "";
        protected string hitSoundString = "";
        protected string muzzleString = "SwingCenter";
        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;

        private float earlyExitTime;
        public float duration;
        private bool hasFired;
        private float hitPauseTimer;
        public OverlapAttack attack;
        protected bool inHitPause;
        private bool hasHopped;
        protected float stopwatch;
        protected Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();

           
            if (this.animator.GetBool("isUsingIndependentSkill") == true)
            {
                hasFired = true;
                base.OnExit();

            }

            else
            {
                this.duration = this.baseDuration / this.attackSpeedStat;
                this.earlyExitTime = this.baseEarlyExitTime / this.attackSpeedStat;
                this.hasFired = false;
                
                base.StartAimMode(0.5f + this.duration, false);
                base.characterBody.outOfCombatStopwatch = 0f;
                this.animator.SetBool("attacking", true);

                HitBoxGroup hitBoxGroup = null;
                Transform modelTransform = base.GetModelTransform();

                if (modelTransform)
                {
                    hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
                }


                this.PlayAttackAnimation();

                this.attack = new OverlapAttack();
                this.attack.damageType = this.damageType;
                this.attack.attacker = base.gameObject;
                this.attack.inflictor = base.gameObject;
                this.attack.teamIndex = base.GetTeam();
                this.attack.damage = this.damageCoefficient * this.damageStat;
                this.attack.procCoefficient = this.procCoefficient;
                this.attack.hitEffectPrefab = this.hitEffectPrefab;
                this.attack.forceVector = this.bonusForce;
                this.attack.pushAwayForce = this.pushForce;
                this.attack.hitBoxGroup = hitBoxGroup;
                this.attack.isCrit = base.RollCrit();
                this.attack.impactSound = this.impactSound;

            }
        }

        protected void chargeChance(float chance, OverlapAttack attack)
        {
            if (base.isAuthority && base.characterBody)
            {
                if (Util.CheckRoll(chance, base.characterBody.master))
                {
                    attack.AddModdedDamageType(DamageTypes.applyCharge);
                }
            }
            
       
        }


        protected virtual void PlayAttackAnimation()
        {

            if (this.animator.GetBool("isFulminating"))
            {
                base.PlayCrossfade("Gesture, Override", "FulminateSlash" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
            }
            else
            {
                base.PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
                /* if (this.inCombo)
               {
                   if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) base.PlayCrossfade("FullBody, Override", "Slash1Combo", "Slash.playbackRate", this.duration, 0.05f);
                   base.PlayCrossfade("Gesture, Override", "Slash1Combo", "Slash.playbackRate", this.duration, 0.05f);
               }
               else 
               {
                   if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) base.PlayCrossfade("FullBody, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
                   base.PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), "Slash.playbackRate", this.duration, 0.05f);
               } */

            }
          



        }

        public override void OnExit()
        {
            if (!this.hasFired && !this.cancelled) this.FireAttack();

            base.OnExit();

            this.animator.SetBool("attacking", false);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
        }

        protected virtual void OnHitEnemyAuthority()
        {
            Util.PlaySound(this.hitSoundString, base.gameObject);
            if (!this.hasStruck && base.characterBody.healthComponent.shield >= base.characterBody.healthComponent.fullShield)
            {
                hasStruck = true;
                var damage = Modules.StaticValues.lightningBombDamageCoefficient * this.damageCoefficient * damageStat;
                
                if (attack.lastFireAverageHitPosition != null)
                {
                    this.FireLightningBall(1, damage, attack.lastFireAverageHitPosition);
                }

                else
                {
                    this.FireLightningBall(1, damage, base.characterBody.corePosition + base.characterDirection.forward * 2);
                }
                
            }
            if (!this.hasHopped)
            {
                if (base.characterMotor && !base.characterMotor.isGrounded && this.hitHopVelocity > 0f)
                {
                    base.SmallHop(base.characterMotor, this.hitHopVelocity);
                }

                this.hasHopped = true;
            }

            if (!this.inHitPause && this.hitStopDuration > 0f)
            {
                this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }

        private void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);

                if (base.isAuthority)
                {
                    this.PlaySwingEffect();
                    base.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                }
            }

            if (base.isAuthority)
            {
                chargeChance(Modules.StaticValues.stormbladeChargeProcCoefficient, this.attack);
                attack.AddModdedDamageType(DamageTypes.healShield);
                if (this.attack.Fire())
                {
                    this.OnHitEnemyAuthority();
                }
            }
        }

        protected virtual void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new BaseMeleeAttack
            {
                swingIndex = index
            });
            inCombo = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.characterBody.currentVehicle)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                base.characterMotor.velocity = this.storedVelocity;
            }
        
            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Swing.playbackRate", 0f);
            }

            if (this.stopwatch >= (this.duration * this.attackStartTime) && this.stopwatch <= (this.duration * this.attackEndTime))
            {
                this.FireAttack();
            }

            if (this.stopwatch >= (this.duration - this.earlyExitTime) && base.isAuthority)
            {
                if (base.inputBank.skill1.down)
                {
                    if (!this.hasFired) this.FireAttack();
                    this.SetNextState();
                    return;
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}