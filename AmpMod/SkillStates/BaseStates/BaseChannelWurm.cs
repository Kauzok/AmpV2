using AmpMod.SkillStates.Amp;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Skills;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;

namespace AmpMod.SkillStates
{
    public abstract class BaseChannelWurm : BaseSkillState
    {
        protected abstract SummonWurm GetNextState();
        private ChildLocator childLocator;
        private Animator animator;
        private float channelDuration = 3f;
        private bool hasBegunChannelling;
        private bool hasChannelled;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = base.GetModelAnimator();
            animator.SetBool("IsChannelling", true);

            base.PlayAnimation("Worm, Override", "WormChannelStart", "LorentzCannon.Playbackrate", 1.4f);

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);


        }


        


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasBegunChannelling)
            {
                hasBegunChannelling = true;
                base.PlayAnimation("Worm, Override", "WormChannel", "LorentzCannon.Playbackrate", channelDuration);

            }
            if (base.inputBank.skill1.justPressed && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
            if (fixedAge >= channelDuration && base.isAuthority)    
            {
                SummonWurm nextState = this.GetNextState();
                animator.SetBool("HasChannelled", true);
                // this.outer.SetNextState(summonState);
                //base.PlayAnimation("Worm, Override", "BufferEmpty", null, channelDuration);
                this.outer.SetNextState(nextState);
                hasBegunChannelling = false;
                hasChannelled = true;
            }


        }



        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("IsChannelling", false);
            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            if (!hasChannelled)
            {
                RefundCooldown();
            }
            

        }

        private void RefundCooldown()
        {
            base.activatorSkillSlot.rechargeStopwatch = (0.9f * base.activatorSkillSlot.finalRechargeInterval);
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }


    }

}

