using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class BaseFireLightningBeam : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
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
