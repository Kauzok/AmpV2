using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    public class NemAmpSpawnVFXController : MonoBehaviour
    {
        public GameObject ringVFX = Assets.spawnSecondaryExplosionEffect;
        public CharacterBody characterBody;
        public void SpawnReleaseRing()
        {
            
            EffectData ringEffect = new EffectData
            {
                scale = 1,
                origin = characterBody.corePosition,
            };
            EffectManager.SpawnEffect(ringVFX, ringEffect, true);
        }
    }
}
