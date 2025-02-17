﻿using AmpMod.SkillStates;
using AmpMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using RoR2;
using EntityStates.Huntress;
using AmpMod.SkillStates.Amp;
using AmpMod.SkillStates.Nemesis_Amp;

namespace AmpMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();
        
        //initialize all the states to be used for skills
        internal static void RegisterStates()
        {

            #region AmpStates
            //generic states
            entityStates.Add(typeof(CancelSkill));
            entityStates.Add(typeof(Survivors.AmpMain));

            //stormblade states
            entityStates.Add(typeof(BaseMeleeAttack));
            entityStates.Add(typeof(SlashCombo));

            //Lorentz Cannon states
            entityStates.Add(typeof(Ferroshot_Old));

            //magnetic vortex states
            entityStates.Add(typeof(Vortex));

            //plasma slash states
            entityStates.Add(typeof(PlasmaSlash));

            //bolt states
            entityStates.Add(typeof(BoltVehicle));
            entityStates.Add(typeof(Surge));

            //pulse leap states
            entityStates.Add(typeof(PulseLeap_Old));
            entityStates.Add(typeof(PulseLeap));

            //fulmination states
            entityStates.Add(typeof(Fulmination));

            //bulwark of storms states
            entityStates.Add(typeof(BaseChannelWurm));
            entityStates.Add(typeof(SummonWurm_Old));
            entityStates.Add(typeof(ChannelWurm));
            entityStates.Add(typeof(CancelWurm));

            //voltaicbombardment states
            entityStates.Add(typeof(BaseSkillAim));
            entityStates.Add(typeof(VoltaicBombardmentFire));
            entityStates.Add(typeof(VoltaicBombardmentAim));
            #endregion

            #region NemAmpStates
            //generic states
            entityStates.Add(typeof(Survivors.NemAmpMain));
            entityStates.Add(typeof(SkillStates.Nemesis_Amp.Components.NemDeathState));
            entityStates.Add(typeof(SkillStates.Nemesis_Amp.Components.NemSpawnState));

            //unstoppable current states
            entityStates.Add(typeof(LightningStream));

            //flux blades states
            entityStates.Add(typeof(FluxBlades));

            //galvanic cleave states

            //static field states
            entityStates.Add(typeof(AimStaticField));

            //voidsurge states
            entityStates.Add(typeof(QuickDash));
            entityStates.Add(typeof(FireLightningSpear));


            //howitzer spark states
            entityStates.Add(typeof(ChargeLightningBeam));
            entityStates.Add(typeof(FireLightningBeam));

            //voltaic onslaught states
            entityStates.Add(typeof(AOELightning));
            #endregion


        }
    }
}