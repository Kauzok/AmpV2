using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.SkillComponents
{
    [RequireComponent(typeof(WormSkillOverride))]
    class WormHealthTracker : MonoBehaviour
    {
        public GenericSkill specialSlot;
        public CharacterBody wormBody;
        public bool hasUnset;
        public CharacterMaster wormMaster;
        public bool hasDied;
        private WormSkillOverride wormOverride;

        public void Awake()
        {
            wormOverride = base.GetComponent<WormSkillOverride>();

        }


        private void FixedUpdate()
        {
            if (wormBody.healthComponent.health <= 0f || wormBody.isActiveAndEnabled == false) //&& wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife) == 0 && wormMaster.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) == 0)
            {   
                if (!hasDied)
                {
                    hasDied = true;
                    Debug.Log("Worm has died");
                }
               
                if (!hasUnset)
                {
                    hasUnset = true;
                    //Debug.Log("unsetting overrides");
                    wormOverride.RpcUnSetOverride();
                    //Destroy(base.gameObject);

                }

            }
        }
    }
}
