using AmpMod.SkillStates.BaseStates;
using RoR2;
using R2API;
using AmpMod;
using UnityEngine;
using RoR2.WwiseUtils;

namespace AmpMod.SkillStates
{
    

    public class SlashCombo : BaseMeleeAttack
    {
        float chargeProc = 100f;
       

        public override void OnEnter()
        {
            var lightningController = base.GetComponent<Modules.AmpLightningController>();

            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.stormbladeDamageCoefficient;
            this.procCoefficient = .1f; 
            this.pushForce = 300f;
            this.bonusForce = Vector3.zero;
            this.baseDuration = .8f;//.96f;
            this.attackStartTime = .2f;//.24f;
            this.attackEndTime = 0.4f;//.48f;
            this.baseEarlyExitTime = 0.4f;//.48f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 4f;
            this.swingSoundString = Modules.StaticValues.stormbladeSwing1String;
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            this.swingEffectPrefab = lightningController.swingEffect;
            this.hitEffectPrefab = lightningController.swingHitEffect;
            this.impactSound = Modules.Assets.stormbladeHitSoundEvent.index;
            //this.hitStopDuration = EntityStates.Merc.GroundLight.hitPauseDuration;

        

            base.OnEnter();
        }

        //code for adding a chance of applying the charge debuff; percent chance is set with chargeProc var
   

        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
           
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

       
      


        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new SlashCombo
            {
                swingIndex = index
            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}