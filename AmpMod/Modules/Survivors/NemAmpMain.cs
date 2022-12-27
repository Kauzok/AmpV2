using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp;
using EntityStates;

namespace AmpMod.Modules.Survivors
{
    public class NemAmpMain : GenericCharacterMain
    {

        public static event Action<Run> onSunSurvived = delegate { };

        private bool sparksActive;
        private Material swordMat;
        private float swordTransition;
        private GameObject sparkActiveEffect;
        private NemAmpLightningController lightningController;
        private ChildLocator childLocator;

        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();

      
     
            this.lightningController = base.characterBody.GetComponent<NemAmpLightningController>();

            if (base.characterBody)
            {
                Transform modelTransform = base.GetModelTransform();

                if (modelTransform)
                {
                    childLocator = modelTransform.GetComponent<ChildLocator>();
                }
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

     
        }

    }
}
