using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace HenryMod.SkillStates
{

	public class Bolt : BaseSkillState
	{
		

		//copied volcanic egg code
		public override void OnEnter()
		{

			Ray aimRay = this.GetAimRay();

			//instantiate bolt prefab to be used in tandem with boltvehicle class
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BoltVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			//adds boltvehicle to the bolt prefab and assigns character to bolt prefab's vehicle seat, finalizing the gameobject that will act as the primary enactor of the bolt skill
			gameObject.AddComponent<BoltVehicle>();
			gameObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
			gameObject.AddComponent<HitBox>();

			//declares object that will be used as a vehicle; in this case, the "fireballvehicle" from risk of rain 2. this uses the fireballvehicle from the game's asset bundle, so the skill will work like a shorter volcanic egg if this line is uncommented	
			//GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));


			//not sure what this does, think it's something with making it work with multiplayer but since i just copy pasted from volcanic egg for this part it's anyone's guess
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
				NetworkServer.SpawnWithClientAuthority(gameObject, networkUser2.gameObject);
			}
			else
			{
				NetworkServer.Spawn(gameObject);
			}

			
		}

		//basic fixedupdate override, you know the drill
		//need to figure out what to add in order to make the skill switch states only if detonateserver is called so cooldown will only start after
		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (base.isAuthority && !gameObject)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}


		
		public override void OnExit()
        {

			base.OnExit();

        }


	}

}