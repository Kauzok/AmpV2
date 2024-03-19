using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using RoR2.Stats;
using R2API;
using System.Collections.Generic;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp;
using AmpMod.Modules;

namespace AmpMod.Modules.Survivors
{
    internal class NemAmp : SurvivorBase
    {
        internal override string bodyName { get; set; } = "NemAmp";

        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override bool isAmp { get; set; } = false;

        internal override bool isNemAmp { get; set; } = true;

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = "NemAmpBody",
            //bodyName = "NemesisAmpBody",
            bodyNameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_NAME",
            //Color of skill names and stuff in menu
            bodyColor = new Color32(139, 0, 255, 255),
            characterPortrait = Modules.Assets.LoadCharacterIcon("Nem"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            moveSpeed = 7.5f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            //jumpPower = 18f,
            maxHealth = 110f,
            subtitleNameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_SUBTITLE",
            //podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };



        internal static Material capeMat = Modules.Assets.CreateMaterial("matNemCape");
        internal static Material suitMat = Modules.Assets.CreateMaterial("matNemSuit");
        internal override int mainRendererIndex { get; set; } = 0;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {

            new CustomRendererInfo
            {
                childName = "Suit",
                material = suitMat,

            },
                new CustomRendererInfo
            {
                childName = "Cape",
                material = capeMat,

            }
            };

        internal override Type characterMainState { get; set; } = typeof(NemAmpMain);

        public override ItemDisplaysBase itemDisplays => new NemAmpItemDisplays();

        internal override UnlockableDef characterUnlockableDef { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private static UnlockableDef masterySkinUnlockableDef;
        private static UnlockableDef dashSkillUnlockableDef;
        private static UnlockableDef bladesSkillUnlockableDef;
        private static UnlockableDef photonSkillUnlockableDef;

        public static readonly StatDef nemAmpTotalVoidEnemiesKilled = StatDef.Register("nemAmpTotalVoidEnemiesKilled", StatRecordType.Sum, StatDataType.ULong, 0.0, null);

        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        internal override void InitializeUnlockables()
        {

            if (!Config.UnlockNemMasterySkin.Value)
            {
                masterySkinUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                masterySkinUnlockableDef.cachedName = "Skins.Origin";
                masterySkinUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY";
                masterySkinUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemMasteryAchievement");
                ContentAddition.AddUnlockableDef(masterySkinUnlockableDef);
            }

            if (!Config.NemUnlockDashSkill.Value)
            {
                dashSkillUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                dashSkillUnlockableDef.cachedName = "Skills.VoidDash";
                dashSkillUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_DASH";
                dashSkillUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemDash");
                ContentAddition.AddUnlockableDef(dashSkillUnlockableDef);
            }

            if (!Config.NemUnlockBladesSkill.Value)
            {
                bladesSkillUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                bladesSkillUnlockableDef.cachedName = "Skills.TrackingBlades";
                bladesSkillUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_BLADES";
                bladesSkillUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemBlades");
                ContentAddition.AddUnlockableDef(bladesSkillUnlockableDef);
            }

           //if (!Config.NemUnlockPhotonSkill.Value)
         //   {
                photonSkillUnlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
                photonSkillUnlockableDef.cachedName = "Skills.Laser";
                photonSkillUnlockableDef.nameToken = AmpPlugin.developerPrefix + "_NEMAMP_BODY_LASER";
                photonSkillUnlockableDef.achievementIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemPhoton");
                ContentAddition.AddUnlockableDef(photonSkillUnlockableDef);
           // } 


        }

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes()
        {
          
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform bigSlashTransform = childLocator.FindChild("CleaveHitbox");
            Modules.Prefabs.SetupHitbox(model, bigSlashTransform, "Cleave");
             

        }

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab, "Nemesis Amp");

            string prefix = AmpPlugin.developerPrefix;


            #region Primary
            //creates Fulmination
             NemAmpOrbTrackingSkillDef primaryLightningStreamDef =
                 Skills.CreatePrimarySkillDef<NemAmpOrbTrackingSkillDef>(new EntityStates.SerializableEntityStateType(typeof(LightningStream)), 
                 true,
                  "Weapon",
                 prefix + "_NEMAMP_BODY_PRIMARY_LIGHTNING_NAME",
                 prefix + "_NEMAMP_BODY_PRIMARY_LIGHTNING_DESCRIPTION",
                 Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemFulmination"),
                 true,
                 new String[] { },
                 false); 
                Modules.Skills.AddPrimarySkill(bodyPrefab, primaryLightningStreamDef);  
             

           /* Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(LightningStream)),
                  "Weapon",
                  prefix + "_NEMAMP_BODY_PRIMARY_LIGHTNING_NAME",
                  prefix + "_NEMAMP_BODY_PRIMARY_LIGHTNING_DESCRIPTION",
                  Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemFulmination"),
                  true,
                  new String[] { },
                  false)); */
            

            //creates Lorentz Blades
            Modules.Skills.AddUnlockablePrimarySkill(bodyPrefab, 
                Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(FluxBlades)),
                "Weapon",
                prefix + "_NEMAMP_BODY_PRIMARY_BLADES_NAME",
                prefix + "_NEMAMP_BODY_PRIMARY_BLADES_DESCRIPTION",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemBlades"),
                true,
                new String[] { },
                false),
                bladesSkillUnlockableDef); 
            #endregion


