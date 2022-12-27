using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FireLightningBeam : BaseState
    {
        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

		private void Fire()
		{
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();

			}
		}


	}
}
