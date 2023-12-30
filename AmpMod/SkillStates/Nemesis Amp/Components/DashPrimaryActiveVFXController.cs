using System.Collections.Generic;
using System.Text;
using AmpMod.Modules;
using RoR2;
using UnityEngine;


namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    class DashPrimaryActiveVFXController : MonoBehaviour
    {

        public Transform point1Transform;
        public Transform point2Transform;
        private ChildLocator childLocator;
        private CharacterBody body;
        private LineRenderer lineRenderer;
        private int numLineRendererPoints;
        private float posRange = .1f;
        private GameObject activeVFX = Assets.plasmaActiveVFX;
        private GameObject activeInstance;
        private bool isActive;
        private CharacterModel model;


        private void Start()
        {
            body = base.GetComponent<CharacterBody>();
            //Debug.Log("found body as " + body);

            activeVFX = base.GetComponent<NemLightningColorController>().dashPrimaryVFX;

            if (body)
            {
                childLocator = body.modelLocator.modelTransform.GetComponent<ChildLocator>();
                point1Transform = childLocator.FindChild("Plasma1Point").transform;
                point2Transform = childLocator.FindChild("Plasma2Point").transform;
                model = body.modelLocator.modelTransform.GetComponent<CharacterModel>();
            }
            
        }

        private void ActivateVFX()
        {
            if (!activeInstance)
            {
               // Debug.Log("instantiating");
                activeInstance = UnityEngine.Object.Instantiate<GameObject>(activeVFX, point1Transform);
                lineRenderer = activeInstance.GetComponent<LineRenderer>();
                //Debug.Log("linerenderer is " + lineRenderer);
                numLineRendererPoints = lineRenderer.positionCount;
            }
            
        }
        private void OnEnable()
        {
            Application.onBeforeRender += CreateLightningNoise;
        }

        private void OnDisable()
        {
            Application.onBeforeRender -= CreateLightningNoise;
        }

        private void Update()
        {
            bool hasOverriden = (body.skillLocator.primary.skillNameToken == "NT_NEMAMP_BODY_UTILITY_LIGHTNINGBALL_NAME");
            //Debug.Log(body.skillLocator.primary.skillNameToken);
            if (body)
            {
                if (hasOverriden && !isActive)
                {
                    //Debug.Log("Activating vfx");
                    ActivateVFX();
                    isActive = true;
                }

                if (activeInstance && lineRenderer)
                {
                    //Debug.Log("making noise");

                   // lineRenderer.SetPosition(0, point1Transform.position);

                    //lineRenderer.SetPosition(2, point2Transform.position);

                    CreateLightningNoise();

                    lineRenderer.enabled = !(model.invisibilityCount > 0);
                }

                if (!hasOverriden && isActive)
                {
                    DeactivateVFX();
                    isActive = false;
                }
            }
          
        }
        private void DeactivateVFX()
        {
            if (activeInstance && isActive)
            {
                UnityEngine.Object.Destroy(activeInstance);
            }
            UnityEngine.Object.Destroy(activeInstance);
        }

        private void CreateLightningNoise()
        {
            {
                if (lineRenderer && point2Transform && point1Transform)
                {

                    lineRenderer.SetPosition(0, this.point1Transform.position);

                    var pos2 = Vector3.Lerp(point1Transform.position, point2Transform.position, 1 / (float)(numLineRendererPoints - 1));
                    pos2.y += .07f;
                    pos2.y += Random.Range(-.02f, .02f);
                    pos2.x += Random.Range(-.05f, .05f);

                    lineRenderer.SetPosition(1, pos2);

                    for (int i = 2; i <= numLineRendererPoints - 3; i++)
                    {
                        //lineRenderer.SetPosition(i, victimBody.corePosition);
                        //float z = ((float)i) * (maxZ) / (float)(numLineRendererPoints - 1);
                        var pos = Vector3.Lerp(point1Transform.position, point2Transform.position, i / (float)(numLineRendererPoints - 1));
                        // pos.x += Random.Range(-posRange, posRange);
                        pos.y += Random.Range(0.1f, .1f+posRange);
                         //pos.z += Random.Range(-posRange, posRange);
                        /* var chooser = Random.Range(1, 4);
                        if (chooser == 1) pos.x += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 2) pos.y += Random.Range(-posRange, posRange) + .2f;
                        else if (chooser == 3) pos.z += Random.Range(-posRange, posRange) + .2f; */


                        // lineRenderer.SetPosition(i, pos);
                        lineRenderer.SetPosition(i, pos);
                    }

                    var pos3 = Vector3.Lerp(point1Transform.position, point2Transform.position, (numLineRendererPoints - 2) / (float)(numLineRendererPoints - 1));
                    pos3.y += .07f;
                    pos3.y += Random.Range(-.02f, .02f);
                    pos3.x += Random.Range(-.05f, .05f);

                    lineRenderer.SetPosition(numLineRendererPoints - 2, pos3);

                    lineRenderer.SetPosition(numLineRendererPoints - 1, point2Transform.position);

                    //Debug.Log(trackingTarget.gameObject.transform.position + " is position");

                }

            }
        }
    }
}
