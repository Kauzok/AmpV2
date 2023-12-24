using System;
using System.Collections.Generic;
using AmpMod.Modules;
using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using R2API;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class PhotonShot : BaseSkillState
    {
		private GameObject muzzleflashEffectPrefab;
		private GameObject hitEffectPrefab;
		private EntityStates.VoidSurvivor.Weapon.FireHandBeam handbeam;
		private GameObject tracerEffectPrefab;
		public float damageCoefficient = StaticValues.baseLaserDamageCoefficient;
		private float force = 1000f;
		private float maxDistance = 1000f;
		private String fireSoundString;
		private float recoilAmplitude = 1f;
		private float baseDuration = .6f;
		private float duration;
		private string muzzle = "IndexR";
		private float bulletRadius = 2;
		private float spreadBloomValue = .2f;
		private StackDamageController stackDamageController;
		private ChildLocator childLocator;
		private NemLightningColorController lightningColorController;


		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = this.baseDuration / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			//base.PlayAnimation(this.animationLayerName, this.animationStateName, this.animationPlaybackRateParam, this.duration);
			base.AddRecoil(-1f * this.recoilAmplitude, -2f * this.recoilAmplitude, -0.5f * this.recoilAmplitude, 0.5f * this.recoilAmplitude);
			base.StartAimMode(aimRay, 2f, false);
			Util.PlaySound(this.fireSoundString, base.gameObject);

			stackDamageController = base.GetComponent<StackDamageController>();

			childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
			lightningColorController = base.GetComponent<NemLightningColorController>();

			muzzleflashEffectPrefab = lightningColorController.specialBeamMuzzleFlash;
			tracerEffectPrefab = lightningColorController.specialBeamTracer;
			hitEffectPrefab = lightningColorController.specialBeamImpactDetonate;

			base.PlayAnimation("Gesture, Additive", "FireLaser", "BaseSkill.playbackRate", this.duration+.4f);

			if (this.muzzleflashEffectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, this.muzzle, false);
			}
			if (base.isAuthority)
			{
				BulletAttack photonShot = new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					muzzleName = this.muzzle,
					maxDistance = this.maxDistance,
					minSpread = 0f,
					maxSpread = base.characterBody.spreadBloomAngle,
					radius = this.bulletRadius,
					falloffModel = BulletAttack.FalloffModel.None,
					smartCollision = true,
					damage = this.damageCoefficient * this.damageStat,
					procCoefficient = 1f,
					force = this.force,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					damageType = DamageType.Stun1s,
					tracerEffectPrefab = this.tracerEffectPrefab,
					hitEffectPrefab = this.hitEffectPrefab
				};
				photonShot.AddModdedDamageType(DamageTypes.nemAmpDetonateCharge);
				photonShot.Fire();
			}
			base.characterBody.AddSpreadBloom(this.spreadBloomValue);

			stackDamageController.newSkillUsed = this;
			stackDamageController.resetComboTimer();
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}
	}
}
