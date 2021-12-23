using HenryMod.SkillStates;
using HenryMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using RoR2;
using EntityStates.Huntress;
using HenryMod.SkillStates.Henry;

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
            
            entityStates.Add(typeof(BoltVehicle));
            entityStates.Add(typeof(Bolt));

            entityStates.Add(typeof(BaseLightningAim));
            entityStates.Add(typeof(VoltaicBombardmentFire));
            entityStates.Add(typeof(CallVoltaicBombardment));
            entityStates.Add(typeof(VoltaicBombardmentAim));

            entityStates.Add(typeof(Fulmination));
        }
    }
}