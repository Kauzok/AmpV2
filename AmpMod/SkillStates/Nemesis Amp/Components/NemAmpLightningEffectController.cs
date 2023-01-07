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
        private NemAmpLightningTracker lightningTracker;
        public TetherVfx lightningTetherInstance;
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

        public void CreateLightningTether(HurtBox hurtbox)
        {
            if (this.lightningTracker)
            {
                if (hurtbox)
                {
                    trackingTarget = hurtbox;
                    //lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, base.transform).GetComponentInChildren<TetherVfx>();
                    //lightningTetherInstance.tetherTargetTransform = hurtbox.gameObject.transform;
                    lineRenderer = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, base.transform).GetComponentInChildren<LineRenderer>();
                    Debug.Log(lineRenderer + " is our linerenderer");
                    
                    lineRenderer.SetPosition(0, base.transform.position);
                    Debug.Log(hurtbox.healthComponent.gameObject);
                    lineRenderer.SetPosition(1, hurtbox.healthComponent.gameObject.transform.position);
                    //lineRenderer.SetPosition(numlineRendererPoints-1, hurtbox.gameObject.transform.position);
                }
            }
        }

        private void Update()
        {
            if (this.lineRenderer)
            {
                if (trackingTarget.healthComponent.gameObject.transform)

                {
                    Debug.Log(trackingTarget.healthComponent.gameObject);
                    //set start of line renderer to 
                    lineRenderer.SetPosition(0, base.gameObject.transform.position);
                    lineRenderer.SetPosition(1, trackingTarget.gameObject.transform.position);

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
