
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RoR2.Orbs;
using UnityEngine;
using RoR2;

namespace HenryMod.SkillStates.BaseStates
{
    //orb meant to be used to implement chain lightning function for fulmination skill
   public class FulminationOrb : LightningOrb
    {
        public override void Begin()
        {
            base.Begin();
            string path;
            
            path = "Prefabs/Effects/OrbEffects/LightningOrbEffect";

            EffectData effectData = new EffectData
            {
                origin = this.origin,
                genericFloat = base.duration
            };
            effectData.SetHurtBoxReference(this.target);
            EffectManager.SpawnEffect(Resources.Load<GameObject>(path), effectData, true);

        }
    }


}