            #region Secondary
            //creates howitzer spark
            SkillDef beamSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_SECONDARY_CHARGEBEAM_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_SECONDARY_CHARGEBEAM_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_SECONDARY_CHARGEBEAM_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemBeam"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeLightningBeam)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
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
                keywordTokens = new string[] { }
            });

            //creates void slash; remember to make a config that allows users to put this as a special skill or a secondary
            SkillDef slashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_SECONDARY_SLASH_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_SECONDARY_SLASH_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_SECONDARY_SLASH_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemSlash"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(LightningSlash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                //fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            }); ;


            Modules.Skills.AddSecondarySkills(bodyPrefab, beamSkillDef, slashSkillDef);
            //Modules.Skills.AddSecondarySkills(bodyPrefab, beamSkillDef);
            #endregion


            #region Utility
            //creates static field
            SkillDef fieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_UTILITY_FIELD_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_UTILITY_FIELD_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_UTILITY_FIELD_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemField"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimStaticField)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                //fullRestockOnAssign = true,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });

            //creates quicksurge
            SkillDef quickDashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_UTILITY_DASH_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_UTILITY_DASH_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_UTILITY_DASH_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texNemDash"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(QuickDash)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                //fullRestockOnAssign = true,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });;


            Modules.Skills.AddUtilitySkills(bodyPrefab, fieldSkillDef);
            Modules.Skills.AddUnlockableUtilitySkill(bodyPrefab, quickDashSkillDef, dashSkillUnlockableDef);
            #endregion


            #region Specials
            //creates voltaic onslaught
            SkillDef stormSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_SPECIAL_SUMMONSTORM_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_SPECIAL_SUMMONSTORM_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_SPECIAL_SUMMONSTORM_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemStorm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AOELightning)),
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
                keywordTokens = new string[] { }
            });

            SkillDef photonSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMAMP_BODY_SPECIAL_LASER_NAME",
                skillNameToken = prefix + "_NEMAMP_BODY_SPECIAL_LASER_NAME",
                skillDescriptionToken = prefix + "_NEMAMP_BODY_SPECIAL_LASER_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemStorm"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(PhotonShot)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                // fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });



            ;


            Modules.Skills.AddSpecialSkills(bodyPrefab, stormSkillDef, photonSkillDef);
            //Modules.Skills.AddUnlockableSpecialSkill(bodyPrefab, photonSkillDef, photonSkillUnlockableDef);
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

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_NEMAMP_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texNemDefault"),
                defaultRenderers,
                mainRenderer,
                updatedModel);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshNemAmp"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshNemCape"),
                    renderer = defaultRenderers[1].renderer
                },
            };

            skins.Add(defaultSkin);
            #endregion
               

            #region MasterySkin
            Material masterySuitMat = Modules.Assets.CreateMaterial("matNemSuitMastery");
            Material masteryCapeMat = Modules.Assets.CreateMaterial("matNemCapeMastery");


            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masterySuitMat,
                masteryCapeMat

            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texNemMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshNemAmp"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshNemCape"),
                    renderer = defaultRenderers[1].renderer
                }
            }; 


            skins.Add(masterySkin);
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