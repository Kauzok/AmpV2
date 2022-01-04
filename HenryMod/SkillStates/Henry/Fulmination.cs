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

		public GameObject lightningEffectPrefab = Modules.Assets.electricStreamEffect;

		public static GameObject impactEffectPrefab = Modules.Assets.electricImpactEffect;

		public static float radius;

		public EffectData fulminationData;

		public static float baseEntryDuration = .5f;

		public static float baseFulminationDuration = 4f;

		public static float totalDamageCoefficient = 22f;

		public static float procCoefficientPerTick;

		public static float tickFrequency = 5f;

		public static float force = 20f;

		private float tickDamageCoefficient;

		private float fulminationStopwatch;

		private float stopwatch;

		public float entryDuration;

		private float fulminationDuration;

		private bool hasBegunFlamethrower;

		private Transform fulminationTransform;



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

		public override void OnExit()
		{
			EntityState.Destroy(this.fulminationTransform.gameObject);

			base.OnExit();
		}

		private void FireGauntlet()
		{
			Ray aimRay = base.GetAimRay();
			if (base.isAuthority)
			{

				//damage dealing component of fulmination
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


				//15% chance of proccing charge on hit
				if (Util.CheckRoll(15f, base.characterBody.master))
				{
					lightningAttack.AddModdedDamageType(Modules.DamageTypes.applyCharge);
				}

				lightningAttack.Fire();


			}
		}




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
			if (fulminationTransform)
			{
				fulminationTransform.forward = direction;
			}
		}


		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

	



	}
}
