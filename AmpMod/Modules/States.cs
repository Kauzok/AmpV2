using AmpMod.SkillStates;
using AmpMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using RoR2;
using EntityStates.Huntress;
using AmpMod.SkillStates.Amp;

namespace AmpMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();
        
        //initialize all the states to be used for skills
        internal static void RegisterStates()
        {
            //stormblade states
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            //Lorentz Cannon states
            entityStates.Add(typeof(Ferroshot));

            //magnetic vortex states
            entityStates.Add(typeof(Vortex));

            //bolt states
            entityStates.Add(typeof(BoltVehicle));
            entityStates.Add(typeof(Surge));

            //pulse leap states
            entityStates.Add(typeof(PulseLeap));

            //fulmination states
            entityStates.Add(typeof(Fulmination));

            //voltaicbombardment states
            entityStates.Add(typeof(BaseLightningAim));
            entityStates.Add(typeof(VoltaicBombardmentFire));
            entityStates.Add(typeof(VoltaicBombardmentAim));

            
        }
    }
}