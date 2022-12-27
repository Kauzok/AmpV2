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
				case LightningOrb.LightningType.BFG:
					path = "Prefabs/Effects/OrbEffects/BeamSphereOrbEffect";
					base.duration = 0.4f;
					break;
				case LightningOrb.LightningType.TreePoisonDart:
					path = "Prefabs/Effects/OrbEffects/TreePoisonDartOrbEffect";
					this.speed = 40f;
					base.duration = base.distanceToTarget / this.speed;
					break;
				case LightningOrb.LightningType.HuntressGlaive:
					path = "Prefabs/Effects/OrbEffects/HuntressGlaiveOrbEffect";
					base.duration = base.distanceToTarget / this.speed;
					this.canBounceOnSameTarget = true;
					break;
				case LightningOrb.LightningType.Loader:
					path = "Prefabs/Effects/OrbEffects/LoaderLightningOrbEffect";
					break;
				case LightningOrb.LightningType.RazorWire:
					path = "Prefabs/Effects/OrbEffects/RazorwireOrbEffect";
					base.duration = 0.2f;
					break;
				case LightningOrb.LightningType.CrocoDisease:
					path = "Prefabs/Effects/OrbEffects/CrocoDiseaseOrbEffect";
					base.duration = 0.6f;
					this.targetsToFindPerBounce = 2;
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
			EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>(path), effectData, true);
		}
	}
}
