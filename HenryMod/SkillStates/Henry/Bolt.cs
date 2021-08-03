using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace HenryMod.SkillStates
{

	public class Bolt : BaseBoltSkill
	{
		public CharacterBody characterBody;
		public HealthComponent healthComponent;
		public InputBankTest inputBank;
		public TeamComponent teamComponent;
		public Indicator targetIndicator;


		public override void OnEnter()
		{
			this.characterBody = base.GetComponent<CharacterBody>();
			this.healthComponent = base.GetComponent<HealthComponent>();
			this.inputBank = base.GetComponent<InputBankTest>();
			this.teamComponent = base.GetComponent<TeamComponent>();
			this.targetIndicator = new Indicator(base.gameObject, null);

			Ray aimRay = this.GetAimRay();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));
			//GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NetworkedObjects/FireballVehicle"), aimRay.origin, Quaternion.LookRotation(aimRay.direction));
			gameObject.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
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


	}

}