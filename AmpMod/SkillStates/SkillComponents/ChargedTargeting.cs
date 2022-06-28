using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace AmpMod.SkillStates.SkillComponents
{
    class ChargedTargeting : MonoBehaviour
    
	{			
		public Transform target { get; set; }

		private void FixedUpdate()
		{
			if (target && !target.gameObject.activeSelf /*&& target.GetComponent<HurtBox>().healthComponent.body.HasBuff(Modules.Buffs.chargeBuildup) */)
			{
				target = null;
			}
		}
	}
}

