using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;

namespace AmpMod.SkillStates.SkillComponents
{
    class PlayMenuSound : MonoBehaviour
    {
        public string soundString;
        private void OnEnable()
        {
            Util.PlaySound(soundString, base.gameObject);
        }
    }
}
