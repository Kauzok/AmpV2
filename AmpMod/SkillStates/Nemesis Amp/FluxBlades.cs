using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;
using RoR2.Projectile;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FluxBlades : BaseSkillState
    {
        public GameObject bladePrefab = null; // Modules.Projectiles.vortexPrefab;
        private Animator animator;
        private float baseChargeTime = .6f;
        private float baseDuration = 1f;
        private float chargeTime;
        private float duration;
        private ChildLocator childLocator;
        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();


            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            animator = base.GetModelAnimator();
            duration = baseDuration / this.attackSpeedStat;
            chargeTime = baseChargeTime / this.attackSpeedStat;
            base.PlayAnimation("Gesture, Override", "LaunchVortex", "BaseSkill.playbackRate", duration);
            animator.SetBool("isUsingIndependentSkill", true);
            
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                //this.leftMuzzleTransform = this.childLocator.FindChild("HandL");

            }


        }

        private void Fire()
        {

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }
}
