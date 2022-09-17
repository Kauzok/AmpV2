using BepInEx.Configuration;
using UnityEngine;

namespace AmpMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> RedSpriteBlueLightning;


        public static void ReadConfig()
        {

            RedSpriteBlueLightning =
                AmpPlugin.instance.Config.Bind<bool>("VFX Settings",
                                                         "Blue Lightning on Mastery Skin",
                                                         false,
                                                         "Makes Amp's Red Sprite skin use blue lightning instead of red");
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