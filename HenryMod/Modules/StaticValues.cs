using System;

namespace HenryMod.Modules
{
    internal static class StaticValues
    {
        internal static string descriptionText = "The Battlemage is a melee/range hybrid that focuses on consistently dealing damage with his electromagnetic attacks.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Stormblade is good at paralyzing enemies for easy damage, but keep in mind you can only paralyze so many enemies at once." + Environment.NewLine + Environment.NewLine
             + "< ! > Gauss Cannon can be used as either a source of even more damage while using Stormblade, or for continuing to apply damage even outside of melee range." + Environment.NewLine + Environment.NewLine
             + "< ! > Bolt's movement speed and invulnerability makes it a great ability for entering and exiting close-ranged combat." + Environment.NewLine + Environment.NewLine
             + "< ! > Fulmination not determined yet." + Environment.NewLine + Environment.NewLine;

        internal const float swordDamageCoefficient = 2f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 16f;

        internal const float ferroshotDamageCoefficient = 1.2f;
    }
}