using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using HenryMod.SkillStates;


namespace HenryMod.SkillStates
{

	public class Bolt : BaseSkillState
	{
		private float duration;
		private float delay = .2f;
		public GameObject boltObject;

		//copied volcanic egg code
		public override void OnEnter()
		{

			base.OnEnter();
			

			Ray aimRay = this.GetAimRay();

			//instantiate bolt prefab to be used in tandem with boltvehicle class
			boltObject = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("BoltVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			//adds boltvehicle to the bolt prefab and assigns character to bolt prefab's vehicle seat, finalizing the gameobject that will act as the primary enactor of the bolt skill
			boltObject.AddComponent<BoltVehicle>();
			Destroy(boltObject.GetComponent<CameraTargetParams>());
			boltObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
			boltObject.AddComponent<HitBox>();

            //declares object that will be used as a vehicle; in this case, the "fireballvehicle" from risk of rain 2. this uses the fireballvehicle from the game's asset bundle, so the skill will work like a shorter volcanic egg if this line is uncommented	
            //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));

            //sets duration of skill equal to what it's set to in boltvehicle
            duration = boltObject.GetComponent<BoltVehicle>().duration;


            //not sure what this does, think it's something with making it work with multiplayer but since i just copy pasted from volcanic egg for this part it's anyone's guess
            #region   
            
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
            #endregion

        }


        //basic fixedupdate override, you know the drill
        //makes it so cooldown only starts when boltObject is destroyed, .i.e. when the player manually cancels or when duration runs out
        public override void FixedUpdate()
		{

			base.FixedUpdate();

			if (fixedAge > delay)
            {
				//makes skill cancel if they hit the button again
				if (base.inputBank.skill3.justPressed)
				{
					boltObject.GetComponent<BoltVehicle>().DetonateServer();
					this.outer.SetNextStateToMain();
				}
				
			}
		

			if (!boltObject)
            {
				this.outer.SetNextStateToMain();
            }
	

			

		}



	


	}

}