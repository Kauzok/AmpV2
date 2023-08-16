using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using RoR2.Orbs;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp.Orbs
{
	public class NemAmpLightningStrikeOrb : GenericDamageOrb, IOrbFixedUpdateBehavior
	{
		public GameObject orbEffect;
		public GameObject lightningEffect;

		public override void Begin()
		{
			base.Begin();
			base.duration = 0.01f;
			if (this.target)
			{
				this.lastKnownTargetPosition = this.target.transform.position;
			}
		}

		public override GameObject GetOrbEffect()
		{
			return lightningEffect;

			//return LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact");
		} 

		public override void OnArrival()
		{
			EffectManager.SpawnEffect(lightningEffect, new EffectData
			{
				origin = this.lastKnownTargetPosition
			}, true);
			if (this.attacker)
			{
				new BlastAttack
				{
					attacker = this.attacker,
					baseDamage = this.damageValue,
					baseForce = 0f,
					bonusForce = Vector3.down * 3000f,
					crit = this.isCrit,
					damageColorIndex = DamageColorIndex.Item,
					damageType = DamageType.Stun1s,
					falloffModel = BlastAttack.FalloffModel.None,
					inflictor = null,
					position = this.lastKnownTargetPosition,
					procChainMask = this.procChainMask,
					procCoefficient = 1f,
					radius = 3f,
					teamIndex = TeamComponent.GetObjectTeam(this.attacker)
				}.Fire();
			}
		}


		public void FixedUpdate()
		{
			if (this.target && base.timeUntilArrival >= LightningStrikeOrb.positionLockDuration)
			{
				this.lastKnownTargetPosition = this.target.transform.position;
			}
		}


		private Vector3 lastKnownTargetPosition;

		private static readonly float positionLockDuration = 0.3f;
	}
}
