using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
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
        private AmpLightningController lightningController;
        private ChildLocator childLocator;

        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();

            if (base.healthComponent.combinedHealth >= 0)
            {
                this.sparksActive = true;

            }
            else
            {
                this.sparksActive = false;
                this.swordTransition = 0;
            }
            this.lightningController = base.characterBody.GetComponent<AmpLightningController>();

            if (base.characterBody)
            {
                Transform modelTransform = base.GetModelTransform();

                if (modelTransform)
                {
                    childLocator = modelTransform.GetComponent<ChildLocator>();
                }
            }

            if (this.childLocator)
            {

                this.childLocator.FindChild("SwordSparks")?.gameObject.SetActive(false);
                bool isRed = lightningController.isRed;
                if (isRed) this.sparkActiveEffect = this.childLocator.FindChild("SwordSparksRed")?.gameObject;
                else this.sparkActiveEffect = this.childLocator.FindChild("SwordSparks")?.gameObject;


            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.healthComponent.combinedHealth >= (0))
            {

                this.sparksActive = true;
            }


            if (this.sparksActive)
            {

                if (this.sparkActiveEffect) this.sparkActiveEffect.SetActive(true);

            }
        }

    }
}
