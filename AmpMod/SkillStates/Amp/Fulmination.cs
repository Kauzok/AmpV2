using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using AmpMod.Modules;
using R2API;
using System;

namespace AmpMod.SkillStates
{
    public class Fulmination : BaseSkillState
	{

		[Header("Effect/Animation Variables")]
		public GameObject lightningEffectPrefab;
		public static GameObject impactEffectPrefab;
		public EffectData fulminationData;
		private AmpLightningController lightningController;
		private Transform fulminationTransform;
		private Transform muzzleTransform;
		private Transform handLTransform;
		private ChildLocator childLocator;
		private bool hasMuzzleEffect;
		private Animator animator;

		[Header("Attack Variables")]
		public static float radius = 1f;
		public static float basetickFrequency = 6f;
		public static float tickFrequency;
		public static float force = 20f;
		private float tickDamageCoefficient;
		public static float procCoefficientPerTick;
		public static float totalDamageCoefficient = Modules.StaticValues.fulminationTotalDamageCoefficient;
		public static float minimumDuration = 1f;
		public static float baseticktotal;

		[Header("Sounds")]
		public string enterSoundString = Modules.StaticValues.fulminationEnterString;
		public string attackSoundString = Modules.StaticValues.fulminationStateString;
		public string endSoundString = Modules.StaticValues.fulminationExitAlterString;
		public uint stopSoundID;

		[Header("Duration/Timer Variables")]
		public static float delayTime = .2f;
		public static float baseEntryDuration = .5f;
		public static float baseFulminationDuration = 3f;
		private float fulminationStopwatch;
		private float stopwatch;
		public float entryDuration;
		private float fulminationDuration;
		private bool hasBegunFulmination;
		private GenericSkill specialSlot;
		public static SkillDef cancelSkillDef;
		string prefix = AmpPlugin.developerPrefix;



		public override void OnEnter()
		{
		
			base.OnEnter();

			lightningController = base.GetComponent<AmpLightningController>();
			lightningEffectPrefab = lightningController.fulminationEffect;
			impactEffectPrefab = lightningController.fulminationHitEffect;
	
			stopwatch = 0f;
			entryDuration = Fulmination.baseEntryDuration / this.attackSpeedStat;
			fulminationDuration = Fulmination.baseFulminationDuration;
			Transform modelTransform = base.GetModelTransform();
			animator = base.GetModelAnimator();

			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(entryDuration + fulminationDuration + 1f);
				childLocator = modelTransform.GetComponent<ChildLocator>();
				handLTransform = childLocator.FindChild("HandL");
			}


			//play enter sound
			Util.PlaySound(Modules.StaticValues.fulminationEnterString, base.gameObject);

			//determines how many times the attack hits based off of attackspeed
			tickFrequency = basetickFrequency * this.attackSpeedStat;
				
			//determines damage of each tick based off of total damage and base tick frequency
			baseticktotal = Mathf.CeilToInt(basetickFrequency * fulminationDuration);
			tickDamageCoefficient = totalDamageCoefficient / baseticktotal;

			animator.SetBool("isUsingIndependentSkill", true);


			//play animation
			base.PlayAnimation("Fulminate, Override", "FulminateStart", "BaseSkill.playbackRate", entryDuration);

			

			animator.SetBool("isFulminating", true);


			cancelSkillDef = Skills.fulminationCancelSkillDef;

			
			specialSlot = base.skillLocator.special;
			if (this.specialSlot && cancelSkillDef)
			{
				this.specialSlot.SetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			} 

		}

		public override void OnExit()
		{
			base.OnExit();


			if (specialSlot && cancelSkillDef)
			{
				specialSlot.UnsetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}



			
			//play exit sound
			Util.PlaySound(endSoundString, base.gameObject);

			//stop sound
			AkSoundEngine.StopPlayingID(stopSoundID, 0);

		

			if (fulminationTransform)
                {
					EntityState.Destroy(fulminationTransform.gameObject);

				}

			animator.SetBool("isFulminating", false);
			animator.SetBool("isUsingIndependentSkill", false);
			base.PlayCrossfade("Fulminate, Override", "FulminateEnd", 0.1f);
			


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
					damage = tickDamageCoefficient * base.characterBody.damage,
					force = 2f,
					muzzleName = muzzleString,
					hitEffectPrefab = impactEffectPrefab,
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

				

				//allows amp to start slashing again
				animator.SetBool("isUsingIndependentSkill", false);

				//code for making electricity vfx come out of left hand
				if (childLocator)
                {
					Transform transform = childLocator.FindChild("HandL");
					
					if (transform)
					{
						fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(lightningEffectPrefab, transform).transform;
					}


				}

				//instantiate effect as gameobject to transform
				//fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.electricStreamEffect, transform).transform;

				//play sound and set stopID
				stopSoundID = Util.PlaySound(attackSoundString, base.gameObject);
				//base.PlayAnimation("Fulminate, Override", "FulminateHold", "BaseSkill.playbackRate", baseFulminationDuration);

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
		/*	if (fixedAge > delayTime)
            {
				if (base.inputBank.skill4.justPressed && base.isAuthority)
                {
					this.outer.SetNextStateToMain();
					return;

                }
            } */

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


	/*	public override InterruptPriority GetMinimumInterruptPriority()
		{

			return InterruptPriority.Any;
		}*/







	}
}
