using R2API;
using System;

namespace AmpMod.Modules
{//need to fix the charactername and description not showing up
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Amp
            string prefix = AmpPlugin.developerPrefix + "_AMP_BODY_";

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Descriptions
            LanguageAPI.Add(prefix + "NAME", Modules.StaticValues.characterName);
            LanguageAPI.Add(prefix + "SUBTITLE", Modules.StaticValues.characterSubtitle);
            LanguageAPI.Add(prefix + "DESCRIPTION", Modules.StaticValues.descriptionText);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", Modules.StaticValues.characterOutro);
            LanguageAPI.Add(prefix + "LORE", Modules.StaticValues.characterLore);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", Modules.StaticValues.characterOutroFailure);
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Charge");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Certain attacks build up stacks of <color=#0091ff>charge</color> on an enemy. When 3 stacks are reached, a burst of static electricity is released that damages the target and enemies near it for <style=cIsDamage>400%</style> damage.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Stormblade");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Slash continuously with your electrified sword for <style=cIsDamage>{100f * StaticValues.stormbladeDamageCoefficient}% damage</style>. Strikes have a <style=cIsDamage>{StaticValues.stormbladeChargeProcCoefficient}%</style> chance of applying <color=#0091ff>charge</color>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FERROSHOT_NAME", "Lorentz Cannon");
            LanguageAPI.Add(prefix + "SECONDARY_FERROSHOT_DESCRIPTION", Helpers.agilePrefix + $"Use electromagnetic fields to fire an array of <style=cIsDamage>6</style> iron sand bullets, each dealing <style=cIsDamage>{100f * StaticValues.ferroshotDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_DASH_NAME", "Bolt");
            LanguageAPI.Add(prefix + "UTILITY_DASH_DESCRIPTION", $"Transform into electricity, gaining <style=cIsUtility>invulnerability</style>, <style=cIsUtility>free movement</style>, and <style=cIsUtility>500% movement speed</style> for 2 seconds. Contact with enemies in this form will apply <color=#0091ff>charge</color> and deal <style=cIsDamage>{100f * StaticValues.boltOverlapDamageCoefficient}%</style> damage.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_CHAIN_NAME", "Fulmination");
            LanguageAPI.Add(prefix + "SPECIAL_CHAIN_DESCRIPTION", Helpers.agilePrefix + $"Fire a chaining stream of electricity that deals <style=cIsDamage>{100f * StaticValues.fulminationTotalDamageCoefficient}% damage</style> and has a <style=cIsDamage>{StaticValues.fulminationChargeProcCoefficient}%</style> chance of applying a stack of <color=#0091ff>charge</color>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_NAME", "Voltaic Bombardment");
            LanguageAPI.Add(prefix + "SPECIAL_LIGHTNING_DESCRIPTION", Helpers.agilePrefix + $"Summon a lightning bolt that strikes the targeted area, damaging all enemies in the vicinity for <style=cIsDamage>{100f * StaticValues.lightningStrikeCoefficient}% damage</style> and applying two stacks of <color=#0091ff>charge</color>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}

