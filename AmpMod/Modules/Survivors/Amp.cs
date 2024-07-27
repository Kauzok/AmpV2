using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using RoR2.Stats;
using R2API;
using System.Collections.Generic;
using UnityEngine;

namespace AmpMod.Modules.Survivors
{
    internal class Amp : SurvivorBase
    {
        //internal override string bodyName { get; set; } = "Henry";
        //Uncomment this line once you've gotten the model setup in unity
        internal override string bodyName { get; set; } = "Amp";

        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override bool isAmp { get; set; } = true;

        internal override bool isNemAmp { get; set; } = false;

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = "AmpBody",
            //bodyName = "AmpVoltBody",
            bodyNameToken = AmpPlugin.developerPrefix + "_AMP_BODY_NAME",
            //Color of skill names and stuff in menu
            bodyColor = new Color32(0, 145, 255, 255),
            characterPortrait = Modules.Assets.LoadCharacterIcon("Amp"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            moveSpeed = 7.5f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            shield = 55f,
            //jumpPower = 18f,
            maxHealth = 55f,
            shieldGrowth = 33f,
            subtitleNameToken = AmpPlugin.developerPrefix + "_AMP_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };

        
       
        internal static Material swordMat = Modules.Assets.CreateMaterial("matSword");
        internal static Material suitMat = Modules.Assets.CreateMaterial("matSuit");
        internal override int mainRendererIndex { get; set; } = 1;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {

                 new CustomRendererInfo
                {
                    childName = "Sword",
                    material = swordMat,
                },
                new CustomRendererInfo
                {
                    childName = "Suit",
                    material = suitMat,
              
                }}; 

        internal override Type characterMainState { get; set; } = typeof(AmpMain);

        public override ItemDisplaysBase itemDisplays => new AmpItemDisplays();

        internal override UnlockableDef characterUnlockableDef { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private static UnlockableDef masterySkinUnlockableDef;
        //private static UnlockableDef grandMasterySkinUnlockableDef;
        private static UnlockableDef wormSkillUnlockableDef;
        private static UnlockableDef plasmaSkillUnlockableDef;
        public static readonly StatDef ampTotalBurnedEnemiesKilled = StatDef.Register("ampTotalBurnedEnemiesKilled", StatRecordType.Sum, StatDataType.ULong, 0.0, null);

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        internal override void InitializeUnlockables()
        {
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Achievements.AmpMasteryAchievement>(true);
            if (!Config.UnlockMasterySkin.Value)
            {
                masterySkinUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                masterySkinUnlockableDef.cachedName = "Skins.RedSprite";
                masterySkinUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY";
                masterySkinUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement");
                ContentAddition.AddUnlockableDef(masterySkinUnlockableDef);
            }

         /*   if (!Config.UnlockGrandMasterySkin.Value)
            {
                grandMasterySkinUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                grandMasterySkinUnlockableDef.cachedName = "Skins.Reformation";
                grandMasterySkinUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_AMP_BODY_GRANDMASTERY";
                grandMasterySkinUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texGrandMasteryAchievement");
                ContentAddition.AddUnlockableDef(grandMasterySkinUnlockableDef);
            } */

            if (!Config.UnlockWormSkill.Value)
            {
                wormSkillUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                wormSkillUnlockableDef.cachedName = "Skills.SummonWurm";
                wormSkillUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_AMP_BODY_USURPER";
                wormSkillUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSummon");
                ContentAddition.AddUnlockableDef(wormSkillUnlockableDef);
            }

            if (!Config.UnlockPlasmaSkill.Value)
            {
                plasmaSkillUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                plasmaSkillUnlockableDef.cachedName = "Skills.PlasmaSlash";
                plasmaSkillUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_AMP_BODY_PLASMA";
                plasmaSkillUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPlasma");
                ContentAddition.AddUnlockableDef(plasmaSkillUnlockableDef);
            }



        }

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");

            Transform spinSlashTransform = childLocator.FindChild("SpinSlashHitbox");
            Modules.Prefabs.SetupHitbox(model, spinSlashTransform, "SpinSlash");

            
          
        }

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab, "Amp");

