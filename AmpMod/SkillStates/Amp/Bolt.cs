﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.SkillStates;
using R2API;


namespace AmpMod.SkillStates
{


	public class Bolt : BaseSkillState
	{
		private float duration;
		private float delay = .2f;
		public GameObject boltObject;
		public InputBankTest inputBank;
		public CharacterMaster master;
		private bool hasEffectiveAuthority;
		public NetworkUser networkUser;
		public NetworkUser networkUser2;

		private void UpdateAuthority()
		{
			this.hasEffectiveAuthority = Util.HasEffectiveAuthority(base.gameObject);
		}




		public override void OnEnter()
		{
			if (!NetworkServer.active) return;


			base.OnEnter();
			UpdateAuthority();

			Ray aimRay = GetAimRay();

			//instantiate bolt prefab to be used in tandem with boltvehicle class
			boltObject = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.boltVehicle, aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			//declares object that will be used as a vehicle; in this case, the "fireballvehicle" from risk of rain 2. this uses the fireballvehicle from the game's asset bundle, so the skill will work like a shorter volcanic egg if this line is uncommented	
			//boltObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));

			boltObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
            //NetworkServer.Spawn(boltObject);	


            //stuff to make it work with multiplayer
            #region networking
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
		}
			#endregion


        //basic fixedupdate override, you know the drill
        //makes it so cooldown only starts when boltObject is destroyed, .i.e. when the player manually cancels or when duration runs out
        public override void FixedUpdate()
		{

			base.FixedUpdate();

			if (NetworkServer.active)
			{

				if (networkUser2)
                {
					
                }

				if (fixedAge > delay)
				{
					//makes skill cancel if they hit the button again
					if (base.inputBank.skill3.justPressed)
					{
						boltObject.GetComponent<BoltVehicle>().DetonateServer();
						this.outer.SetNextStateToMain();
					}

				}


				//exit if boltobject is destroyed
				if (!boltObject)
				{
					this.outer.SetNextStateToMain();
				}

			}

		}

	}

}