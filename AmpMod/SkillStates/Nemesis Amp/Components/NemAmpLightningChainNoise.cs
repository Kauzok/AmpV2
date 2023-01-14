using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Orbs;    


namespace AmpMod.SkillStates.Nemesis_Amp.Components
{


    public class NemAmpLightningChainNoise : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private Transform endTransform;
        private float duration = .2f;
        private float age;
        public GameObject endGameObject;
        public Vector3 startPosition;
        private float posRange = 0.5f;
        private EffectComponent effectComponent;
        private int numLineRendererPoints;
        private Vector3 lastKnownTargetPosition;
        public bool callArrivalIfTargetIsGone = true;

        private void Start()
        {
            lineRenderer = base.GetComponent<LineRenderer>();
            numLineRendererPoints = lineRenderer.positionCount;
            if (endGameObject)
            {
                //Debug.Log(endGameObject + " is end gameobject");
                endTransform = endGameObject.transform;
            }
            this.lastKnownTargetPosition = (this.endTransform ? this.endTransform.position : this.startPosition);
        }

        private void Update()
        {
            age += Time.deltaTime;
            CreateLightningNoise();
            if (age > duration)
            {
                Destroy(base.gameObject);
            }
        }
     

        private void CreateLightningNoise()
        {
            {
                if (lineRenderer && endTransform)
                {

                    lineRenderer.SetPosition(0, this.startPosition);

                    for (int i = 1; i < numLineRendererPoints - 1; i++)
                    {
                        //lineRenderer.SetPosition(i, victimBody.corePosition);
                        //float z = ((float)i) * (maxZ) / (float)(numLineRendererPoints - 1);
                        var pos = Vector3.Lerp(endTransform.position, startPosition, i / 11f);
                        pos.x += Random.Range(-posRange, posRange);
                        pos.y += Random.Range(-posRange, posRange);
                        /* var chooser = Random.Range(1, 4);
                        if (chooser == 1) pos.x += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 2) pos.y += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 3) pos.z += Random.Range(-posRange, posRange) + .2f; */


                        lineRenderer.SetPosition(i, pos);
                    }
                    lineRenderer.SetPosition(numLineRendererPoints - 1, endTransform.position);

                    //Debug.Log(trackingTarget.gameObject.transform.position + " is position");

                }

            }
        }
    }
}
