using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using RoR2.Stats;
using R2API;
using System.Collections.Generic;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp;

namespace AmpMod.Modules.Survivors
{
    internal class NemAmp : SurvivorBase
    {
        internal override string bodyName { get; set; } = "Henry";
        //Uncomment this line once you've gotten the model setup in unity
        //internal override string bodyName { get; set; } = "Nemesis Amp";

        internal override GameObject bodyPrefab { get; set; }
        internal override GameObject displayPrefab { get; set; }

        internal override ConfigEntry<bool> characterEnabled { get; set; }

        internal override bool isAmp { get; set; } = false;

        internal override bool isNemAmp { get; set; } = true;

        internal override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            armor = 20f,
            armorGrowth = 0f,
            bodyName = "HenryBody",
            //bodyName = "NemesisAmpBody",
            bodyNameToken = AmpPlugin.developerPrefix + "_AMP_BODY_NAME",
            //bodyNameToken = AmpPlugin.developerPrefix + "_NEMESISAMP_BODY_NAME",
            //Color of skill names and stuff in menu
            bodyColor = new Color32(139, 0, 255, 255),
            characterPortrait = Modules.Assets.LoadCharacterIcon("Amp"),
            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            damage = 12f,
            moveSpeed = 7.5f,
            healthGrowth = 33f,
            healthRegen = 1.5f,
            jumpCount = 1,
            //jumpPower = 18f,
            maxHealth = 110f,
            subtitleNameToken = AmpPlugin.developerPrefix + "_NEMESISAMP_BODY_SUBTITLE",
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod")
        };


        internal static Material henryMat = Modules.Assets.CreateMaterial("matHenry");

        internal static Material swordMat = Modules.Assets.CreateMaterial("matSword");
        internal static Material suitMat = Modules.Assets.CreateMaterial("matSuit");
        internal override int mainRendererIndex { get; set; } = 2;

        internal override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] {

             new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = henryMat,
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                    material = henryMat,
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = henryMat
                }};

        internal override Type characterMainState { get; set; } = typeof(NemAmpMain);

        public override ItemDisplaysBase itemDisplays => new AmpItemDisplays();

        internal override UnlockableDef characterUnlockableDef { get; set; }
        internal override List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        internal override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        internal override void InitializeUnlockables()
        {
          


        }

        internal override void InitializeDoppelganger()
        {
            base.InitializeDoppelganger();
        }

        internal override void InitializeHitboxes()
        {
           /* ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");

            Transform spinSlashTransform = childLocator.FindChild("SpinSlashHitbox");
            Modules.Prefabs.SetupHitbox(model, spinSlashTransform, "SpinSlash");

            */

        }

        internal override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab, "Nemesis Amp");

            string prefix = AmpPlugin.developerPrefix;


            #region Primary
            //creates Fulmination
            /* NemAmpOrbTrackingSkillDef primaryLightningStreamDef =
                 Skills.CreatePrimarySkillDef<NemAmpOrbTrackingSkillDef>(new EntityStates.SerializableEntityStateType(typeof(LightningStream)),
                  "Weapon",
                 prefix + "_NEMESISAMP_BODY_PRIMARY_LIGHTNING_NAME",
                 prefix + "_NEMESISAMP_BODY_PRIMARY_LIGHTNING_DESCRIPTION",
                 Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texStormblade"),
                 true,
                 new String[] { }); 
                Modules.Skills.AddPrimarySkill(bodyPrefab, primaryLightningStreamDef);  
             */

            Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(LightningStream)),
                  "Weapon",
                  prefix + "_NEMESISAMP_BODY_PRIMARY_LIGHTNING_NAME",
                  prefix + "_NEMESISAMP_BODY_PRIMARY_LIGHTNING_DESCRIPTION",
                  Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texStormblade"),
                  true,
                  new String[] { }));

            //creates Flux Blades
           /* Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(FluxBlades)),
                "Weapon",
                prefix + "_NEMESISAMP_BODY_PRIMARY_BLADES_NAME",
                prefix + "_NEMESISAMP_BODY_PRIMARY_BLADES_DESCRIPTION",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texStormblade"),
                true,
                new String[] { })); */
            #endregion


            #region Secondary
            //creates howitzer spark
            SkillDef beamSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMESISAMP_BODY_SECONDARY_CHARGEBEAM_NAME",
                skillNameToken = prefix + "_NEMESISAMP_BODY_SECONDARY_CHARGEBEAM_NAME",
                skillDescriptionToken = prefix + "_NEMESISAMP_BODY_SECONDARY_CHARGEBEAM_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLorentz"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeLightningBeam)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 3f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            });




            Modules.Skills.AddSecondarySkills(bodyPrefab, beamSkillDef);
            #endregion


            #region Utility
            //creates static field
            SkillDef fieldSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMESISAMP_BODY_UTILITY_FIELD_NAME",
                skillNameToken = prefix + "_NEMESISAMP_BODY_UTILITY_FIELD_NAME",
                skillDescriptionToken = prefix + "_NEMESISAMP_BODY_UTILITY_FIELD_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSurge"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimStaticField)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 7f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                //fullRestockOnAssign = true,
                fullRestockOnAssign = false,
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

            //creates quicksurge
            SkillDef quickDashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMESISAMP_BODY_UTILITY_DASH_NAME",
                skillNameToken = prefix + "_NEMESISAMP_BODY_UTILITY_DASH_NAME",
                skillDescriptionToken = prefix + "_NEMESISAMP_BODY_UTILITY_DASH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSurge"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(QuickDash)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
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
            });


            Modules.Skills.AddUtilitySkills(bodyPrefab, fieldSkillDef, quickDashSkillDef);
            #endregion


            #region Specials
            //creates voltaic onslaught
            SkillDef stormSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMESISAMP_BODY_SPECIAL_SUMMONSTORM_NAME",
                skillNameToken = prefix + "_NEMESISAMP_BODY_SPECIAL_SUMMONSTORM_NAME",
                skillDescriptionToken = prefix + "_NEMESISAMP_BODY_SPECIAL_SUMMONSTORM_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texFulmination"),
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

            //creates void slash; remember to make a config that allows users to put this as a special skill or a secondary
            SkillDef slashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_NEMESISAMP_BODY_SPECIAL_SLASH_NAME",
                skillNameToken = prefix + "_NEMESISAMP_BODY_SPECIAL_SLASH_NAME",
                skillDescriptionToken = prefix + "_NEMESISAMP_BODY_SPECIAL_SLASH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texFulmination"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeSlash)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                // fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { }
            }); ;

            ;


            Modules.Skills.AddSpecialSkills(bodyPrefab, stormSkillDef);
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
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_NEMESISAMP_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySword"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryGun"),
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                    renderer = defaultRenderers[instance.mainRendererIndex].renderer
                }
            };

            skins.Add(defaultSkin);
            #endregion


            #region MasterySkin
            /* Material masterySuitMat = Modules.Assets.CreateMaterial("matSpriteSuit");
            Material masterySwordMat = Modules.Assets.CreateMaterial("matSpriteSword");


            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masterySuitMat,
                masterySwordMat

            });

          /*  SkinDef masterySkin = Modules.Skins.CreateSkinDef(AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY_SKIN_NAME",
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
                    renderer = defaultRenderers[1].renderer
                }
            }; 





            if (!Config.RedSpriteBlueLightning.Value)
            {
                Skins.AddCSSSkinChangeResponse(masterySkin, Skins.ampCSSEffect.REDSPRITE);
            }


            skins.Add(masterySkin); */
            #endregion


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