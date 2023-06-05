
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Events;
using UnityEngine.Networking;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpLightningTetherController : NetworkBehaviour
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
        private Transform origin;

        private void Start()    
        {
            lightningTracker = base.GetComponent<NemAmpLightningTracker>();
            lineRendererPrefab = lightningTetherVFX.GetComponentInChildren<LineRenderer>();
            numLineRendererPoints = lineRendererPrefab.positionCount;

   
        }

        public void CreateLightningTether(GameObject attacker, Transform origin)
        {
            if (this.lightningTracker)
            {
                if (lightningTracker.GetTrackingTarget())
                {
                    //store attacker gameobject and origin transform in class
                    if (attacker)
                    {
                        this.attacker = attacker;
                        this.origin = origin;
                    }

                    //if we already have a tether instance destroy it so we don't make duplicates
                    if (lightningTetherInstance)
                    {
                        DestroyLightningTether();
                    }

                    //save the current gameobject being tracked & its characterbody
                    oldLightningTarget = lightningTracker.GetTrackingTarget().healthComponent.gameObject;
                    victimBody = oldLightningTarget.GetComponent<CharacterBody>();
                    
                    //spawn lightning tether and parent its transform to our origin transform
                    lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, origin.position, origin.rotation);                
                    lightningTetherInstance.transform.parent = origin;

                    //grab linerenderer component
                    if (lightningTetherInstance)
                    {
                        lineRenderer = lightningTetherInstance.GetComponent<LineRenderer>();
                    }

                    #region manual set pos
                    //set positions
                    if (victimBody)
                     {
                         lineRenderer.SetPosition(0, lightningTetherInstance.transform.parent.position);

                         lineRenderer.SetPosition(numLineRendererPoints - 1, victimBody.corePosition);
                     }
                    #endregion

                    #region automated set pos (ethanol's linerenderer)
                    /*Vector3 startPos = origin.position;
                    Vector3 endPos = victimBody.corePosition;
                    int interVal = (int)Mathf.Abs(Vector3.Distance(endPos, startPos));

                    if (interVal <= 0)
                    {
                        interVal = 2;
                    }

                    Vector3[] numberofpositions = new Vector3[interVal];

                    for (int i = 0; i < numberofpositions.Length; i++)
                    {
                        numberofpositions[i] = Vector3.Lerp(startPos, endPos, (float)i / interVal);
                        numberofpositions[i].z = Mathf.Lerp(startPos.z, endPos.z, (float)i / interVal);
                    }
                    
                    lineRenderer.positionCount = interVal;
                    lineRenderer.SetPositions(numberofpositions);
                    numLineRendererPoints = numberofpositions.Length;
                    Debug.Log(numLineRendererPoints);*/
                    #endregion


                    //lineRenderer.SetPosition(numlineRendererPoints-1, hurtbox.gameObject.transform.position);


                }
            }
        }

        private void CreateLightningNoise()
        {
            if (lightningTetherInstance)
            {
                if (lineRenderer && victimBody && this.attacker && this.origin)
                {
                    //Debug.Log(numLineRendererPoints + " in update");
                    //lineRenderer.SetPosition(0, this.lightningTetherInstance.transform.parent.position);
                    //Debug.Log("updating at " + this.origin.position);
                    //Debug.Log(numLineRendererPoints);
                    for (int i = 0; i <= numLineRendererPoints - 1; i++)
                    {
                        //lineRenderer.SetPosition(i, victimBody.corePosition);
                        //float z = ((float)i) * (maxZ) / (float)(numLineRendererPoints - 1);
                        var pos = Vector3.Lerp(victimBody.corePosition, lightningTetherInstance.transform.parent.position, i / (float)(numLineRendererPoints-1));
                        pos.x += Random.Range(-posRange, posRange);
                        pos.y += Random.Range(-posRange, posRange);
                        pos.z += Random.Range(-posRange, posRange);
                        /* var chooser = Random.Range(1, 4);
                        if (chooser == 1) pos.x += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 2) pos.y += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 3) pos.z += Random.Range(-posRange, posRange) + .2f; */


                        lineRenderer.SetPosition(i, pos);
                    }
                    //lineRenderer.SetPosition(lineRenderer.positionCount - 1, victimBody.corePosition);

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
                        CreateLightningTether(attacker, origin);
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
