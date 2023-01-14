
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Events;
using UnityEngine.Networking;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
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
        private GameObject oldLightningTarget;
        private CharacterBody victimBody;
        private float posRange = 0.5f;
        private float lightningTickFrequency = .1f;
        private float maxZ = 8f;
        public bool isAttacking;

        private void Start()    
        {
            lightningTracker = base.GetComponent<NemAmpLightningTracker>();
            lineRendererPrefab = lightningTetherVFX.GetComponentInChildren<LineRenderer>();
            numLineRendererPoints = lineRendererPrefab.positionCount;
        }

        public void CreateLightningTether(GameObject attacker)
        {
            if (this.lightningTracker)
            {
                if (lightningTracker.GetTrackingTarget())
                {
                    if (attacker)
                    {
                        this.attacker = attacker;
                    }

                    if (lightningTetherInstance)
                    {
                        DestroyLightningTether();
                    }

                    oldLightningTarget = lightningTracker.GetTrackingTarget().healthComponent.gameObject;
                    victimBody = oldLightningTarget.GetComponent<CharacterBody>();
                    lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, attacker.gameObject.transform);
                    
                    lightningTetherInstance.transform.parent = attacker.gameObject.transform;
                    if (lightningTetherInstance)
                    {
                        lineRenderer = lightningTetherInstance.GetComponent<LineRenderer>();
                    }

                    if (victimBody)
                    {
                        lineRenderer.SetPosition(0, attacker.transform.position);
                        for (int i = 1; i < numLineRendererPoints - 1; i++)
                        {
                            lineRenderer.SetPosition(i, victimBody.corePosition);
                        }
                        lineRenderer.SetPosition(numLineRendererPoints - 1, victimBody.corePosition);
                    }

                    //lineRenderer.SetPosition(numlineRendererPoints-1, hurtbox.gameObject.transform.position);
                }
            }
        }

        private void CreateLightningNoise()
        {
            if (lightningTetherInstance)
            {
                if (lineRenderer && victimBody && this.attacker)
                {

                    lineRenderer.SetPosition(0, this.attacker.transform.position);

                    for (int i = 1; i < numLineRendererPoints - 1; i++)
                    {
                        //lineRenderer.SetPosition(i, victimBody.corePosition);
                        //float z = ((float)i) * (maxZ) / (float)(numLineRendererPoints - 1);
                        var pos = Vector3.Lerp(victimBody.corePosition, attacker.transform.position, i / 11f);
                        pos.x += Random.Range(-posRange, posRange);
                        pos.y += Random.Range(-posRange, posRange);
                        /* var chooser = Random.Range(1, 4);
                        if (chooser == 1) pos.x += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 2) pos.y += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 3) pos.z += Random.Range(-posRange, posRange) + .2f; */


                        lineRenderer.SetPosition(i, pos);
                    }
                    lineRenderer.SetPosition(numLineRendererPoints - 1, victimBody.corePosition);

                    //Debug.Log(trackingTarget.gameObject.transform.position + " is position");

                }

            }
        }
        private void Update()
        {
            if (oldLightningTarget)
            {
                if (lightningTracker.GetTrackingTarget())
                {
                    //if we're no longer tracking the same enemy (i.e. the attack switched targets), redo the tether
                    if (oldLightningTarget.transform.position != this.lightningTracker.GetTrackingTarget().healthComponent.transform.position && attacker && this.isAttacking)
                    {
                        CreateLightningTether(attacker);
                    }
                }
              
                
            }
            CreateLightningNoise();  
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
