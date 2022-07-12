using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Projectile;
using RoR2;


	namespace AmpMod.SkillStates.SkillComponents
	{

		// Token: 0x02000BBB RID: 3003
		[RequireComponent(typeof(ChargedTargeting))]
		public class ChargedHoming : MonoBehaviour
		{
			private void Start()
			{
				if (!NetworkServer.active)
				{
					base.enabled = false;
					return;
				}
				this.transform = base.transform;
				targetComponent = base.GetComponent<ChargedTargeting>();
			}

			private void FixedUpdate()
			{	
				
				if (this.targetComponent.target)
				{
					//if (targetComponent.target.GetComponent<HurtBox>().healthComponent.body.HasBuff(Modules.Buffs.chargeBuildup))
					

					Vector3 vector = this.targetComponent.target.transform.position - this.transform.position;
					if (this.yAxisOnly)
					{
						vector.y = 0f;
					}
					if (vector != Vector3.zero)
					{
						this.transform.forward = Vector3.RotateTowards(this.transform.forward, vector, this.rotationSpeed * 0.017453292f * Time.fixedDeltaTime, 0f);
						//Debug.Log("homing");
					}
					
					
					/* else if (!targetComponent.target.gameObject.GetComponent<CharacterBody>())
					{
					Debug.Log("No Character Body Found");
					} */
				}
			}

		// Token: 0x040042E7 RID: 17127
			[Tooltip("Constrains rotation to the Y axis only.")]
			public bool yAxisOnly = false;

			// Token: 0x040042E8 RID: 17128
			[Tooltip("How fast to rotate in degrees per second. Rotation is linear.")]
			public float rotationSpeed = 120;

			// Token: 0x040042E9 RID: 17129
			private new Transform transform;

			// Token: 0x040042EA RID: 17130
			private ChargedTargeting targetComponent;
		}
	}



