using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using R2API;
using System;

namespace AmpMod.SkillStates
{
	public class Fulmination : BaseSkillState
	{

		[Header("Effect Variables")]
		public GameObject lightningEffectPrefab = Modules.Assets.electricStreamEffect;
		public static GameObject impactEffectPrefab = Modules.Assets.electricImpactEffect;
		public EffectData fulminationData;
		private Transform fulminationTransform;
		private Transform handLTransform;
		private ChildLocator childLocator;


		[Header("Attack Variables")]
		public static float radius;
		public static float basetickFrequency = 5f;
		public static float tickFrequency;
		public static float force = 20f;
		private float tickDamageCoefficient = 1.1f;
		public static float procCoefficientPerTick;
		public static float totalDamageCoefficient = Modules.StaticValues.fulminationTotalDamageCoefficient;
		public static float minimumDuration = 1f;

		[Header("Sounds")]
		public string enterSoundString = Modules.StaticValues.fulminationEnterString;
		public string attackSoundString = Modules.StaticValues.fulminationStateString;
		public string endSoundString = Modules.StaticValues.fulminationExitAlterString;
		public uint stopSoundID;

		[Header("Duration/Timer Variables")]
		public static float delayTime = .2f;
		public static float baseEntryDuration = .5f;
		public static float baseFulminationDuration = 4f;
		private float fulminationStopwatch;
		private float stopwatch;
		public float entryDuration;
		private float fulminationDuration;
		private bool hasBegunFulmination;
		private GenericSkill specialSlot;
		public static SkillDef cancelSkillDef;



		public override void OnEnter()
		{
		
			base.OnEnter();
			stopwatch = 0f;
			entryDuration = Fulmination.baseEntryDuration / this.attackSpeedStat;
			fulminationDuration = Fulmination.baseFulminationDuration;
			Transform modelTransform = base.GetModelTransform();

			if (base.characterBody)
            {
                base.characterBody.SetAimTimer(entryDuration + fulminationDuration + 1f);
				childLocator = modelTransform.GetComponent<ChildLocator>();
				handLTransform = childLocator.FindChild("HandL");
			}
            //play enter sound
            Util.PlaySound(Modules.StaticValues.fulminationEnterString, base.gameObject);

			//determines how many times the attack hits based off of 
			tickFrequency = basetickFrequency * this.attackSpeedStat;
			base.PlayAnimation("Fulminate, Override", "FulminateStart", "Shootgun.PlaybackRate", entryDuration);

			specialSlot = base.skillLocator.special;
			if (this.specialSlot ) //&& cancelSkillDef != null)
			{
				this.specialSlot.SetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}

		}

		public override void OnExit()
		{

			//play exit sound
			Util.PlaySound(endSoundString, base.gameObject);

			//stop sound
			AkSoundEngine.StopPlayingID(stopSoundID, 0);

			if (specialSlot && cancelSkillDef != null)
			{
				specialSlot.UnsetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}


			if (fulminationTransform)
                {
					EntityState.Destroy(fulminationTransform.gameObject);

				}


			base.PlayCrossfade("Fulminate, Override", "FulminateEnd", 0.1f);
			base.OnExit();


		}

		private void FireLightning(String muzzleString)
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
					muzzleName = muzzleString,
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
			
			stopwatch += Time.fixedDeltaTime;
			if (stopwatch >= entryDuration && !hasBegunFulmination)
			{
				hasBegunFulmination = true;
				

				//code for making electricity vfx come out of left hand
				if (childLocator)
                {
					Transform transform = childLocator.FindChild("HandL");
					
					if (transform)
					{
						fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.electricStreamEffect, transform).transform;
					}


				}

				//instantiate effect as gameobject to transform
				//fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.electricStreamEffect, transform).transform;

				//play sound and set stopID
				stopSoundID = Util.PlaySound(attackSoundString, base.gameObject);
				base.PlayAnimation("Fulminate, Override", "FulminateHold", "Shoot.Playbackrate", baseFulminationDuration);

				//fire actual damage dealing bulletattack
				FireLightning("HandL");
			}

			//updates effect to point forward and fires attack repeatedly to create a consistent damage dealing stream
			if (hasBegunFulmination)
			{
				fulminationStopwatch += Time.deltaTime;
				if (fulminationStopwatch > 1f / Fulmination.tickFrequency)
				{
					fulminationStopwatch -= 1f / Fulmination.tickFrequency;
					FireLightning("HandL");
				}
				UpdateFulminationEffect();
			}

			//lets player cancel ability by pressing key again
			if (fixedAge > delayTime)
            {
				if (base.inputBank.skill4.justPressed && base.isAuthority)
                {
					
					this.outer.SetNextStateToMain();
					return;
                }
            } 

			//ends skill when time runs out
			if (stopwatch >= fulminationDuration + entryDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
					
				return;
			}
			
			
		}

		//update effect of lightning stream; make it point where player is facing
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

			return InterruptPriority.Any;


		}







	}
}
