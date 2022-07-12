using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Audio;


namespace AmpMod.SkillStates
{
    class HeatShock : BaseSkillState
    {

        private float duration = .75f;
        protected string hitboxName = "SpinSlash";
        public OverlapAttack attack;
        protected DamageType damageType = DamageType.IgniteOnHit;
        protected float damageCoefficient = Modules.StaticValues.spinSlashDamageCoefficient;
        protected float procCoefficient = 1f;
        protected GameObject swingEffectPrefab = Modules.Assets.heatSwing;
        protected GameObject hitEffectPrefab = Modules.Assets.heatHit;
        protected float stopwatch;
        private ChildLocator childLocator;
        protected Animator animator;
        private DamageTrail fireTrail;
        private float fireDamageCoefficient = Modules.StaticValues.fireTrailTickDamageCoefficient;
        protected float pushForce = 300f;
        protected Vector3 bonusForce = Vector3.zero;
        protected NetworkSoundEventIndex impactSound = Modules.Assets.heatShockHitSoundEvent.index;
        private bool hasFired = false;
        private string chargeSoundString = Modules.StaticValues.heatChargeString;
        public uint stopChargeSound;
        protected string swingSoundString = Modules.StaticValues.heatSwingString;
        private String muzzleString = "HeatSwing";
        private bool inGroundedState;


        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            animator.SetBool("isUsingIndependentSkill", true);

            //Util.PlayAttackSpeedSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            childLocator = modelTransform.GetComponent<ChildLocator>();
            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
            }

            if (isGrounded)
            {
                inGroundedState = true;
                base.PlayAnimation("FullBody, Override", "SpinningSlash", null, this.duration);
                stopChargeSound = Util.PlaySound(chargeSoundString, base.gameObject);
                this.attack = new OverlapAttack();
                this.attack.damageType = this.damageType;
                this.attack.attacker = base.gameObject;
                this.attack.inflictor = base.gameObject;
                this.attack.teamIndex = base.GetTeam();
                this.attack.damage = this.damageCoefficient * base.characterBody.damage;
                this.attack.procCoefficient = this.procCoefficient;
                this.attack.hitEffectPrefab = this.hitEffectPrefab;
                this.attack.forceVector = this.bonusForce;
                this.attack.pushAwayForce = this.pushForce;
                this.attack.hitBoxGroup = hitBoxGroup;
                this.attack.isCrit = base.RollCrit();
                this.attack.impactSound = this.impactSound;
            }

            else if (!isGrounded)
            {


            }
      


        }

        private void FireGroundAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
               
                AkSoundEngine.StopPlayingID(stopChargeSound, 0);
                //Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);
                Util.PlaySound(swingSoundString, base.gameObject);
                if (base.isAuthority)
                {
                    this.PlaySwingEffect();
                    
                }
            }

            if (base.isAuthority)
            {
                if (this.attack.Fire())
                {

                }
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (inGroundedState)
            {
                //fireTrail.transform.position = childLocator.FindChild("SwordPlace").position;
                if (fixedAge > .5f && !hasFired)
                {
                    FireGroundAttack();
                    hasFired = true;
                    Debug.Log("firing");
                }
                if (fixedAge >= duration)
                {
                    this.outer.SetNextStateToMain();
                }
            }
      

        }
        protected void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
        }


        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("isUsingIndependentSkill", false);
        }
    }
}
