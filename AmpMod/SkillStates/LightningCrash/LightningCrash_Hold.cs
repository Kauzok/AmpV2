using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using AmpMod.Modules;

namespace AmpMod.SkillStates.LightningCrash
{
    public class LightningCrash_Hold : BaseSkillState
    {
        public static float duration = 1.5f;

        public static GameObject jumpEffect;
        public static GameObject jumpEffectMastery;
        private string skinNameToken;
        private float waitDuration = .1f;
        private ChildLocator childLocator;

        private GameObject hoverPrefab = Assets.strikeHover;
        private Transform hoverTransform;
        public static GameObject areaIndicator = LightningCrash_Jump.areaIndicator;
        public static GameObject areaIndicatorOOB;

        [HideInInspector]
        public GameObject areaIndicatorInstance;

        [HideInInspector]
        public GameObject areaIndicatorInstanceOOB;

        private bool controlledExit = false;

        public bool imAFilthyFuckingLiar = false;

        private CameraTargetParams.CameraParamsOverrideHandle camOverrideHandle;
        private CharacterCameraParamsData slamCameraParams = new CharacterCameraParamsData
        {
            maxPitch = 88f,
            minPitch = 25f,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = slamCameraPosition,
            wallCushion = 0.1f,
        };

        [HideInInspector]
        public static Vector3 slamCameraPosition = new Vector3(2.6f, -2.0f, -8f);

        public override void OnEnter()
        {
            base.OnEnter();


            characterBody.hideCrosshair = true;

            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();

            if (this.childLocator)
            {
                Transform hoverMuzzle = this.childLocator.FindChild("StrikeHover");
                this.hoverTransform = UnityEngine.Object.Instantiate<GameObject>(hoverPrefab, hoverMuzzle).transform;

                PlayAnimation("FullBody, Override", "CrashHold", "BaseSkill.playbackRate", 3f);
            }

            if (isAuthority)
            {
                skinNameToken = GetModelTransform().GetComponentInChildren<ModelSkinController>().skins[characterBody.skinIndex].nameToken;


                CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest
                {
                    cameraParamsData = slamCameraParams,
                    priority = 0f
                };

                camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0f);

                characterBody.SetAimTimer(duration);

                areaIndicatorInstance = UnityEngine.Object.Instantiate(areaIndicator);
                //areaIndicatorInstanceOOB = UnityEngine.Object.Instantiate(areaIndicatorOOB);
                areaIndicatorInstance.SetActive(true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isAuthority)
                FixedUpdateAuthority();
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
                    imAFilthyFuckingLiar = true;
                    //areaIndicatorInstance.SetActive(true);
                    //areaIndicatorInstanceOOB.SetActive(false);
                    areaIndicatorInstance.transform.position = raycastHit.point;
                    areaIndicatorInstance.transform.up = raycastHit.normal;
                }
                else
                {
                    imAFilthyFuckingLiar = false;
                    //areaIndicatorInstance.SetActive(false);
                    //areaIndicatorInstanceOOB.SetActive(true);
                    areaIndicatorInstance.transform.position = aimRay.GetPoint(maxDistance);
                    areaIndicatorInstance.transform.up = -aimRay.direction;
                }
            }
        }

        private void FixedUpdateAuthority()
        {
            //Debug.Log("based.fixedage > waitduration " + (base.fixedAge > waitDuration));
            if ((fixedAge >= duration || !inputBank.skill4.down || inputBank.skill1.down) && base.fixedAge > waitDuration)
            {
                LightningCrash_Fire nextState = new LightningCrash_Fire();
                nextState.wasLiedTo = imAFilthyFuckingLiar; //probably every time LOL 
                outer.SetNextState(nextState);
            }
            else if (inputBank.jump.down)
            {
                base.SmallHop(base.characterMotor, base.characterBody.jumpPower * 1.5f);
                PlayAnimation("FullBody, Override", "CrashJumpCancel", "BaseSkill.playbackRate", duration);
                characterBody.SetAimTimer(duration);
                outer.SetNextStateToMain();
            }
            else
                HandleMovement();
        }

        public void HandleMovement()
        {
            characterMotor.velocity = Vector3.zero;
        }

        public override void OnExit()
        {
            base.OnExit();

            characterBody.hideCrosshair = false;

            if (hoverTransform)
            {
                Destroy(hoverTransform.gameObject);
            }
            if ( controlledExit == false)
                //exeController.meshExeAxe.SetActive(false);

            if (cameraTargetParams)
            {
                cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 1f);
            }

            if (areaIndicatorInstance)
            {
                Destroy(areaIndicatorInstance.gameObject);
            }

            if (areaIndicatorInstanceOOB)
            {
                Destroy(areaIndicatorInstanceOOB.gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
