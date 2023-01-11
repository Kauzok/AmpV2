using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    class NemAmpLightningController : MonoBehaviour
    {
        private CharacterModel characterModel;
        private void Awake()
        {
            characterModel = base.GetComponent<CharacterModel>();
        }
    }
}
