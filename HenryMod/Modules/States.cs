using HenryMod.SkillStates;
using HenryMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using RoR2;
using EntityStates.Huntress;

namespace HenryMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            entityStates.Add(typeof(Ferroshot));
            
            entityStates.Add(typeof(BaseBoltSkill));
            entityStates.Add(typeof(Bolt));

            entityStates.Add(typeof(BaseLightningAim));
            entityStates.Add(typeof(VoltaicBombardment));

            entityStates.Add(typeof(Fulmination));
        }
    }
}