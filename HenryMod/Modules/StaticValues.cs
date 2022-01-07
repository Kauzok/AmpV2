using System;
using UnityEngine;

namespace HenryMod.Modules
{
    //static values for use in skillstates and other references for easy adjustment
    internal static class StaticValues
    {

        public const string characterName = "Battlemage";
        public const string characterSubtitle = "Electromagnetic Warrior";
        public const string characterOutro = "..and so he left, with an undeniable freedom.";
        public const string characterOutroFailure = "..and so he vanished, a slave for eternity.";


        internal static string descriptionText = "The Battlemage is a melee/range hybrid that focuses on consistently dealing damage with his electromagnetic attacks.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Stormblade is good at paralyzing enemies for easy damage, but keep in mind you can only paralyze so many enemies at once." + Environment.NewLine + Environment.NewLine
             + "< ! > Lorentz Cannon can be used as either a source of extra damage while using Stormblade, or for continuing to apply damage even outside of melee range." + Environment.NewLine + Environment.NewLine
             + "< ! > Bolt's movement speed and invulnerability make it a great ability for entering and exiting close-ranged combat." + Environment.NewLine + Environment.NewLine
             + "< ! > Fulmination and Voltaic Bombardment are great for dealing damage to many enemies at once, or melting a single strong enemy." + Environment.NewLine + Environment.NewLine;
        
        internal static float boltBlastDamageCoefficient = .2f;
        
        internal const float stormbladeDamageCoefficient = 2f;

        internal const float stormbladeChargeProcCoefficient = 20f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 4f;

        internal const float ferroshotDamageCoefficient = 1.2f;

        internal const float fulminationDamageCoefficient = 1.1f;

        internal const float fulminationTotalDamageCoefficient = 20f;

        internal const float fulminationChargeProcCoefficient = 25f;

        internal const int chargeMaxStacks = 3;

        internal const float chargeDuration = 5;

        internal const float chargeDamageCoefficient = 4f;

        internal const float lightningStrikeCoefficient = 10f;


        [Header("Charge Sound Strings")]
        internal const string chargeExplosionString = "PlayChargeExplosion";

        [Header("Stormblade Sound Strings")]
        internal const string stormbladeSwing1String = "PlayStormbladeSwing1";
        internal const string stormbladeSwing2String = "PlayStormbladeSwing2";
        internal const string stormbladeHit1String = "PlayStormbladeHit1";
        internal const string stormbladeHit2String = "PlayStormbladeHit2";
        internal const string stormbladeHit3String = "PlayStormbladeHit3";
        internal const string stormbladeHit4String = "PlayStormbladeHit4";

        [Header("Ferroshot/Gauss Cannon Sound Strings")]
        internal const string ferroshotPrepString = "PlayFerroshotCreate";
        internal const string ferroshotLaunchString = "PlayFerroshotLaunch";
        internal const string ferroshotLaunchAlterString = "PlayFerroshotLaunchAlter";
        internal const string ferroshotPrepAlterString = "PlayFerroshotCreateAlter";

        [Header("Bolt Sound Strings")]
        internal const string boltState2SecString = "PlayBoltState2sec";
        internal const string boltStateFullString = "PlayBoltStateFull";
        internal const string boltEnterString = "PlayBoltEnter";
        internal const string boltExitString = "PlayBoltExit";
        internal const string boltState2SecWindString = "PlayBoltStateWind";
        internal const string boltState2SecAlterString = "PlayBoltState2SecAlter";

        [Header("Fulmination Sound Strings")]
        internal const string fulminationEnterString = "PlayFulminationEnter";
        internal const string fulminationStateString = "PlayFulminationState";
        internal const string fulminationExitString = "PlayFulminationExit";
        internal const string fulminationExitAlterString = "PlayFulminationExitAlter";
     





    }
}