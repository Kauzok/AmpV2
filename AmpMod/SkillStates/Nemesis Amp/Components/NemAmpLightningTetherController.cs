
using System.Text;
using UnityEngine;
using RoR2;
using System.Runtime.InteropServices;
using AmpMod.Modules;
using UnityEngine.Networking;
using AmpMod.SkillStates.Nemesis_Amp.Components;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpLightningTetherController : NetworkBehaviour
    {
        public GameObject lightningTetherVFX;
        private GameObject attacker;
        public bool isBlue;
        private NemAmpLightningTracker lightningTracker;
        public GameObject lightningTetherInstance;
        private HurtBox trackingTarget;
        private LineRenderer lineRenderer;
        private LineRenderer lineRendererPrefab;
        private int numLineRendererPoints;
        private GameObject oldLightningTarget;
        private NemLightningColorController lightningController;
        private CharacterBody victimBody;
        private GameObject victimObject;
        private GameObject ownerObject;
        private float posRange = .3f;// 0.5f;
        private float lightningTickFrequency = .1f;
        private float maxZ = 8f;
        public bool isAttacking;
        private Transform origin;
        private CharacterBody baseBody;
        private CharacterModel model;

        private NetworkInstanceId ___targetRootNetId;
        private NetworkInstanceId ___ownerRootNetId;

        private void Start()    
        {
            lightningTracker = base.GetComponent<NemAmpLightningTracker>();
            
            
            this.model = base.GetComponentInChildren<CharacterModel>();
            baseBody = base.GetComponent<CharacterBody>();

            if (this.model.GetComponent<ModelSkinController>().skins[this.baseBody.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME" && !Config.NemOriginPurpleLightning.Value)
            {
                lightningTetherVFX = Assets.lightningStreamEffectBlue;
            }
            else
            {
                lightningTetherVFX = Assets.lightningStreamEffect;
            }

            lineRendererPrefab = lightningTetherVFX.GetComponentInChildren<LineRenderer>();
            numLineRendererPoints = lineRendererPrefab.positionCount;
        }

        public void CreateLightningTether(GameObject attacker, Transform origin)
        {
            if (this.lightningTracker)
            {
                if (lightningTracker.GetTrackingTarget())
                {
                    //set the gameobjects of the victim & owner, which will be synced for networking purposes
                    victimObject = lightningTracker.GetTrackingTarget().healthComponent.gameObject;
                    ownerObject = base.gameObject;

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
                    victimObject = oldLightningTarget;

                    //spawn lightning tether and parent its transform to our origin transform
                    lightningTetherInstance = UnityEngine.Object.Instantiate<GameObject>(lightningTetherVFX, origin.position, origin.rotation);                
                    lightningTetherInstance.transform.parent = origin;

                    //debug check to make sure that the problem actually is the origin
                    Debug.Log(origin.name + " is our origin");

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
        #region networking
        public GameObject NetworktargetRoot
        {
            get
            {
                return this.victimObject;
            }
            [param: In]
            set
            {
                base.SetSyncVarGameObject(value, ref this.victimObject, 1U, ref this.___targetRootNetId);
            }
        }

        public GameObject NetworkownerRoot
        {
            get
            {
                return this.ownerObject;
            }
            [param: In]
            set
            {
                base.SetSyncVarGameObject(value, ref this.ownerObject, 2U, ref this.___ownerRootNetId);
            }
        }
        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(this.victimObject);
                writer.Write(this.ownerObject);
                return true;
            }
            bool flag = false;
            if ((base.syncVarDirtyBits & 1U) != 0U)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(this.victimObject);
            }
            if ((base.syncVarDirtyBits & 2U) != 0U)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }
                writer.Write(this.ownerObject);
            }
            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }
        #endregion

    }
}
