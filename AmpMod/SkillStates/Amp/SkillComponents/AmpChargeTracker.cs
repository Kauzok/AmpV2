using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Unity;
using UnityEngine.Networking;
using AmpMod.Modules;
using RoR2;
using R2API.Networking;
using R2API.Networking.Interfaces;

namespace AmpMod.SkillStates
{

    //tracker class used for custom debuff
    public class AmpChargeTracker : MonoBehaviour
    {
        /// <summary>
        /// Original person who first attacked the enemy.
        /// 
        /// Used for deciding how the charge explosion should be handled.
        /// </summary>
        public GameObject owner;
        
        /// <summary>
        /// Victim of the attack that is now started the charge on the enemy
        /// </summary>
        public GameObject victim;

        /// <summary>
        /// Body of the owner
        /// </summary>
        public CharacterBody ownerBody;

        /// <summary>
        /// Body of the victim
        /// </summary>
        public CharacterBody victimBody;


        private bool makeOrbs;
        private GameObject orbPrefab;
        private GameObject orbPrefabFull;
        private GameObject orbPair;
        private bool secondOrbActive;


        private void Start()
        { 
            orbPrefab = owner.gameObject.GetComponent<AmpLightningController>().chargeOrb;
            orbPrefabFull = owner.gameObject.GetComponent<AmpLightningController>().chargeOrbFull;
            ownerBody = owner?.GetComponent<CharacterBody>();
            victimBody = victim?.GetComponent<CharacterBody>();
            makeOrbs = Config.chargeOrbsEnable.Value;
        }       

        [Server]
        private void CheckOrbDraw()
        {
            if (!makeOrbs) return;

            // If the victim is dead then destroy the orbs and don't run any orb checks
            if (victimBody && victimBody.healthComponent.health <= 0)
            {
                DestroyOrbs();
                return;
            }

            // Find stacks of charge on victim
            int chargeCount = victimBody.GetBuffCount(Buffs.chargeBuildup);

            // If at least one charge is on the victim spawn the pair 
            if (chargeCount >= 1 && !orbPair)
            {
                // Debug.Log("Creating new orb pair for " + victim.GetComponent<NetworkIdentity>().netId
                if (chargeCount == 2)
                {
                    orbPair = UnityEngine.Object.Instantiate<GameObject>(orbPrefabFull, victim.transform);
                    secondOrbActive = true;
                } 
                else
                {
                    orbPair = UnityEngine.Object.Instantiate<GameObject>(orbPrefab, victim.transform);
                }

                NetworkedBodyAttachment chargeOrbAttachment = orbPair.GetComponent<NetworkedBodyAttachment>();
                chargeOrbAttachment.AttachToGameObjectAndSpawn(victim, null);
            }
            //enable second orb; NOTE: doesn't show up for clients
            else if (chargeCount == 2 && !secondOrbActive)
            {
                // Debug.Log("Enabling orb 2");
                // Debug.Log(orbPair.GetComponent<NetworkIdentity>().netId);
                new SyncOrbs(orbPair.GetComponent<NetworkIdentity>().netId, SyncOrbs.METHOD.EnableSecondOrb).Send(NetworkDestination.Clients);
                secondOrbActive = true;
            }
            // If charge count drops and second orb active remove the second orb
            else if (chargeCount == 1 && secondOrbActive)
            {
                new SyncOrbs(orbPair.GetComponent<NetworkIdentity>().netId, SyncOrbs.METHOD.DisableSecondOrb).Send(NetworkDestination.Clients);
            }
            // Destroy orb object if charge falls to 0
            else if (chargeCount == 0 && orbPair)
            {
                DestroyOrbs();
            }
        }

        private void FixedUpdate()
        {
            CheckOrbDraw(); 
        }

       /// <summary>
       /// Method to destroy the orb pair.
       /// </summary>
       private void DestroyOrbs()
        {
            if (orbPair)
            {
                UnityEngine.Object.Destroy(orbPair);
                orbPair = null;
                secondOrbActive = false;
            }

        }

        public class SyncOrbs : INetMessage
        {
            public enum METHOD
            {
                EnableSecondOrb,
                DisableSecondOrb
            }
            NetworkInstanceId netId;
            METHOD method;

            public SyncOrbs() {}

            public SyncOrbs(NetworkInstanceId netId, METHOD method)
            {
                this.netId = netId;
                this.method = method;
            }

            void ISerializableObject.Deserialize(NetworkReader reader)
            {
                netId = reader.ReadNetworkId();
                method = (METHOD)reader.ReadInt32();
            }

            void INetMessage.OnReceived()
            {
                // Debug.Log("The neworkID to sync is: " + netId);
                GameObject gameObject = Util.FindNetworkObject(netId);
                // Debug.Log("This is the network gameObject: " + gameObject);
                if (!gameObject)
                {
                    Debug.LogWarning("SyncOrbs: Given network id associates with no known gameobject");
                    return;
                }

                switch (method)
                {
                    case METHOD.EnableSecondOrb:
                        EnableSecondOrb(gameObject);
                        break;
                    case METHOD.DisableSecondOrb:
                        DisableSecondOrb(gameObject);
                        break;
                    default:
                        Debug.LogError("SyncOrbs: Invalid method sent to client");
                        break;
                }
            }

            void ISerializableObject.Serialize(NetworkWriter writer)
            {
                writer.Write(netId);
                writer.Write((int)method);
            }

            private void EnableSecondOrb(GameObject go)
            { 
                go.GetComponent<ChildLocator>().FindChild("Orb2Location").gameObject.SetActive(true);
            }

            private void DisableSecondOrb(GameObject go)
            { 
                go.GetComponent<ChildLocator>().FindChild("Orb2Location").gameObject.SetActive(false);
            }
        }
    }
}
