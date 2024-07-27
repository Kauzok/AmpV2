using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;
using R2API;
using AmpMod.Modules;

namespace AmpMod.SkillStates.LightningCrash
{
    public class LightningCrash_Fire : BaseSkillState
    {
        public static float baseDamageCoefficient = StaticValues.lightningStrikeCoefficient;
        public static float slamRadius = 12f;
        public static float procCoefficient;
        public static float recoil;
        private GameObject strikeBoltInstance;
        private Transform modelTransform;
        private CharacterModel characterModel;
        private GameObject boltPrefab = Assets.strikeBoltPrefab;
        private float manualExitThreshold = 6f;
        private bool isInvis;
        public static GameObject slamEffect = Assets.strikeBlastPrefab;
        private float slamSpeed = 300f;
        public static float duration = 1f;
        private bool hasSlammed = false;
        private Vector3 dashVector = Vector3.zero;


        public bool wasLiedTo = false;

        private CameraTargetParams.CameraParamsOverrideHandle camOverrideHandle;
        private CharacterCameraParamsData slamCameraParams = new CharacterCameraParamsData
        {
            maxPitch = 88f,
            minPitch = -70f,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = slamCameraPosition,
            wallCushion = 0.1f,
        };
        public static Vector3 slamCameraPosition = new Vector3(0f, 0.0f, -32.5f);



       
        public override void OnEnter()
        {
            base.OnEnter();

            On.RoR2.TeleportHelper.TeleportGameObject_GameObject_Vector3 += TeleportGameObject;

 
            modelTransform = GetModelTransform();

            if (modelTransform)
            {
                this.characterModel = modelTransform.GetComponent<CharacterModel>();
            }

            if (characterModel)
            {
                characterModel.invisibilityCount++;
                isInvis = true;
              
            }

            this.strikeBoltInstance = UnityEngine.Object.Instantiate<GameObject>(this.boltPrefab);
            this.strikeBoltInstance.transform.SetParent(base.transform, false);

            characterBody.hideCrosshair = true;
            PlayAnimation("FullBody, Override", "SpecialSwing", "Special.playbackRate", duration * 0.8f);

            characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            characterMotor.onHitGroundAuthority += GroundSlam;

            characterBody.isSprinting = true;

            characterBody.SetAimTimer(duration);

            

            if (isAuthority)
            {
                CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = slamCameraParams,
                    priority = 1f
                    
                };
                camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0.1f);
            }
        }

        private void TeleportGameObject(On.RoR2.TeleportHelper.orig_TeleportGameObject_GameObject_Vector3 orig, GameObject gameObject, Vector3 newPosition)
        {
            orig(gameObject, newPosition);
            if (base.characterBody == gameObject.GetComponent<CharacterBody>())
            {
                Fire(newPosition);
                Debug.Log("Cancelling due to teleport");
                this.outer.SetNextStateToMain();
            }
            
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
                FixedUpdateAuthority();

            if (base.fixedAge > manualExitThreshold)
            {
                
                Fire(base.characterBody.corePosition);
                outer.SetNextStateToMain();
            }
        }

        private void FixedUpdateAuthority()
        {

            characterDirection.forward = dashVector;
            if (!hasSlammed)
            {
                hasSlammed = true;
                dashVector = inputBank.aimDirection;

            }

            else
            {
               
                HandleMovement();

            }

        }

        public void HandleMovement()
        {
            characterMotor.rootMotion += dashVector * slamSpeed * Time.fixedDeltaTime;
        }

        private void Fire(Vector3 position)
        {
            float damage = baseDamageCoefficient;

            bool crit = RollCrit();
            BlastAttack blast = new BlastAttack()
            {
                radius = slamRadius,
                procCoefficient = procCoefficient,
                position = position,
                attacker = gameObject,
                teamIndex = teamComponent.teamIndex,
                crit = crit,
                baseDamage = characterBody.damage * damage,
                damageColorIndex = DamageColorIndex.Default,
                falloffModel = BlastAttack.FalloffModel.None,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                damageType = DamageType.BypassOneShotProtection
            };
            blast.AddModdedDamageType(AmpMod.Modules.DamageTypes.healShield);
            blast.Fire();

            if (isInvis)
            {
                characterModel.invisibilityCount--;
                isInvis = false;
            }

            if (modelTransform)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = .75f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, temporaryOverlay.duration, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matMercEnergized");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }
            AddRecoil(-0.4f * recoil, -0.8f * recoil, -0.3f * recoil, 0.3f * recoil);


            
        }
        private void GroundSlam(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            //get number of enemies hit to divide damage
            //LogCore.LogI($"Velocity {hitGroundInfo.velocity}");

            Fire(hitGroundInfo.position);
            outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
            characterBody.hideCrosshair = false;
            //Debug.Log("Exiting");
            PlayAnimation("FullBody, Override", "CrashLand", "BaseSkill.playbackRate", duration);
            if (slamEffect)
            {
                EffectData slamData = new EffectData
                {
                    origin = base.characterBody.footPosition,
                    scale = 1f,
                };
                EffectManager.SpawnEffect(slamEffect, slamData, true);

            }
            if (strikeBoltInstance)
            {
                Destroy(strikeBoltInstance);
            }
            characterMotor.onHitGroundAuthority -= GroundSlam;
            characterBody.bodyFlags -= CharacterBody.BodyFlags.IgnoreFallDamage;
            if (cameraTargetParams)
            {
                cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 1.2f);
            }

            On.RoR2.TeleportHelper.TeleportGameObject_GameObject_Vector3 -= TeleportGameObject;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
