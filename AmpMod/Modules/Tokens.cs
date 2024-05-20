using R2API;
using System;

namespace AmpMod.Modules
{
    internal static class Tokens
    {
        public static void GenerateTokens()
        {
            AddTokens();
            Language.PrintOutput("Amp.txt");
        }


        internal static void AddTokens()
        {
            #region Amp
            string prefix = AmpPlugin.developerPrefix + "_AMP_BODY_";
            string chargedPrefix = "<color=#0091ff>Charged. </color>"; 

            #region Skins
            Language.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Red Sprite");
            Language.Add(prefix + "GOLEM_SKIN_NAME", "Reformation");
            #endregion

            #region Descriptions
            Language.Add(prefix + "NAME", Modules.StaticValues.characterName);
            Language.Add(prefix + "SUBTITLE", Modules.StaticValues.characterSubtitle);
            Language.Add(prefix + "DESCRIPTION", Modules.StaticValues.descriptionText);
            Language.Add(prefix + "OUTRO_FLAVOR", Modules.StaticValues.characterOutro);
            Language.Add(prefix + "LORE", Modules.StaticValues.characterLore);
            Language.Add(prefix + "OUTRO_FAILURE", Modules.StaticValues.characterOutroFailure);
            Language.Add(prefix + "KEYWORD_CHARGE", StaticValues.chargeDesc);
            Language.Add(prefix + "KEYWORD_DOUBLECHARGE", StaticValues.doubleChargeDesc);
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Charge");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"Attacks have a varying chance of applying <color=#0091ff>charge</color> on enemies.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Modified Shockblade");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + chargedPrefix +  $"Slash continuously with your electrified sword for <style=cIsDamage>{100f * StaticValues.stormbladeDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_FERROSHOT_NAME", "Lorentz Cannon");
            Language.Add(prefix + "SECONDARY_FERROSHOT_DESCRIPTION", Helpers.agilePrefix + $"Use electromagnetic fields to fire an array of <style=cIsDamage>6</style> iron sand bullets, each dealing <style=cIsDamage>{100f * StaticValues.ferroshotDamageCoefficient}% damage</style>. Bullets <style=cIsUtility>home</style> onto <color=#0091ff>charged</color> & <color=#4cceff>electrified</color> enemies.");
            #endregion

            #region Secondary2
            Language.Add(prefix + "SECONDARY_VORTEX_NAME", "Magnetic Vortex");
            Language.Add(prefix + "SECONDARY_VORTEX_DESCRIPTION", Helpers.agilePrefix + $"Launch a magnetic singularity that <style=cIsUtility>pulls enemies into it</style> for <style=cIsDamage>{100f * StaticValues.vortexDamageCoefficient}% damage</style> per second, and explodes for <style=cIsDamage>{100f * StaticValues.vortexExplosionCoefficient}% damage</style>.");
            #endregion

            #region Secondary3
            Language.Add(prefix + "SECONDARY_PLASMASLASH_NAME", "Plasma Slash");
            Language.Add(prefix + "SECONDARY_PLASMASLASH_DESCRIPTION",  $"Perform a sweeping slash with your sword for <style=cIsDamage>{100f * StaticValues.spinSlashDamageCoefficient}%</style> damage, <style=cIsDamage>burning</style> enemies. If in the air, launch a <style=cIsDamage>burning</style> wave of plasma for <style=cIsDamage>{100f * StaticValues.fireBeamDamageCoefficient}%</style> damage. <color=#0091ff>Charged</color> & <color=#4cceff>electrified</color> enemies <style=cIsDamage>burn stronger</style>.");

            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_DASH_NAME", "Surge");
            Language.Add(prefix + "UTILITY_DASH_DESCRIPTION", chargedPrefix + $"Transform into lightning, becoming <style=cIsUtility>invulnerable</style>. Deal <style=cIsDamage>{100f * StaticValues.boltOverlapDamageCoefficient}%</style> damage on contact. Explode on exit for <style=cIsDamage>{100f * StaticValues.boltBlastDamageCoefficient}% damage</style>.");
            Language.Add(prefix + "SPECIAL_CANCELDASH_NAME", "Cancel Surge");
            Language.Add(prefix + "SPECIAL_CANCELDASH_DESCRIPTION", "Exit Surge early.");
            #endregion

            #region Utility2
            Language.Add(prefix + "UTILITY_BOOST_NAME", "Pulse Leap");
            Language.Add(prefix + "UTILITY_BOOST_DESCRIPTION", $"Magnetically <style=cIsUtility>boost</style> yourself forward, creating a <color=#0091ff>charged</color> explosion that deals <style=cIsDamage>{100f * StaticValues.boostDamageCoefficient}% damage</style>. Can boost up to <style=cIsUtility>3 times</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_CHAIN_NAME", "Fulmination");
            Language.Add(prefix + "SPECIAL_CHAIN_DESCRIPTION", Helpers.agilePrefix + $"Fire a <style=cIsUtility>chaining</style> stream of electricity that deals <style=cIsDamage>{100f * StaticValues.fulminationTotalDamageCoefficient}% damage</style> and has a <style=cIsDamage>{StaticValues.fulminationChargeProcCoefficient}%</style> chance of being <color=#0091ff>charged</color>.");
            Language.Add(prefix + "SPECIAL_CANCELCHAIN_NAME", "Cancel Fulmination");
            Language.Add(prefix + "SPECIAL_CANCELCHAIN_DESCRIPTION", "Exit Fulmination early.");
            #endregion

            #region Special2
            Language.Add(prefix + "SPECIAL_LIGHTNING_NAME", "Voltaic Bombardment");
            Language.Add(prefix + "SPECIAL_LIGHTNING_DESCRIPTION", Helpers.agilePrefix + $"<color=#0091ff>Double Charged</color>. Summon a lightning bolt for <style=cIsDamage>{100f * StaticValues.lightningStrikeCoefficient}% damage</style>. Hit yourself or allies with the bolt to become <style=cIsUtility>overcharged</style>, boosting attack and movement speed.");
            #endregion

            #region Special3
            Language.Add(prefix + "SPECIAL_WORM_NAME", "Bulwark of Storms");
            Language.Add(prefix + "SPECIAL_WORM_DISPLAY_NAME", "Melvin");
            Language.Add(prefix + "SPECIAL_WORM_DESCRIPTION",  $"<style=cIsUtility>Channel</style> for 3 seconds, then summon an <style=cIsDamage>Overloading Worm</style> for <style=cIsUtility>30 seconds</style> that has <style=cIsHealing>300% your health</style> and inherits ALL your items.");
            Language.Add(prefix + "SPECIAL_WORMCANCEL_NAME", "Return");
            Language.Add(prefix + "SPECIAL_WORMCANCEL_DESCRIPTION", "Return Melvin to the depths.");
            #endregion

            #region Achievements
            Language.Add("ACHIEVEMENT_AMPMASTERYUNLOCK_NAME", "Amp: Mastery");
            Language.Add("ACHIEVEMENT_AMPMASTERYUNLOCK_DESCRIPTION", "As Amp, beat the game or obliterate on Monsoon.");
            Language.Add(AmpPlugin.developerPrefix + "_AMP_BODY_MASTERY", "Amp: Mastery");

            Language.Add("ACHIEVEMENT_AMPPLASMAUNLOCK_NAME", "Amp: Electric Burns");
            Language.Add("ACHIEVEMENT_AMPPLASMAUNLOCK_DESCRIPTION", "As Amp, kill 100 burning enemies.");
            Language.Add(AmpPlugin.developerPrefix + "_AMP_BODY_PLASMA", "Amp: Electric Burns");

            Language.Add("ACHIEVEMENT_AMPWORMUNLOCK_NAME", "Amp: Usurper");
            Language.Add("ACHIEVEMENT_AMPWORMUNLOCK_DESCRIPTION", "As Amp, beat an Overloading Worm at its own game.");
            Language.Add(AmpPlugin.developerPrefix + "_AMP_BODY_USURPER", "Amp: Usurper");

            Language.Add("ACHIEVEMENT_AMPGRANDMASTERYUNLOCK_NAME", "Amp: Grand Mastery");
            Language.Add("ACHIEVEMENT_AMPGRANDMASTERYUNLOCK_DESCRIPTION", "As Amp, beat the game or obliterate on Typhoon.");
            Language.Add(AmpPlugin.developerPrefix + "_AMP_BODY_GRANDMASTERY", "Amp: Grand Mastery");
            #endregion
            #endregion

            #region NemAmp

            string nemPrefix = AmpPlugin.developerPrefix + "_NEMAMP_BODY_";
            string nemBodyPrefix = AmpPlugin.developerPrefix + "_NEMAMP_BODY_";

            #region Skins
            Language.Add(nemPrefix + "DEFAULT_SKIN_NAME", "Default");
            Language.Add(nemPrefix + "MASTERY_SKIN_NAME", "Origin");
            #endregion

            #region Descriptions
            Language.Add(nemBodyPrefix + "NAME", StaticValues.nemCharacterName);
            Language.Add(nemBodyPrefix + "SUBTITLE", StaticValues.nemCharacterSubtitle);
            Language.Add(nemBodyPrefix + "DESCRIPTION", StaticValues.nemDescriptionText);
            Language.Add(nemPrefix + "OUTRO_FLAVOR", StaticValues.nemCharacterOutro);
            Language.Add(nemPrefix + "LORE", StaticValues.nemCharacterLore);
            Language.Add(nemPrefix + "OUTRO_FAILURE", StaticValues.nemCharacterOutroFailure);
            #endregion

            #region Passive
            Language.Add(nemPrefix + "PASSIVE_NAME", "Gathering Storm");
            Language.Add(nemPrefix + "PASSIVE_DESCRIPTION", $"Using a skill <style=cIsUtility>within {StaticValues.comboTimeInterval} seconds</style> after using a different skill will <style=cIsDamage>increase its damage by {100f * StaticValues.growthDamageCoefficient}%</style>, with a maximum bonus of <style=cIsDamage>{100f * StaticValues.growthBuffMaxStacks * StaticValues.growthDamageCoefficient}%</style>.");
            #endregion

            #region Primary
            Language.Add(nemPrefix + "PRIMARY_LIGHTNING_NAME", "Fulmination");
            Language.Add(nemPrefix + "PRIMARY_LIGHTNING_DESCRIPTION", $"Lock on and shock an enemy for <style=cIsDamage>{Math.Round(100f * StaticValues.lightningStreamPerSecondDamageCoefficient * StaticValues.lightningStreamBaseTickTime)}% damage</style>. At <style=cIsDamage>maximum Gathering Storm charge</style>, this attack <style=cIsUtility>chains</style>.");
            #endregion

            #region Primary2
            Language.Add(nemPrefix + "PRIMARY_BLADES_NAME", "Lorentz Blades");
            Language.Add(nemPrefix + "PRIMARY_BLADES_DESCRIPTION", $"Fire 3 <style=cIsUtility>tracking</style> iron sand daggers, each dealing <style=cIsDamage>{100f * StaticValues.bladeDamageCoefficient}% damage</style> on hit. At <style=cIsDamage>maximum Gathering Storm charge</style>, fire <style=cIsDamage>6</style> blades.");
            #endregion

            #region Secondary
            Language.Add(nemPrefix + "SECONDARY_CHARGEBEAM_NAME", "Furious Spark");
            Language.Add(nemPrefix + "SECONDARY_CHARGEBEAM_DESCRIPTION", $"Charge a <style=cIsUtility>piercing</style> beam of electricity that deals <style=cIsDamage>{100f * StaticValues.chargeBeamMinDamageCoefficient}%-{100f * StaticValues.chargeBeamMaxDamageCoefficient}% damage</style>. For every enemy pierced, the beam deals <style=cIsDamage>{100f * StaticValues.additionalPierceDamageCoefficient}% of its damage</style> as bonus damage to the next enemy hit.");
            #endregion

            #region Secondary2
            Language.Add(nemPrefix + "SECONDARY_SLASH_NAME", "Galvanic Cleave");
            Language.Add(nemPrefix + "SECONDARY_SLASH_DESCRIPTION", $"Form a blade of void lightning and slash, dealing <style=cIsDamage>{100f * StaticValues.voidSlashDamageCoefficient}% damage</style>. Use this skill in the air to perform a <style=cIsDamage>lunging thrust</style> instead.");
            #endregion

            #region Utility
            Language.Add(nemPrefix + "UTILITY_FIELD_NAME", "Static Field");
            Language.Add(nemPrefix + "UTILITY_FIELD_DESCRIPTION", $"Create an <style=cIsUtility>electric field</style> that <style=cIsUtility>slows and weakens</style> enemies inside, while <style=cIsUtility>boosting attack speed </style>for you and allies. Enemies in the field take <style=cIsDamage>200% damage</style>.");
            #endregion

            #region Utility2
            Language.Add(nemPrefix + "UTILITY_DASH_NAME", "Voidsurge");
            Language.Add(nemPrefix + "UTILITY_DASH_DESCRIPTION", $"Transform into lightning, becoming <style=cIsUtility>intangible</style> and dashing a short distance. Your next primary cast fires a a <style=cIsUtility>shocking</style> mass of plasma for <style=cIsDamage>{100f * StaticValues.lightningBallDamageCoefficient}% damage</style>.");

            Language.Add(nemPrefix + "UTILITY_LIGHTNINGBALL_NAME", "Throw Plasma Stake");
            Language.Add(nemPrefix + "UTILITY_LIGHTNINGBALL_DESCRIPTION", $"Throw a <style=cIsUtility>shocking</style> mass of plasma for <style=cIsDamage>{100f * StaticValues.lightningBallDamageCoefficient}% damage</style>.");
            #endregion

            #region Special
            Language.Add(nemPrefix + "SPECIAL_SUMMONSTORM_NAME", "Voltaic Onslaught");
            Language.Add(nemPrefix + "SPECIAL_SUMMONSTORM_DESCRIPTION", $"Dealing damage adds stacks of <color=#8b00ff>Controlled Charge</color>. Activate this skill to summon a lightning bolt onto every enemy in a radius; the lightning bolt deals <style=cIsDamage>{100f * StaticValues.baseBoltDamageCoefficient}% base damage</style> with an extra <style=cIsDamage>{100f * StaticValues.additionalBoltDamageCoefficient}% damage for every stack of Controlled Charge</style>.");
            #endregion

            #region Special2
            Language.Add(nemPrefix + "SPECIAL_LASER_NAME", "Irradiance");
            Language.Add(nemPrefix + "SPECIAL_LASER_DESCRIPTION", $"Fire a beam of light for <style=cIsDamage>{100f * StaticValues.baseLaserDamageCoefficient}% damage</style>. The laser <style=cIsDamage>detonates</style> stacks of <color=#8b00ff>Controlled Charge</color> for an additional <style=cIsDamage>{100f * StaticValues.additionalLaserDamageCoefficient}% damage, spawning additional light beams that <style=cIsUtility>chain</style> to nearby enemies.</style>");
            #endregion



            #region Achievements
            Language.Add("ACHIEVEMENT_NEMAMPMASTERYUNLOCK_NAME", "Nemesis Amp: Mastery");
            Language.Add("ACHIEVEMENT_NEMAMPMASTERYUNLOCK_DESCRIPTION", "As Nemesis Amp, beat the game or obliterate on Monsoon.");
            Language.Add(AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY", "Nemesis Amp: Mastery");

            Language.Add("ACHIEVEMENT_NEMAMPDASHUNLOCK_NAME", "Nemesis Amp: 299,792,458 m/s");
            Language.Add("ACHIEVEMENT_NEMAMPDASHUNLOCK_DESCRIPTION", "As Nemesis Amp, complete a stage in under 3 minutes.");
            Language.Add(AmpPlugin.developerPrefix + "_NEMAMP_BODY_DASH", "Nemesis Amp: 299,792,458 m/s");

            Language.Add("ACHIEVEMENT_NEMAMPBLADESUNLOCK_NAME", "Nemesis Amp: Sharpshocker");
            Language.Add("ACHIEVEMENT_NEMAMPBLADESUNLOCK_DESCRIPTION", "As Nemesis Amp, pierce 5 enemies in a row with Furious Spark.");
            Language.Add(AmpPlugin.developerPrefix + "_NEMAMP_BODY_BLADES", "Nemesis Amp: Sharpshocker");

            Language.Add("ACHIEVEMENT_NEMAMPLASERUNLOCK_NAME", "Nemesis Amp: Lux Vindictae");
            Language.Add("ACHIEVEMENT_NEMAMPLASERUNLOCK_DESCRIPTION", "As Nemesis Amp, kill 50 creatures of the Void.");
            Language.Add(AmpPlugin.developerPrefix + "_NEMAMP_BODY_LASER", "Nemesis Amp: Lux Vindictae");

            #endregion

            #endregion


            #region Mithrix Quotes
            Language.Add("MITHRIX_SEE_AMP_1", "You wield stolen power. Relinquish it through death.");
            Language.Add("MITHRIX_SEE_AMP_2", "Watch, brother. I will prove the fragility of your constructs.");
            Language.Add("MITHRIX_SEE_AMP_3", "The power of a wurm... but faint.");

            Language.Add("MITHRIX_KILL_AMP_1", "A pity. You may have been a useful servant.");
            Language.Add("MITHRIX_KILL_AMP_2", "Frail sparks.");

            Language.Add("MITHRIX_SEE_NEMAMP_1", "The power of a wurm... but defiled.");
            Language.Add("MITHRIX_SEE_NEMAMP_2", "Your rage will end here, wurm.");
            Language.Add("MITHRIX_SEE_NEMAMP_3", "Warped and stolen power. Your blasphemy ends here.");

            Language.Add("MITHRIX_KILL_NEMAMP_1", "Die with your corruption.");
            Language.Add("MITHRIX_KILL_NEMAMP_2", "Slow lightning.");
            #endregion

        }
    }
}

