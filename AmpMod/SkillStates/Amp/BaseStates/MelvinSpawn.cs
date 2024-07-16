using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.Skills;
using AmpMod.SkillStates.SkillComponents;

namespace AmpMod.SkillStates.BaseStates
{
    public class MelvinSpawn : GenericCharacterDeath
    {
        public static GameObject initialDeathExplosionEffect = EntityStates.MagmaWorm.DeathState.initialDeathExplosionEffect;
        public static string deathSoundString = EntityStates.MagmaWorm.DeathState.deathSoundString;
        public static float leapDuration = 10f;
        public static float fallDuration = 1.5f;
        public static float deathDuration = 2f;

        private float stopwatch;
        private bool hasStartedDeath;
        public object wormSkill;
        private float riseSpeed = 50f;
        private float fallSpeed = 50f;
        private WormBodyPositionsDriver wormBodyPositionsDriver;

        // Token: 0x040010F2 RID: 4338
        private Vector3? targetPosition;

        private Vector3? landPosition;

        private float rotationCount = 1.5f;

        private bool hasSetWormBody;
        private float turnRadius = 5f;

        private bool hasDebugged;

        // Token: 0x040010F3 RID: 4339
        private static readonly float fastTurnThreshold = Mathf.Cos(0.5235988f);

        // Token: 0x040010F4 RID: 4340
        private static readonly float slowTurnThreshold = Mathf.Cos(1.0471976f);

        // Token: 0x040010F5 RID: 4341
        private static readonly float fastTurnRate = 170f;

        // Token: 0x040010F6 RID: 4342
        private static readonly float slowTurnRate = 90f;

        
        public override void OnEnter()
        {
            base.OnEnter();

            //Debug.Log("died")
            this.wormBodyPositionsDriver = base.GetComponent<WormBodyPositionsDriver>();
            if (base.isAuthority)
            {
                //this.targetPosition = base.gameObject.transform.position + 200 * Vector3.up;
                this.targetPosition = new Vector3(wormBodyPositionsDriver.chaserPosition.x + .3f, base.gameObject.transform.position.y + 200, wormBodyPositionsDriver.chaserPosition.z + .2f);


            }

            Debug.Log("rotation is " + base.GetComponent<WormHealthTracker>().rotation);
            Debug.Log(this.wormBodyPositionsDriver.chaserPosition + " is chaser position");
            Debug.Log(targetPosition + " is target position");
            Debug.Log(wormBodyPositionsDriver.chaserVelocity + " is OnEnter chaser velocity");
            

        }

        private void manageLeap()
        {
            
            if (!hasSetWormBody)
            {
                //targetPosition = new Vector3(.1f, targetPosition.Value.y, -.1f);
                wormBodyPositionsDriver.chaserVelocity = new Vector3(18f, .5f, -8f);
                hasSetWormBody = true;
                Debug.Log(this.wormBodyPositionsDriver.chaserPosition + " is initial chaser position");

            }
            //define a vector that points from the chaserposition to the target position
            Vector3 vector = this.targetPosition.Value - this.wormBodyPositionsDriver.chaserPosition;


            //if said vector isn't 0 and the chaservelocity isn't zero
            if (vector != Vector3.zero && this.wormBodyPositionsDriver.chaserVelocity != Vector3.zero)
            {

                float num2 = MelvinSpawn.fastTurnRate;
                if (num2 != 0f)
                {

                    this.wormBodyPositionsDriver.chaserVelocity = Vector3RotateTowards(this.wormBodyPositionsDriver.chaserVelocity, vector, 0.017453292f * num2 * Time.fixedDeltaTime, 0f);
                  
                    if (!hasDebugged)
                    {
                        Debug.Log("chaser is underground: " + wormBodyPositionsDriver.chaserIsUnderground);
                        Debug.Log("INITIAL velocity vector is " + wormBodyPositionsDriver.chaserVelocity);
                        Debug.Log("INITIAL target vector is " + vector);
                        hasDebugged = true;
                    }

                    Debug.Log("current velocity vector is " + wormBodyPositionsDriver.chaserVelocity);
                    Debug.Log("target vector is " + vector);

                }
            }
        }

