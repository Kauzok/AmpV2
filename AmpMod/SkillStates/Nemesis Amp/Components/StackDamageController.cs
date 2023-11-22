using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using EntityStates;
using AmpMod.Modules;
using RoR2.HudOverlay;
using UnityEngine.UI;
using RoR2.UI;
using System.Linq;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    [RequireComponent(typeof(CharacterBody))]
    class StackDamageController : MonoBehaviour
    {
        [Header("Passive Functionality")]
        private CharacterBody body;
        private float comboTime = Modules.StaticValues.comboTimeInterval;
        private float damageBuffRemovalRate = Modules.StaticValues.growthBuffDisappearanceRate;
        private int maxBuffStacks = StaticValues.growthBuffMaxStacks;
        private BaseState lastSkillUsed;
        public BaseState newSkillUsed;
        private ChildLocator childLocator;
        private float repeatTimer;
        private float comboTimer;
        private bool isInCombo;
        private float prevTime;
        private float buffRemovalTimer = 0f;
        private int growthBuffCount;

        [Header("Passive UI")]
        public GameObject meterPrefab = Modules.Assets.passiveMeter;
        private Image passiveMeter;
        private bool allCreated;
        [SerializeField]
        private TMPro.TextMeshProUGUI meterText;
        private OverlayController overlayController;
        private GameObject maxSparks;
        private float textFadeInTime = 1f;
        private float textFadeOutTime = 1f;
        private float textFlashTimer;
        private Color meterTextMaxColor = new Color(255, 210, 0);

        [Header("Passive VFX")]
        private CharacterModel characterModel;

        private void Awake()
        {
            body = base.GetComponent<CharacterBody>();
            //start with no skills used, so lastskillused is null
            lastSkillUsed = null;

            //start with a nullified prevTime
            prevTime = -1f;
        }


        private void FixedUpdate()
        {

            #region passive functionality
            if (NetworkServer.active)
            {
                growthBuffCount = body.GetBuffCount(Buffs.damageGrowth);
                buffRemovalTimer -= Time.fixedDeltaTime;
                //constantly count with timer, it's a float so this won't overflow unless you keep the game open for multiple years
                comboTimer += Time.fixedDeltaTime;
                //use another timer that doesn't reset on skill use; this one only resets when we have a combo, in order to prevent players from maintaining damage by spamming their m1
                repeatTimer += Time.fixedDeltaTime;

                if (newSkillUsed != null && lastSkillUsed != null)
                {
                    //now, we will only apply the buff if the new skill used is different from the last skill used and if prevTime is valid
                    if ((newSkillUsed.GetType().Name != lastSkillUsed.GetType().Name) && (prevTime != -1f) && (prevTime < comboTime))
                    {
                        
                        repeatTimer = 0f;
                        //Debug.Log("continuing");
                        //this means we are now in a skill combo
                        isInCombo = true;

                        //we now nullify prevTime by setting it = -1; through this, we essentially use prevTime as both our checker for whether or not a skill was used, and if the skill was used within a specific amount of time since the last skill
                        prevTime = -1f;

                        //if we don't have the max amount of growthbuffstacks, then add a stack
                        if (growthBuffCount < maxBuffStacks)
                        {
                            //Debug.Log("adding buff");
                            body.AddBuff(Buffs.damageGrowth);
                        }

                    }
                }
                //if we aren't in a combo, start removing damage buff stacks
                if (repeatTimer > comboTime)
                {
                    //can't use growthbuffCount here because that gets assigned at the start of fixedUpdate and won't change over the course of the while loop
                    if (body.GetBuffCount(Buffs.damageGrowth) > 0)
                    {
                        comboTimer += Time.fixedDeltaTime;
                        RemoveBuffTimed();


                    }
                }

            }

            //now the last skill used is the skill we just used
            lastSkillUsed = newSkillUsed;
            #endregion

            #region passive UI
            if (overlayController == null)
            {
                CreateOverlay();
            }
            else
            {
                UpdateValues();
            }
 

            #endregion

        }
        private void OverlayController_onInstanceAdded(OverlayController overlayController, GameObject instance)
        {   
            instance.transform.localPosition = new Vector3(-685f, -600f, 0f);
            float sizeScale = 0.14f;
            instance.transform.localScale = new Vector3(.14f, sizeScale, sizeScale);
            instance.transform.rotation = Quaternion.Euler(0, 0, 2.8f);

            //batteryRings = instance.transform.Find("OuterRings").GetComponent<Image>();
            passiveMeter = instance.transform.Find("MeterFill").GetComponent<Image>();
            //batteryGlow = instance.transform.Find("Glow").GetComponent<Image>();
            meterText = instance.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
            //batteryPip = instance.transform.Find("Pip").GetComponent<Image>();
            maxSparks = instance.transform.Find("Max Sparks").gameObject;
            meterText.color = new Color(255, 255, 255, 1);
            

            if (passiveMeter && meterText) allCreated = true;

        }

        private void OnDisable()
        {
            if (overlayController != null)
            {
                overlayController.onInstanceAdded -= OverlayController_onInstanceAdded;
                HudOverlayManager.RemoveOverlay(overlayController);
            }

        }

        private void FlashText()
        {
            textFlashTimer += Time.fixedDeltaTime;
            if (textFlashTimer < textFadeInTime)
            {
                meterText.color = new Color(meterTextMaxColor.r, meterTextMaxColor.g, meterTextMaxColor.b, textFlashTimer / textFadeInTime);
            }
            else if (textFlashTimer < textFadeInTime + textFadeOutTime)
            {
                meterText.color = new Color(meterTextMaxColor.r, meterTextMaxColor.g, meterTextMaxColor.b, 1 - (textFlashTimer - (textFadeInTime))/textFadeOutTime);
            } 
            else
            {
                textFlashTimer = 0;
            }
        }

        private void UpdateValues()
        {
            if (overlayController == null) return;
            if (!passiveMeter || !meterText) return;

            float fill = Mathf.Clamp01(((float)body.GetBuffCount(Buffs.damageGrowth)) / ((float)(maxBuffStacks)));
            passiveMeter.fillAmount = fill;
            //Debug.Log("fill amount is " + fill);
            //batteryRings.fillAmount = fill;
            //batteryGlow.fillAmount = fill;

            //Debug.Log("setting text to " + Mathf.FloorToInt(fill).ToString() + "%");

            if (fill == 1)
            {
                maxSparks.SetActive(true);
                meterText.SetText("MAX");
                FlashText();

            }
            else if (fill != 1) 
            {
                maxSparks.SetActive(false);
                meterText.color = new Color(255, 255, 255, 1);
                meterText.SetText((fill * 100) + "%");
            }
        }

        private void CreateOverlay()
        {
            OverlayCreationParams overlayCreationParams = new OverlayCreationParams
            {
                prefab = Modules.Assets.passiveMeter,
                childLocatorEntry = "CrosshairExtras"
            };

            UpdateValues();
            overlayController = HudOverlayManager.AddOverlay(base.gameObject, overlayCreationParams);
            overlayController.onInstanceAdded += OverlayController_onInstanceAdded;
        }

        public void RemoveBuffTimed()
        {
            if (!NetworkServer.active || buffRemovalTimer > 0f)
            {
                return;
            }
            //Debug.Log(buffRemovalTimer);
            buffRemovalTimer = .4f;
            //Debug.Log(buffRemovalTimer + " after reset");
            body.RemoveBuff(Buffs.damageGrowth);
            //Debug.Log("removing buff");
        }

        public void resetComboTimer()
        {
            //store the current value of the timer in prevTime
            prevTime = comboTimer;
            comboTimer = 0f;
        }
    }
}
