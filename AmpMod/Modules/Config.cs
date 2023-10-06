using BepInEx.Configuration;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace AmpMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> RedSpriteBlueLightning;
        public static ConfigEntry<bool> NemOriginPurpleLightning;
        public static ConfigEntry<bool> chargeOrbsEnable;
        public static ConfigEntry<bool> UnlockMasterySkin;
        public static ConfigEntry<bool> UnlockGrandMasterySkin;
        public static ConfigEntry<bool> UnlockWormSkill;
        public static ConfigEntry<bool> UnlockPlasmaSkill;
        public static ConfigEntry<bool> UnlockNemesisAmp;
        public static ConfigEntry<bool> NemUnlockDashSkill;
        public static ConfigEntry<bool> NemUnlockBladesSkill;

        public static void ReadConfig()
        {
            /*Dictionary<ConfigDefinition, string> orphanedEntries = (Dictionary<ConfigDefinition, string>)typeof(ConfigFile).GetProperty("OrphanedEntries",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(AmpPlugin.instance.Config);
            orphanedEntries.Clear(); */

            chargeOrbsEnable =
                AmpPlugin.instance.Config.Bind<bool>("VFX Settings",
                                                         "Enable Charge Orbs",
                                                         true,
                                                         "Display electric orbs above the heads of charged enemies");

            RedSpriteBlueLightning =
                AmpPlugin.instance.Config.Bind<bool>("VFX Settings",
                                             "Blue Lightning on Mastery Skin",
                                             false,
                                             "Makes Amp's Red Sprite skin use blue lightning instead of red");

            NemOriginPurpleLightning =
                AmpPlugin.instance.Config.Bind<bool>("VFX Settings",
                                             "Purple Lightning on Nemesis Mastery Skin",
                                             false,
                                             "Makes Nemesis Amp's Origin skin use purple lightning instead of blue");

            UnlockMasterySkin =
               AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                        "Unlock Mastery Skin",
                                                        false,
                                                        "Unlocks Amp's Mastery Skin");

            UnlockGrandMasterySkin =
               AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                        "Unlock Grand Mastery Skin",
                                                        false,
                                                        "Unlocks Amp's Grand Mastery Skin");

            UnlockWormSkill =
              AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                       "Unlock Usurper",
                                                       false,
                                                       "Unlocks Amp's Usurper Achievement & Corresponding Skill");
            
            UnlockPlasmaSkill =
             AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                      "Unlock Electric Burns",
                                                      false,
                                                      "Unlocks Amp's Electric Burns Achievement & Corresponding Skill");

            UnlockNemesisAmp =
                             AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                      "Unlock Nemesis Amp",
                                                      false,
                                                      "Unlocks Nemesis Amp by default");

            NemUnlockBladesSkill = AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                      "Unlock Sharpshocker",
                                                      false,
                                                      "Unlocks Nemesis Amp's Sharpshocker Achievement & Corresponding Skill");

            NemUnlockDashSkill = AmpPlugin.instance.Config.Bind<bool>("Unlockable Settings",
                                                   "Unlock 1/√(ε₀μ₀)",
                                                   false,
                                                   "Unlocks Nemesis Amp's 1/√(ε₀μ₀) Achievement & Corresponding Skill");
        }

        // this helper automatically makes config entries for disabling survivors
        internal static ConfigEntry<bool> CharacterEnableConfig(string characterName)
        {
            return AmpPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this character"));
        }

        internal static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return AmpPlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}