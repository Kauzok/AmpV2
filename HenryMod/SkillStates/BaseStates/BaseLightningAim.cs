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
		private float exitDuration
		{
			get
			{
				return BaseLightningAim.baseExitDuration / this.attackSpeedStat;
			}
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0011A6F8 File Offset: 0x001188F8
		public override void OnEnter()
		{
			base.OnEnter();
			this.modelAnimator = base.GetModelAnimator();
			base.PlayAnimation("Gesture, Override", "PrepSupplyDrop");
			base.PlayAnimation("Gesture, Additive", "PrepSupplyDrop");
			if (this.modelAnimator)
			{
				this.modelAnimator.SetBool("PrepSupplyDrop", true);
			}
			Transform transform = base.FindModelChild(EntityStates.Captain.Weapon.SetupSupplyDrop.effectMuzzleString);
			if (transform)
			{
				this.effectMuzzleInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Captain.Weapon.SetupSupplyDrop.effectMuzzlePrefab, transform);
			}
			this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;
			if (BaseLightningAim.crosshairOverridePrefab)
			{
				base.characterBody.crosshairPrefab = BaseLightningAim.crosshairOverridePrefab;
			}
			Util.PlaySound(BaseLightningAim.enterSoundString, base.gameObject);
			this.blueprints = UnityEngine.Object.Instantiate<GameObject>(BaseLightningAim.blueprintPrefab, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation).GetComponent<BlueprintController>();
			if (base.cameraTargetParams)
			{
				this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
			}
			this.originalPrimarySkill = base.skillLocator.primary;
			this.originalSecondarySkill = base.skillLocator.secondary;
			base.skillLocator.primary = base.skillLocator.FindSkill("SupplyDrop1");
			base.skillLocator.secondary = base.skillLocator.FindSkill("SupplyDrop2");
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0011A85C File Offset: 0x00118A5C
		public override void Update()
		{
			base.Update();
			this.currentPlacementInfo = BaseLightningAim.GetPlacementInfo(base.GetAimRay(), base.gameObject);
			if (this.blueprints)
			{
				this.blueprints.PushState(this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, this.currentPlacementInfo.ok);
			}
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0011A8C0 File Offset: 0x00118AC0
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.characterDirection)
			{
				base.characterDirection.moveVector = base.GetAimRay().direction;
			}
			if (base.isAuthority && this.beginExit)
			{
				this.timerSinceComplete += Time.fixedDeltaTime;
				if (this.timerSinceComplete > this.exitDuration)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x0011A934 File Offset: 0x00118B34
		public override void OnExit()
		{
			if (!this.outer.destroying)
			{
				Util.PlaySound(BaseLightningAim.exitSoundString, base.gameObject);
			}
			if (this.effectMuzzleInstance)
			{
				EntityState.Destroy(this.effectMuzzleInstance);
			}
			base.characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
			base.skillLocator.primary = this.originalPrimarySkill;
			base.skillLocator.secondary = this.originalSecondarySkill;
			if (this.modelAnimator)
			{
				this.modelAnimator.SetBool("PrepSupplyDrop", false);
			}
			if (this.blueprints)
			{
				EntityState.Destroy(this.blueprints.gameObject);
				this.blueprints = null;
			}
			CameraTargetParams.AimRequest aimRequest = this.aimRequest;
			if (aimRequest != null)
			{
				aimRequest.Dispose();
			}
			base.OnExit();
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x0011AA04 File Offset: 0x00118C04
		public static BaseLightningAim.PlacementInfo GetPlacementInfo(Ray aimRay, GameObject gameObject)
		{
			float num = 0f;
			CameraRigController.ModifyAimRayIfApplicable(aimRay, gameObject, out num);
			Vector3 vector = -aimRay.direction;
			Vector3 vector2 = Vector3.up;
			Vector3 lhs = Vector3.Cross(vector2, vector);
			BaseLightningAim.PlacementInfo result = default(BaseLightningAim.PlacementInfo);
			result.ok = false;
			RaycastHit raycastHit;
			if (Physics.Raycast(aimRay, out raycastHit, BaseLightningAim.maxPlacementDistance, LayerIndex.world.mask) && raycastHit.normal.y > BaseLightningAim.normalYThreshold)
			{
				vector2 = raycastHit.normal;
				vector = Vector3.Cross(lhs, vector2);
				result.ok = true;
			}
			result.rotation = Util.QuaternionSafeLookRotation(vector, vector2);
			Vector3 point = raycastHit.point;
			result.position = point;
			return result;
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x0006E4AF File Offset: 0x0006C6AF
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		// Token: 0x04003FA7 RID: 16295
		public static GameObject crosshairOverridePrefab;

		// Token: 0x04003FA8 RID: 16296
		public static string enterSoundString;

		// Token: 0x04003FA9 RID: 16297
		public static string exitSoundString;

		// Token: 0x04003FAA RID: 16298
		public static GameObject effectMuzzlePrefab;

		// Token: 0x04003FAB RID: 16299
		public static string effectMuzzleString;

		// Token: 0x04003FAC RID: 16300
		public static float baseExitDuration;

		// Token: 0x04003FAD RID: 16301
		public static float maxPlacementDistance;

		// Token: 0x04003FAE RID: 16302
		public static GameObject blueprintPrefab;

		// Token: 0x04003FAF RID: 16303
		public static float normalYThreshold;

		// Token: 0x04003FB0 RID: 16304
		private BaseLightningAim.PlacementInfo currentPlacementInfo;

		// Token: 0x04003FB1 RID: 16305
		private GameObject defaultCrosshairPrefab;

		// Token: 0x04003FB2 RID: 16306
		private GenericSkill primarySkillSlot;

		// Token: 0x04003FB3 RID: 16307
		private AimAnimator modelAimAnimator;

		// Token: 0x04003FB4 RID: 16308
		private GameObject effectMuzzleInstance;

		// Token: 0x04003FB5 RID: 16309
		private Animator modelAnimator;

		// Token: 0x04003FB6 RID: 16310
		private float timerSinceComplete;

		// Token: 0x04003FB7 RID: 16311
		private bool beginExit;

		// Token: 0x04003FB8 RID: 16312
		private GenericSkill originalPrimarySkill;

		// Token: 0x04003FB9 RID: 16313
		private GenericSkill originalSecondarySkill;

		// Token: 0x04003FBA RID: 16314
		private BlueprintController blueprints;

		// Token: 0x04003FBB RID: 16315
		private CameraTargetParams.AimRequest aimRequest;

		// Token: 0x02000C05 RID: 3077
		public struct PlacementInfo
		{
			// Token: 0x060045D0 RID: 17872 RVA: 0x0011AABB File Offset: 0x00118CBB
			public void Serialize(NetworkWriter writer)
			{
				writer.Write(this.ok);
				writer.Write(this.position);
				writer.Write(this.rotation);
			}

			// Token: 0x060045D1 RID: 17873 RVA: 0x0011AAE1 File Offset: 0x00118CE1
			public void Deserialize(NetworkReader reader)
			{
				this.ok = reader.ReadBoolean();
				this.position = reader.ReadVector3();
				this.rotation = reader.ReadQuaternion();
			}

			// Token: 0x04003FBC RID: 16316
			public bool ok;

			// Token: 0x04003FBD RID: 16317
			public Vector3 position;

			// Token: 0x04003FBE RID: 16318
			public Quaternion rotation;
		}
	}
}