        Vector3 Vector3RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta)
        {
            // replicates Unity Vector3.RotateTowards

            //find the difference in angle between the current vector and the target vector in radians
            float delta = Vector3.Angle(current, target) * Mathf.Deg2Rad;
            //find the magnitude difference between the target vector and the current vector
            float magDiff = target.magnitude - current.magnitude;
            //sign of the magnitude difference
            float sign = Mathf.Sign(magDiff);
            //
            float maxMagDelta = Mathf.Min(maxMagnitudeDelta, Mathf.Abs(magDiff));
            float diff = Mathf.Min(1.0f, maxRadiansDelta / delta);
            Debug.Log(sign + " is sign");
            return Vector3.SlerpUnclamped(current.normalized, target.normalized, diff) *
            (current.magnitude + maxMagDelta * sign);
            
        }
        private void manageFall()
        {
            Debug.Log("falling");
            //define falling position
            //define a vector that points from the chaserposition to the target position
            Vector3 vector = this.targetPosition.Value + this.wormBodyPositionsDriver.chaserPosition;
            Debug.Log("vector difference magnitude is " + vector.magnitude);
            //if said vector isn't 0 and the chaservelocity isn't zero
            if (vector != Vector3.zero && this.wormBodyPositionsDriver.chaserVelocity != Vector3.zero)
            {

                float num2 = 5*MelvinSpawn.fastTurnRate;
                if (num2 != 0f)
                {
                    Debug.Log("rotating");
                    Vector3 speedDown = fallSpeed * Vector3.down;

                    Vector3.RotateTowards(this.wormBodyPositionsDriver.chaserVelocity, vector, 0.017453292f * num2 * Time.fixedDeltaTime, 1f);
                    Debug.Log("velocity is" + wormBodyPositionsDriver.chaserVelocity);
                    Debug.Log("target vector is " + vector);
                }
            }
        }
        public override void FixedUpdate()
        {
            this.stopwatch += Time.fixedDeltaTime;

            if (NetworkServer.active && this.targetPosition != null && stopwatch < leapDuration)
            {

                manageLeap();
            }

            if (NetworkServer.active && this.targetPosition != null && stopwatch > leapDuration && stopwatch < leapDuration + fallDuration)
            {
                manageFall();
            }

            if (NetworkServer.active && this.stopwatch > MelvinSpawn.leapDuration + fallDuration && !hasStartedDeath)
            {
                hasStartedDeath = true;
                startDeath();
                Debug.Log("starting death");
            }

            if (NetworkServer.active && this.stopwatch > MelvinSpawn.leapDuration + MelvinSpawn.deathDuration && hasStartedDeath)
            {
                EntityState.Destroy(base.gameObject);

            }
        }
        private void startDeath()
        {

            WormBodyPositions2 component = base.GetComponent<WormBodyPositions2>();
            WormBodyPositionsDriver component2 = base.GetComponent<WormBodyPositionsDriver>();
            if (component)
            {
                component2.yDamperConstant = 0f;
                component2.ySpringConstant = 0f;
                component2.maxTurnSpeed = 0f;
                component.meatballCount = 0;
                //Util.PlaySound(MelvinSpawn.deathSoundString, component.bones[0].gameObject);
            }
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                PrintController printController = modelTransform.gameObject.AddComponent<PrintController>();
                printController.printTime = MelvinSpawn.deathDuration;
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
                            component4.maxDuration = MelvinSpawn.deathDuration;
                        }
                    }
                }
                if (NetworkServer.active)
                {
                    EffectManager.SimpleMuzzleFlash(MelvinSpawn.initialDeathExplosionEffect, base.gameObject, "HeadCenter", true);
                }
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            Debug.Log("exiting");
        }
    }
}
