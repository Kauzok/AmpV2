using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace AmpMod.SkillStates.SkillComponents
{
	internal class SandedController : MonoBehaviour
	{
		private TemporaryOverlay temporaryOverlay;
		public GameObject target;
		private Material sandedMaterial = Modules.Assets.sandedMaterial;
		public CharacterBody sandedBody;
		private float duration = Modules.StaticValues.sandedDuration;
		private float age;

		private void Start()
		{
			this.temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
			this.temporaryOverlay.originalMaterial = this.sandedMaterial;


			if (this.target)
			{
				CharacterModel component = this.target.GetComponent<CharacterModel>();
				if (component)
				{
					if (this.temporaryOverlay)
					{
						this.temporaryOverlay.AddToCharacerModel(component);
					}

				}
			}
		}



		private void FixedUpdate()
        {
			
			if (!sandedBody.HasBuff(Modules.Buffs.sandedDebuff)) 
			{
				if (this.temporaryOverlay)
				{
					Debug.Log(age);
					UnityEngine.Object.Destroy(this.temporaryOverlay);
				}
				UnityEngine.Component.Destroy(this);
			}
        
        }

		private void OnDestroy()
		{

			


		}
	}
}
