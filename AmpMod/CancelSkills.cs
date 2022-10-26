using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Skills;
using UnityEngine;

namespace AmpMod
{
    public static class CancelSkills
    {
        public static SkillDef surgeCancelSkillDef;
        public static SkillDef fulminationCancelSkillDef;
		public static string prefix = AmpPlugin.developerPrefix;

		public static void createCancelSkills()
        {
			surgeCancelSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
			{
				skillName = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
				skillNameToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_NAME",
				skillDescriptionToken = prefix + "_AMP_BODY_SPECIAL_CANCELDASH_DESCRIPTION",
				skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCancelSurge"),
				activationStateMachineName = "Slide",
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
		}


    }
}
