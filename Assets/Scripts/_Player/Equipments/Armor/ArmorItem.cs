using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ArmorItem : Item
    {
        [Header("Armor Item Stats.")]
        public float weight;

        [Header("Armor Vanilla Stats.")]
        public float durability;

        [Header("Damage Reduction Stats.")]
        public P_Armor_DamageReductionTypeEnum damageReductionType;
        public float dmgReduct;
        
        [Header("VS Reduction Stats.")]
        public P_Armor_VsReductionTypeEnum vsReductionType;
        public float vsReduct;

        [Header("Status Resistance Stats.")]
        public float bleed_resistance;
        public float poison_resistance;
        public float frost_resistance;
        public float curse_resistance;
        public float poise;

        public void ChangeStatsWithArmor(StatsAttributeHandler _statsHandler)
        {
            Add_DamageReduction_ByType();
            Add_VsReduction_ByType();

            //_statsHandler.multi_bleed_resistance += bleed_resistance;
            //_statsHandler.multi_poison_resistance += poison_resistance;
            //_statsHandler.multi_frost_resistance += frost_resistance;
            //_statsHandler.multi_curse_resistance += curse_resistance;
            //_statsHandler.b_poise += poise;
            //_statsHandler.b_cur_equip_load += weight;

            void Add_DamageReduction_ByType()
            {
                switch (damageReductionType)
                {
                    case P_Armor_DamageReductionTypeEnum.Physical:
                        _statsHandler.multi_physical_reduction += dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Magic:
                        _statsHandler.multi_magic_reduction += dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Fire:
                        _statsHandler.multi_fire_reduction += dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Lightning:
                        _statsHandler.multi_lightning_reduction += dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Dark:
                        _statsHandler.multi_dark_reduction += dmgReduct;
                        break;
                }
            }

            void Add_VsReduction_ByType()
            {
                switch (vsReductionType)
                {
                    case P_Armor_VsReductionTypeEnum.strike:
                        _statsHandler.multi_strike_reduction += vsReduct;
                        break;
                    case P_Armor_VsReductionTypeEnum.slash:
                        _statsHandler.multi_slash_reduction += vsReduct;
                        break;
                    case P_Armor_VsReductionTypeEnum.thrust:
                        _statsHandler.multi_thrust_reduction += vsReduct;
                        break;
                }
            }
        }

        public void UndoArmorStatsChanges(StatsAttributeHandler _statsHandler)
        {
            Minus_DamageReduction_ByType();
            Minus_VsReduction_ByType();

            //_statsHandler.multi_bleed_resistance -= bleed_resistance;
            //_statsHandler.multi_poison_resistance -= poison_resistance;
            //_statsHandler.multi_frost_resistance -= frost_resistance;
            //_statsHandler.multi_curse_resistance -= curse_resistance;
            //_statsHandler.b_poise -= poise;
            //_statsHandler.b_cur_equip_load -= weight;

            void Minus_DamageReduction_ByType()
            {
                switch (damageReductionType)
                {
                    case P_Armor_DamageReductionTypeEnum.Physical:
                        _statsHandler.multi_physical_reduction -= dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Magic:
                        _statsHandler.multi_magic_reduction -= dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Fire:
                        _statsHandler.multi_fire_reduction -= dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Lightning:
                        _statsHandler.multi_lightning_reduction -= dmgReduct;
                        break;
                    case P_Armor_DamageReductionTypeEnum.Dark:
                        _statsHandler.multi_dark_reduction -= dmgReduct;
                        break;
                }
            }

            void Minus_VsReduction_ByType()
            {
                switch (vsReductionType)
                {
                    case P_Armor_VsReductionTypeEnum.strike:
                        _statsHandler.multi_strike_reduction -= vsReduct;
                        break;
                    case P_Armor_VsReductionTypeEnum.slash:
                        _statsHandler.multi_slash_reduction -= vsReduct;
                        break;
                    case P_Armor_VsReductionTypeEnum.thrust:
                        _statsHandler.multi_thrust_reduction -= vsReduct;
                        break;
                }
            }
        }
    }

    public enum P_Armor_DamageReductionTypeEnum
    {
        Physical,
        Magic,
        Fire,
        Lightning,
        Dark
    }

    public enum P_Armor_VsReductionTypeEnum
    {
        strike,
        slash,
        thrust
    }
}
