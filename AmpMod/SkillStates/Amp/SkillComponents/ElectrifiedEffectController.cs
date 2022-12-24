using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace AmpMod.SkillStates.SkillComponents
{
	internal class ElectrifiedEffectController : MonoBehaviour
	{
		private TemporaryOverlay temporaryOverlay;
		public GameObject target;
		private Material electrifiedMaterial = Modules.Assets.electrifiedMaterial;
		public CharacterBody electrifiedBody;
		private float duration = Modules.StaticValues.electrifiedDuration;
		private float age;

		private void Start()
		{
			this.temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
			this.temporaryOverlay.originalMaterial = this.electrifiedMaterial;


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
			
			if (!electrifiedBody.HasBuff(Modules.Buffs.electrified)) 
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
