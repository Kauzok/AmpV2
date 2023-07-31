using UnityEngine.Networking;
using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.Modules;
using RoR2.Skills;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class QuickDash : BaseState
    {
		private Vector3 forwardVector;
		private Vector3 blinkVector = Vector3.zero;
		private float stopwatch;
		[SerializeField]
		public GameObject blinkVfxPrefab = Modules.Assets.dashVFXPrefab;
		[SerializeField]
		public float overlayDuration = .3f;
		[SerializeField]
		public Material overlayMaterial;
		private GameObject blinkEffectPrefab = Assets.dashEnterEffect;
		private float duration = .4f;
		private float upSpeed = 0f;
		private CharacterModel characterModel;
		private GameObject blinkVfxInstance;
		private Transform modelTransform;
		private uint soundID;
		public static QuickDash src = new QuickDash();
		private string beginSoundString = StaticValues.surgeEnterString;
		private HurtBoxGroup hurtboxGroup;
		public float speedCoefficient = 5f;
		public AnimationCurve forwardSpeed;
		private StackDamageController stackDamageController;
		private string endSoundString = StaticValues.surgeExitString;
		public static SkillDef primaryOverrideSkillDef = Modules.Skills.fireLightningBallSkillDef;
		private string loopSound = StaticValues.surgeFlightString;
		private uint cancelID;

		public override void OnEnter()
		{
			base.OnEnter();
			#region old
			/*EntityStates.VoidSurvivor.VoidBlinkBase.VoidBlinkDown blinkDown = new EntityStates.VoidSurvivor.VoidBlinkBase.VoidBlinkDown();
			forwardSpeed = blinkDown.forwardSpeed;
			//Debug.Log(forwardSpeed);

			this.soundID = Util.PlaySound(this.beginSoundString, base.gameObject);
			this.forwardVector = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
			this.modelTransform = base.GetModelTransform();
			if (this.modelTransform)
			{
				this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
				this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
			}
			if (this.characterModel)
			{
				this.characterModel.invisibilityCount++;
			}
			if (this.hurtboxGroup)
			{
				HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
				int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
				hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
			}
			if (NetworkServer.active)
			{
				Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
			}
			this.blinkVfxInstance = UnityEngine.Object.Instantiate<GameObject>(this.blinkVfxPrefab);
			this.blinkVfxInstance.transform.SetParent(base.transform, false);
			this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject)); */
			#endregion
			stackDamageController = base.GetComponent<StackDamageController>();
			stackDamageController.newSkillUsed = this;
			stackDamageController.resetComboTimer();
			Util.PlaySound(beginSoundString, base.gameObject);
			//cancelID = Util.PlaySound(this.loopSound, base.gameObject);

			this.modelTransform = base.GetModelTransform();
			if (this.modelTransform)
			{
				this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
				this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
			}
			if (this.characterModel)
			{
				this.characterModel.invisibilityCount++;
			}

			if (this.hurtboxGroup)
			{
				HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
				int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
				hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
			}
			this.blinkVfxInstance = UnityEngine.Object.Instantiate<GameObject>(this.blinkVfxPrefab);
			this.blinkVfxInstance.transform.SetParent(base.transform, false);
			this.blinkVector = this.GetBlinkVector();

            //set lightning ball as primary skill
            if (base.skillLocator.primary && primaryOverrideSkillDef)
            {
				base.skillLocator.primary.SetSkillOverride(src, primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Contextual);

				/*if (base.skillLocator.primary.skillNameToken != "NT_UTILITY_LIGHTNINGBALL_NAME")
                {
					base.skillLocator.primary.SetSkillOverride(src, primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Contextual);
				} */
				
			} 




            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
		}

        protected Vector3 GetBlinkVector()
		{
			return ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.stopwatch += Time.fixedDeltaTime;
			if (base.characterMotor && base.characterDirection)
			{
				base.characterMotor.velocity = Vector3.zero;
				base.characterMotor.rootMotion += this.blinkVector * (this.moveSpeedStat * this.speedCoefficient * Time.fixedDeltaTime);
			}
			if (this.stopwatch >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
			}
		}

		private void CreateBlinkEffect(Vector3 origin)
		{
			EffectData effectData = new EffectData();
			effectData.rotation = Util.QuaternionSafeLookRotation(this.GetBlinkVector());
			effectData.origin = origin;
			EffectManager.SpawnEffect(this.blinkEffectPrefab, effectData, false);
		}

		public override void OnExit()
		{
			AkSoundEngine.StopPlayingID(this.cancelID);
			if (!this.outer.destroying)
			{
				Util.PlaySound(this.endSoundString, base.gameObject);
				this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
			}
			if (this.blinkVfxInstance)
			{
				VfxKillBehavior.KillVfxObject(this.blinkVfxInstance);
			}
			if (this.characterModel)
			{
				this.characterModel.invisibilityCount--;
			}
			if (this.hurtboxGroup)
			{
				HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
				int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
				hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
			}
	
			base.OnExit();
		}


	}
}
