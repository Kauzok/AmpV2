using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Events;
using UnityEngine.Networking;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp.Orbs
{
    public class NemAmpLightningEffectController : NetworkBehaviour
    {
        private GameObject lightningTetherVFX = Assets.lightningStreamEffect;
        private GameObject attacker;
        private NemAmpLightningTracker lightningTracker;
        public GameObject lightningTetherInstance;
        private HurtBox trackingTarget;
        private LineRenderer lineRenderer;
        private LineRenderer lineRendererPrefab;
        private int numLineRendererPoints;

        private void Start()    
        {
            lightningTracker = base.GetComponent<NemAmpLightningTracker>();
            lineRendererPrefab = lightningTetherVFX.GetComponentInChildren<LineRenderer>();
            numLineRendererPoints = lineRendererPrefab.positionCount;
        }

        public void CreateLightningTether(GameObject attacker, HurtBox hurtbox)
        {
            if (this.lightningTracker)
            {
                if (hurtbox)
                {
                    trackingTarget = hurtbox;
                    if (attacker)
                    {
                        this.attacker = attacker;
                    }

                    //lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, base.transform).GetComponentInChildren<TetherVfx>();
                    //lightningTetherInstance.tetherTargetTransform = hurtbox.gameObject.transform;
                    lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, attacker.gameObject.transform);
                    
                    lightningTetherInstance.transform.parent = attacker.gameObject.transform;
                    if (lightningTetherInstance)
                    {
                        lineRenderer = lightningTetherInstance.GetComponent<LineRenderer>();
                    }
                    


                    Debug.Log(lineRenderer + " is our linerenderer");
                    
                    lineRenderer.SetPosition(0, attacker.transform.position);
                    Debug.Log(hurtbox.healthComponent.gameObject);
                    lineRenderer.SetPosition(1, hurtbox.healthComponent.gameObject.transform.position);
                    //lineRenderer.SetPosition(numlineRendererPoints-1, hurtbox.gameObject.transform.position);
                }
            }
        }

        private void Update()
        {
            if (lightningTetherInstance)
            {
                if (lineRenderer && trackingTarget)
                {
                    if (trackingTarget.healthComponent && this.attacker)
                    //Debug.Log(trackingTarget.healthComponent.gameObject);
                    //set start of line renderer to gameobject
                    if (trackingTarget.healthComponent.gameObject.transform)
                        {
                            lineRenderer.SetPosition(0, this.attacker.transform.position);
                            lineRenderer.SetPosition(1, trackingTarget.healthComponent.gameObject.transform.position);
                        }
                    
                    //Debug.Log(trackingTarget.gameObject.transform.position + " is position");

                }
                
            }
        }

        public void DestroyLightningTether()
        {
            if (lineRenderer)
            {
                UnityEngine.Object.Destroy(lineRenderer.gameObject);
            }
        }
    }
}
