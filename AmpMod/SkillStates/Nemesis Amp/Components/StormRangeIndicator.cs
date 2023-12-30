using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using AmpMod.Modules;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    internal class StormRangeIndicator : MonoBehaviour
    {

        public CharacterBody body;
        private bool isBlue;
        public CharacterModel characterModel;
        private bool isActive;
        MeshRenderer renderer;

        private void Start()
        {

            renderer = base.gameObject.GetComponentInChildren<MeshRenderer>();
            //Debug.Log("found renderer as " + renderer);
        }

        private void FixedUpdate()
        {
            if (body)
            {
                bool hasStormSkill = (body.skillLocator.special.skillNameToken == "NT_NEMAMP_BODY_SPECIAL_SUMMONSTORM_NAME");

                if (!hasStormSkill && renderer.enabled)
                {
                    renderer.enabled = false;
                }
                else if (hasStormSkill && !renderer.enabled)
                {
                    renderer.enabled = true;
                }
            }
           
        }


    }
}
