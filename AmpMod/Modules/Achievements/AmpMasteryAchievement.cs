using RoR2;
using System;
using UnityEngine;
using Amp.Modules;

namespace AmpMod.Modules.Achievements
{
    internal class AmpMasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY";
        //the name of the sprite in your bundle
        public override string AchievementSpriteName => "texMasteryAchievement";
        //the token of your character's unlock achievement if you have one
        public override string PrerequisiteUnlockableIdentifier => AmpPlugin.developerPrefix + "_AMP_BODY_UNLOCKABLE_REWARD_ID";

        public override string RequiredCharacterBody => "AmpBody";
        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}