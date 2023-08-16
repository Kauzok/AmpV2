using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using AmpMod.Modules;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    class NemAmpBuffVFXController : MonoBehaviour
    {
        private CharacterModel characterModel;
        private CharacterBody characterBody;
        private bool hasFlashed;
        private bool isMaxed;
        private GameObject buffOnEffect;
        private GameObject buffOffEffect;
        private TemporaryOverlay buffOverlay;
        private NemLightningColorController lightningController;
        private Transform modelTransform;
        private Material overlayMat;
        private ChildLocator childLocator;
        private Transform sparkEffect;
        private string maxStartString = StaticValues.enterMaxSoundString;
        private string maxEndString = StaticValues.exitMaxSoundString;
        private string loopString = StaticValues.loopMaxSoundString;
        private uint endLoopID;


        private void Start()
        {
            characterBody = base.GetComponent<CharacterBody>();
            modelTransform = base.GetComponent<ModelLocator>().modelTransform;
            characterModel = modelTransform.GetComponent<CharacterModel>();
            childLocator = modelTransform.GetComponent<ChildLocator>();
            sparkEffect = this.childLocator.FindChild("BuffLightning");
            //overlayMat = LegacyResourcesAPI.Load<Material>("RoR2/DLC1/VoidWardCrab/matVoidWardCrabOverlay");
            lightningController = base.GetComponent<NemLightningColorController>();
            buffOnEffect = lightningController.buffOnVFX;
            overlayMat = lightningController.buffOverlay;
            buffOffEffect = lightningController.buffOffVFX;
        }

        private void FixedUpdate()
        {
            if (characterBody.GetBuffCount(Buffs.damageGrowth) == StaticValues.growthBuffMaxStacks & !isMaxed)
            {
                EffectData flashEffect = new EffectData
                {
                    origin = characterBody.corePosition,
                    scale = 1f
                };
                EffectManager.SpawnEffect(buffOnEffect.gameObject, flashEffect, true);
                isMaxed = true;

                Debug.Log(characterModel);
                buffOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                buffOverlay.animateShaderAlpha = true;
                buffOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                buffOverlay.destroyComponentOnEnd = true;
                buffOverlay.duration = float.PositiveInfinity;
                buffOverlay.originalMaterial = overlayMat;
                buffOverlay.AddToCharacerModel(characterModel);

                sparkEffect.gameObject.SetActive(true);

                Util.PlaySound(maxStartString, base.gameObject);
                endLoopID = Util.PlaySound(loopString, base.gameObject);
            }

            if (characterBody.GetBuffCount(Buffs.damageGrowth) < StaticValues.growthBuffMaxStacks && isMaxed)
            {
                isMaxed = false;
                EffectData flashEffect = new EffectData
                {
                    origin = characterBody.corePosition,
                    scale = 1f
                };
                EffectManager.SpawnEffect(buffOffEffect.gameObject, flashEffect, true);

                AkSoundEngine.StopPlayingID(endLoopID);
                Util.PlaySound(maxEndString, base.gameObject);
                if (this.buffOverlay)
                {
                    Destroy(buffOverlay);
                }

                if (sparkEffect.gameObject.activeInHierarchy)
                {
                    sparkEffect.gameObject.SetActive(false);
                }
            }
        }
    }
}
