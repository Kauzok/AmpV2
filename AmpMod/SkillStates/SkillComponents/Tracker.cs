using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity;
using UnityEngine.Networking;
using AmpMod.Modules;
using RoR2;

namespace AmpMod.SkillStates
{

    //tracker class used for custom debuff
    public class Tracker : NetworkBehaviour
    {
        public GameObject owner;
        public CharacterBody ownerBody;
        private ChildLocator orb1Locator;
        public CharacterBody victimBody;
        public int chargeCount;
        public bool makeOrbs;
        public GameObject victim;
        public GameObject orbPrefab;
        public GameObject orb1;
        private Transform SecondOrb;
        private bool orb2Active;


        private void Start()
        {
            orbPrefab = owner.gameObject.GetComponent<AmpLightningController>().chargeOrb;
            victimBody = victim?.GetComponent<CharacterBody>();
            makeOrbs = Modules.Config.chargeOrbsEnable.Value;
            orb2Active = false;
        }
        

        private void FixedUpdate()
        {
            if (!makeOrbs) return;

            chargeCount = victimBody.GetBuffCount(Buffs.chargeBuildup);
            if (chargeCount == 1 && !orb1)
            {
                orb1 = UnityEngine.Object.Instantiate<GameObject>(orbPrefab, victim.transform);
                NetworkedBodyAttachment chargeOrbAttachment1 = orb1.GetComponent<NetworkedBodyAttachment>();
                chargeOrbAttachment1.AttachToGameObjectAndSpawn(victim, null);

                orb1Locator = orb1.GetComponent<ChildLocator>();
                SecondOrb = orb1Locator.FindChild("Orb2Location");
            }

            else if (chargeCount == 2 && orb1 && !SecondOrb.gameObject.activeInHierarchy)
            {
                Debug.Log("Enabling orb 2");
                SecondOrb.gameObject.SetActive(true);
                Debug.Log(SecondOrb.gameObject.activeInHierarchy);
            }

            //for voltaic bombardment that applies two stacks of charge immediately
            else if (chargeCount == 2 && !orb1 && !SecondOrb)
            {
                orb1 = UnityEngine.Object.Instantiate<GameObject>(orbPrefab, victim.transform);
                NetworkedBodyAttachment chargeOrbAttachment1 = orb1.GetComponent<NetworkedBodyAttachment>();
                chargeOrbAttachment1.AttachToGameObjectAndSpawn(victim, null);
                
                orb1Locator = orb1.GetComponent<ChildLocator>();
                SecondOrb = orb1Locator.FindChild("Orb2Location");
                SecondOrb.gameObject.SetActive(true);


            }

            else if (chargeCount == 1 && SecondOrb.gameObject.activeInHierarchy)
            {
                SecondOrb.gameObject.SetActive(false);
            }

            else if (chargeCount == 0 && orb1)
            {
                UnityEngine.Object.Destroy(orb1);
            }

            if (victimBody && victimBody.healthComponent.health <= 0)
            {
                DestroyOrbs();
            }
                
            }
        

       public void DestroyOrbs()
        {
            if (orb1)
            {
                //Debug.Log("Destroying orb1");
                //Debug.Log(victimBody.name);
                UnityEngine.Object.Destroy(orb1);
                orb1 = null;
            }

            if (orb1 && SecondOrb && SecondOrb.gameObject.activeInHierarchy)
            {
                //Debug.Log("Destroying orb2");
                SecondOrb.gameObject.SetActive(false);
            }

        }


    }
}
