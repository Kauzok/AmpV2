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

            //generic states
            entityStates.Add(typeof(CancelSkill));

            //stormblade states
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            //Lorentz Cannon states
            entityStates.Add(typeof(Ferroshot));

            //magnetic vortex states
            entityStates.Add(typeof(Vortex));

            //plasma slash states
            entityStates.Add(typeof(PlasmaSlash));

            //bolt states
            entityStates.Add(typeof(BoltVehicle));
            entityStates.Add(typeof(Surge));

            //pulse leap states
            entityStates.Add(typeof(PulseLeap));
            entityStates.Add(typeof(AltPulseLeap));

            //fulmination states
            entityStates.Add(typeof(Fulmination));

            //bulwark of storms states
            entityStates.Add(typeof(BaseChannelWurm));
            entityStates.Add(typeof(SummonWurm));
            entityStates.Add(typeof(ChannelWurm));
            entityStates.Add(typeof(CancelWurm));

            //voltaicbombardment states
            entityStates.Add(typeof(BaseLightningAim));
            entityStates.Add(typeof(VoltaicBombardmentFire));
            entityStates.Add(typeof(VoltaicBombardmentAim));

            
        }
    }
}