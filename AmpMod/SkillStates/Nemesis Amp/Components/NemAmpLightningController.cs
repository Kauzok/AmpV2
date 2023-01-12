using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using AmpMod.Modules;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    class NemAmpLightningController : MonoBehaviour
    {
        private CharacterModel characterModel;
        private CharacterBody characterBody;
        private void Awake()
        {
            characterModel = base.GetComponent<CharacterModel>();
            characterBody = base.GetComponent<CharacterBody>();
        }

        private void FixedUpdate()
        {
            if (characterBody.GetBuffCount(Buffs.damageGrowth) == StaticValues.growthBuffMaxStacks)
            {

            }
        }
    }
}
