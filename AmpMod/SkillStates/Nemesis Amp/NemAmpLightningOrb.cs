using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using RoR2.Orbs;

namespace AmpMod.SkillStates.Nemesis_Amp
{
	public class NemAmpLightningOrb : GenericDamageOrb, IOrbFixedUpdateBehavior
	{
		public GameObject orbEffect;


		public override void Begin()
		{
			base.Begin();
			base.duration = 0.5f;
			if (this.target)
			{
				this.lastKnownTargetPosition = this.target.transform.position;
			}
		}

		/*public override GameObject GetOrbEffect()
		{
			return LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect");
		} */

		public override void OnArrival()
		{
			EffectManager.SpawnEffect(orbEffect, new EffectData
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
