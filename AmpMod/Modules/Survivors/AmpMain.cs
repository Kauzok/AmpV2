using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using EntityStates;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using HarmonyLib;
using JetBrains.Annotations;

namespace AmpMod.Modules.Survivors
{
    public class AmpMain : GenericCharacterMain
    {

        public static event Action<Run> onSunSurvived = delegate { };

        public event Action shieldDepleted;
        public event Action shieldReplenished;
        private bool swordActive;
        private Material swordMat;
        private float swordTransition;
        private GameObject swordActiveEffect;
        private AmpLightningController lightningController;
        private ChildLocator childLocator;
        private bool hasShield;
        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            
            base.characterBody.AddBuff(Buffs.shieldDamageBoost);
           

            if (base.healthComponent.combinedHealth >=  0)
            {
                this.swordActive = true;
               
            }
            else
            {
                this.swordActive = false;
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
                if (isRed) this.swordActiveEffect = this.childLocator.FindChild("SwordSparksRed")?.gameObject;
                else this.swordActiveEffect = this.childLocator.FindChild("SwordSparks")?.gameObject;


            }

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.healthComponent)
            {
                if (base.healthComponent.combinedHealth >= (0))
                {
                    this.swordActive = true;
                }
            }

            if (base.healthComponent.shield >= .5*base.healthComponent.fullShield && !base.characterBody.HasBuff(Buffs.shieldDamageBoost))
            {
                base.healthComponent.body.AddBuff(Buffs.shieldDamageBoost);
            }


            else if (base.healthComponent.shield < .5 * base.healthComponent.fullShield && base.characterBody.HasBuff(Buffs.shieldDamageBoost)) {

                base.healthComponent.body.RemoveBuff(Buffs.shieldDamageBoost);
            }


            if (this.swordActive)
            {
                
                if (this.swordActiveEffect) this.swordActiveEffect.SetActive(true);
                
            }
        }

    }
}
