﻿using AmpMod.SkillStates.BaseStates;
using RoR2;
using R2API;
using UnityEngine;
using RoR2.WwiseUtils;
using AmpMod.SkillStates.BaseStates;

namespace AmpMod.SkillStates
{
    

    public class SlashCombo : BaseMeleeAttack
    {
        float chargeProc = 100f;

        public override void OnEnter()
        {
            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.stormbladeDamageCoefficient;
            this.procCoefficient = .1f; //determines length of the shock -> need to figure out how to lower proc chance, should be 15%
            this.pushForce = 300f;
            this.bonusForce = Vector3.zero;
            this.baseDuration = .8f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 4f;
            this.swingSoundString = Modules.StaticValues.stormbladeSwing1String;
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;
            this.impactSound = Modules.Assets.stormbladeHitSoundEvent.index;

            //chargeChance(20f, this.attack);
            
            


            base.OnEnter();
        }

        //code for adding a chance of applying the charge debuff; percent chance is set with chargeProc var
      /*  c
      */

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