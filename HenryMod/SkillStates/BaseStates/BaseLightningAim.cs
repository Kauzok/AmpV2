using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using RoR2.Orbs;
using RoR2.Skills;

namespace HenryMod.SkillStates
{
	// Token: 0x02000B12 RID: 2834
	public class BaseLightningAim : BaseState
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060045A7 RID: 17831 RVA: 0x00119F7E File Offset: 0x0011817E
		private float exitDuration
		{
			get
			{
				return BaseLightningAim.baseExitDuration / this.attackSpeedStat;
			}
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00119F8C File Offset: 0x0011818C
		public override void OnEnter()
		{
			base.OnEnter();
			this.primarySkillSlot = (base.skillLocator ? base.skillLocator.primary : null);
			if (this.primarySkillSlot)
			{
				this.primarySkillSlot.SetSkillOverride(this, BaseLightningAim.primarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}
			this.modelAnimator = base.GetModelAnimator();
			if (this.modelAnimator)
			{
				this.modelAnimator.SetBool("PrepAirstrike", true);
			}
			base.PlayCrossfade("Gesture, Override", "PrepAirstrike", 0.1f);
			base.PlayCrossfade("Gesture, Additive", "PrepAirstrike", 0.1f);
			Transform transform = base.FindModelChild(BaseLightningAim.effectMuzzleString);
			if (transform)
			{
				this.effectMuzzleInstance = UnityEngine.Object.Instantiate<GameObject>(BaseLightningAim.effectMuzzlePrefab, transform);
			}
			this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;
			if (BaseLightningAim.crosshairOverridePrefab)
			{
				base.characterBody.crosshairPrefab = BaseLightningAim.crosshairOverridePrefab;
			}
			Util.PlaySound(BaseLightningAim.enterSoundString, base.gameObject);
			Util.PlaySound("Play_captain_shift_active_loop", base.gameObject);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x0011A0A8 File Offset: 0x001182A8
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.characterDirection)
			{
				base.characterDirection.moveVector = base.GetAimRay().direction;
			}
			if (!this.primarySkillSlot || this.primarySkillSlot.stock == 0)
			{
				this.beginExit = true;
			}
			if (this.beginExit)
			{
				this.timerSinceComplete += Time.fixedDeltaTime;
				if (this.timerSinceComplete > this.exitDuration)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0011A138 File Offset: 0x00118338
		public override void OnExit()
		{
			if (this.primarySkillSlot)
			{
				this.primarySkillSlot.UnsetSkillOverride(this, BaseLightningAim.primarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}
			Util.PlaySound(BaseLightningAim.exitSoundString, base.gameObject);
			Util.PlaySound("Stop_captain_shift_active_loop", base.gameObject);
			if (this.effectMuzzleInstance)
			{
				EntityState.Destroy(this.effectMuzzleInstance);
			}
			if (this.modelAnimator)
			{
				this.modelAnimator.SetBool("PrepAirstrike", false);
			}
			base.characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
			base.OnExit();
		}

		// Token: 0x04003F87 RID: 16263
		public static SkillDef primarySkillDef;	

		// Token: 0x04003F88 RID: 16264
		public static GameObject crosshairOverridePrefab;

		// Token: 0x04003F89 RID: 16265
		public static string enterSoundString;

		// Token: 0x04003F8A RID: 16266
		public static string exitSoundString;

		// Token: 0x04003F8B RID: 16267
		public static GameObject effectMuzzlePrefab;

		// Token: 0x04003F8C RID: 16268
		public static string effectMuzzleString;

		// Token: 0x04003F8D RID: 16269
		public static float baseExitDuration;

		// Token: 0x04003F8E RID: 16270
		private GameObject defaultCrosshairPrefab;

		// Token: 0x04003F8F RID: 16271
		private GenericSkill primarySkillSlot;

		// Token: 0x04003F90 RID: 16272
		private GameObject effectMuzzleInstance;

		// Token: 0x04003F91 RID: 16273
		private Animator modelAnimator;

		// Token: 0x04003F92 RID: 16274
		private float timerSinceComplete;

		// Token: 0x04003F93 RID: 16275
		private bool beginExit;
	}
}
