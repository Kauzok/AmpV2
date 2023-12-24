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
        private GameObject indicatorPrefab;
        private GameObject indicatorInstance;
        private bool isEnabled;
        private NemLightningColorController nemLightningColorController;
        private CharacterBody body;
        private bool isBlue;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
        }
        private void Start()
        {
            nemLightningColorController = base.GetComponent<NemLightningColorController>();
            isBlue = nemLightningColorController.isBlue;
            if (isBlue)
            {
                indicatorPrefab = Assets.stormRangeIndicatorBlue;
            }
            else
            {
                indicatorPrefab = Assets.stormRangeIndicator;
            }

            //only spawn the indicator on nemamp so only the player can see it (only works if you're a client)
            Debug.Log(base.GetComponent<EntityStateMachine>().networkIdentity.hasAuthority);
            if (Util.HasEffectiveAuthority(base.GetComponent<EntityStateMachine>().networkIdentity))
            {
                //Debug.Log("instantiating indicator");
                indicatorInstance = UnityEngine.Object.Instantiate<GameObject>(indicatorPrefab, body.corePosition, Quaternion.identity);
                indicatorInstance.transform.parent = base.gameObject.transform;
            }
            

            if (NetworkServer.active)
            {

                //indicatorInstance = UnityEngine.Object.Instantiate<GameObject>(indicatorPrefab, body.corePosition, Quaternion.identity);
                //indicatorInstance.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(base.gameObject, null);
               // UnityEngine.Object.Destroy(indicatorInstance);
                //indicatorInstance = null;
            }
           
         
        }


    }
}
