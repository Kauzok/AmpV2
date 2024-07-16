using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using UnityEngine;
using AmpMod.Modules;

namespace AmpMod.SkillStates.LightningCrash
{
    public class LightningCrash_Jump : BaseState
    {
        private Vector3 flyVector = Vector3.zero;
        public static AnimationCurve speedCoefficientCurve;
        public static float duration = 0.1f;
        public static float crosshairDur = 0.75f;

        public static GameObject jumpEffect;
        public static GameObject jumpEffectMastery;
        public static Material jumpMaterialMastery;
        private GameObject blinkPrefab = Assets.strikeBlinkPrefab;
        private CharacterModel characterModel;
        private Transform modelTransform;
        private HurtBoxGroup hurtboxGroup;
        private Vector3 worldBlinkVector;
        private bool begunBlink;
        public float jumpCoefficient = 15f;
        private bool isInvisibleIntangible;

        private bool hasPlacedCrosshair = false;
        private bool controlledExit = false;

        public static GameObject areaIndicator = EntityStates.Huntress.ArrowRain.areaIndicatorPrefab;
        public static GameObject areaIndicatorOOB;

        [HideInInspector]
        public GameObject areaIndicatorInstance;

        [HideInInspector]
        public GameObject areaIndicatorInstanceOOB;

        public static Vector3 slamCameraPosition = new Vector3(2.6f, -2.0f, -8f);


        private CameraTargetParams.CameraParamsOverrideHandle camOverrideHandle;
        private CharacterCameraParamsData slamCameraParams = new CharacterCameraParamsData
        {
            maxPitch = 88f,
            minPitch = 25f,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = slamCameraPosition,
            wallCushion = 0.1f,
        };
        

        public override void OnEnter()
        {
            base.OnEnter();

            if (base.characterMotor)
            {
                base.characterMotor.velocity = Vector3.zero;
            }
            characterBody.hideCrosshair = true;
            characterBody.SetAimTimer(duration);

            flyVector = Vector3.up;
            CreateBlinkEffect(base.characterBody.corePosition);
            Util.PlaySound("Play_item_use_lighningArm", base.gameObject);
            modelTransform = GetModelTransform();

            if (modelTransform)
            {
                this.characterModel = modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();

            }
            PlayAnimation("FullBody, Override", "SpecialJump", "Special.playbackRate", duration);

            if (isAuthority)
            {
                characterMotor.Motor.ForceUnground();

                CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = slamCameraParams,
                    priority = 1f
                };
                camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0.5f);


            }
            Vector3 direction = base.GetAimRay().direction;
            direction.y = 0f;
            direction.Normalize();
            Vector3 up = Vector3.up;
            Vector3 blinkVector = new Vector3(0, 1, 0);
            this.worldBlinkVector = Matrix4x4.TRS(base.transform.position, Util.QuaternionSafeLookRotation(direction, up), new Vector3(1f, 1f, 1f)).MultiplyPoint3x4(blinkVector) - base.transform.position;
            this.worldBlinkVector.Normalize();
        }

        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.worldBlinkVector);
            effectData.origin = origin;
            EffectManager.SpawnEffect(this.blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge >= duration * crosshairDur && !hasPlacedCrosshair)
            {
                hasPlacedCrosshair = true;
                areaIndicatorInstance = UnityEngine.Object.Instantiate(areaIndicator);
                //areaIndicatorInstanceOOB = UnityEngine.Object.Instantiate(areaIndicatorOOB);
                areaIndicatorInstance.SetActive(true);
            }

            if (!begunBlink)
            {
                begunBlink = true;
                if (this.characterModel)
                {
                    this.characterModel.invisibilityCount++;

                }
                if (this.hurtboxGroup)
                {
                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                }

                isInvisibleIntangible = true;
            }

            if (base.isAuthority)
            {
                FixedUpdateAuthority();
            }
                
        }

        public override void Update()
        {
            base.Update();
            UpdateAreaIndicator();
        }

        private void UpdateAreaIndicator()
        {
            if (areaIndicatorInstance)
            {
                float maxDistance = 256f;

                Ray aimRay = GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(aimRay, out raycastHit, maxDistance, LayerIndex.CommonMasks.bullet))
                {

                    areaIndicatorInstance.transform.position = raycastHit.point;
                    areaIndicatorInstance.transform.up = raycastHit.normal;
                }
                else
                {
 
                    areaIndicatorInstance.transform.position = aimRay.GetPoint(maxDistance);
                    areaIndicatorInstance.transform.up = -aimRay.direction;
                }
            }
        }

        private void FixedUpdateAuthority()
        {
            if (fixedAge >= duration)
            {
                controlledExit = true;
                /* if (inputBank.skill4.down)
                 {
                     LightningCrash_Hold nextState = new LightningCrash_Hold();
                     outer.SetNextState(nextState);
                 }
                 else
                 {
                     LightningCrash_Fire nextState = new LightningCrash_Fire();
                     outer.SetNextState(nextState);
                 } */
                LightningCrash_Hold nextState = new LightningCrash_Hold();
                outer.SetNextState(nextState);
            }
            else
            {
                HandleMovement();
            }
               
        }

        public void HandleMovement()
        {
            base.characterMotor.rootMotion += this.worldBlinkVector * (base.characterBody.jumpPower * this.jumpCoefficient * Time.fixedDeltaTime);

        }

        public override void OnExit()
        {
            base.OnExit();
            characterBody.hideCrosshair = false;

            if (cameraTargetParams)
            {
                cameraTargetParams.RemoveParamsOverride(camOverrideHandle, .1f);
            }

            if (areaIndicatorInstance)
            {
                Destroy(areaIndicatorInstance.gameObject);
            }

            if (areaIndicatorInstanceOOB)
            {
                Destroy(areaIndicatorInstanceOOB.gameObject);
            }
            

            if (isInvisibleIntangible)
            {
                if (this.characterModel)
                {
                    this.characterModel.invisibilityCount--;
                }
                if (this.hurtboxGroup)
                {
                    HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                    int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                    hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
                }
            }

            if (modelTransform)
            {
                CreateBlinkEffect(base.characterBody.corePosition);
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = .75f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, temporaryOverlay.duration, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matMercEnergized");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
