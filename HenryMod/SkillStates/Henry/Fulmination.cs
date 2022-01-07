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

		public static float delayTime = .2f;

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

		public string enterSoundString = Modules.StaticValues.fulminationEnterString;

		public string attackSoundString = Modules.StaticValues.fulminationStateString;

		public string endSoundString = Modules.StaticValues.fulminationExitAlterString;

		public uint stopSoundID;

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
			//play enter sound
			Util.PlaySound(Modules.StaticValues.fulminationEnterString, base.gameObject);

			//how many times the attack hits
			float num = this.fulminationDuration * Fulmination.tickFrequency;

			this.tickDamageCoefficient = Fulmination.totalDamageCoefficient / num;

		}

		public override void OnExit()
		{
			//remove effect and stop sound
			EntityState.Destroy(this.fulminationTransform.gameObject);
			AkSoundEngine.StopPlayingID(stopSoundID, 0);

			//play exit sound
			Util.PlaySound(endSoundString, base.gameObject);

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
	
				//instantiate effect as gameobject to transform
				this.fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.electricStreamEffect, transform).transform;

				//play sound and set stopID
				stopSoundID = Util.PlaySound(attackSoundString, base.gameObject);

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

			//lets player cancel ability by pressing key again; weird error showed up when i added this, so check this out if fulmination exhibits any strange behavior
			if (fixedAge > delayTime)
            {
				if (base.inputBank.skill4.justPressed && base.isAuthority)
                {
					this.outer.SetNextStateToMain();
                }
            }

			if (this.stopwatch >= this.fulminationDuration + this.entryDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();  
				return;
			}
		}

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
