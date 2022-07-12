using RoR2;
using System;
using UnityEngine;

namespace AmpMod.Modules.Achievements
{
    internal class AmpWormAchievement : ModdedUnlockable
    {

        public bool electricWormKilled;

        public override string AchievementIdentifier { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texWormAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString(AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString(AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString(AmpPlugin.developerPrefix + "_AMP_BODY_WORMUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(Survivors.Amp.instance.bodyName);
        }


        private void CheckDeath(DamageReport report)
        {
            if (report is null) return;
            if (report.victimBody is null) return;
            if (report.attackerBody is null) return;

            if (report.victimTeamIndex != TeamIndex.Player)
            {
                if (report.victimBodyIndex == BodyCatalog.FindBodyIndex("ElectricWormBody"))
                {
                    this.electricWormKilled = true;

                    //Debug.LogWarning("killed worm");
                    //Debug.LogWarning($"wom: {magmaWormKilled}, vag: {wanderingVagrantKilled}, tit: {stoneTitanKilled}");
                }


                if (electricWormKilled)
                {
                    
                    base.Grant();
                    
                }
            }
        }

        private void ResetOnRunStart(Run run)
        {
            this.ResetKills();
        }

        private void ResetKills()
        {
            electricWormKilled = false;
        }

        public override void OnInstall()
        {

            base.OnInstall();
            this.ResetKills();
            GlobalEventManager.onCharacterDeathGlobal += this.CheckDeath;
            Run.onRunStartGlobal += ResetOnRunStart;

        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            GlobalEventManager.onCharacterDeathGlobal -= this.CheckDeath;
            Run.onRunStartGlobal -= ResetOnRunStart;
        }

    }
}
