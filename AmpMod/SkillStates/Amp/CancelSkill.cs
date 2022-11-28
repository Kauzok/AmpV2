using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Skills;

namespace AmpMod.SkillStates
{
	class CancelSkill : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority)
			{
				//this.outer.SetNextState(EntityStateCatalog.InstantiateState(this.nextStanceState));
				return;
			}
		}

	}
}
