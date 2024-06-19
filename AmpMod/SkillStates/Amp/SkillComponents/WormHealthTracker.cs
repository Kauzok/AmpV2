using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.SkillComponents
{

    class WormHealthTracker : MonoBehaviour
    {
        public GenericSkill specialSlot;
        public CharacterBody wormBody;
        public bool hasUnset;
        public CharacterMaster wormMaster;
        public bool hasDied;
        public GameObject owner;
        public object wormSkill;
        public SkillDef cancelSkillDef;
        public Quaternion rotation;

        public void Awake()
        {
            wormBody = base.GetComponent<CharacterBody>();

        }


        private void FixedUpdate()
        {
          /*  if (wormBody.healthComponent.health <= 0f || wormBody.isActiveAndEnabled == false) //&& wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife) == 0 && wormMaster.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) == 0)
            {   
                if (!hasDied)
                {
                    hasDied = true;
                    Debug.Log("Worm has died");
                }
               


            } */
        }
    }
}
