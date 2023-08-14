using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmpMod.Modules
{
    internal static class Skills
    {
        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();
        internal static SkillDef surgeCancelSkillDef;
        public static SkillDef fulminationCancelSkillDef;
        public static SkillDef fireLightningBallSkillDef;
        public static string prefix = AmpPlugin.developerPrefix;

        internal static void CreateSkillFamilies(GameObject targetPrefab, string BodyName)
        {

            foreach (GenericSkill obj in targetPrefab.GetComponentsInChildren<GenericSkill>())
            {
                AmpPlugin.DestroyImmediate(obj);
            }

            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            skillLocator.primary = targetPrefab.AddComponent<GenericSkill>();
            SkillFamily primaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (primaryFamily as ScriptableObject).name = targetPrefab.name + "PrimaryFamily";
            primaryFamily.variants = new SkillFamily.Variant[0];
            skillLocator.primary._skillFamily = primaryFamily;

            //for making passive
            CreatePassiveSkill(skillLocator, BodyName);
           

            skillLocator.secondary = targetPrefab.AddComponent<GenericSkill>();
            SkillFamily secondaryFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (secondaryFamily as ScriptableObject).name = targetPrefab.name + "SecondaryFamily";
            secondaryFamily.variants = new SkillFamily.Variant[0];
            skillLocator.secondary._skillFamily = secondaryFamily;
            

            skillLocator.utility = targetPrefab.AddComponent<GenericSkill>();
            SkillFamily utilityFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (utilityFamily as ScriptableObject).name = targetPrefab.name + "UtilityFamily";
            utilityFamily.variants = new SkillFamily.Variant[0];
            skillLocator.utility._skillFamily = utilityFamily;

            skillLocator.special = targetPrefab.AddComponent<GenericSkill>();
            SkillFamily specialFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (specialFamily as ScriptableObject).name = targetPrefab.name + "SpecialFamily";
            specialFamily.variants = new SkillFamily.Variant[0];
            skillLocator.special._skillFamily = specialFamily;


            skillFamilies.Add(primaryFamily);
            skillFamilies.Add(secondaryFamily);
            skillFamilies.Add(utilityFamily);
            skillFamilies.Add(specialFamily);

            fireLightningBallSkillDef = CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.Nemesis_Amp.FireLightningBall)),
                "Weapon",
                prefix + "_NEMAMP_BODY_UTILITY_LIGHTNINGBALL_NAME",
                prefix + "_NEMAMP_BODY_UTILITY_LIGHTNINGBALL_DESCRIPTION",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemPlasmaBall"),
                false,
                new String[] { },
                true);

            surgeCancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCancelSurge"),
                activationStateMachineName = "Slide",
                activationState = new SerializableEntityStateType(typeof(SkillStates.CancelSkill)),
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


            fulminationCancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_AMP_BODY_SPECIAL_CANCELCHAIN_NAME",
                skillNameToken = prefix + "_AMP_BODY_SPECIAL_CANCELCHAIN_NAME",
                skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_CANCELCHAIN_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCancelFulmination"),
                activationStateMachineName = "Slide",
                baseMaxStock = 0,
                baseRechargeInterval = 0,
                activationState = new SerializableEntityStateType(typeof(SkillStates.CancelSkill)),
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
            }); ;
        }

       internal static void CreatePassiveSkill(SkillLocator skillLocator, string BodyName)
        {
          if (BodyName == "Amp" )
            {
                string prefix = AmpPlugin.developerPrefix + "_AMP_BODY_";
                skillLocator.passiveSkill.enabled = true;
                skillLocator.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCharge");
                skillLocator.passiveSkill.skillNameToken = prefix + "PASSIVE_NAME";
                skillLocator.passiveSkill.skillDescriptionToken = prefix + "PASSIVE_DESCRIPTION";
                skillLocator.passiveSkill.keywordToken = prefix + "KEYWORD_CHARGE";
            }
            else
            {
                string prefix = AmpPlugin.developerPrefix + "_NEMAMP_BODY_";
                skillLocator.passiveSkill.enabled = true;
                skillLocator.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texNemPassive");
                skillLocator.passiveSkill.skillNameToken = prefix + "PASSIVE_NAME";
                skillLocator.passiveSkill.skillDescriptionToken = prefix + "PASSIVE_DESCRIPTION";
            }
 
 
            
        }

        // this could all be a lot cleaner but at least it's simple and easy to work with
        internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();
            
            SkillFamily skillFamily = skillLocator.primary.skillFamily;
           

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.secondary.skillFamily;
           

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddUnlockableSecondarySkill(GameObject targetPrefab, SkillDef skillDef, UnlockableDef unlockableDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.secondary.skillFamily;


            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null),
                unlockableDef = unlockableDef
            };
        }

        internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSecondarySkill(targetPrefab, i);
            }
        }

        internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddUtilitySkill(targetPrefab, i);
            }
        }

        internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSpecialSkill(targetPrefab, i);
            }
        }
        internal static void AddUnlockableSpecialSkill(GameObject targetPrefab, SkillDef skillDef, UnlockableDef unlockableDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null),
                unlockableDef = unlockableDef
            };
        }

        internal static SkillDef CreatePrimarySkillDef(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile, string[] keywordTokens, bool canBeOverriden)
        {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            skillDef.skillName = skillNameToken;
            skillDef.skillNameToken = skillNameToken;
            skillDef.skillDescriptionToken = skillDescriptionToken;
            skillDef.icon = skillIcon;
            
            skillDef.activationState = state;
            skillDef.activationStateMachineName = stateMachine;
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.forceSprintDuringState = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;
            skillDef.resetCooldownTimerOnUse = false;
            skillDef.isCombatSkill = true;
            skillDef.mustKeyPress = false;
            skillDef.cancelSprintingOnActivation = !agile;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 0;
            skillDef.stockToConsume = 0;
            skillDef.keywordTokens = keywordTokens;
            
           /* if (!canBeOverriden)
            {
                skillDef.interruptPriority = InterruptPriority.Skill;
            } */

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
        {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            skillDef.skillName = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;
            
            skillDef.keywordTokens = skillDefInfo.keywordTokens;

            skillDefs.Add(skillDef);

            return skillDef;
        }
        public static T CreateSkillDef<T>(SkillDefInfo skillDefInfo) where T : SkillDef
        {

            T skillDef = ScriptableObject.CreateInstance<T>();

            popuplateSkillDef(skillDefInfo, skillDef);

            skillDefs.Add(skillDef);

            return skillDef;
        }

        public static T CreatePrimarySkillDef<T>(SerializableEntityStateType state, string stateMachine, string skillNameToken, string skillDescriptionToken, Sprite skillIcon, bool agile, string[] keywordTokens, bool canBeOverriden) where T : SkillDef
        {
             T skillDef = ScriptableObject.CreateInstance<T>();

            skillDef.skillName = skillNameToken;
            skillDef.skillNameToken = skillNameToken;
            skillDef.skillDescriptionToken = skillDescriptionToken;
            skillDef.icon = skillIcon;

            skillDef.activationState = state;
            skillDef.activationStateMachineName = stateMachine;
            skillDef.baseMaxStock = 1;
            skillDef.baseRechargeInterval = 0;
            skillDef.beginSkillCooldownOnSkillEnd = false;
            skillDef.canceledFromSprinting = false;
            skillDef.forceSprintDuringState = false;
            skillDef.fullRestockOnAssign = true;
            skillDef.interruptPriority = InterruptPriority.Any;
            skillDef.resetCooldownTimerOnUse = false;
            skillDef.isCombatSkill = true;
            skillDef.mustKeyPress = false;
            skillDef.cancelSprintingOnActivation = !agile;
            skillDef.rechargeStock = 1;
            skillDef.requiredStock = 0;
            skillDef.stockToConsume = 0;
            skillDef.keywordTokens = keywordTokens;

            skillDefs.Add(skillDef);

            if (!canBeOverriden)
            {
                skillDef.interruptPriority = InterruptPriority.Any;
            }

            return skillDef;
        }

        private static void popuplateSkillDef(SkillDefInfo skillDefInfo, SkillDef skillDef)
        {
            skillDef.skillName = skillDefInfo.skillName;
            (skillDef as ScriptableObject).name = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;
        }


    }
}

internal class SkillDefInfo
{
    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public string activationStateMachineName;
    public int baseMaxStock;
    public float baseRechargeInterval;
    public bool beginSkillCooldownOnSkillEnd;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool fullRestockOnAssign;
    public InterruptPriority interruptPriority;
    public bool resetCooldownTimerOnUse;
    public bool isCombatSkill;
    public bool mustKeyPress;
    public bool cancelSprintingOnActivation;
    public int rechargeStock;
    public int requiredStock;
    public int stockToConsume;

    public string[] keywordTokens;
}