using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using AmpMod.Modules;

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

            indicatorInstance = UnityEngine.Object.Instantiate<GameObject>(indicatorPrefab, body.corePosition, Quaternion.identity);
            indicatorInstance.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(base.gameObject, null);
        }

    }
}
