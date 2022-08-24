using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.SkillStates;
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
		public NetworkUser networkUser;
		public NetworkUser networkUser2;
		private GenericSkill utilitySlot;
		private float ageCheck;
		private bool exitedEarly;
		public static SkillDef cancelSkillDef;
		string prefix = AmpPlugin.developerPrefix;

		private void UpdateAuthority()
		{
			hasEffectiveAuthority = Util.HasEffectiveAuthority(base.gameObject);
		}

		public override void OnEnter()
		{
			if (!NetworkServer.active) return;


			base.OnEnter();

			cancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
			{
				skillName = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
				skillNameToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
				skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_DESCRIPTION",
				skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCancelSurge"),
				activationStateMachineName = "Slide",
				baseMaxStock = 0,
				baseRechargeInterval = 0,
				beginSkillCooldownOnSkillEnd = false,
				canceledFromSprinting = false,
				forceSprintDuringState = false,
				fullRestockOnAssign = false,
				interruptPriority = EntityStates.InterruptPriority.Any,
				resetCooldownTimerOnUse = false,
				isCombatSkill = false,
				mustKeyPress = true,
				cancelSprintingOnActivation = false,
				rechargeStock = 0,
				requiredStock = 0,
				stockToConsume = 0,
			});


			utilitySlot = base.skillLocator.utility;
			if (this.utilitySlot && cancelSkillDef)
			{
				this.utilitySlot.SetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}


			Ray aimRay = GetAimRay();

			//instantiate bolt prefab to be used in tandem with boltvehicle class
			boltObject = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.boltVehicle, aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			//declares object that will be used as a vehicle; in this case, the "fireballvehicle" from risk of rain 2. this uses the fireballvehicle from the game's asset bundle, so the skill will work like a shorter volcanic egg if this line is uncommented	
			//boltObject = UnityEngine.Object.Instantiate<GameObject>(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			boltObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
            //NetworkServer.Spawn(boltObject);


			#region Skill Networking
            //stuff to make it work with multiplayer
            CharacterBody characterBody = this.characterBody;

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
			networkUser2 = networkUser;

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
			if (!NetworkServer.active) return;
			if (NetworkServer.active && !base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
			{
				base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
				base.characterMotor.onHitGroundServer += this.CharacterMotor_onHitGround;
			}

			if (utilitySlot && cancelSkillDef)
			{
				utilitySlot.UnsetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
			}

			base.OnExit();


			//boltObject.GetComponent<BoltVehicle>().DetonateServer();
			var bolt = boltObject?.GetComponent<BoltVehicle>();
			var boltSeat = boltObject?.GetComponent<VehicleSeat>();
			/*if (bolt && ageCheck < duration)
			{
				Debug.Log("exiting early");
				boltSeat.exitVelocityFraction = 0f;
				bolt.DetonateServer();
			} */
			if (bolt)
            {
				bolt.DetonateServer();
			}

		}




	}

}