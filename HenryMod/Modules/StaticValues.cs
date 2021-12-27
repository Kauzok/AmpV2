using System;

namespace HenryMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "The Battlemage is a melee/range hybrid that focuses on consistently dealing damage with his electromagnetic attacks.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Stormblade is good at paralyzing enemies for easy damage, but keep in mind you can only paralyze so many enemies at once." + Environment.NewLine + Environment.NewLine
             + "< ! > Gauss Cannon can be used as either a source of even more damage while using Stormblade, or for continuing to apply damage even outside of melee range." + Environment.NewLine + Environment.NewLine
             + "< ! > Bolt's movement speed and invulnerability make it a great ability for entering and exiting close-ranged combat." + Environment.NewLine + Environment.NewLine
             + "< ! > Fulmination and Voltaic Bombardment are great for dealing damage to many enemies at once, or melting a single strong enemy." + Environment.NewLine + Environment.NewLine;

        internal const float swordDamageCoefficient = 2f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 4f;

        internal const float ferroshotDamageCoefficient = 1.2f;

        internal const float fulminationDamageCoefficient = 1.1f;

        internal const float fulminationTotalDamageCoefficient = 20f;

        internal const float fulminationChargeProcCoefficient = 15f;

        internal const int chargeMaxStacks = 3;

        internal const float chargeDuration = 5;

        internal const float chargeDamageCoefficient = 4f;

        internal const float lightningStrikeCoefficient = 10f;



    }
}