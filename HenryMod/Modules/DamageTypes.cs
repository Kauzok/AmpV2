using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using HenryMod;
using R2API;


namespace HenryMod.Modules
{
    //Declare damage types; effects are created with hooks in HenryPlugin.cs
    public class DamageTypes
    {

        //on hit apply charge
        public static DamageAPI.ModdedDamageType applyCharge = DamageAPI.ReserveDamageType();

        //on hit apply two stacks of charge
        public static DamageAPI.ModdedDamageType apply2Charge = DamageAPI.ReserveDamageType();

        //on hit chain to enemies with fulmination's chain effect
        public static DamageAPI.ModdedDamageType fulminationChain = DamageAPI.ReserveDamageType();

       
        

    }
}
