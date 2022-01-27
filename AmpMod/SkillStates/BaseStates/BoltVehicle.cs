using EntityStates;
using System;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.Collections.Generic;
using System.Timers;
using R2API;

namespace AmpMod.SkillStates
{
	[RequireComponent(typeof(VehicleSeat))]
	[RequireComponent(typeof(Rigidbody))]
	public class BoltVehicle : MonoBehaviour, ICameraStateProvider
	{
		[Header("Vehicle Parameters")]
		public float duration = 2f;
		public float initialSpeed = 50f;
		public float targetSpeed = 50f;
		public float acceleration = 1000f;
		public string stateSoundString = Modules.StaticValues.boltState2SecWindString;
		public float cameraLerpTime = .25f;
		public GameObject enterEffectPrefab;
		public string enterSoundString = Modules.StaticValues.boltEnterString;
		public bool exitAllowed;
		public uint stopID;

		[Header("Blast Parameters")]
		public GameObject exitEffectPrefab;
		public string exitSoundString = Modules.StaticValues.boltExitString;
		public float blastRadius = 1f;
		private BlastAttack boltBlast;

		[Header("Overlap Parameters")]
		public float overlapDamageCoefficient = Modules.StaticValues.boltOverlapDamageCoefficient;
		public float overlapProcCoefficient = 1f;
		public float overlapForce = .5f;
		public float overlapFireFrequency = 30f;
		public float overlapResetFrequency = 1f;
		public float overlapVehicleDurationBonusPerHit;
		public GameObject overlapHitEffectPrefab = Modules.Assets.electricImpactEffect;

		[Header("Misc. Variables")]
		private float age;
		public bool hasDetonatedServer;
		private VehicleSeat vehicleSeat;
		private Rigidbody rigidbody;
		private OverlapAttack overlapAttack;
		private float overlapFireAge;
		private float overlapResetAge;
        private float blastDamageCoefficient = Modules.StaticValues.boltBlastDamageCoefficient;

        public void Awake()
		{

			hasDetonatedServer = false;

			//sets hooks to modify scripts that comprise the character's behavior on entry and exit of a vehicleseat
			vehicleSeat = base.GetComponent<VehicleSeat>();
			this.rigidbody = base.GetComponent<Rigidbody>();
			this.vehicleSeat.onPassengerEnter += this.OnPassengerEnter;
			this.vehicleSeat.onPassengerExit += this.OnPassengerExit;

			//makes it so you can't exit the vehicleseat manually; since i allow the player to do that in the actual bolt code, this is really just to remove the annoying prompt that shows up when this is enabled
			vehicleSeat.exitVehicleAllowedCheck.AddCallback(new CallbackCheck<Interactability, CharacterBody>.CallbackDelegate(this.CheckExitAllowed));

			
			
		}

		private void CheckExitAllowed(CharacterBody characterBody, ref Interactability? resultOverride)
		{
			resultOverride = new Interactability?(this.exitAllowed ? Interactability.Available : Interactability.Disabled);
		}


