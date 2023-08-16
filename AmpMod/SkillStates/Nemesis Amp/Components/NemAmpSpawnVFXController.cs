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
        public GameObject burstVFX = Assets.spawnSecondaryExplosionEffect;
        public GameObject dustVFX = Assets.spawnDustEffect;
        private String dustSFX = Modules.StaticValues.spawnDustSoundString;
        private string burstSFX = Modules.StaticValues.spawnBurstSoundString;
        public ChildLocator childLocator;
        private CharacterModel characterModel;
        public CharacterBody characterBody;
        private bool isBlue;
        private void Start()
        {
            
            characterBody = base.GetComponent<CharacterBody>();
            childLocator = base.gameObject.GetComponent<ChildLocator>();
            if (this.characterModel.GetComponent<ModelSkinController>().skins[this.characterBody.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME")
            {
                isBlue = true;
            }

            if (isBlue)
            {
                burstVFX = Assets.spawnSecondaryExplosionEffectBlue;
            }
            else
            {
                burstVFX = Assets.spawnSecondaryExplosionEffect;
            }
        }
        public void SpawnReleaseRing()
        {
            
            EffectData ringEffect = new EffectData
            {
                scale = 1,
                origin = characterBody.corePosition,
            };
            EffectManager.SpawnEffect(burstVFX, ringEffect, true);
            Util.PlaySound(burstSFX, base.gameObject);
        }

        public void SpawnDust()
        {
            Transform leftHandTransform = childLocator.FindChild("HandL");
            EffectData dustEffect = new EffectData
            {
                scale = 1,
                origin = leftHandTransform.position,    
            };
            EffectManager.SpawnEffect(dustVFX, dustEffect, true);
            Util.PlaySound(dustSFX, base.gameObject);
            
            
        }
    }
    
}
