using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Orbs;
using System.Collections.Generic;
using static HenryMod.SkillStates.BaseStates.FulminationOrb;

namespace HenryMod.SkillStates
{
    public class Fulmination : BaseState
    {
		public override void OnEnter()
		{
			base.OnEnter();
			this.stopwatch = 0f;
			this.entryDuration = Fulmination.baseEntryDuration / this.attackSpeedStat;
			this.flamethrowerDuration = Fulmination.baseFlamethrowerDuration;
			
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(this.entryDuration + this.flamethrowerDuration + 1f);
			}
			
			float num = this.flamethrowerDuration * Fulmination.tickFrequency;
			this.tickDamageCoefficient = Fulmination.totalDamageCoefficient / num;
			if (base.isAuthority && base.characterBody)
			{
				this.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
			}
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x000FA6E8 File Offset: 0x000F88E8
		public override void OnExit()
		{
			Util.PlaySound(Fulmination.endAttackSoundString, base.gameObject);
			base.PlayCrossfade("Gesture, Additive", "ExitFlamethrower", 0.1f);
		
			if (this.fulminationTransform)
			{
				EntityState.Destroy(this.fulminationTransform.gameObject);
			}
			base.OnExit();
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x000FA75C File Offset: 0x000F895C
		private void FireGauntlet(string muzzleString)
		{
			Ray aimRay = base.GetAimRay();
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					damage = this.tickDamageCoefficient * this.damageStat,
					force = Fulmination.force,
					muzzleName = muzzleString,
					hitEffectPrefab = Fulmination.impactEffectPrefab,
					isCrit = this.isCrit,
					radius = Fulmination.radius,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = Fulmination.procCoefficientPerTick,
					maxDistance = this.maxDistance,
					smartCollision = true,
					damageType = (Util.CheckRoll(Fulmination.ignitePercentChance, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic)
				}.Fire();
	
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
				Util.PlaySound(Fulmination.startAttackSoundString, base.gameObject);
				base.PlayAnimation("Gesture, Additive", "Flamethrower", "Flamethrower.playbackRate", this.flamethrowerDuration);
				if (this.childLocator)
				{
					Transform transform2 = this.childLocator.FindChild("MuzzleRight");
				
					if (transform2)
					{
						this.fulminationTransform = UnityEngine.Object.Instantiate<GameObject>(this.flamethrowerEffectPrefab, transform2).transform;
					}
					if (this.fulminationTransform)
					{
						this.fulminationTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.flamethrowerDuration;
					}
				}
				this.FireGauntlet("MuzzleCenter");
			}
			if (this.hasBegunFlamethrower)
			{
				this.flamethrowerStopwatch += Time.deltaTime;
				if (this.flamethrowerStopwatch > 1f / Fulmination.tickFrequency)
				{
					this.flamethrowerStopwatch -= 1f / Fulmination.tickFrequency;
					this.FireGauntlet("MuzzleCenter");
				}
				this.UpdateFlamethrowerEffect();
			}
			if (this.stopwatch >= this.flamethrowerDuration + this.entryDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x000FAA48 File Offset: 0x000F8C48
		private void UpdateFlamethrowerEffect()
		{
			Ray aimRay = base.GetAimRay();
			Vector3 direction = aimRay.direction;
			Vector3 direction2 = aimRay.direction;
			if (this.fulminationTransform)
			{
				this.fulminationTransform.forward = direction2;
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
		public static float baseEntryDuration = 1f;

		// Token: 0x0400367E RID: 13950
		public static float baseFlamethrowerDuration = 4f;

		// Token: 0x0400367F RID: 13951
		public static float totalDamageCoefficient = 1.2f;

		// Token: 0x04003680 RID: 13952
		public static float procCoefficientPerTick;

		// Token: 0x04003681 RID: 13953
		public static float tickFrequency;

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
		private float flamethrowerStopwatch;

		// Token: 0x04003689 RID: 13961
		private float stopwatch;

		// Token: 0x0400368A RID: 13962
		private float entryDuration;

		// Token: 0x0400368B RID: 13963
		private float flamethrowerDuration;

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
