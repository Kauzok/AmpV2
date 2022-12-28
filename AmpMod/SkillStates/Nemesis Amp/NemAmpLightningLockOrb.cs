using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Orbs;
using UnityEngine;
using RoR2;

namespace AmpMod.SkillStates.Nemesis_Amp
{
	class NemAmpLightningLockOrb : LightningOrb
	{
		// Token: 0x060040C3 RID: 16579 RVA: 0x0010BF54 File Offset: 0x0010A154
		public override void Begin()
		{
			base.duration = 0.1f;
			string path = null;
			switch (this.lightningType)
			{
				case LightningOrb.LightningType.Ukulele:
					path = "Prefabs/Effects/OrbEffects/LightningOrbEffect";
					break;
				case LightningOrb.LightningType.Tesla:
					path = "Prefabs/Effects/OrbEffects/TeslaOrbEffect";
					break;
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
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(this.target);
			if (this.lightningType != LightningType.Loader)
            {
				EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>(path), effectData, true);
			}
			
		}
	}
}
