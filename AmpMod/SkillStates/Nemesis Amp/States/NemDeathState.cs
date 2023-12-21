using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using UnityEngine.Networking;
using AmpMod.Modules;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    internal class NemDeathState : GenericCharacterDeath
    {
        private GameObject deathEffect;
        private float waitDuration = 1.4f;
        private float deathDuration = 2.5f;
        private Animator animator;
        private Material overlayMat;
        private CharacterModel characterModel;
        private NemLightningColorController lightningColorController;
        private Transform modelTransform;
        private bool hasSpawnedExplosion;
        private string explosionSoundString = StaticValues.deathExplosionSoundString;
        private string chargeSoundString = StaticValues.deathChargeSoundString;
        private uint stopCharge;
        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            characterMotor.velocity = Vector3.zero;
            animator = base.GetModelAnimator();
            lightningColorController = base.GetComponent<NemLightningColorController>();
            stopCharge = Util.PlaySound(chargeSoundString, base.gameObject);

            overlayMat = lightningColorController.deathOverlay;
            deathEffect = lightningColorController.deathExplosionVFX;
            
            modelTransform = base.GetComponent<ModelLocator>().modelTransform;
            characterModel = modelTransform.GetComponent<CharacterModel>();

            TemporaryOverlay temporaryOverlay = this.cachedModelTransform.gameObject.AddComponent<TemporaryOverlay>();
            temporaryOverlay.duration = waitDuration;
            //temporaryOverlay.destroyObjectOnEnd = true;
            //temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matShatteredGlass");
            temporaryOverlay.originalMaterial = overlayMat;
            //temporaryOverlay.destroyEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BrittleDeath");
            temporaryOverlay.inspectorCharacterModel = this.cachedModelTransform.gameObject.GetComponent<CharacterModel>();
            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            temporaryOverlay.animateShaderAlpha = true;

            this.PlayAnimation("Death, Override", "Death", "Death.playbackRate", waitDuration + .7f);

            if (base.characterMotor)
            {
                base.characterMotor.enabled = false;
            }

        }

        private void spawnDeathExplosion()
        {

            AkSoundEngine.StopPlayingID(stopCharge, 0);

            Util.PlaySound(explosionSoundString, base.gameObject);
            if (NetworkServer.active)
            {
                EffectData effectData = new EffectData
                {
                    scale = 1,
                    origin = base.gameObject.transform.position, 
                };
                EffectManager.SpawnEffect(deathEffect, effectData, true);

                //EntityState.Destroy(base.gameObject);
            }
            //EntityState.Destroy(base.gameObject);
            this.OnPreDestroyBodyServer();
            base.DestroyModel();
            EntityState.Destroy(base.gameObject);
            
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;  

           // Debug.Log("in fixed update");
            //Debug.Log(stopwatch);
            if (this.stopwatch >= waitDuration-.22f && !hasSpawnedExplosion)
            {
                spawnDeathExplosion();
                //Debug.Log("spawning death explosion");
                hasSpawnedExplosion = true;
                
            }

            if (this.stopwatch >= deathDuration && hasSpawnedExplosion && NetworkServer.active)
            {
                //base.DestroyModel();
                //EntityState.Destroy(base.gameObject);
               // Debug.Log("destroying body");
                base.DestroyBodyAsapServer();
            }
        }

        public override void OnExit()
        {
            //base.DestroyModel();
            //double check to make sure
            Debug.Log("exiting");
            if (base.gameObject)
            {
                EntityState.Destroy(base.gameObject);
            }
            base.OnExit();
        }
    }
}
