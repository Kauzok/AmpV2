using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.Skills;

namespace AmpMod.SkillStates.BaseStates
{
    public class MelvinDeathState : GenericCharacterDeath
    {
        public static GameObject initialDeathExplosionEffect = EntityStates.MagmaWorm.DeathState.initialDeathExplosionEffect;
        public static string deathSoundString = EntityStates.MagmaWorm.DeathState.deathSoundString;
        public static float duration = 2f;
        private float stopwatch;
        public object wormSkill;


        public override void OnEnter()
        {
            base.OnEnter();

            //Debug.Log("died")


            WormBodyPositions2 component = base.GetComponent<WormBodyPositions2>();
            WormBodyPositionsDriver component2 = base.GetComponent<WormBodyPositionsDriver>();
            if (component)
            {
                component2.yDamperConstant = 0f;
                component2.ySpringConstant = 0f;
                component2.maxTurnSpeed = 0f;
                component.meatballCount = 0;
                //Util.PlaySound(MelvinDeathState.deathSoundString, component.bones[0].gameObject);
            }
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                PrintController printController = modelTransform.gameObject.AddComponent<PrintController>();
                printController.printTime = MelvinDeathState.duration;
                printController.enabled = true;
                printController.startingPrintHeight = 99999f;
                printController.maxPrintHeight = 99999f;
                printController.startingPrintBias = 1f;
                printController.maxPrintBias = 3.5f;
                printController.animateFlowmapPower = true;
                printController.startingFlowmapPower = 1.14f;
                printController.maxFlowmapPower = 30f;
                printController.disableWhenFinished = false;
                printController.printCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                ParticleSystem[] componentsInChildren = modelTransform.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].Stop();
                }
                ChildLocator component3 = modelTransform.GetComponent<ChildLocator>();
                if (component3)
                {
                    Transform transform = component3.FindChild("PP");
                    if (transform)
                    {
                        PostProcessDuration component4 = transform.GetComponent<PostProcessDuration>();
                        if (component4)
                        {
                            component4.enabled = true;
                            component4.maxDuration = MelvinDeathState.duration;
                        }
                    }
                }
                if (NetworkServer.active)
                {
                    EffectManager.SimpleMuzzleFlash(MelvinDeathState.initialDeathExplosionEffect, base.gameObject, "HeadCenter", true);
                }
            }
        }

        public override void FixedUpdate()
        {
            this.stopwatch += Time.fixedDeltaTime;
            if (NetworkServer.active && this.stopwatch > MelvinDeathState.duration)
            {
                EntityState.Destroy(base.gameObject);
            }
        }

    }
}
