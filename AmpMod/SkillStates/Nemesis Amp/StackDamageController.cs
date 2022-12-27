using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using UnityEngine.Networking;
using EntityStates;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    [RequireComponent(typeof(CharacterBody))]
    class StackDamageController : MonoBehaviour
    {
        private CharacterBody body;
        private float comboTime = Modules.StaticValues.comboTimeInterval;
        private float damageBuffRemovalRate = Modules.StaticValues.growthBuffDisappearanceRate;
        private int maxBuffStacks = StaticValues.growthBuffMaxStacks;
        private BaseState lastSkillUsed;
        public BaseState newSkillUsed;
        private ChildLocator childLocator;
        private float comboTimer;
        private bool isInCombo;
        private float prevTime;
        private float buffRemovalTimer;
        private int growthBuffCount;
        

        private void Start()
        {
            body = base.GetComponent<CharacterBody>();
            childLocator = base.GetComponent<ChildLocator>();

            //start with no skills used, so lastskillused is null
            lastSkillUsed = null;

            //start with a nullified prevTime
            prevTime = -1f;
        }

        private void FixedUpdate()
        {

            growthBuffCount = body.GetBuffCount(Buffs.damageGrowth);

            if (NetworkServer.active)
            {
                //constantly count with timer, it's a float so this won't overflow unless you keep the game open for multiple years
                comboTimer += Time.fixedDeltaTime;


                //now, we will only apply the buff if the new skill used is different from the last skill used and if prevTime is valid
                if (newSkillUsed != lastSkillUsed && lastSkillUsed != null && prevTime != -1f && prevTime < comboTime)
                {
                    //this means we are now in a skill combo
                    isInCombo = true;

                    //we now nullify prevTime by setting it = -1; through this, we essentially use prevTime as both our checker for whether or not a skill was used, and if the skill was used within a specific amount of time since the last skill
                    prevTime = -1f;

                    //if we don't have the max amount of growthbuffstacks, then add a stack
                    if (growthBuffCount <= maxBuffStacks)
                    {
                        body.AddBuff(Buffs.damageGrowth);
                    }


                }

                //if it's been longer than comboTime, tell us we're not in a combo
                if (comboTimer > comboTime)
                {
                    isInCombo = false;

                }
                
                //if we aren't in a combo, start removing damage buff stacks
                if (!isInCombo)
                {
                    while (growthBuffCount > 0)
                    {
                        comboTimer += Time.fixedDeltaTime;
                        this.buffRemovalTimer -= Time.fixedDeltaTime;

                        //removes a stack of the buff every .4 seconds, the value of damageBuffRemovalRate
                        if (this.buffRemovalTimer <= 0f)
                        {
                            buffRemovalTimer = damageBuffRemovalRate;
                            body.RemoveBuff(Buffs.damageGrowth);

                        }

                    }
                }




            }

            //now the last skill used is the skill we just used
            lastSkillUsed = newSkillUsed;


        }

        public void resetComboTimer()
        {
            //store the current value of the timer in prevTime
            prevTime = comboTimer;
            comboTimer = 0f;
        }
    }
}
