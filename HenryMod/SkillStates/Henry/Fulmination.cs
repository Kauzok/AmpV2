using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Orbs;
using System.Collections.Generic;
using static HenryMod.SkillStates.BaseStates.FulminationOrb;
using R2API;

namespace HenryMod.SkillStates
{
    public class Fulmination : BaseSkillState
    {
		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			this.entryDuration = Fulmination.baseEntryDuration / this.attackSpeedStat;
			this.fulminationDuration = Fulmination.baseFulminationDuration;
			
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(this.entryDuration + this.fulminationDuration + 1f);
			}

			//how many times the attack hits
			float num = this.fulminationDuration * Fulmination.tickFrequency;
			
			this.tickDamageCoefficient = Fulmination.totalDamageCoefficient / num;
	
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x000FA6E8 File Offset: 0x000F88E8
		public override void OnExit()
		{
			EntityState.Destroy(this.fulminationTransform.gameObject);
			
			base.OnExit();
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x000FA75C File Offset: 0x000F895C
		private void FireGauntlet()
		{
			Ray aimRay = base.GetAimRay();
			if (base.isAuthority)
			{
				BulletAttack lightningAttack = new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					damage = this.tickDamageCoefficient * base.characterBody.damage,
					force = 2f,
					hitEffectPrefab = Modules.Assets.electricImpactEffect,
					isCrit = base.characterBody.RollCrit(),
					radius = Fulmination.radius,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = 1f,
					maxDistance = 20f,
					smartCollision = true,
					damageType = DamageType.Generic
				};
				lightningAttack.AddModdedDamageType(Modules.DamageTypes.fulminationChain);

				
				//figure out how to set the charactermaster component
				if (Util.CheckRoll(30f, base.characterBody.master))
                {
					lightningAttack.AddModdedDamageType(Modules.DamageTypes.applyCharge);
                }

				lightningAttack.Fire();
				
	
			}
		}


		// Token: 0x06003F54 RID: 16212 RVA: 0x000FA894 File Offset: 0x000F8A94
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (this.stopwatch >= this.entryDuration && !this.hasBegunFlamethrower)
			{
				this.hasBegunFlamethrower = true;

				this.fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.electricStreamEffect, transform).transform;
				

				this.FireGauntlet();
			}
			if (this.hasBegunFlamethrower)
			{
				this.fulminationStopwatch += Time.deltaTime;
				if (this.fulminationStopwatch > 1f / Fulmination.tickFrequency)
				{
					this.fulminationStopwatch -= 1f / Fulmination.tickFrequency;
					this.FireGauntlet();
				}
				this.UpdateFulminationEffect();
			}
			if (this.stopwatch >= this.fulminationDuration + this.entryDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x000FAA48 File Offset: 0x000F8C48
		private void UpdateFulminationEffect()
		{
			Ray aimRay = base.GetAimRay();
			Vector3 direction = aimRay.direction;
			Vector3 direction2 = aimRay.direction;
			if (fulminationTransform)
			{
				fulminationTransform.forward = direction2;
			}
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00013F7C File Offset: 0x0001217C
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

		// Token: 0x04003678 RID: 13944
		[SerializeField]
		public GameObject flamethrowerEffectPrefab = Modules.Assets.electricStreamEffect;

		// Token: 0x04003679 RID: 13945
		public static GameObject impactEffectPrefab = Modules.Assets.electricImpactEffect;

		// Token: 0x0400367A RID: 13946
		public static GameObject tracerEffectPrefab;

		// Token: 0x0400367B RID: 13947
		[SerializeField]
		public float maxDistance;

		// Token: 0x0400367C RID: 13948
		public static float radius;

		// Token: 0x0400367D RID: 13949
		public static float baseEntryDuration = .5f;

		// Token: 0x0400367E RID: 13950
		public static float baseFulminationDuration = 4f;

		// Token: 0x0400367F RID: 13951
		public static float totalDamageCoefficient = 22f;

		// Token: 0x04003680 RID: 13952
		public static float procCoefficientPerTick;

		// Token: 0x04003681 RID: 13953
		public static float tickFrequency = 5f;

		// Token: 0x04003682 RID: 13954
		public static float force = 20f;

		// Token: 0x04003683 RID: 13955
		public static string startAttackSoundString;

		// Token: 0x04003684 RID: 13956
		public static string endAttackSoundString;

		// Token: 0x04003685 RID: 13957
		public static float ignitePercentChance;

		// Token: 0x04003686 RID: 13958
		public static float recoilForce;

		// Token: 0x04003687 RID: 13959
		private float tickDamageCoefficient;

		// Token: 0x04003688 RID: 13960
		private float fulminationStopwatch;

		// Token: 0x04003689 RID: 13961
		private float stopwatch;

		// Token: 0x0400368A RID: 13962
		public float entryDuration;

		// Token: 0x0400368B RID: 13963
		private float fulminationDuration;

		// Token: 0x0400368C RID: 13964
		private bool hasBegunFlamethrower;

		// Token: 0x0400368D RID: 13965
		private ChildLocator childLocator;

		// Token: 0x0400368E RID: 13966
		

		// Token: 0x0400368F RID: 13967
		private Transform fulminationTransform;


		// Token: 0x04003692 RID: 13970
		private bool isCrit;

		// Token: 0x04003693 RID: 13971
		private const float flamethrowerEffectBaseDistance = 16f;
	}

}
