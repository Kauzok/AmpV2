using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.Modules;
using RoR2.Skills;

namespace AmpMod.SkillStates
{


    public class Surge : BaseSkillState
	{
		private float duration = 1.5f;
		private float delay = .2f;
		public GameObject boltObject;
		public InputBankTest inputBank;
		public CharacterMaster master;
		public bool naturalEnd;
		private bool hasEffectiveAuthority;
		private GameObject boltVehicle;
		private VehicleSeat boltSeat;
		private GenericSkill utilitySlot;
		NetworkBehaviour networkBehaviour;
		private float ageCheck;
		AmpLightningController lightningController;
		private bool exitedEarly;
		public static SkillDef cancelSkillDef;
		string prefix = AmpPlugin.developerPrefix;

		private void UpdateAuthority()
		{
			hasEffectiveAuthority = Util.HasEffectiveAuthority(base.gameObject);
		}


		public override void OnEnter()
		{
			

			base.OnEnter();

			if (!NetworkServer.active) return;

			UpdateAuthority();


			lightningController = base.GetComponent<AmpLightningController>();
			networkBehaviour = base.GetComponent<NetworkBehaviour>();


			cancelSkillDef = Skills.surgeCancelSkillDef;


			utilitySlot = base.skillLocator.utility;
			if (this.utilitySlot && cancelSkillDef)
			{
				this.utilitySlot.SetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}

			//FireSurgeDash();




			Ray aimRay = GetAimRay();

			//instantiate bolt prefab to be used in tandem with boltvehicle class
			boltObject = UnityEngine.Object.Instantiate<GameObject>(lightningController.surgeVehicle, aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			//declares object that will be used as a vehicle; in this case, the "fireballvehicle" this uses the fireballvehicle from the game's asset bundle, so the skill will work like a shorter volcanic egg if this line is uncommented	
			//boltObject = UnityEngine.Object.Instantiate<GameObject>(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));
			//Debug.Log("setting passenger");

			boltObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
			//boltSeat = boltObject.GetComponent<VehicleSeat>();
			//NetworkServer.Spawn(boltObject);


			#region Skill Networking
			//stuff to make it work with multiplayer
			CharacterBody characterBody = this.characterBody;
			NetworkUser networkUser;

			if (characterBody == null)
			{
				networkUser = null;
			}
			else
			{
				CharacterMaster master = characterBody.master;
				if (master == null)
				{
					networkUser = null;
				}
				else
				{
					PlayerCharacterMasterController playerCharacterMasterController = master.playerCharacterMasterController;
					networkUser = ((playerCharacterMasterController != null) ? playerCharacterMasterController.networkUser : null);
				}
			}
			NetworkUser networkUser2 = networkUser;

			if (networkUser2)
			{
				NetworkServer.SpawnWithClientAuthority(boltObject, networkUser2.gameObject);
			}
			else
			{
				NetworkServer.Spawn(boltObject);
			}

				#endregion
			
		}



		



		//basic fixedupdate override, you know the drill
		//makes it so cooldown only starts when boltObject is destroyed, i.e. when the player manually cancels or when duration runs out
		public override void FixedUpdate()
		{

			base.FixedUpdate();


			ageCheck += Time.fixedDeltaTime;

			if (fixedAge > delay)
			{

				//makes skill cancel if they hit the button again
				if (base.inputBank.skill3.justPressed && base.isAuthority)
				{
					this.outer.SetNextStateToMain();
					return;
				}

			

			}
				//if duration runs out cancel skill
			if (fixedAge > duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				//ageCheck = fixedAge; something to try to fix the 'exiting early' thing in multiplayer
				return;

			}

		}
		//called in onExit instead of fixedUpdate to make it play nice with networking

		private void CharacterMotor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
		{
			if (base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
			{
				base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
			}



			base.characterMotor.onHitGroundServer -= this.CharacterMotor_onHitGround;
		}



		public override void OnExit()
        {

			base.OnExit();



			if (NetworkServer.active && !base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
			{
				base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
				base.characterMotor.onHitGroundServer += this.CharacterMotor_onHitGround;
			}

			if (utilitySlot && cancelSkillDef)
			{
				utilitySlot.UnsetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}

			

			//if (!NetworkServer.active) return;
			//boltObject.GetComponent<BoltVehicle>().DetonateServer();
			var bolt = boltObject?.GetComponent<BoltVehicle>();

			if (bolt && ageCheck < duration)
			{
				Debug.Log("exiting early");
				boltObject.GetComponent<VehicleSeat>().exitVelocityFraction = 0f;
				bolt.DetonateServer();
			} 
			else if (bolt)
            {
				bolt.DetonateServer();
			}

		}




	}

}