		//calls detonateserver and returns camera back to normal
		private void OnPassengerExit(GameObject passenger)
		{

			//stop state sound
			AkSoundEngine.StopPlayingID(stopID, 0);
			//Debug.Log("Stopping" + stopID);

			//play exit sound
			Util.PlaySound(exitSoundString, base.gameObject);

			if (NetworkServer.active)
			{
				DetonateServer();
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

		//initializes vehicleseat and bolt's overlapattack
		public void OnPassengerEnter(GameObject passenger)
		{
			if (!this.vehicleSeat.currentPassengerInputBank)
			{
				return;
			}
			
			//code for enter effect/sound
			EffectData enterEffectData = new EffectData
			{
				origin = vehicleSeat.currentPassengerBody.corePosition,
				scale = 10f
			};
			enterEffectPrefab = Modules.Assets.boltEnterEffect;
			EffectManager.SpawnEffect(enterEffectPrefab, enterEffectData, true);
			Util.PlaySound(enterSoundString, base.gameObject);
		

			//moves vehicle in direction of player's aim direction at previously set speed
			Vector3 aimDirection = vehicleSeat.currentPassengerInputBank.aimDirection;
			rigidbody.rotation = Quaternion.LookRotation(aimDirection);
			rigidbody.velocity = aimDirection * initialSpeed;

			//to be used as reference when need to call player's characterbody
			CharacterBody currentPassengerBody = vehicleSeat.currentPassengerBody;

			//gives invincibility when in boltstate
			currentPassengerBody.AddBuff(Modules.Buffs.invulnerableBuff);

			//creates overlapattack for damage and applying charge on hit
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

			//adjusts camera on boltstate entry; will adjust later to make camera transition smoother
			/*foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList)
			{
				if (cameraRigController.target == passenger)
				{
					//original values: 0f, this.cameralerptime
					cameraRigController.SetOverrideCam(this, 0f);
					cameraRigController.SetOverrideCam(null, this.cameraLerpTime);
				}
			} */

			//play state sound
			stopID = Util.PlaySound(stateSoundString, base.gameObject);			
		}

		//destroys gameobject and reverts player back to normal state
		public void DetonateServer()
		{
			if (hasDetonatedServer)
			{
				return;
			}
			hasDetonatedServer = true;

			//same purpose as stated above
			CharacterBody currentPassengerBody = vehicleSeat.currentPassengerBody;
			//removes invincibility
			currentPassengerBody.RemoveBuff(Modules.Buffs.invulnerableBuff);

			//spawns exiteffect on bolt state exit
			if (currentPassengerBody)
			{
				EffectData effectData = new EffectData
				{
					origin = base.transform.position,
					scale = 1.5f
				};
				exitEffectPrefab = Modules.Assets.boltExitEffect;
				//exitEffectPrefab = Modules.Assets.testLightningEffect;
				EffectManager.SpawnEffect(exitEffectPrefab, effectData, true);
		
			}
			//spawns blastattack that applies charge and deals 20% damage on exit
			boltBlast = new BlastAttack
			{
				attacker = currentPassengerBody.gameObject,
				baseDamage = blastDamageCoefficient * currentPassengerBody.damage,
				baseForce = 0f,
				attackerFiltering = AttackerFiltering.NeverHit,
				crit = currentPassengerBody.RollCrit(),
				damageColorIndex = DamageColorIndex.Item,
				damageType = DamageType.Stun1s,
				falloffModel = BlastAttack.FalloffModel.Linear,
				inflictor = base.gameObject,
				position = base.transform.position,
				procChainMask = default(ProcChainMask),
				procCoefficient = 1f,
				radius = 10f,
				teamIndex = currentPassengerBody.teamComponent.teamIndex
			};
			boltBlast.AddModdedDamageType(Modules.DamageTypes.applyCharge);
			boltBlast.Fire();

			//destroy vehicle
			UnityEngine.Object.Destroy(base.gameObject);
			
		}

		
		//fixedupdate to call every tick in game to update effects, attacks, etc.
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
				//fires the overlapattack
				if (overlapFireAge > 1f / overlapFireFrequency)
				{
					overlapAttack.Fire();

					//just a relic from when i was applying the charge debuff manually; keepin in case i need it for testing
					/* var hits = new List<HurtBox>();
					foreach (HurtBox hit in hits) { }*/
					
				}

				//resets ignored health components of the overlapattack at a frequency determined by vars; this is what determines how often the attack will apply itself to a hurtbox
				if (overlapResetAge >= 1f / overlapResetFrequency)
				{
					overlapAttack.ResetIgnoredHealthComponents();
					overlapResetAge = 0f;
				} 

			}
		
				//vectors and stuff to keep the playercharacter moving in boltstate
				Ray originalAimRay = vehicleSeat.currentPassengerInputBank.GetAimRay();
				float num;
				originalAimRay = CameraRigController.ModifyAimRayIfApplicable(originalAimRay, base.gameObject, out num);
				Vector3 velocity = this.rigidbody.velocity;
				Vector3 target = originalAimRay.direction * this.targetSpeed;
				Vector3 a = Vector3.MoveTowards(velocity, target, this.acceleration * Time.fixedDeltaTime);
				this.rigidbody.MoveRotation(Quaternion.LookRotation(originalAimRay.direction));
				this.rigidbody.AddForce(a - velocity, ForceMode.VelocityChange);
			
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

