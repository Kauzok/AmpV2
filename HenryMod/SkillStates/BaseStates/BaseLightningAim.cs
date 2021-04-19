using System;
using RoR2;
using UnityEngine;

namespace EntityStates.Huntress
{
	// Token: 0x02000B12 RID: 2834
	public class BaseLightningAim : BaseState
	{
		// Token: 0x06004142 RID: 16706 RVA: 0x00103C28 File Offset: 0x00101E28
		public override void OnEnter()
		{
			base.OnEnter();
			Util.PlaySound(this.beginLoopSoundString, base.gameObject);
			this.huntressTracker = base.GetComponent<HuntressTracker>();
			if (this.huntressTracker)
			{
				this.huntressTracker.enabled = false;
			}
			if (base.cameraTargetParams)
			{
				this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00103C94 File Offset: 0x00101E94
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.characterMotor)
			{
				base.characterMotor.velocity = Vector3.zero;
			}
			if (base.isAuthority && base.inputBank)
			{
				if (base.skillLocator && base.skillLocator.utility.IsReady() && base.inputBank.skill3.justPressed)
				{
					this.outer.SetNextStateToMain();
					return;
				}
				if (base.fixedAge >= this.maxDuration || base.inputBank.skill1.justPressed || base.inputBank.skill4.justPressed)
				{
					this.HandlePrimaryAttack();
				}
			}
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00004381 File Offset: 0x00002581
		protected virtual void HandlePrimaryAttack()
		{
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00103D54 File Offset: 0x00101F54
		public override void OnExit()
		{
			base.PlayAnimation("FullBody, Override", "FireArrowRain");
			Util.PlaySound(this.endLoopSoundString, base.gameObject);
			Util.PlaySound(this.fireSoundString, base.gameObject);
			CameraTargetParams.AimRequest aimRequest = this.aimRequest;
			if (aimRequest != null)
			{
				aimRequest.Dispose();
			}
			if (this.huntressTracker)
			{
				this.huntressTracker.enabled = true;
			}
			base.OnExit();
		}

		// Token: 0x04003951 RID: 14673
		[SerializeField]
		public float maxDuration;

		// Token: 0x04003952 RID: 14674
		[SerializeField]
		public string beginLoopSoundString;

		// Token: 0x04003953 RID: 14675
		[SerializeField]
		public string endLoopSoundString;

		// Token: 0x04003954 RID: 14676
		[SerializeField]
		public string fireSoundString;

		// Token: 0x04003955 RID: 14677
		private HuntressTracker huntressTracker;

		// Token: 0x04003956 RID: 14678
		private CameraTargetParams.AimRequest aimRequest;
	}
}
