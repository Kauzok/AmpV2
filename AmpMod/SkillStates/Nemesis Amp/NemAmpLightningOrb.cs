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
		// Token: 0x060040CA RID: 16586 RVA: 0x0010C4D0 File Offset: 0x0010A6D0
		public override void Begin()
		{
			base.Begin();
			base.duration = 0.5f;
			if (this.target)
			{
				this.lastKnownTargetPosition = this.target.transform.position;
			}
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0010C506 File Offset: 0x0010A706
		public override GameObject GetOrbEffect()
		{
			return LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect");
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0010C514 File Offset: 0x0010A714
		public override void OnArrival()
		{
			EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), new EffectData
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

		// Token: 0x060040CD RID: 16589 RVA: 0x0010C5FD File Offset: 0x0010A7FD
		public void FixedUpdate()
		{
			if (this.target && base.timeUntilArrival >= LightningStrikeOrb.positionLockDuration)
			{
				this.lastKnownTargetPosition = this.target.transform.position;
			}
		}

		// Token: 0x04003F42 RID: 16194
		private Vector3 lastKnownTargetPosition;

		// Token: 0x04003F43 RID: 16195
		private static readonly float positionLockDuration = 0.3f;
	}
}
