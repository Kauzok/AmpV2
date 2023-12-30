using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using EntityStates;
using UnityEngine.Networking;
using AmpMod.Modules;
using R2API;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class ChargeLightBlast : BaseState
    {

		//min charge means instant; how long it takes for the shotgun blast to come out
		private float baseBlastChargeDuration = .1f;
		private float blastChargeDuration;

		//threshold duration; releasing before this much time has passed makes you do the shotgun burst
		private float staticMinBeamThresholdDuration = .15f;
		private float baseBeamThresholdDuration = .7f;
		private float thresholdDuration;
		private Transform muzzleTransform;
		private string muzzleName;
		private uint stopID;
		private string enterSoundString;
		public static GameObject chargeupVfxPrefab;
		public static GameObject holdChargeVfxPrefab;
		private GameObject chargeupVfxGameObject;
		private GameObject holdChargeVfxGameObject;
		public static string playChargeSoundString;
		public static string stopChargeSoundString;
		
		private bool released;

        [Header("Beam Variables")]
		public float baseLaserBeamDamageCoefficient = StaticValues.baseLaserDamageCoefficient;
		private float maxDistance = 10000f;
		private float bulletRadius = 2f;
		private float force = 10f;
		private GameObject tracerEffectPrefab = Assets.photonTracer;

		[Header("Blast Variables")]
		public float baseLaserBlastDamageCoefficient = StaticValues.baseLaserBlastDamageCoefficient;
		private float blastRadius = 6f;

		public override void OnEnter()
		{
			base.OnEnter();
			this.blastChargeDuration = baseBlastChargeDuration / this.attackSpeedStat;
			this.thresholdDuration = baseBeamThresholdDuration / this.attackSpeedStat;

			//the beam will always take at least .15 seconds to charge; this is done to prevent players from no longer having the ability to shoot the normal blasts
			if (thresholdDuration < staticMinBeamThresholdDuration) thresholdDuration = staticMinBeamThresholdDuration;

			//base.PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "playbackRate", this.thresholdDuration, 0.1f);
			//base.PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "playbackRate", this.thresholdDuration, 0.1f);
			this.muzzleTransform = base.FindModelChild(muzzleName);
			if (this.muzzleTransform)
			{
				this.chargeupVfxGameObject = UnityEngine.Object.Instantiate<GameObject>(chargeupVfxPrefab, this.muzzleTransform);
				this.chargeupVfxGameObject.GetComponent<ScaleParticleSystemDuration>().newDuration = this.thresholdDuration;
			}
			stopID = Util.PlayAttackSpeedSound(enterSoundString, base.gameObject, this.attackSpeedStat);
			Util.PlaySound(playChargeSoundString, base.gameObject);
		}

		public override void OnExit()
		{
			if (this.chargeupVfxGameObject)
			{
				EntityState.Destroy(this.chargeupVfxGameObject);
				this.chargeupVfxGameObject = null;
			}
			if (this.holdChargeVfxGameObject)
			{
				EntityState.Destroy(this.holdChargeVfxGameObject);
				this.holdChargeVfxGameObject = null;
			}
			AkSoundEngine.StopPlayingID(this.stopID);
			//Util.PlaySound(stopChargeSoundString, base.gameObject);
			base.OnExit();
		}

		public override void Update()
		{
			base.Update();
			base.characterBody.SetSpreadBloom(base.age / this.thresholdDuration, true);
		}

		private void FireBlast()
        {
			Ray aimray = base.GetAimRay();
			aimray.direction.Normalize();
			BlastAttack photonBlast = new BlastAttack
			{
				attacker = base.gameObject,
				baseDamage = baseLaserBlastDamageCoefficient * base.characterBody.damage,
				baseForce = 2f,
				attackerFiltering = AttackerFiltering.NeverHitSelf,
				crit = base.characterBody.RollCrit(),
				damageColorIndex = DamageColorIndex.Item,
				damageType = DamageType.Generic,
				falloffModel = BlastAttack.FalloffModel.None,
				inflictor = base.gameObject,

				//blastattack is positioned 5 units in front of characrter
				position = base.characterBody.corePosition + 3f * (aimray.direction),
				procChainMask = default(ProcChainMask),
				procCoefficient = 1f,
				radius = this.blastRadius

			};
			photonBlast.AddModdedDamageType(DamageTypes.nemAmpDetonateCharge);
			photonBlast.Fire();
		}

		private void FireBeam()
        {
			if (base.isAuthority)
			{
				Ray aimRay = base.GetAimRay();
				BulletAttack photonShot = new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					muzzleName = this.muzzleName,
					maxDistance = this.maxDistance,
					minSpread = 0f,
					maxSpread = base.characterBody.spreadBloomAngle,
					radius = this.bulletRadius,
					falloffModel = BulletAttack.FalloffModel.None,
					smartCollision = true,
					damage = this.baseLaserBeamDamageCoefficient * this.damageStat,
					procCoefficient = 1f,
					force = this.force,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					damageType = DamageType.Stun1s,
					tracerEffectPrefab = this.tracerEffectPrefab,
					///hitEffectPrefab = this.hitEffectPrefab
				};
				photonShot.AddModdedDamageType(DamageTypes.nemAmpDetonateCharge);
				photonShot.Fire();
			}
			//base.characterBody.AddSpreadBloom(this.spreadBloomValue);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			base.characterBody.SetAimTimer(1f);
			Mathf.Clamp01(base.fixedAge / this.thresholdDuration);
			if (base.fixedAge >= this.thresholdDuration)
			{
				if (this.chargeupVfxGameObject)
				{
					EntityState.Destroy(this.chargeupVfxGameObject);
					this.chargeupVfxGameObject = null;
				}
				if (!this.holdChargeVfxGameObject && this.muzzleTransform)
				{
					this.holdChargeVfxGameObject = UnityEngine.Object.Instantiate<GameObject>(holdChargeVfxPrefab, this.muzzleTransform);
				}
			}
			if (base.isAuthority)
			{
				if (!this.released && (!base.inputBank || !base.inputBank.skill4.down))
				{
					this.released = true;
				}
				if (this.released && base.fixedAge >= thresholdDuration)
				{
					FireBeam();
					this.outer.SetNextStateToMain();
				}

                else if (this.released && base.fixedAge <= thresholdDuration)
                {
					FireBlast();
					this.outer.SetNextStateToMain();
                }
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0000B4BB File Offset: 0x000096BB
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}
	}
}
