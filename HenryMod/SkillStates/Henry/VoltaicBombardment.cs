using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Huntress
{
	// Token: 0x02000B11 RID: 2833
	public class VoltaicBombardment : BaseLightningAim
	{
		// Token: 0x0600413A RID: 16698 RVA: 0x00103A50 File Offset: 0x00101C50
		public override void OnEnter()
		{
			base.OnEnter();
			base.PlayAnimation("FullBody, Override", "LoopArrowRain");
			if (ArrowRain.areaIndicatorPrefab)
			{
				this.areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
				this.areaIndicatorInstance.transform.localScale = new Vector3(ArrowRain.arrowRainRadius, ArrowRain.arrowRainRadius, ArrowRain.arrowRainRadius);
			}
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x00103AB4 File Offset: 0x00101CB4
		private void UpdateAreaIndicator()
		{
			if (this.areaIndicatorInstance)
			{
				float maxDistance = 1000f;
				RaycastHit raycastHit;
				if (Physics.Raycast(base.GetAimRay(), out raycastHit, maxDistance, LayerIndex.world.mask))
				{
					this.areaIndicatorInstance.transform.position = raycastHit.point;
					this.areaIndicatorInstance.transform.up = raycastHit.normal;
				}
			}
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00103B24 File Offset: 0x00101D24
		public override void Update()
		{
			base.Update();
			this.UpdateAreaIndicator();
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00103B32 File Offset: 0x00101D32
		protected override void HandlePrimaryAttack()
		{
			base.HandlePrimaryAttack();
			this.shouldFireArrowRain = true;
			this.outer.SetNextStateToMain();
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00103B4C File Offset: 0x00101D4C
		protected void DoFireArrowRain()
		{
			EffectManager.SimpleMuzzleFlash(ArrowRain.muzzleFlashEffect, base.gameObject, "Muzzle", false);
			if (this.areaIndicatorInstance && this.shouldFireArrowRain)
			{
				ProjectileManager.instance.FireProjectile(ArrowRain.projectilePrefab, this.areaIndicatorInstance.transform.position, this.areaIndicatorInstance.transform.rotation, base.gameObject, this.damageStat * ArrowRain.damageCoefficient, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00103BE7 File Offset: 0x00101DE7
		public override void OnExit()
		{
			if (this.shouldFireArrowRain && !this.outer.destroying)
			{
				this.DoFireArrowRain();
			}
			if (this.areaIndicatorInstance)
			{
				EntityState.Destroy(this.areaIndicatorInstance.gameObject);
			}
			base.OnExit();
		}

		// Token: 0x0400394A RID: 14666
		public static float arrowRainRadius;

		// Token: 0x0400394B RID: 14667
		public static float damageCoefficient;

		// Token: 0x0400394C RID: 14668
		public static GameObject projectilePrefab;

		// Token: 0x0400394D RID: 14669
		public static GameObject areaIndicatorPrefab;

		// Token: 0x0400394E RID: 14670
		public static GameObject muzzleFlashEffect;

		// Token: 0x0400394F RID: 14671
		private GameObject areaIndicatorInstance;

		// Token: 0x04003950 RID: 14672
		private bool shouldFireArrowRain;
	}
}
