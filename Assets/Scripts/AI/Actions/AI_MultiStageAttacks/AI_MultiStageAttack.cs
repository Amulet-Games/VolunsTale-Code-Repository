using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_MultiStageAttack : ScriptableObject
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs _aiMSARefs;

        //[Header("MSA Type")]
        //public MSATypeEnum currentMSAType;

        [Header("Mods")]
        [Tooltip("Don't set this to bigger than 0 if no consuming Stamina is intended. If this value is bigger than 0 then it will try to Deplete Enemy stamina by reaching out to mods." +
            " Which will require 'Enemy Stamina Mod' or 'Egil Stamina Mod' to work. ")]
        public float staminaUsage;

        [Header("Attack RootMotion Info")]
        [Tooltip("Check this if you Don't want to change attack root motion velocity based on distance to player.")]
        public bool ignoreRootMotionCalculation;

        [Tooltip("How far the enemy move when performing this attack?")]
        public float attackRootMotionVelocity;

        [Tooltip("The amount of turning prediction for this attack.")]
        public float _playerPredictOffset;

        [Header("Special Camera Effect")]
        [Tooltip("Check this if you want player's camera follow enemy Y axis movement.")]
        public bool applyControllerCameraYMovement;

        [Tooltip("Check this if you want player camera zoom in.")]
        public bool applyControllerCameraZoom;

        public abstract void Execute(AIManager ai);
        
        public enum MSATypeEnum
        {
            AICombo,
            RollAttack,
            ParryAttack
        }

        public enum ComboAnimStateEnum
        {
            e_combo_1_b,
            e_combo_1_c,
            e_combo_1_d,
            e_combo_2_b,
            e_combo_2_c,
            e_combo_2_d,
            e_combo_3_b,
            e_combo_3_c,
            e_combo_3_d,
            e_combo_4_b,
            e_combo_4_c,
            e_combo_4_d,
            e_combo_5_b,
            e_combo_5_c,
            e_combo_5_d,
            e_combo_6_b,
            e_combo_6_c,
            e_combo_6_d,
            e_combo_7_b,
            e_combo_7_c,
            e_combo_7_d,
            e_combo_8_b,
            e_combo_8_c,
            e_combo_8_d,
            e_combo_9_b,
            e_combo_9_c,
            e_combo_9_d,
            e_combo_10_b,
            e_combo_10_c,
            e_combo_10_d,
            e_combo_11_b,
            e_combo_11_c,
            e_combo_11_d,
            e_combo_12_b,
            e_combo_12_c,
            e_combo_12_d,
        }

        public enum ParryAttackStateEnum
        {
            e_parry_attack_1,
            e_RS_parry_attack_1,
            e_LS_parry_attack_1,
        }

        public enum RollAttackAnimStateEnum
        {
            e_roll_attack_1,
            e_roll_attack_2,
            e_roll_attack_3,
        }
    }
}