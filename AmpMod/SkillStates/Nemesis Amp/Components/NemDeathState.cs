using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using AmpMod.SkillStates.BaseStates;
using UnityEngine.Networking;
using AmpMod.Modules;
using UnityEngine;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    internal class NemDeathState : GenericCharacterDeath
    {
        private GameObject deathEffect = Assets.deathExplosionEffect;
        private float waitDuration = 1.4f;
        private float deathDuration = 2.5f;
        private Animator animator;
        private Material overlayMat = Assets.matDeathOverlay;
        private CharacterModel characterModel;
        private Transform modelTransform;
        private bool hasSpawnedExplosion;

        public override void OnEnter()
        {
            base.OnEnter();
            characterMotor.velocity = Vector3.zero;
            animator = base.GetModelAnimator();
            
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


        }

        private void spawnDeathExplosion()
        {
            if (NetworkServer.active)
            {
                EffectData effectData = new EffectData
                {
                    scale = 1,
                    origin = base.gameObject.transform.position, 
                };
                EffectManager.SpawnEffect(deathEffect, effectData, true);


                Debug.Log("spawning death effect");
                this.DestroyModel();
                Debug.Log("destroying model");
            }
            //EntityState.Destroy(base.gameObject);
            //this.OnPreDestroyBodyServer();
            //EntityState.Destroy(base.gameObject);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= waitDuration-.22f && !hasSpawnedExplosion)
            {
                spawnDeathExplosion();
                hasSpawnedExplosion = true;
                
            }

            if (base.fixedAge >= deathDuration)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}
