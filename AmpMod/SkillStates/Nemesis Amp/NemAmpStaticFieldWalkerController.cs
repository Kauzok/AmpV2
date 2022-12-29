using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    internal class NemAmpStaticFieldWalkerController : MonoBehaviour
	{
		public float dropInterval = 0.15f;

		public GameObject firePillarPrefab;

		public float pillarAngle = 45f;

		public bool curveToCenter = true;
		private float moveSign;
		private ProjectileController projectileController;

		private ProjectileDamage projectileDamage;
		private Vector3 lastCenterPosition;
		private float timer;
		private Vector3 currentPillarVector = Vector3.up;
		private void Awake()
		{
			this.projectileController = base.GetComponent<ProjectileController>();
			this.projectileDamage = base.GetComponent<ProjectileDamage>();
			this.lastCenterPosition = base.transform.position;
			this.timer = this.dropInterval / 2f;
			this.moveSign = 1f;
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x00119A90 File Offset: 0x00117C90
		private void Start()
		{
			if (this.projectileController.owner)
			{
				Vector3 position = this.projectileController.owner.transform.position;
				Vector3 vector = base.transform.position - position;
				vector.y = 0f;
				if (vector.x != 0f && vector.z != 0f)
				{
					this.moveSign = Mathf.Sign(Vector3.Dot(base.transform.right, vector));
				}
			}
			this.UpdateDirections();
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x00119B20 File Offset: 0x00117D20
		private void UpdateDirections()
		{
			if (!this.curveToCenter)
			{
				return;
			}
			Vector3 vector = base.transform.position - this.lastCenterPosition;
			vector.y = 0f;
			if (vector.x != 0f && vector.z != 0f)
			{
				vector.Normalize();
				Vector3 vector2 = Vector3.Cross(Vector3.up, vector);
				base.transform.forward = vector2 * this.moveSign;
				this.currentPillarVector = Quaternion.AngleAxis(this.pillarAngle, vector2) * Vector3.Cross(vector, vector2);
			}
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x00119BBC File Offset: 0x00117DBC
		private void FixedUpdate()
		{
			if (this.projectileController.owner)
			{
				this.lastCenterPosition = this.projectileController.owner.transform.position;
			}
			this.UpdateDirections();
			if (NetworkServer.active)
			{
				this.timer -= Time.fixedDeltaTime;
				if (this.timer <= 0f)
				{
					this.timer = this.dropInterval;
					if (this.firePillarPrefab)
					{
						ProjectileManager.instance.FireProjectile(this.firePillarPrefab, base.transform.position, Util.QuaternionSafeLookRotation(this.currentPillarVector), this.projectileController.owner, this.projectileDamage.damage, this.projectileDamage.force, this.projectileDamage.crit, this.projectileDamage.damageColorIndex, null, -1f);
					}
				}
			}
		}
	}
}
