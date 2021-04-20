using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;
using UnityEngine.Networking;
using EntityStates.Captain;

namespace HenryMod.SkillStates
{
	// Token: 0x02000B11 RID: 2833
	public class VoltaicBombardment : BaseSkillState
	{
		private float duration
		{
			get
			{
				return VoltaicBombardment.baseDuration / this.attackSpeedStat;
			}
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x0011AB18 File Offset: 0x00118D18
		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				this.placementInfo = BaseLightningAim.GetPlacementInfo(base.GetAimRay(), base.gameObject);
				if (this.placementInfo.ok)
				{
					base.activatorSkillSlot.DeductStock(1);
				}
			}
			if (this.placementInfo.ok)
			{
				EffectManager.SimpleMuzzleFlash(this.muzzleflashEffect, base.gameObject, VoltaicBombardment.muzzleString, false);
				base.characterBody.SetAimTimer(3f);
				base.PlayAnimation("Gesture, Override", "CallSupplyDrop", "CallSupplyDrop.playbackRate", this.duration);
				base.PlayAnimation("Gesture, Additive", "CallSupplyDrop", "CallSupplyDrop.playbackRate", this.duration);
				if (NetworkServer.active)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.supplyDropPrefab, this.placementInfo.position, this.placementInfo.rotation);
					gameObject.GetComponent<TeamFilter>().teamIndex = base.teamComponent.teamIndex;
					gameObject.GetComponent<GenericOwnership>().ownerObject = base.gameObject;
					ProjectileDamage component = gameObject.GetComponent<ProjectileDamage>();
					component.crit = base.RollCrit();
					component.damage = this.damageStat * 300f;
					component.damageColorIndex = DamageColorIndex.Default;
					component.force = VoltaicBombardment.impactDamageForce;
					component.damageType = DamageType.Shock5s;
					NetworkServer.Spawn(gameObject);
				}
			}
			else
			{
				base.PlayCrossfade("Gesture, Override", "BufferEmpty", 0.1f);
				base.PlayCrossfade("Gesture, Additive", "BufferEmpty", 0.1f);
			}
			EntityStateMachine entityStateMachine = EntityStateMachine.FindByCustomName(base.gameObject, "Skillswap");
			if (entityStateMachine)
			{
				entityStateMachine.SetNextStateToMain();
			}
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x0011ACAE File Offset: 0x00118EAE
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge > this.duration)
			{
				this.outer.SetNextStateToMain();
			}
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x0006E9B6 File Offset: 0x0006CBB6
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Pain;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x0011ACD7 File Offset: 0x00118ED7
		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			this.placementInfo.Serialize(writer);
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x0011ACEC File Offset: 0x00118EEC
		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.placementInfo.Deserialize(reader);
		}

		// Token: 0x04003FBF RID: 16319
		[SerializeField]
		public GameObject muzzleflashEffect;

		// Token: 0x04003FC0 RID: 16320
		[SerializeField]
		public GameObject supplyDropPrefab;

		// Token: 0x04003FC1 RID: 16321
		public static string muzzleString;

		// Token: 0x04003FC2 RID: 16322
		public static float baseDuration;

		// Token: 0x04003FC3 RID: 16323
		public static float impactDamageCoefficient;

		// Token: 0x04003FC4 RID: 16324
		public static float impactDamageForce;

		// Token: 0x04003FC5 RID: 16325
		public BaseLightningAim.PlacementInfo placementInfo;
	}
}

