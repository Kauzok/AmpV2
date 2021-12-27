using EntityStates;
using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.Collections.Generic;
using System.Timers;
using R2API;

namespace HenryMod.SkillStates
{
	// Token: 0x0200028D RID: 653
	[RequireComponent(typeof(VehicleSeat))]
	[RequireComponent(typeof(Rigidbody))]
	public class BoltVehicle : MonoBehaviour, ICameraStateProvider
	{
		[Header("Vehicle Parameters")]
		public float duration = 10f;
		public float initialSpeed = 50f;
		public float targetSpeed = 50f;
		public float acceleration = 1000f;
		public float cameraLerpTime = .25f;

		[Header("Blast Parameters")]
		public bool detonateOnCollision;
		public GameObject exitEffectPrefab;
		public string explosionSoundString;
		public float blastRadius = 1f;

		[Header("Overlap Parameters")]
		public float overlapDamageCoefficient = .5f;
		public float overlapProcCoefficient = 1f;
		public float overlapForce = .5f;
		public float overlapFireFrequency = 30f;
		public float overlapResetFrequency = 1f;
		public float overlapVehicleDurationBonusPerHit;
		public GameObject overlapHitEffectPrefab;

		[Header("Misc. Variables")]
		private float age;
		public bool hasDetonatedServer;
		private VehicleSeat vehicleSeat;
		private Rigidbody rigidbody;
		private OverlapAttack overlapAttack;
		private float overlapFireAge;
		private float overlapResetAge;


		public void Awake()
		{
			hasDetonatedServer = false;
			vehicleSeat = base.GetComponent<VehicleSeat>();
			this.vehicleSeat.onPassengerEnter += this.OnPassengerEnter;
			this.vehicleSeat.onPassengerExit += this.OnPassengerExit;
			this.rigidbody = base.GetComponent<Rigidbody>();
			
		}


		private void OnPassengerExit(GameObject passenger)
		{
			if (NetworkServer.active)
			{
				this.DetonateServer();
			}
			foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList)
			{
				if (cameraRigController.target == passenger)
				{
					//original values: 0f, this.cameralerptime
					cameraRigController.SetOverrideCam(this, .5f);
					cameraRigController.SetOverrideCam(null, .5f);
				}
			}
		}

		private void OnPassengerEnter(GameObject passenger)
		{
			if (!this.vehicleSeat.currentPassengerInputBank)
			{
				return;
			}
			Vector3 aimDirection = vehicleSeat.currentPassengerInputBank.aimDirection;
			rigidbody.rotation = Quaternion.LookRotation(aimDirection);
			rigidbody.velocity = aimDirection * initialSpeed;
			CharacterBody currentPassengerBody = vehicleSeat.currentPassengerBody;
			currentPassengerBody.AddBuff(Modules.Buffs.invulnerableBuff);
			overlapAttack = new OverlapAttack
			{
				attacker = currentPassengerBody.gameObject,
				damage = overlapDamageCoefficient * currentPassengerBody.damage,
				pushAwayForce = overlapForce,
				isCrit = currentPassengerBody.RollCrit(),
				damageColorIndex = DamageColorIndex.Item,
				inflictor = base.gameObject,
				procChainMask = default(ProcChainMask),
				procCoefficient = 1f,
				teamIndex = currentPassengerBody.teamComponent.teamIndex,
				hitBoxGroup = base.gameObject.GetComponent<HitBoxGroup>(),
				hitEffectPrefab = overlapHitEffectPrefab
			};
			overlapAttack.AddModdedDamageType(Modules.DamageTypes.applyCharge);
			
			
		}
		
		public void DetonateServer()
		{
			if (hasDetonatedServer)
			{
				return;
			}
			hasDetonatedServer = true;
			CharacterBody currentPassengerBody = vehicleSeat.currentPassengerBody;
			currentPassengerBody.RemoveBuff(Modules.Buffs.invulnerableBuff);
			if (currentPassengerBody)
			{
				EffectData effectData = new EffectData
				{
					origin = base.transform.position,
					scale = blastRadius
				};
				exitEffectPrefab = Modules.Assets.electricExplosionEffect;
				//exitEffectPrefab = Modules.Assets.testLightningEffect;
				EffectManager.SpawnEffect(exitEffectPrefab, effectData, true);
		
			}
			Util.PlaySound(explosionSoundString, base.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
			
		}

		
		private void FixedUpdate()
		{
			if (!vehicleSeat)
			{
				return;
			}
			if (!this.vehicleSeat.currentPassengerInputBank)
			{
				return;
			}
			age += Time.fixedDeltaTime;
			this.overlapFireAge += Time.fixedDeltaTime;
			this.overlapResetAge += Time.fixedDeltaTime;


			if (NetworkServer.active)
			{

				
				if (overlapFireAge > 1f / overlapFireFrequency)
				{
					var hits = new List<HurtBox>();
					overlapAttack.Fire(hits);
					//figure out how to delay this reset
					foreach (HurtBox hit in hits)
					{

						//applyCharge(hit);
					


					}
				}
				if (overlapResetAge >= 1f / overlapResetFrequency)
				{
					overlapAttack.ResetIgnoredHealthComponents();
					overlapResetAge = 0f;
				} 

			}
			Ray originalAimRay = vehicleSeat.currentPassengerInputBank.GetAimRay();
			float num;
			originalAimRay = CameraRigController.ModifyAimRayIfApplicable(originalAimRay, base.gameObject, out num);
			Vector3 velocity = this.rigidbody.velocity;
			Vector3 target = originalAimRay.direction * this.targetSpeed;
			Vector3 a = Vector3.MoveTowards(velocity, target, this.acceleration * Time.fixedDeltaTime);
			this.rigidbody.MoveRotation(Quaternion.LookRotation(originalAimRay.direction));
			this.rigidbody.AddForce(a - velocity, ForceMode.VelocityChange);
			if (NetworkServer.active && duration <= age)
			{
				DetonateServer();
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (this.detonateOnCollision && NetworkServer.active)
			{
				this.DetonateServer();
			
			}
		}

		public void GetCameraState(CameraRigController cameraRigController, ref CameraState cameraState)
		{
		}

		public bool IsUserLookAllowed(CameraRigController cameraRigController)
		{
			return true;
		}

		public bool IsUserControlAllowed(CameraRigController cameraRigController)
		{
			return true;
		}

		public bool IsHudAllowed(CameraRigController cameraRigController)
		{
			return true;
		}

	
	}
}

