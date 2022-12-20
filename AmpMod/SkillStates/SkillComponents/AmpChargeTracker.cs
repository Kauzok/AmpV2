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
    public class AmpChargeTracker : NetworkBehaviour
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
            makeOrbs = Config.chargeOrbsEnable.Value;
        }


        [ClientRpc]
        private void RpcDisableOrb2(NetworkInstanceId networkId)
        {
            var gameObject = Util.FindNetworkObject(networkId);
            if (gameObject)
            {
                var parentLocator = gameObject.GetComponent<ChildLocator>();
                var childOrb = parentLocator.FindChild("Orb2Location");
                childOrb.gameObject.SetActive(false);
            }

        }
        
        //given the networkid of an orb gameobject, enable its child
        [ClientRpc]
        private void RpcEnableOrb2(NetworkInstanceId networkId)
        {
            var gameObject = Util.FindNetworkObject(networkId);
            if (gameObject)
            {
                var parentLocator = gameObject.GetComponent<ChildLocator>();
                var childOrb = parentLocator.FindChild("Orb2Location");
                Debug.Log("child gameobject is " + childOrb.gameObject);
                childOrb.gameObject.SetActive(true);
            }

        }

        private void FixedUpdate()
        {
            if (!makeOrbs) return;

            //find stacks of charge on victim
            chargeCount = victimBody.GetBuffCount(Buffs.chargeBuildup);

            //if one stack of charge, spawn first orb and access the second one but don't enable it
            if (chargeCount == 1 && !orb1)
            {
                orb1 = UnityEngine.Object.Instantiate<GameObject>(orbPrefab, victim.transform);
                NetworkedBodyAttachment chargeOrbAttachment1 = orb1.GetComponent<NetworkedBodyAttachment>();
                chargeOrbAttachment1.AttachToGameObjectAndSpawn(victim, null);
                var parentLocator = orb1.GetComponent<ChildLocator>();
                SecondOrb = parentLocator.FindChild("Orb2Location");
            }

            //enable second orb; NOTE: doesn't show up for clients
            else if (chargeCount == 2 && orb1 && !SecondOrb.gameObject.activeInHierarchy)
            {
                Debug.Log("Enabling orb 2");
                Debug.Log(orb1.GetComponent<NetworkIdentity>().netId);
                SecondOrb.gameObject.SetActive(true);
                //RpcEnableOrb2(orb1.GetComponent<NetworkIdentity>().netId);
                //RpcEnableOrb2(orb1.GetComponent<NetworkIdentity>().netId);
            }

            //for voltaic bombardment that applies two stacks of charge immediately
            else if (chargeCount == 2 && !orb1 && !SecondOrb)
            {
                orb1 = UnityEngine.Object.Instantiate<GameObject>(orbPrefab, victim.transform);
                NetworkedBodyAttachment chargeOrbAttachment1 = orb1.GetComponent<NetworkedBodyAttachment>();
                chargeOrbAttachment1.AttachToGameObjectAndSpawn(victim, null);
                RpcEnableOrb2(orb1.GetComponent<NetworkIdentity>().netId);

            }
            //disable second orb if charge falls back down to 1 stack (e.g. time runs out on second stack)
           /* else if (chargeCount == 1 && 
            {
                SecondOrb.gameObject.SetActive(false);
            }
            //destroy orb object if charge falls to 0
            else if (chargeCount == 0 && orb1)
            {
                UnityEngine.Object.Destroy(orb1);
            } */

            if (victimBody && victimBody.healthComponent.health <= 0)
            {
                DestroyOrbs();
            }
                
        }
        

       //method to destroy both orbs (occurs upon victim death and proc of charge's explosion)
       public void DestroyOrbs()
        {
            if (orb1)
            {
                //Debug.Log("Destroying orb1");
                //Debug.Log(victimBody.name);
                UnityEngine.Object.Destroy(orb1);
                orb1 = null;
            }

        }


    }
}
