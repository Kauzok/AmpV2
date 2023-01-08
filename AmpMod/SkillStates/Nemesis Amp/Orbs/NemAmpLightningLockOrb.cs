using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Orbs;
using UnityEngine;
using RoR2.Orbs;
using RoR2;
using Debug = UnityEngine.Debug;

namespace AmpMod.SkillStates.Nemesis_Amp
{
	class NemAmpLightningLockOrb : GenericDamageOrb
	{
		public LightningOrb.LightningType lightningType;
		public override void Begin()
		{
            //Debug.Log("lightning attack spawning");
            //base.Begin();
            base.duration = 0.1f;
			//this.speed = 120f;
			string path = null;
			switch (this.lightningType)
			{
				case LightningOrb.LightningType.Loader:
					path = null;
					break;
				case LightningOrb.LightningType.MageLightning:
					path = "Prefabs/Effects/OrbEffects/MageLightningOrbEffect";
					base.duration = 0.1f;
					break;
			}
			EffectData effectData = new EffectData
			{
				//origin = this.origin,
				origin = this.target.healthComponent.gameObject.transform.position,
				genericFloat = base.duration,
			};
			effectData.SetHurtBoxReference(this.target);
			if (this.lightningType != LightningOrb.LightningType.Loader)
            {
				EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>(path), effectData, true);
			}
            else
            {
				EffectManager.SpawnEffect(Modules.Assets.lightningStreamImpactEffect, effectData, true);
			}
			
		}
	}
}