            string prefix = AmpPlugin.developerPrefix;


            #region Primary
            //creates Stormblade
            Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)), 
                "Weapon", 
                prefix + "_AMP_BODY_PRIMARY_SLASH_NAME", 
                prefix + "_AMP_BODY_PRIMARY_SLASH_DESCRIPTION", 
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texStormblade"), 
                true, 
                new String[] { "KEYWORD_AGILE", prefix + "_AMP_BODY_KEYWORD_CHARGE", prefix + "_AMP_BODY_KEYWORD_HEALSHIELD" },
                true));
            #endregion


            #region Secondary
            //creates ferroshot/Lorentz Cannon
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SECONDARY_FERROSHOT_NAME",
                skillNameToken = prefix + "_AMP_BODY_SECONDARY_FERROSHOT_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SECONDARY_FERROSHOT_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLorentz"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Ferroshot_Hitscan)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", prefix + "_AMP_BODY_KEYWORD_SANDED"}
            });

            SkillDef vortexSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SECONDARY_VORTEX_NAME",
                skillNameToken = prefix + "_AMP_BODY_SECONDARY_VORTEX_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SECONDARY_VORTEX_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texVortex"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Vortex)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 9f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE" }
            }); 

            SkillDef burnSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SECONDARY_PLASMASLASH_NAME",
                skillNameToken = prefix + "_AMP_BODY_SECONDARY_PLASMASLASH_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SECONDARY_PLASMASLASH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPlasma"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PlasmaSlash)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 3.5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "_AMP_BODY_KEYWORD_HEALSHIELD" }
            }); 


            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef, vortexSkillDef);
            Modules.Skills.AddUnlockableSecondarySkill(bodyPrefab, burnSkillDef, plasmaSkillUnlockableDef);
            #endregion


            #region Utility
            //creates bolt
            SkillDef dashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_UTILITY_DASH_NAME",
                skillNameToken = prefix + "_AMP_BODY_UTILITY_DASH_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSurge"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Surge)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                //fullRestockOnAssign = true,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "_AMP_BODY_KEYWORD_CHARGE" }
            });

            //creates pulse leap 
            
            SkillDef boostSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_UTILITY_BOOST_NAME",
                skillNameToken = prefix + "_AMP_BODY_UTILITY_BOOST_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_UTILITY_BOOST_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPulse"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PulseLeap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "_AMP_BODY_KEYWORD_CHARGE" }
            }) ;

            
            Modules.Skills.AddUtilitySkills(bodyPrefab, dashSkillDef, boostSkillDef);
            #endregion


            #region Specials
            //creates fulmination
            SkillDef chainSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_CHAIN_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_CHAIN_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_CHAIN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texFulmination"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fulmination)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                // fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_AGILE", prefix + "_AMP_BODY_KEYWORD_CHARGE" }
            });
          
            //creates voltaic bombardment
            SkillDef lightningSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_LIGHTNING_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_LIGHTNING_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_LIGHTNING_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texVoltaic"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.LightningCrash.LightningCrash_Jump)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { prefix + "_KEYWORD_REPLENISHING" }//, prefix + "_AMP_BODY_KEYWORD_DOUBLECHARGE" }
            });
            //creates 
            SkillDef wormSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_WORM_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_WORM_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_WORM_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSummon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SummonWurm)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 15f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                // fullRestockOnAssign = true,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE", prefix + "_AMP_BODY_KEYWORD_DOUBLECHARGE" }


            }); 

            ;


            Modules.Skills.AddSpecialSkills(bodyPrefab, chainSkillDef, lightningSkillDef);
            Modules.Skills.AddUnlockableSpecialSkill(bodyPrefab, wormSkillDef, wormSkillUnlockableDef);
            #endregion
        }

        public static GameObject UpdateGameObjectShader(GameObject originalModel, bool dither = true)
        {

            GameObject result;
            if (originalModel)
            {
                foreach (MeshRenderer meshRenderer in originalModel.GetComponentsInChildren<MeshRenderer>())
                {

                    if (meshRenderer)
                    {
                        meshRenderer.material.shader = HGstandard;
                        Debug.Log("Changing shaders");
                        if (dither)
                        {
                            Debug.Log("Adding dither");
                            meshRenderer.material.EnableKeyword("DITHER");
                        }
                    }
                }
                result = originalModel;
            }
            else
            {
                result = null;
            }
            return result;
        }
        public static Shader HGstandard = LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");




        internal override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;

            GameObject updatedModel = UpdateGameObjectShader(model);

            CharacterModel characterModel = updatedModel.GetComponent<CharacterModel>();

            GameObject displayPrefab = this.displayPrefab;
            ChildLocator displayChildLocator = displayPrefab.GetComponent<ChildLocator>();
            Skins.ampCSSPreviewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            Skins.ampCSSPreviewController.bodyPrefab = bodyPrefab;

            ChildLocator childLocator = updatedModel.GetComponent<ChildLocator>();

            GameObject sword = childLocator.FindChild("SwordPlace").gameObject;
            Skins.allGameObjectActivations.Add(sword);
            Skins.defaultResponses = Skins.ampCSSPreviewController.skinChangeResponses;
            SkinDef.GameObjectActivation[] defaultActivations = Skins.getActivations(sword);


            ModelSkinController skinController = updatedModel.AddComponent<ModelSkinController>();
            

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_AMP_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                updatedModel);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Sword"),
                    renderer = defaultRenderers[0].renderer,
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Suit"),
                    renderer = defaultRenderers[1].renderer
                },
               /* new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                } */
            };
            Modules.Skins.AddCSSSkinChangeResponse(defaultSkin, Skins.ampCSSEffect.DEFAULT);

            skins.Add(defaultSkin);
            #endregion


            #region MasterySkin
            Material masterySuitMat = Modules.Assets.CreateMaterial("matSpriteSuit");
            Material masterySwordMat = Modules.Assets.CreateMaterial("matSpriteSword");


            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masterySuitMat,
                masterySwordMat

            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("SpriteSuitMesh"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("Sword"),
                    renderer = defaultRenderers[1/*instance.mainRendererIndex*/].renderer
                }
            };

          

            

            if (!Config.RedSpriteBlueLightning.Value)
            {
                Skins.AddCSSSkinChangeResponse(masterySkin, Skins.ampCSSEffect.REDSPRITE);
            }
            
            
            skins.Add(masterySkin);
            #endregion


            #region ReformationSkin
            //MasteryAmp
            Material golemSuitMat = Modules.Assets.CreateMaterial("matEnel");
            Material golemSwordMat = Modules.Assets.CreateMaterial("matEnel");



            CharacterModel.RendererInfo[] golemRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                //golesuitMat, golemSwordMat
                golemSuitMat,
                golemSwordMat

            });

            SkinDef golemSkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_AMP_BODY_GOLEM_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texReformationSkin"),
                golemRendererInfos,
                mainRenderer,
                updatedModel);
                //grandMasterySkinUnlockableDef);

            golemSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    //masterybody
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("EnelBodyMesh"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    //masterysword
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("EnelRod"),
                    renderer = defaultRenderers[1].renderer
                }
            };

            Skins.AddCSSSkinChangeResponse(golemSkin, Skins.ampCSSEffect.DEFAULT);

            skins.Add(golemSkin);
            #endregion


            skinController.skins = skins.ToArray();
        }

      



        private static CharacterModel.RendererInfo[] SkinRendererInfos(CharacterModel.RendererInfo[] defaultRenderers, Material[] materials)
        {
            CharacterModel.RendererInfo[] newRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(newRendererInfos, 0);

            newRendererInfos[0].defaultMaterial = materials[0];
            newRendererInfos[1].defaultMaterial = materials[1];
            //newRendererInfos[instance.mainRendererIndex].defaultMaterial = materials[2];

            return newRendererInfos;
        }
    }
}