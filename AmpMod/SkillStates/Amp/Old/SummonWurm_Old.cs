﻿using AmpMod.Modules;
using EntityStates;
using RoR2;
using R2API;
using UnityEngine;
using RoR2.Skills;
using UnityEngine.Networking;

namespace AmpMod.SkillStates
{
    public class SummonWurm_Old : BaseSkillState
    {
        string prefix = AmpPlugin.developerPrefix;
        private GenericSkill specialSlot;
        public static SkillDef cancelSkillDef;
        public static CharacterMaster wormMaster;
        public CharacterBody wormBody;
        public static GameObject wormExplosion;
        public static object src = new SummonWurm_Old();
        private bool hasSpawned;
        private float wormLifeDuration = 30f;
        private Animator animator;
        private string summonSoundString = Modules.StaticValues.wormSummonString;


        public override void OnEnter()
        {

            base.OnEnter();
            base.PlayAnimation("Worm, Override", "WormChannelRelease", "BaseSkill.playbackRate", .5f);

            

            animator = base.GetModelAnimator();

            cancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_WORMCANCEL_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_WORMCANCEL_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_WORMCANCEL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texReturn"),
                activationStateMachineName = "Slide",
                activationState = new SerializableEntityStateType(typeof(CancelWurm)),
                baseMaxStock = 0,
                baseRechargeInterval = 0,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 0,
                stockToConsume = 0,
            });
            specialSlot = base.skillLocator.special;
            if (this.specialSlot && cancelSkillDef != null)
            {
                this.specialSlot.SetSkillOverride(src, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }


            if (base.characterBody)
            {
                SpawnWorm(base.characterBody);
            }
            


            animator.SetBool("HasChannelled", false);

        }

        private void SpawnWorm(CharacterBody characterBody)
        {
            GameObject masterPrefab = Modules.Assets.melvinPrefab;

           // masterPrefab.GetComponent<CharacterMaster>().GetBody().baseMaxHealth = base.characterBody.baseMaxHealth * 3f;

            Util.PlaySound(summonSoundString, base.gameObject);

            //body.baseMaxHealth = 1f;

            EffectManager.SimpleMuzzleFlash(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleFlashEffectPrefab, base.gameObject, "HandL", false);

            //Debug.Log("Spawning worm");


            //figure out why this doesnt make worm follow amp
            //masterPrefab.gameObject.GetComponent<BaseAI>().leader.gameObject = characterBody.gameObject;
            //masterPrefab.GetComponent<HealthComponent>().health = 3*base.characterBody.maxHealth;

            if (NetworkServer.active)
            {


                MasterSummon wormSummon = new MasterSummon
                {
                    masterPrefab = masterPrefab,
                    ignoreTeamMemberLimit = false,
                    teamIndexOverride = TeamIndex.Player,
                    summonerBodyObject = characterBody.gameObject,
                    position = characterBody.corePosition + new Vector3(0, 0, 2),
                    rotation = characterBody.transform.rotation,
                    inventoryToCopy = characterBody.inventory,
                    inventoryItemCopyFilter = index => index != RoR2Content.Items.ExtraLife.itemIndex && index != DLC1Content.Items.ExtraLifeVoid.itemIndex,
                    

                };

                wormSummon.preSpawnSetupCallback = master => master.inventory.GiveItem(Assets.wormHealth);


                wormMaster = wormSummon.Perform();
            }
            
            

            if (wormMaster)
            {
                wormBody = wormMaster.GetBody();

                wormMaster.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = wormLifeDuration;
                //wormMaster.gameObject.GetComponent<BaseAI>().leader.gameObject = base.characterBody.gameObject;
                //wormMaster.onBodyStart += SetupWorm;
               // R2API.RecalculateStatsAPI.GetStatCoefficients += recalcWorm;
                

                if (NetworkServer.active)
                {
                    // Debug.Log(wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife.itemIndex) + "dios in inventory");
                    for (int i = -1; i <= wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife.itemIndex) + 1; i++)
                    {
                        wormMaster.inventory.RemoveItem(RoR2Content.Items.ExtraLife.itemIndex);
                    }


                    //Debug.Log(wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife.itemIndex) + "dios in inventory");

                    for (int i = 0; i <= wormMaster.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid.itemIndex) + 1; i++)
                    {
                        wormMaster.inventory.RemoveItem(DLC1Content.Items.ExtraLifeVoid.itemIndex);


                    }

                 
                }

                if (wormMaster.GetBody())
                {
                    //Debug.Log("adding worm tracker");
                    GameObject wormObject = wormMaster.GetBodyObject();

                   

                    var healthTracker = wormObject.AddComponent<SkillComponents.WormHealthTracker>();
                   // healthTracker.wormBody = wormMaster.GetBody();
                    healthTracker.owner = base.gameObject;
                    healthTracker.wormSkill = src;
                    
                    healthTracker.wormMaster = wormMaster;
                    healthTracker.cancelSkillDef = cancelSkillDef;
                    healthTracker.specialSlot = base.skillLocator.special;



                }

                    
            }

            
            

          

        }



        private void SetupWorm(CharacterBody body)
        {
            body.statsDirty = true;

            //body.baseMaxHealth = 3f * base.characterBody.baseMaxHealth;

            // body.maxHealth = body.baseMaxHealth;
            //   body.baseNameToken = prefix + "_AMP_BODY_SPECIAL_WORM_DISPLAY_NAME";

            //body.statsDirty = true;

            /*   var healthTracker = body.gameObject.AddComponent<SkillComponents.WormHealthTracker>();
               healthTracker.specialSlot = base.skillLocator.special;
               healthTracker.wormBody = body;
               healthTracker.wormSkill = src;
               healthTracker.cancelSkillDef = cancelSkillDef;
               healthTracker.wormMaster = wormMaster; */

        }


        private void recalcWorm(CharacterBody body, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {


            if (body.bodyIndex == wormBody.bodyIndex)
            {
                args.baseHealthAdd -= (wormBody.baseMaxHealth - base.characterBody.baseMaxHealth * 3f);
            }
        

            /*   var healthTracker = body.gameObject.AddComponent<SkillComponents.WormHealthTracker>();
               healthTracker.specialSlot = base.skillLocator.special;
               healthTracker.wormBody = body;
               healthTracker.wormSkill = src;
               healthTracker.cancelSkillDef = cancelSkillDef;
               healthTracker.wormMaster = wormMaster; */

        }


        public override void OnExit()
        {
            
            base.OnExit();
            this.specialSlot.UnsetSkillOverride(this, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            //  wormBody.moveSpeed = 0f;
            // Debug.Log(wormBody.moveSpeed);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();

            }


        }
     }

}
