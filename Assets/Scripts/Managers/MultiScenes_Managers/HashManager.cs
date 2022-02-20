using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class HashManager : MonoBehaviour
    {
        public AnimStateVariables animStateVariables;

        #region Public.
        [Header("Public Transition Parameter")]
        public int vertical_hash;
        public int horizontal_hash;
        public int vertical_whole_hash;
        public int horizontal_whole_hash;
        #endregion

        #region Enemy.

        #region Enemy Transition Parameter.
        public int e_IsFacedPlayer_hash;
        public int e_IsInteracting_hash;
        public int e_IsArmed_hash;
        public int e_IsGrounded_hash;
        public int e_IsLockOnMoveAround_hash;
        public int e_IsKnockedDown_hash;
        public int e_IsInParryWindow_hash;
        public int e_IsDead_hash;
        public int e_mod_IsUsingSecondWeapon_hash;
        public int e_mod_IsTired_hash;
        public int e_mod_IsBlocking_hash;
        public int e_mod_IsMovingInFixDirection_hash;
        public int e_mod_IsRightStance_hash;
        public int e_mod_isWaitingToParry_hash;
        public int e_mod_isIn2ndPhase_hash;
        public int e_mod_isIn3rdPhase_hash;
        public int e_mod_isIn4thPhase_hash;
        public int e_javelin_isEquiped_hash;
        public int e_javelin_isAiming_hash;
        #endregion

        #region Egil Begin Armed Locomotion State.
        public int e_egil_1P_armed_locomotion_hash;
        #endregion

        #region Enemy Attack Animation States.

        #region Normal.
        public int e_attack_1_hash;
        public int e_attack_2_hash;
        public int e_attack_3_hash;
        public int e_attack_4_hash;
        public int e_attack_5_hash;
        public int e_attack_6_hash;
        public int e_attack_7_hash;
        public int e_attack_8_hash;
        public int e_attack_9_hash;
        #endregion

        #region Roll Attacks.
        public int e_roll_attack_1_hash;
        public int e_roll_attack_2_hash;
        public int e_roll_attack_3_hash;

        public int e_roll_attack_1_ready_hash;
        public int e_roll_attack_2_ready_hash;
        public int e_roll_attack_3_ready_hash;

        public int e_roll_attack_1_ready_roll_tree_hash;
        public int e_roll_attack_2_ready_roll_tree_hash;
        public int e_roll_attack_3_ready_roll_tree_hash;
        #endregion

        #region Throw Attacks.
        public int e_throw_attack_1_hash;
        public int e_throw_attack_2_hash;
        public int e_throw_attack_3_hash;
        #endregion

        #region Aiming Player Mod.
        public int e_aim_attack_1_hash;
        public int e_aim_attack_2_hash;
        #endregion

        #region Combos.
        public int e_combo_1_a_hash;
        public int e_combo_1_b_hash;
        public int e_combo_1_c_hash;
        public int e_combo_1_d_hash;

        public int e_combo_2_a_hash;
        public int e_combo_2_b_hash;
        public int e_combo_2_c_hash;
        public int e_combo_2_d_hash;

        public int e_combo_3_a_hash;
        public int e_combo_3_b_hash;
        public int e_combo_3_c_hash;
        public int e_combo_3_d_hash;

        public int e_combo_4_a_hash;
        public int e_combo_4_b_hash;
        public int e_combo_4_c_hash;
        public int e_combo_4_d_hash;

        public int e_combo_5_a_hash;
        public int e_combo_5_b_hash;
        public int e_combo_5_c_hash;
        public int e_combo_5_d_hash;

        public int e_combo_6_a_hash;
        public int e_combo_6_b_hash;
        public int e_combo_6_c_hash;
        public int e_combo_6_d_hash;

        public int e_combo_7_a_hash;
        public int e_combo_7_b_hash;
        public int e_combo_7_c_hash;
        public int e_combo_7_d_hash;

        public int e_combo_8_a_hash;
        public int e_combo_8_b_hash;
        public int e_combo_8_c_hash;
        public int e_combo_8_d_hash;

        public int e_combo_9_a_hash;
        public int e_combo_9_b_hash;
        public int e_combo_9_c_hash;
        public int e_combo_9_d_hash;

        public int e_combo_10_a_hash;
        public int e_combo_10_b_hash;
        public int e_combo_10_c_hash;
        public int e_combo_10_d_hash;

        public int e_combo_11_a_hash;
        public int e_combo_11_b_hash;
        public int e_combo_11_c_hash;
        public int e_combo_11_d_hash;

        public int e_combo_12_a_hash;
        public int e_combo_12_b_hash;
        public int e_combo_12_c_hash;
        public int e_combo_12_d_hash;
        #endregion

        #region Parry Player Mod.
        public int e_LS_parry_waiting_start_hash;
        public int e_RS_parry_waiting_start_hash;

        public int e_parry_attack_1_ready_hash;
        public int e_parry_attack_1_hash;

        public int e_RS_parry_attack_1_ready_hash;
        public int e_RS_parry_attack_1_hash;

        public int e_LS_parry_attack_1_ready_hash;
        public int e_LS_parry_attack_1_hash;
        #endregion

        #region Throw Returnal Projectile Mod.
        public int e_throw_ReturnalProjectile_start_hash;
        public int e_throw_ReturnalProjectile_end_hash;
        #endregion

        #region Damage Particle Mod.
        public int e_dp_area_attack_1_hash;
        public int e_dp_area_attack_2_hash;
        public int e_dp_area_attack_3_hash;
        public int e_dp_proj_attack_1_hash;
        public int e_dp_proj_attack_2_hash;
        public int e_dp_proj_attack_3_hash;
        #endregion

        #endregion

        #region Enemy Roll Mod.
        public int e_roll_1_hash;
        public int e_roll_2_hash;
        public int e_roll_3_hash;
        public int e_roll_tree_hash;
        public int e_RS_roll_1_hash;
        public int e_LS_roll_1_hash;
        #endregion

        #region Fix Direction Move Mod.
        public int e_fix_direction_180;
        public int e_fix_direction_r90;
        public int e_fix_direction_l90;
        public int e_fix_direction_end;

        public int e_SW_fix_direction_180;
        public int e_SW_fix_direction_r90;
        public int e_SW_fix_direction_l90;

        public int e_FW_fix_direction_180;
        public int e_FW_fix_direction_r90;
        public int e_FW_fix_direction_l90;
        #endregion

        #region Enemy Hit Animation States.

        #region Enemy Blocking Mod.
        public int e_block_break_hash;
        public int e_block_react_hash;
        #endregion

        #region Dual Weapon Mod.
        public int e_armed_FW_hit_small_r_hash;
        public int e_armed_FW_hit_small_l_hash;
        public int e_armed_FW_hit_small_f_hash;

        public int e_armed_FW_hit_big_r_hash;
        public int e_armed_FW_hit_big_l_hash;
        public int e_armed_FW_hit_big_f_hash;

        public int e_armed_SW_hit_small_r_hash;
        public int e_armed_SW_hit_small_l_hash;
        public int e_armed_SW_hit_small_f_hash;

        public int e_armed_SW_hit_big_r_hash;
        public int e_armed_SW_hit_big_l_hash;
        public int e_armed_SW_hit_big_f_hash;
        #endregion

        #region Two Stance Mod.
        public int e_armed_RS_hit_small_r_hash;
        public int e_armed_RS_hit_small_l_hash;
        public int e_armed_RS_hit_small_f_hash;

        public int e_armed_RS_hit_big_r_hash;
        public int e_armed_RS_hit_big_l_hash;
        public int e_armed_RS_hit_big_f_hash;

        public int e_armed_LS_hit_small_r_hash;
        public int e_armed_LS_hit_small_l_hash;
        public int e_armed_LS_hit_small_f_hash;

        public int e_armed_LS_hit_big_r_hash;
        public int e_armed_LS_hit_big_l_hash;
        public int e_armed_LS_hit_big_f_hash;
        #endregion

        #region Armed.
        public int e_armed_hit_small_r_hash;
        public int e_armed_hit_small_l_hash;
        public int e_armed_hit_small_f_hash;

        public int e_armed_hit_big_r_hash;
        public int e_armed_hit_big_l_hash;
        public int e_armed_hit_big_f_hash;
        #endregion

        #region UnArmed.
        public int e_unarmed_hit_small_r_hash;
        public int e_unarmed_hit_small_l_hash;
        public int e_unarmed_hit_small_f_hash;

        public int e_unarmed_hit_big_r_hash;
        public int e_unarmed_hit_big_l_hash;
        public int e_unarmed_hit_big_f_hash;
        #endregion

        #endregion

        #region Enemy Knock Down Animation States.
        public int e_armed_FW_knockDown_hash;
        public int e_armed_SW_knockDown_hash;
        public int e_armed_SW_knockDown_HitFromBack_hash;

        public int e_armed_RS_knockDown_hash;
        public int e_armed_LS_knockDown_hash;

        public int e_armed_knockDown_hash;
        public int e_unarmed_knockDown_hash;
        #endregion

        #region Enemy Death Animation States.
        public int e_armed_FW_death_hash;
        public int e_armed_SW_death_hash;

        public int e_armed_RS_death_hash;
        public int e_armed_LS_death_hash;

        public int e_armed_death_hash;
        public int e_unarmed_death_hash;

        public int e_pw_death_hash;
        #endregion

        #region Enemy Sheath Animation States.
        public int e_sheath_First_hash;
        public int e_sheath_Second_hash;
        public int e_unsheath_First_hash;
        public int e_unsheath_Second_hash;
        #endregion

        #region Enemy Falling Animation States.
        public int e_unarmed_falling_hash;
        public int e_armed_falling_hash;
        #endregion

        #region Enemy Turning Animation States.

        #region Root.
        /* First Weapon/Second Weapon. */
        public int e_armed_FW_turn_left_90_hash;
        public int e_armed_FW_turn_right_90_hash;
        public int e_armed_SW_turn_left_90_hash;
        public int e_armed_SW_turn_right_90_hash;

        /* Right Stance/Left Stance. */
        public int e_armed_RS_turn_left_90_hash;
        public int e_armed_RS_turn_right_90_hash;
        public int e_armed_LS_turn_left_90_hash;
        public int e_armed_LS_turn_right_90_hash;

        /* First Weapon Right Stance/ First Weapon Left Stance. */
        public int e_armed_FW_RS_turn_left_90_hash;
        public int e_armed_FW_RS_turn_right_90_hash;
        public int e_armed_FW_LS_turn_left_90_hash;
        public int e_armed_FW_LS_turn_right_90_hash;

        /* Second Weapon Right Stance/ Second Weapon Left Stance. */
        public int e_armed_SW_RS_turn_left_90_hash;
        public int e_armed_SW_RS_turn_right_90_hash;
        public int e_armed_SW_LS_turn_left_90_hash;
        public int e_armed_SW_LS_turn_right_90_hash;

        public int e_armed_turn_left_90_hash;
        public int e_armed_turn_right_90_hash;

        public int e_unarmed_turn_left_90_hash;
        public int e_unarmed_turn_right_90_hash;

        public int e_aim_turn_left_45_hash;
        public int e_aim_turn_right_45_hash;
        #endregion

        #region Inplace.
        /* First Weapon/Second Weapon. */
        public int e_armed_FW_turn_left_inplace_hash;
        public int e_armed_FW_turn_right_inplace_hash;
        public int e_armed_SW_turn_left_inplace_hash;
        public int e_armed_SW_turn_right_inplace_hash;

        /* Right Stance/Left Stance. */
        public int e_armed_RS_turn_left_inplace_hash;
        public int e_armed_RS_turn_right_inplace_hash;
        public int e_armed_LS_turn_left_inplace_hash;
        public int e_armed_LS_turn_right_inplace_hash;

        /* First Weapon Right Stance/ First Weapon Left Stance. */
        public int e_armed_FW_RS_turn_left_inplace_hash;
        public int e_armed_FW_RS_turn_right_inplace_hash;
        public int e_armed_FW_LS_turn_left_inplace_hash;
        public int e_armed_FW_LS_turn_right_inplace_hash;

        /* Second Weapon Right Stance/ Second Weapon Left Stance. */
        public int e_armed_SW_RS_turn_left_inplace_hash;
        public int e_armed_SW_RS_turn_right_inplace_hash;
        public int e_armed_SW_LS_turn_left_inplace_hash;
        public int e_armed_SW_LS_turn_right_inplace_hash;

        public int e_armed_turn_left_inplace_hash;
        public int e_armed_turn_right_inplace_hash;

        public int e_unarmed_turn_left_inplace_hash;
        public int e_unarmed_turn_right_inplace_hash;

        public int e_aim_turn_left_inplace_hash;
        public int e_aim_turn_right_inplace_hash;
        #endregion

        #endregion

        #region Enemy Parry Recevied States.
        public int e_parry_received_start_r_hash;

        #region Execution.
        public int e_axe_parryExecute_received_hash;
        public int e_fist_parryExecute_received_hash;
        public int e_gs_parryExecute_received_hash;
        public int e_shield_parryExecute_received_hash;
        public int e_strs_parryExecute_received_hash;
        #endregion

        #endregion

        #region Enemy Aiming Mod.
        public int e_aim_start_hash;
        public int e_aim_loop_hash;
        public int e_aim_quit_hash;

        public int e_javelin_pickup_hash;
        public int e_javelin_brokenBounceBack_hash;
        #endregion

        #region Enemy Taunt Mod.
        public int e_taunt_1_hash;
        public int e_taunt_2_hash;
        public int e_taunt_3_hash;
        #endregion

        #region Enemy Evolve Mod.
        public int e_evolve_start_hash;
        public int e_evolve_end_hash;
        #endregion
        
        #region Enemy Execution Mod.
        public int e_execution_opening_full_hash;
        #endregion

        #region Egil Intro Poses.
        public int egil_intro_pose_1_hash;
        #endregion

        #region Egil Stamina Mod.
        public int egil_injured_hash;
        public int egil_injured_revenge_attack_1_hash;
        #endregion

        #region Egil Kinematic Motion Attack Mod.
        public int egil_P3_KMJ_1stHalf_hash;
        public int egil_P3_KMJ_2ndHalf_hash;
        public int egil_P3_KMJ_land_hash;

        public int egil_P2_KMJ_1stHalf_hash;
        public int egil_P2_KMJ_2ndHalf_hash;
        public int egil_P2_KMJ_land_hash;
        #endregion

        #region Egil Second Phase Mod.
        public int egil_Taunt_Chain_1_hash;
        public int egil_1P_empty_override_hash;
        public int egil_2P_empty_override_hash;
        public int egil_3P_empty_override_hash;
        public int egil_ChangePhase2_Chain_3_hash;
        #endregion

        #endregion

        #region Player.

        #region Player Transition Parameter
        [Header("Player Transition Parameter")]
        public int p_IsNeglecting_hash;
        public int p_IsRunning_hash;
        public int p_IsSprinting_hash;
        public int p_IsGrounded_hash;
        public int p_IsBlocking_hash;
        public int p_IsTwoHanding_hash;
        public int p_IsGetupReady_hash;
        public int p_IsBonfireEnd_hash;
        public int p_IsLevelupBegin_hash;
        public int p_IsTriggerFullyHeld_hash;
        public int p_IsAnimationJobFinished_hash;
        public int p_IsHandleIKJobFinished_hash;
        #endregion

        #region Player Anim State Speed Multi.
        [Header("Anim State Speed Multi.")]
        public int p_HoldAttackSpeedMulti_hash;
        #endregion

        #region Player Base Layer
        [Header("Player Base Layer")]

        /// Oh Idle Right.
        public int p_axe_locomotion_hash;
        public int p_fist_locomotion_hash;
        public int p_gs_locomotion_hash;
        public int p_shield_locomotion_hash;
        public int p_strs_locomotion_hash;
        public int p_bow_locomotion_hash;
        public int p_catalysis_locomotion_hash;

        /// Th Idle.
        public int p_axe_locomotion_th_hash;
        public int p_fist_locomotion_th_hash;
        public int p_gs_locomotion_th_hash;
        public int p_shield_locomotion_th_hash;
        public int p_strs_locomotion_th_hash;
        public int p_bow_locomotion_th_hash;
        public int p_catalysis_locomotion_th_hash;
        #endregion
        
        #region Player Chest Right Hand Override Layer
        [Header("Player Chest Right Hand Override Layer")]

        #region Sheath / UnSheath.
        /// Rh Unsheath.
        public int p_axe_unSheath_r_hash;
        public int p_gs_unSheath_r_hash;
        public int p_shield_unSheath_r_hash;
        public int p_strs_unSheath_r_hash;
        public int p_bow_unSheath_r_hash;
        public int p_catalysis_unSheath_r_hash;

        /// Rh Sheath.
        public int p_axe_sheath_r_hash;
        public int p_gs_sheath_r_hash;
        public int p_shield_sheath_r_hash;
        public int p_strs_sheath_r_hash;
        public int p_bow_sheath_r_hash;
        public int p_catalysis_sheath_r_hash;

        public int p_axe_sheath_r_backpack_hash;
        public int p_gs_sheath_r_backpack_hash;
        public int p_shield_sheath_r_backpack_hash;
        public int p_strs_sheath_r_backpack_hash;
        public int p_bow_sheath_r_backpack_hash;
        public int p_catalysis_sheath_r_backpack_hash;
        #endregion

        /// Items.
        public int p_item_throw_horizontal_hash;
        public int p_item_throw_high_hash;
        public int p_item_throw_mid_hash;
        public int p_item_throw_low_hash;

        /// Interactions.
        public int p_int_bonfire_ignite_hash;
        #endregion

        #region Player Chest Left Hand Override Layer
        [Header("Player Chest Left Hand Override Layer")]

        #region Sheath / UnSheath.
        /// Lh Unsheath.
        public int p_axe_unSheath_l_hash;
        public int p_gs_unSheath_l_hash;
        public int p_shield_unSheath_l_hash;
        public int p_strs_unSheath_l_hash;
        public int p_bow_unSheath_l_hash;
        public int p_catalysis_unSheath_l_hash;

        /// Lh Sheath.
        public int p_axe_sheath_l_hash;
        public int p_gs_sheath_l_hash;
        public int p_shield_sheath_l_hash;
        public int p_strs_sheath_l_hash;
        public int p_bow_sheath_l_hash;
        public int p_catalysis_sheath_l_hash;

        public int p_axe_sheath_l_backpack_hash;
        public int p_gs_sheath_l_backpack_hash;
        public int p_shield_sheath_l_backpack_hash;
        public int p_strs_sheath_l_backpack_hash;
        public int p_bow_sheath_l_backpack_hash;
        public int p_catalysis_sheath_l_backpack_hash;
        #endregion

        #region Oppose1 Blocking Start / React.
        /// Oppose1 Blocking Start.
        public int p_axe_oppose1_blocking_start_hash;
        public int p_gs_oppose1_blocking_start_hash;
        public int p_shield_oppose1_blocking_start_hash;
        public int p_strs_oppose1_blocking_start_hash;

        /// Oppose1 Blocking React.
        public int p_axe_oppose1_blocking_react_hash;
        public int p_gs_oppose1_blocking_react_hash;
        public int p_shield_oppose1_blocking_react_hash;
        public int p_strs_oppose1_blocking_react_hash;
        #endregion

        #endregion

        #region Player Upper Body Override Layer
        [Header("Player Upper Body Override Layer")]

        #region Light2 Blocking Start / React.
        /// Light2 Blocking Start.
        public int p_axe_light2_blocking_start_hash;
        public int p_gs_light2_blocking_start_hash;
        public int p_shield_light2_blocking_start_hash;
        public int p_strs_light2_blocking_start_hash;

        /// Light2 Blocking React.
        public int p_axe_light2_blocking_react_hash;
        public int p_gs_light2_blocking_react_hash;
        public int p_shield_light2_blocking_react_hash;
        public int p_strs_light2_blocking_react_hash;
        #endregion

        #region Damaged.

        #region Axe.
        public int p_axe_1h_hit_small_f_hash;
        public int p_axe_1h_hit_small_b_hash;
        public int p_axe_1h_hit_small_l_hash;
        public int p_axe_1h_hit_small_r_hash;

        public int p_axe_2h_hit_small_f_hash;
        public int p_axe_2h_hit_small_b_hash;
        public int p_axe_2h_hit_small_l_hash;
        public int p_axe_2h_hit_small_r_hash;
        #endregion

        #region Shield.
        public int p_shield_1h_hit_small_f_hash;
        public int p_shield_1h_hit_small_b_hash;
        public int p_shield_1h_hit_small_l_hash;
        public int p_shield_1h_hit_small_r_hash;

        public int p_shield_2h_hit_small_f_hash;
        public int p_shield_2h_hit_small_b_hash;
        public int p_shield_2h_hit_small_l_hash;
        public int p_shield_2h_hit_small_r_hash;
        #endregion

        #region Fist.
        public int p_fist_1h_hit_small_f_hash;
        public int p_fist_1h_hit_small_b_hash;
        public int p_fist_1h_hit_small_l_hash;
        public int p_fist_1h_hit_small_r_hash;

        public int p_fist_2h_hit_small_f_hash;
        public int p_fist_2h_hit_small_b_hash;
        public int p_fist_2h_hit_small_l_hash;
        public int p_fist_2h_hit_small_r_hash;
        #endregion

        #endregion

        #region Fist Two Hand Sheath / UnSheath
        public int p_fist_th_sheath_hash;
        public int p_fist_th_unSheath_hash;
        public int p_fist_th_sheath_fullBody_hash;
        public int p_fist_th_unSheath_fullBody_hash;
        #endregion

        #region Two Handing Switch Weapon.
        public int p_switchToRh_hash;
        public int p_switchToLh_hash;
        public int p_passToRh_hash;
        public int p_passToLh_hash;
        #endregion

        public int p_item_vessel_empty_hash;
        #endregion

        #region Player Full Body Left Hand Override Layer
        [Header("Player Full Body Left Hand Override Layer")]

        #region Empty.
        public int p_empty_fullbody_lh_overide_hash;
        #endregion

        #region Oppose1 Blocking Break.
        /// Oppose1 Blocking Break;
        public int p_axe_oppose1_blocking_break_hash;
        public int p_gs_oppose1_blocking_break_hash;
        public int p_shield_oppose1_blocking_break_hash;
        public int p_strs_oppose1_blocking_break_hash;
        #endregion

        #region Interaction.
        public int p_int_pickup_up_hash;
        public int p_int_pickup_mid_hash;
        public int p_int_pickup_down_hash;
        #endregion

        #endregion

        #region Player Full Body Right Hand Override Layer
        [Header("Player Full Body Right Hand Override Layer")]

        #region Empty.
        public int p_empty_fullbody_rh_overide_hash;
        #endregion

        #region Interaction.
        public int p_int_takeChest_hash;
        #endregion

        #endregion

        #region Player Full Body Override Layer
        [Header("Player Full Body Override Layer")]

        #region Empty.
        public int p_empty_fullBody_override_hash;
        #endregion

        #region Light2 Blocking Start / React / Break.
        public int p_axe_light2_blocking_break_hash;
        public int p_gs_light2_blocking_break_hash;
        public int p_shield_light2_blocking_break_hash;
        public int p_strs_light2_blocking_break_hash;
        #endregion

        #region Heavy1 Charge Enchant.
        public int p_axe_heavy1_charge_enchant_hash;
        #endregion

        #region Roll.
        public int p_axe_lockon_rolls_tree_hash;
        public int p_fist_1h_lockon_rolls_tree_hash;
        public int p_fist_2h_lockon_rolls_tree_hash;
        public int p_fist_evade_tree_hash;
        public int p_gs_lockon_rolls_tree_hash;
        public int p_shield_lockon_rolls_tree_hash;
        public int p_strs_lockon_rolls_tree_hash;
        public int p_backstep_hash;
        #endregion

        #region Jump.
        public int p_unarmed_jump_start_hash;
        public int p_unarmed_fall_start_hash;
        public int p_unarmed_land_hash;
        public int p_armed_jump_start_hash;
        public int p_armed_fall_start_hash;
        public int p_armed_land_hash;
        #endregion

        #region Sprinting.
        public int p_fist_sprint_start_hash;
        public int p_light_sprint_start_hash;
        public int p_sprint_end_hash;
        #endregion

        #region Parry Received.
        public int p_parry_received_hash;
        #endregion
        
        #region Damaged.

        #region Axe.
        public int p_axe_1h_hit_big_f_hash;
        public int p_axe_1h_hit_big_b_hash;
        public int p_axe_1h_hit_big_l_hash;
        public int p_axe_1h_hit_big_r_hash;

        public int p_axe_1h_knockback_hash;
        public int p_axe_1h_death_hash;

        public int p_axe_2h_hit_big_f_hash;
        public int p_axe_2h_hit_big_b_hash;
        public int p_axe_2h_hit_big_l_hash;
        public int p_axe_2h_hit_big_r_hash;

        public int p_axe_2h_knockback_hash;
        public int p_axe_2h_death_hash;
        #endregion

        #region Shield.
        public int p_shield_1h_hit_big_f_hash;
        public int p_shield_1h_hit_big_b_hash;
        public int p_shield_1h_hit_big_l_hash;
        public int p_shield_1h_hit_big_r_hash;

        public int p_shield_1h_knockback_hash;
        public int p_shield_1h_death_hash;

        public int p_shield_2h_hit_big_f_hash;
        public int p_shield_2h_hit_big_b_hash;
        public int p_shield_2h_hit_big_l_hash;
        public int p_shield_2h_hit_big_r_hash;

        public int p_shield_2h_knockback_hash;
        public int p_shield_2h_death_hash;
        #endregion

        #region Fist.
        public int p_fist_1h_hit_big_f_hash;
        public int p_fist_1h_hit_big_b_hash;
        public int p_fist_1h_hit_big_l_hash;
        public int p_fist_1h_hit_big_r_hash;

        public int p_fist_1h_knockback_hash;
        public int p_fist_1h_death_hash;

        public int p_fist_2h_hit_big_f_hash;
        public int p_fist_2h_hit_big_b_hash;
        public int p_fist_2h_hit_big_l_hash;
        public int p_fist_2h_hit_big_r_hash;

        public int p_fist_2h_knockback_hash;
        public int p_fist_2h_death_hash;
        #endregion

        #endregion
        
        #region Interaction.
        public int p_int_bonfire_start_hash;

        public int p_int_cantOpen_hash;
        public int p_int_openDoor_hash;
        public int p_int_openChest_hash;
        
        public int p_int_levelup_end_hash;
        #endregion

        #region Parry Execution.
        public int p_axe_parryExecute_present_hash;
        public int p_fist_parryExecute_present_hash;
        public int p_gs_parryExecute_present_hash;
        public int p_shield_parryExecute_present_hash;
        public int p_strs_parryExecute_present_hash;
        #endregion

        #region Getup.

        #region Axe.
        public int p_axe_1h_getup_faceUp_hash;
        public int p_axe_1h_getup_faceDown_hash;
        public int p_axe_2h_getup_faceUp_hash;
        public int p_axe_2h_getup_faceDown_hash;
        #endregion

        #region Shield.
        public int p_shield_1h_getup_faceUp_hash;
        public int p_shield_1h_getup_faceDown_hash;
        public int p_shield_2h_getup_faceUp_hash;
        public int p_shield_2h_getup_faceDown_hash;
        #endregion

        #region Fist.
        public int p_fist_1h_getup_faceUp_hash;
        public int p_fist_1h_getup_faceDown_hash;
        public int p_fist_2h_getup_faceUp_hash;
        public int p_fist_2h_getup_faceDown_hash;
        #endregion

        #endregion

        #region Revive.

        #region Axe.
        public int p_axe_revive_hash;
        #endregion

        #region Shield.
        public int p_shield_revive_hash;
        #endregion

        #region Fist.
        public int p_fist_revive_hash;
        #endregion

        #endregion

        #endregion

        #endregion

        public static HashManager singleton;
        private void Awake()
        {
            if(singleton == null)
                singleton = this;
            else
                Destroy(gameObject);

            GenerateHashForVariables(animStateVariables.enemyAnimStates, animStateVariables.playerAnimStates);

            #region Public.
            //Public Transition Parameter  ---------------------------------------------------------------------------------
            vertical_hash = Animator.StringToHash("vertical");
            horizontal_hash = Animator.StringToHash("horizontal");

            vertical_whole_hash = Animator.StringToHash("vertical_whole");
            horizontal_whole_hash = Animator.StringToHash("horizontal_whole");
            #endregion

            #region Enemy Transition Parameters.
            //Enemy Transition Parameter  ---------------------------------------------------------------------------------
            e_IsFacedPlayer_hash = Animator.StringToHash("e_IsFacedPlayer");
            e_IsInteracting_hash = Animator.StringToHash("e_IsInteracting");
            e_IsArmed_hash = Animator.StringToHash("e_IsArmed");
            e_IsGrounded_hash = Animator.StringToHash("e_IsGrounded");
            e_IsLockOnMoveAround_hash = Animator.StringToHash("e_IsLockOnMoveAround");
            e_IsKnockedDown_hash = Animator.StringToHash("e_IsKnockedDown");
            e_IsInParryWindow_hash = Animator.StringToHash("e_IsInParryWindow");
            e_IsDead_hash = Animator.StringToHash("e_IsDead");
            e_mod_IsUsingSecondWeapon_hash = Animator.StringToHash("e_mod_IsUsingSecondWeapon");
            e_mod_IsTired_hash = Animator.StringToHash("e_mod_IsTired");
            e_mod_IsBlocking_hash = Animator.StringToHash("e_mod_IsBlocking");
            e_mod_IsMovingInFixDirection_hash = Animator.StringToHash("e_mod_IsMovingInFixDirection");
            e_mod_IsRightStance_hash = Animator.StringToHash("e_mod_IsRightStance");
            e_mod_isWaitingToParry_hash = Animator.StringToHash("e_mod_isWaitingToParry");
            e_mod_isIn2ndPhase_hash = Animator.StringToHash("e_mod_isIn2ndPhase");
            e_mod_isIn3rdPhase_hash = Animator.StringToHash("e_mod_isIn3rdPhase");
            e_mod_isIn4thPhase_hash = Animator.StringToHash("e_mod_isIn4thPhase");

            //Power Weapon Transition Parameter  --------------------------------------------------------------------------
            e_javelin_isEquiped_hash = Animator.StringToHash("e_javelin_isEquiped");
            e_javelin_isAiming_hash = Animator.StringToHash("e_javelin_isAiming");
            #endregion

            #region Egil Begin Armed Locomotion State.
            //Egil Begin Armed Locomotion State  --------------------------------------------------------------------------
            e_egil_1P_armed_locomotion_hash = Animator.StringToHash("e_egil_1P_armed_locomotion");
            #endregion

            #region Enemy Attack Animation States.
            //Enemy Attack Animation State  ---------------------------------------------------------------------------------
            e_attack_1_hash = Animator.StringToHash("e_attack_1");
            e_attack_2_hash = Animator.StringToHash("e_attack_2");
            e_attack_3_hash = Animator.StringToHash("e_attack_3");
            e_attack_4_hash = Animator.StringToHash("e_attack_4");
            e_attack_5_hash = Animator.StringToHash("e_attack_5");
            e_attack_6_hash = Animator.StringToHash("e_attack_6");
            e_attack_7_hash = Animator.StringToHash("e_attack_7");
            e_attack_8_hash = Animator.StringToHash("e_attack_8");
            e_attack_9_hash = Animator.StringToHash("e_attack_9");
            
            e_roll_attack_1_hash = Animator.StringToHash("e_roll_attack_1");
            e_roll_attack_2_hash = Animator.StringToHash("e_roll_attack_2");
            e_roll_attack_3_hash = Animator.StringToHash("e_roll_attack_3");

            e_roll_attack_1_ready_hash = Animator.StringToHash("e_roll_attack_1_ready");
            e_roll_attack_2_ready_hash = Animator.StringToHash("e_roll_attack_2_ready");
            e_roll_attack_3_ready_hash = Animator.StringToHash("e_roll_attack_3_ready");

            e_roll_attack_1_ready_roll_tree_hash = Animator.StringToHash("e_roll_attack_1_ready_roll_tree");
            e_roll_attack_2_ready_roll_tree_hash = Animator.StringToHash("e_roll_attack_2_ready_roll_tree");
            e_roll_attack_3_ready_roll_tree_hash = Animator.StringToHash("e_roll_attack_3_ready_roll_tree");

            e_throw_attack_1_hash = Animator.StringToHash("e_throw_attack_1");
            e_throw_attack_2_hash = Animator.StringToHash("e_throw_attack_2");
            e_throw_attack_3_hash = Animator.StringToHash("e_throw_attack_3");

            e_aim_attack_1_hash = Animator.StringToHash("e_aim_attack_1");
            e_aim_attack_2_hash = Animator.StringToHash("e_aim_attack_2");
            
            e_combo_1_a_hash = Animator.StringToHash("e_combo_1_a");
            e_combo_1_b_hash = Animator.StringToHash("e_combo_1_b");
            e_combo_1_c_hash = Animator.StringToHash("e_combo_1_c");
            e_combo_1_d_hash = Animator.StringToHash("e_combo_1_d");

            e_combo_2_a_hash = Animator.StringToHash("e_combo_2_a");
            e_combo_2_b_hash = Animator.StringToHash("e_combo_2_b");
            e_combo_2_c_hash = Animator.StringToHash("e_combo_2_c");
            e_combo_2_d_hash = Animator.StringToHash("e_combo_2_d");

            e_combo_3_a_hash = Animator.StringToHash("e_combo_3_a");
            e_combo_3_b_hash = Animator.StringToHash("e_combo_3_b");
            e_combo_3_c_hash = Animator.StringToHash("e_combo_3_c");
            e_combo_3_d_hash = Animator.StringToHash("e_combo_3_d");

            e_combo_4_a_hash = Animator.StringToHash("e_combo_4_a");
            e_combo_4_b_hash = Animator.StringToHash("e_combo_4_b");
            e_combo_4_c_hash = Animator.StringToHash("e_combo_4_c");
            e_combo_4_d_hash = Animator.StringToHash("e_combo_4_d");

            e_combo_5_a_hash = Animator.StringToHash("e_combo_5_a");
            e_combo_5_b_hash = Animator.StringToHash("e_combo_5_b");
            e_combo_5_c_hash = Animator.StringToHash("e_combo_5_c");
            e_combo_5_d_hash = Animator.StringToHash("e_combo_5_d");

            e_combo_6_a_hash = Animator.StringToHash("e_combo_6_a");
            e_combo_6_b_hash = Animator.StringToHash("e_combo_6_b");
            e_combo_6_c_hash = Animator.StringToHash("e_combo_6_c");
            e_combo_6_d_hash = Animator.StringToHash("e_combo_6_d");

            e_combo_7_a_hash = Animator.StringToHash("e_combo_7_a");
            e_combo_7_b_hash = Animator.StringToHash("e_combo_7_b");
            e_combo_7_c_hash = Animator.StringToHash("e_combo_7_c");
            e_combo_7_d_hash = Animator.StringToHash("e_combo_7_d");

            e_combo_8_a_hash = Animator.StringToHash("e_combo_8_a");
            e_combo_8_b_hash = Animator.StringToHash("e_combo_8_b");
            e_combo_8_c_hash = Animator.StringToHash("e_combo_8_c");
            e_combo_8_d_hash = Animator.StringToHash("e_combo_8_d");

            e_combo_9_a_hash = Animator.StringToHash("e_combo_9_a");
            e_combo_9_b_hash = Animator.StringToHash("e_combo_9_b");
            e_combo_9_c_hash = Animator.StringToHash("e_combo_9_c");
            e_combo_9_d_hash = Animator.StringToHash("e_combo_9_d");

            e_combo_10_a_hash = Animator.StringToHash("e_combo_10_a");
            e_combo_10_b_hash = Animator.StringToHash("e_combo_10_b");
            e_combo_10_c_hash = Animator.StringToHash("e_combo_10_c");
            e_combo_10_d_hash = Animator.StringToHash("e_combo_10_d");

            e_combo_11_a_hash = Animator.StringToHash("e_combo_11_a");
            e_combo_11_b_hash = Animator.StringToHash("e_combo_11_b");
            e_combo_11_c_hash = Animator.StringToHash("e_combo_11_c");
            e_combo_11_d_hash = Animator.StringToHash("e_combo_11_d");

            e_combo_12_a_hash = Animator.StringToHash("e_combo_12_a");
            e_combo_12_b_hash = Animator.StringToHash("e_combo_12_b");
            e_combo_12_c_hash = Animator.StringToHash("e_combo_12_c");
            e_combo_12_d_hash = Animator.StringToHash("e_combo_12_d");

            e_LS_parry_waiting_start_hash = Animator.StringToHash("e_LS_parry_waiting_start");
            e_RS_parry_waiting_start_hash = Animator.StringToHash("e_RS_parry_waiting_start");

            e_parry_attack_1_ready_hash = Animator.StringToHash("e_parry_attack_1_ready");
            e_parry_attack_1_hash = Animator.StringToHash("e_parry_attack_1");

            e_RS_parry_attack_1_ready_hash = Animator.StringToHash("e_RS_parry_attack_1_ready");
            e_RS_parry_attack_1_hash = Animator.StringToHash("e_RS_parry_attack_1");

            e_LS_parry_attack_1_ready_hash = Animator.StringToHash("e_LS_parry_attack_1_ready");
            e_LS_parry_attack_1_hash = Animator.StringToHash("e_LS_parry_attack_1");
            #endregion

            #region Enemy Roll Animation States.
            //Enemy Roll Animation State  ----------------------------------------------------------------------------------
            e_roll_1_hash = Animator.StringToHash("e_roll_1");
            e_roll_2_hash = Animator.StringToHash("e_roll_2");
            e_roll_3_hash = Animator.StringToHash("e_roll_3");
            e_roll_tree_hash = Animator.StringToHash("e_roll_tree");
            e_RS_roll_1_hash = Animator.StringToHash("e_RS_roll_1");
            e_LS_roll_1_hash = Animator.StringToHash("e_LS_roll_1");
            #endregion

            #region Enemy Move In Fix Direction States.
            //Enemy Move In Fix Direction Animation State  -----------------------------------------------------------------
            e_fix_direction_180 = Animator.StringToHash("e_fix_direction_180");
            e_fix_direction_l90 = Animator.StringToHash("e_fix_direction_l90");
            e_fix_direction_r90 = Animator.StringToHash("e_fix_direction_r90");
            e_fix_direction_end = Animator.StringToHash("e_fix_direction_end");

            e_SW_fix_direction_180 = Animator.StringToHash("e_SW_fix_direction_180");
            e_SW_fix_direction_l90 = Animator.StringToHash("e_SW_fix_direction_l90");
            e_SW_fix_direction_r90 = Animator.StringToHash("e_SW_fix_direction_r90");

            e_FW_fix_direction_180 = Animator.StringToHash("e_FW_fix_direction_180");
            e_FW_fix_direction_l90 = Animator.StringToHash("e_FW_fix_direction_l90");
            e_FW_fix_direction_r90 = Animator.StringToHash("e_FW_fix_direction_r90");
            #endregion

            #region Enemy Hit Animation States.
            //Enemy Hit Animation State  ------------------------------------------------------------------------------------
            e_block_break_hash = Animator.StringToHash("e_block_break");
            e_block_react_hash = Animator.StringToHash("e_block_react");

            // Dual Weapon.
            e_armed_FW_hit_small_r_hash = Animator.StringToHash("e_armed_FW_hit_small_r");
            e_armed_FW_hit_small_l_hash = Animator.StringToHash("e_armed_FW_hit_small_l");
            e_armed_FW_hit_small_f_hash = Animator.StringToHash("e_armed_FW_hit_small_f");

            e_armed_FW_hit_big_r_hash = Animator.StringToHash("e_armed_FW_hit_big_r");
            e_armed_FW_hit_big_l_hash = Animator.StringToHash("e_armed_FW_hit_big_l");
            e_armed_FW_hit_big_f_hash = Animator.StringToHash("e_armed_FW_hit_big_f");

            e_armed_SW_hit_small_r_hash = Animator.StringToHash("e_armed_SW_hit_small_r");
            e_armed_SW_hit_small_l_hash = Animator.StringToHash("e_armed_SW_hit_small_l");
            e_armed_SW_hit_small_f_hash = Animator.StringToHash("e_armed_SW_hit_small_f");

            e_armed_SW_hit_big_r_hash = Animator.StringToHash("e_armed_SW_hit_big_r");
            e_armed_SW_hit_big_l_hash = Animator.StringToHash("e_armed_SW_hit_big_l");
            e_armed_SW_hit_big_f_hash = Animator.StringToHash("e_armed_SW_hit_big_f");

            // Two Stance.
            e_armed_RS_hit_small_r_hash = Animator.StringToHash("e_armed_RS_hit_small_r");
            e_armed_RS_hit_small_l_hash = Animator.StringToHash("e_armed_RS_hit_small_l");
            e_armed_RS_hit_small_f_hash = Animator.StringToHash("e_armed_RS_hit_small_f");

            e_armed_RS_hit_big_r_hash = Animator.StringToHash("e_armed_RS_hit_big_r");
            e_armed_RS_hit_big_l_hash = Animator.StringToHash("e_armed_RS_hit_big_l");
            e_armed_RS_hit_big_f_hash = Animator.StringToHash("e_armed_RS_hit_big_f");

            e_armed_LS_hit_small_r_hash = Animator.StringToHash("e_armed_LS_hit_small_r");
            e_armed_LS_hit_small_l_hash = Animator.StringToHash("e_armed_LS_hit_small_l");
            e_armed_LS_hit_small_f_hash = Animator.StringToHash("e_armed_LS_hit_small_f");

            e_armed_LS_hit_big_r_hash = Animator.StringToHash("e_armed_LS_hit_big_r");
            e_armed_LS_hit_big_l_hash = Animator.StringToHash("e_armed_LS_hit_big_f");
            e_armed_LS_hit_big_f_hash = Animator.StringToHash("e_armed_LS_hit_big_l");
            
            // Normal.
            e_armed_hit_small_r_hash = Animator.StringToHash("e_armed_hit_small_r");
            e_armed_hit_small_l_hash = Animator.StringToHash("e_armed_hit_small_l");
            e_armed_hit_small_f_hash = Animator.StringToHash("e_armed_hit_small_f");

            e_armed_hit_big_r_hash = Animator.StringToHash("e_armed_hit_big_r");
            e_armed_hit_big_l_hash = Animator.StringToHash("e_armed_hit_big_l");
            e_armed_hit_big_f_hash = Animator.StringToHash("e_armed_hit_big_f");
            
            e_unarmed_hit_small_r_hash = Animator.StringToHash("e_unarmed_hit_small_r");
            e_unarmed_hit_small_l_hash = Animator.StringToHash("e_unarmed_hit_small_l");
            e_unarmed_hit_small_f_hash = Animator.StringToHash("e_unarmed_hit_small_f");

            e_unarmed_hit_big_r_hash = Animator.StringToHash("e_unarmed_hit_big_r");
            e_unarmed_hit_big_l_hash = Animator.StringToHash("e_unarmed_hit_big_l");
            e_unarmed_hit_big_f_hash = Animator.StringToHash("e_unarmed_hit_big_f");
            #endregion

            #region Enemy Knock Down Animation States.
            e_armed_FW_knockDown_hash = Animator.StringToHash("e_armed_FW_knockDown");
            e_armed_SW_knockDown_hash = Animator.StringToHash("e_armed_SW_knockDown");
            e_armed_SW_knockDown_HitFromBack_hash = Animator.StringToHash("e_armed_SW_knockDown_HitFromBack");

            e_armed_RS_knockDown_hash = Animator.StringToHash("e_armed_RS_knockDown");
            e_armed_LS_knockDown_hash = Animator.StringToHash("e_armed_LS_knockDown");

            e_armed_knockDown_hash = Animator.StringToHash("e_armed_knockDown");
            e_unarmed_knockDown_hash = Animator.StringToHash("e_unarmed_knockDown");
            #endregion

            #region Enemy On Death Animator States.
            e_armed_FW_death_hash = Animator.StringToHash("e_armed_FW_death");
            e_armed_SW_death_hash = Animator.StringToHash("e_armed_SW_death");

            e_armed_RS_death_hash = Animator.StringToHash("e_armed_RS_death");
            e_armed_LS_death_hash = Animator.StringToHash("e_armed_LS_death");

            e_armed_death_hash = Animator.StringToHash("e_armed_death");
            e_unarmed_death_hash = Animator.StringToHash("e_unarmed_death");

            e_pw_death_hash = Animator.StringToHash("e_pw_death");
            #endregion

            #region Enemy Sheath Animation States.
            //Enemy Sheath Animation State  ---------------------------------------------------------------------------------
            e_sheath_First_hash = Animator.StringToHash("e_sheath_First");
            e_sheath_Second_hash = Animator.StringToHash("e_sheath_Second");
            e_unsheath_First_hash = Animator.StringToHash("e_unsheath_First");
            e_unsheath_Second_hash = Animator.StringToHash("e_unsheath_Second");
            #endregion

            #region Enemy Falling Animation States.
            //Enemy Falling Animation State  ----------------------------------------------------------------------------------
            e_unarmed_falling_hash = Animator.StringToHash("e_unarmed_falling");
            e_armed_falling_hash = Animator.StringToHash("e_armed_falling");
            #endregion

            #region Enemy Turning Animation States.
            //Enemy Turning Animation State  --------------------------------------------------------------------------------
            e_armed_FW_turn_left_90_hash = Animator.StringToHash("e_armed_FW_turn_left_90");
            e_armed_FW_turn_right_90_hash = Animator.StringToHash("e_armed_FW_turn_right_90");
            e_armed_SW_turn_left_90_hash = Animator.StringToHash("e_armed_SW_turn_left_90");
            e_armed_SW_turn_right_90_hash = Animator.StringToHash("e_armed_SW_turn_right_90");

            e_armed_RS_turn_left_90_hash = Animator.StringToHash("e_armed_RS_turn_left_90");
            e_armed_RS_turn_right_90_hash = Animator.StringToHash("e_armed_RS_turn_right_90");
            e_armed_LS_turn_left_90_hash = Animator.StringToHash("e_armed_LS_turn_left_90");
            e_armed_LS_turn_right_90_hash = Animator.StringToHash("e_armed_LS_turn_right_90");

            e_armed_FW_RS_turn_left_90_hash = Animator.StringToHash("e_armed_FW_RS_turn_left_90");
            e_armed_FW_RS_turn_right_90_hash = Animator.StringToHash("e_armed_FW_RS_turn_right_90");
            e_armed_FW_LS_turn_left_90_hash = Animator.StringToHash("e_armed_FW_LS_turn_left_90");
            e_armed_FW_LS_turn_right_90_hash = Animator.StringToHash("e_armed_FW_LS_turn_right_90");

            e_armed_SW_RS_turn_left_90_hash = Animator.StringToHash("e_armed_SW_RS_turn_left_90");
            e_armed_SW_RS_turn_right_90_hash = Animator.StringToHash("e_armed_SW_RS_turn_right_90");
            e_armed_SW_LS_turn_left_90_hash = Animator.StringToHash("e_armed_SW_LS_turn_left_90");
            e_armed_SW_LS_turn_right_90_hash = Animator.StringToHash("e_armed_SW_LS_turn_right_90");

            e_armed_turn_left_90_hash = Animator.StringToHash("e_armed_turn_left_90");
            e_armed_turn_right_90_hash = Animator.StringToHash("e_armed_turn_right_90");

            e_unarmed_turn_left_90_hash = Animator.StringToHash("e_unarmed_turn_left_90");
            e_unarmed_turn_right_90_hash = Animator.StringToHash("e_unarmed_turn_right_90");

            e_aim_turn_left_45_hash = Animator.StringToHash("e_aim_turn_left_45");
            e_aim_turn_right_45_hash = Animator.StringToHash("e_aim_turn_right_45");

            e_armed_FW_turn_left_inplace_hash = Animator.StringToHash("e_armed_FW_turn_left_inplace");
            e_armed_FW_turn_right_inplace_hash = Animator.StringToHash("e_armed_FW_turn_right_inplace");
            e_armed_SW_turn_left_inplace_hash = Animator.StringToHash("e_armed_SW_turn_left_inplace");
            e_armed_SW_turn_right_inplace_hash = Animator.StringToHash("e_armed_SW_turn_right_inplace");

            e_armed_RS_turn_left_inplace_hash = Animator.StringToHash("e_armed_RS_turn_left_inplace");
            e_armed_RS_turn_right_inplace_hash = Animator.StringToHash("e_armed_RS_turn_right_inplace");
            e_armed_LS_turn_left_inplace_hash = Animator.StringToHash("e_armed_LS_turn_left_inplace");
            e_armed_LS_turn_right_inplace_hash = Animator.StringToHash("e_armed_LS_turn_right_inplace");

            e_armed_FW_RS_turn_left_inplace_hash = Animator.StringToHash("e_armed_FW_RS_turn_left_inplace");
            e_armed_FW_RS_turn_right_inplace_hash = Animator.StringToHash("e_armed_FW_RS_turn_right_inplace");
            e_armed_FW_LS_turn_left_inplace_hash = Animator.StringToHash("e_armed_FW_LS_turn_left_inplace");
            e_armed_FW_LS_turn_right_inplace_hash = Animator.StringToHash("e_armed_FW_LS_turn_right_inplace");

            e_armed_SW_RS_turn_left_inplace_hash = Animator.StringToHash("e_armed_SW_RS_turn_left_inplace");
            e_armed_SW_RS_turn_right_inplace_hash = Animator.StringToHash("e_armed_SW_RS_turn_right_inplace");
            e_armed_SW_LS_turn_left_inplace_hash = Animator.StringToHash("e_armed_SW_LS_turn_left_inplace");
            e_armed_SW_LS_turn_right_inplace_hash = Animator.StringToHash("e_armed_SW_LS_turn_right_inplace");

            e_armed_turn_left_inplace_hash = Animator.StringToHash("e_armed_turn_left_inplace");
            e_armed_turn_right_inplace_hash = Animator.StringToHash("e_armed_turn_right_inplace");

            e_unarmed_turn_left_inplace_hash = Animator.StringToHash("e_unarmed_turn_left_inplace");
            e_unarmed_turn_right_inplace_hash = Animator.StringToHash("e_unarmed_turn_right_inplace");

            e_aim_turn_left_inplace_hash = Animator.StringToHash("e_aim_turn_left_inplace");
            e_aim_turn_right_inplace_hash = Animator.StringToHash("e_aim_turn_right_inplace");
            #endregion

            #region Enemy Parry Recevied States.
            //Enemy Turning Animation State  ---------------------------------------------------------------------------------
            e_parry_received_start_r_hash = Animator.StringToHash("e_parry_received_start_r");

            #region Execution.
            e_axe_parryExecute_received_hash = Animator.StringToHash("e_axe_parryExecute_received");
            e_fist_parryExecute_received_hash = Animator.StringToHash("e_fist_parryExecute_received");
            e_gs_parryExecute_received_hash = Animator.StringToHash("e_gs_parryExecute_received");
            e_shield_parryExecute_received_hash = Animator.StringToHash("e_shield_parryExecute_received");
            e_strs_parryExecute_received_hash = Animator.StringToHash("e_strs_parryExecute_received");
            #endregion
            
            #endregion

            #region Enemy Aiming Animation States.
            //Enemy Aiming Animation State  ---------------------------------------------------------------------------------
            e_aim_start_hash = Animator.StringToHash("e_aim_start");
            e_aim_loop_hash = Animator.StringToHash("e_aim_loop");
            e_aim_quit_hash = Animator.StringToHash("e_aim_quit");
            #endregion

            #region Enemy Taunt Animation States.
            //Enemy Taunt Animation State  ----------------------------------------------------------------------------------
            e_taunt_1_hash = Animator.StringToHash("e_taunt_1");
            e_taunt_2_hash = Animator.StringToHash("e_taunt_2");
            e_taunt_3_hash = Animator.StringToHash("e_taunt_3");
            #endregion

            #region Enemy Throw Returnal Projectile Animation States.
            e_throw_ReturnalProjectile_start_hash = Animator.StringToHash("e_throw_ReturnalProjectile_start");
            e_throw_ReturnalProjectile_end_hash = Animator.StringToHash("e_throw_ReturnalProjectile_end");
            #endregion

            #region Enemy Power Weapon Animation States.
            //Enemy Power Weapon Animation State  ---------------------------------------------------------------------------
            e_javelin_pickup_hash = Animator.StringToHash("e_javelin_pickup");
            e_javelin_brokenBounceBack_hash = Animator.StringToHash("e_javelin_broken_bounceBack");
            #endregion
            
            #region Enemy Evolve Animation States.
            e_evolve_start_hash = Animator.StringToHash("e_evolve_start");
            e_evolve_end_hash = Animator.StringToHash("e_evolve_end");
            #endregion

            #region Damage Particle Attack Animation States.
            e_dp_area_attack_1_hash = Animator.StringToHash("e_dp_area_attack_1");
            e_dp_area_attack_2_hash = Animator.StringToHash("e_dp_area_attack_2");
            e_dp_area_attack_3_hash = Animator.StringToHash("e_dp_area_attack_3");
            e_dp_proj_attack_1_hash = Animator.StringToHash("e_dp_proj_attack_1");
            e_dp_proj_attack_2_hash = Animator.StringToHash("e_dp_proj_attack_2");
            e_dp_proj_attack_3_hash = Animator.StringToHash("e_dp_proj_attack_3");
            #endregion

            #region Enemy Execution Animation States.
            e_execution_opening_full_hash = Animator.StringToHash("e_execution_opening_full");
            #endregion

            #region Egil Intro Poses.
            egil_intro_pose_1_hash = Animator.StringToHash("egil_intro_pose_1");
            #endregion

            #region Egil Stamina Mod.
            egil_injured_revenge_attack_1_hash = Animator.StringToHash("egil_injured_revenge_attack_1");
            egil_injured_hash = Animator.StringToHash("egil_injured");
            #endregion

            #region Egil Kinematic Motion Attack Mod.
            egil_P3_KMJ_1stHalf_hash = Animator.StringToHash("egil_P3_KMJ_1stHalf");
            egil_P3_KMJ_2ndHalf_hash = Animator.StringToHash("egil_P3_KMJ_2ndHalf");
            egil_P3_KMJ_land_hash = Animator.StringToHash("egil_P3_KMJ_land");

            egil_P2_KMJ_1stHalf_hash = Animator.StringToHash("egil_P2_KMJ_1stHalf");
            egil_P2_KMJ_2ndHalf_hash = Animator.StringToHash("egil_P2_KMJ_2ndHalf");
            egil_P2_KMJ_land_hash = Animator.StringToHash("egil_P2_KMJ_land");
            #endregion

            #region Egil Second Phase Mod.
            egil_Taunt_Chain_1_hash = Animator.StringToHash("egil_Taunt_Chain_1");
            egil_1P_empty_override_hash = Animator.StringToHash("egil_1P_empty_override");
            egil_2P_empty_override_hash = Animator.StringToHash("egil_2P_empty_override");
            egil_3P_empty_override_hash = Animator.StringToHash("egil_3P_empty_override");
            egil_ChangePhase2_Chain_3_hash = Animator.StringToHash("egil_ChangePhase2_Chain_3");
            #endregion
            
            /// Player.

            #region Player Transition Parameter.
            //Player Transition Parameter  --------------------------------------------------------------------------------------
            p_IsNeglecting_hash = Animator.StringToHash("p_IsNeglecting");
            p_IsRunning_hash = Animator.StringToHash("p_IsRunning");
            p_IsSprinting_hash = Animator.StringToHash("p_IsSprinting");
            p_IsGrounded_hash = Animator.StringToHash("p_IsGrounded");
            p_IsBlocking_hash = Animator.StringToHash("p_IsBlocking");
            p_IsGetupReady_hash = Animator.StringToHash("p_IsGetupReady");
            p_IsTwoHanding_hash = Animator.StringToHash("p_IsTwoHanding");
            p_IsBonfireEnd_hash = Animator.StringToHash("p_IsBonfireEnd");
            p_IsLevelupBegin_hash = Animator.StringToHash("p_IsLevelupBegin");
            p_IsTriggerFullyHeld_hash = Animator.StringToHash("p_IsTriggerFullyHeld");
            p_IsAnimationJobFinished_hash = Animator.StringToHash("p_IsAnimationJobFinished");
            p_IsHandleIKJobFinished_hash = Animator.StringToHash("p_IsHandleIKJobFinished");
            #endregion

            #region Player Anim State Speed Multi.
            //Player Anim State Speed Multi  ------------------------------------------------------------------------------------
            p_HoldAttackSpeedMulti_hash = Animator.StringToHash("p_HoldAttackSpeedMulti");
            #endregion

            #region Player Base Layer.
            //Player Base Layer --------------------------------------------------------------------------------------

            /// Rh Locomotion.
            p_axe_locomotion_hash = Animator.StringToHash("p_axe_locomotion");
            p_fist_locomotion_hash = Animator.StringToHash("p_fist_locomotion");
            p_gs_locomotion_hash = Animator.StringToHash("p_gs_locomotion");
            p_shield_locomotion_hash = Animator.StringToHash("p_shield_locomotion");
            p_strs_locomotion_hash = Animator.StringToHash("p_strs_locomotion");
            p_bow_locomotion_hash = Animator.StringToHash("p_bow_locomotion");
            p_catalysis_locomotion_hash = Animator.StringToHash("p_catalysis_locomotion");

            /// Th Locomotion.
            p_axe_locomotion_th_hash = Animator.StringToHash("p_axe_locomotion_th");
            p_fist_locomotion_th_hash = Animator.StringToHash("p_fist_locomotion_th");
            p_gs_locomotion_th_hash = Animator.StringToHash("p_gs_locomotion_th");
            p_shield_locomotion_th_hash = Animator.StringToHash("p_shield_locomotion_th");
            p_strs_locomotion_th_hash = Animator.StringToHash("p_strs_locomotion_th");
            p_bow_locomotion_th_hash = Animator.StringToHash("p_bow_locomotion_th");
            p_catalysis_locomotion_th_hash = Animator.StringToHash("p_catalysis_locomotion_th");
            #endregion
            
            #region Player Chest Right Hand Override Layer
            //Player Chest Right Hand Override Layer ------------------------------------------------------------------------

            #region Sheath / UnSheath.
            /// Rh Unsheath.
            p_axe_unSheath_r_hash = Animator.StringToHash("p_axe_unSheath_r");
            p_gs_unSheath_r_hash = Animator.StringToHash("p_gs_unSheath_r");
            p_shield_unSheath_r_hash = Animator.StringToHash("p_shield_unSheath_r");
            p_strs_unSheath_r_hash = Animator.StringToHash("p_strs_unSheath_r");
            p_bow_unSheath_r_hash = Animator.StringToHash("p_bow_unSheath_r");
            p_catalysis_unSheath_r_hash = Animator.StringToHash("p_catalysis_unSheath_r");

            /// Rh Sheath.
            p_axe_sheath_r_hash = Animator.StringToHash("p_axe_sheath_r");
            p_gs_sheath_r_hash = Animator.StringToHash("p_gs_sheath_r");
            p_shield_sheath_r_hash = Animator.StringToHash("p_shield_sheath_r");
            p_strs_sheath_r_hash = Animator.StringToHash("p_strs_sheath_r");
            p_bow_sheath_r_hash = Animator.StringToHash("p_bow_sheath_r");
            p_catalysis_sheath_r_hash = Animator.StringToHash("p_catalysis_sheath_r");

            p_axe_sheath_r_backpack_hash = Animator.StringToHash("p_axe_sheath_r_backpack");
            p_gs_sheath_r_backpack_hash = Animator.StringToHash("p_gs_sheath_r_backpack");
            p_shield_sheath_r_backpack_hash = Animator.StringToHash("p_shield_sheath_r_backpack");
            p_strs_sheath_r_backpack_hash = Animator.StringToHash("p_strs_sheath_r_backpack");
            p_bow_sheath_r_backpack_hash = Animator.StringToHash("p_bow_sheath_r_backpack");
            p_catalysis_sheath_r_backpack_hash = Animator.StringToHash("p_catalysis_sheath_r_backpack");
            #endregion
            
            /// Item.
            p_item_throw_horizontal_hash = Animator.StringToHash("p_item_throw_horizontal");
            p_item_throw_high_hash = Animator.StringToHash("p_item_throw_horizontal");
            p_item_throw_mid_hash = Animator.StringToHash("p_item_throw_horizontal");
            p_item_throw_low_hash = Animator.StringToHash("p_item_throw_horizontal");

            /// Interactions.
            p_int_bonfire_ignite_hash = Animator.StringToHash("p_int_bonfire_ignite");
            #endregion

            #region Player Chest Left Hand Override Layer 
            //Player Chest Left Hand Override Layer ------------------------------------------------------------------------------

            #region Sheath / UnSheath.
            /// Lh Unsheath.
            p_axe_unSheath_l_hash = Animator.StringToHash("p_axe_unSheath_l");
            p_gs_unSheath_l_hash = Animator.StringToHash("p_gs_unSheath_l");
            p_shield_unSheath_l_hash = Animator.StringToHash("p_shield_unSheath_l");
            p_strs_unSheath_l_hash = Animator.StringToHash("p_strs_unSheath_l");
            p_bow_unSheath_l_hash = Animator.StringToHash("p_bow_unSheath_l");
            p_catalysis_unSheath_l_hash = Animator.StringToHash("p_catalysis_unSheath_l");

            /// Lh Sheath.
            p_axe_sheath_l_hash = Animator.StringToHash("p_axe_sheath_l");
            p_gs_sheath_l_hash = Animator.StringToHash("p_gs_sheath_l");
            p_shield_sheath_l_hash = Animator.StringToHash("p_shield_sheath_l");
            p_strs_sheath_l_hash = Animator.StringToHash("p_strs_sheath_l");
            p_bow_sheath_l_hash = Animator.StringToHash("p_bow_sheath_l");
            p_catalysis_sheath_l_hash = Animator.StringToHash("p_catalysis_sheath_l");

            p_axe_sheath_l_backpack_hash = Animator.StringToHash("p_axe_sheath_l_backpack");
            p_gs_sheath_l_backpack_hash = Animator.StringToHash("p_gs_sheath_l_backpack");
            p_shield_sheath_l_backpack_hash = Animator.StringToHash("p_shield_sheath_l_backpack");
            p_strs_sheath_l_backpack_hash = Animator.StringToHash("p_strs_sheath_l_backpack");
            p_bow_sheath_l_backpack_hash = Animator.StringToHash("p_bow_sheath_l_backpack");
            p_catalysis_sheath_l_backpack_hash = Animator.StringToHash("p_catalysis_sheath_l_backpack");
            #endregion

            #region Oppose1 Blocking Start / React.
            /// Oppose1 Blocking Start.
            p_axe_oppose1_blocking_start_hash = Animator.StringToHash("p_axe_oppose1_blocking_start");
            p_gs_oppose1_blocking_start_hash = Animator.StringToHash("p_gs_oppose1_blocking_start");
            p_shield_oppose1_blocking_start_hash = Animator.StringToHash("p_shield_oppose1_blocking_start");
            p_strs_oppose1_blocking_start_hash = Animator.StringToHash("p_strs_oppose1_blocking_start");

            /// Oppose1 Blocking React.
            p_axe_oppose1_blocking_react_hash = Animator.StringToHash("p_axe_oppose1_blocking_react");
            p_gs_oppose1_blocking_react_hash = Animator.StringToHash("p_gs_oppose1_blocking_react");
            p_shield_oppose1_blocking_react_hash = Animator.StringToHash("p_shield_oppose1_blocking_react");
            p_strs_oppose1_blocking_react_hash = Animator.StringToHash("p_strs_oppose1_blocking_react");
            #endregion
            #endregion

            #region Player Upper Body Override Layer
            //Player Upper Body Override Layer ------------------------------------------------------------------------------------

            #region Light2 Blocking Start / React.
            /// Light2 Blocking Start.
            p_axe_light2_blocking_start_hash = Animator.StringToHash("p_axe_light2_blocking_start");
            p_gs_light2_blocking_start_hash = Animator.StringToHash("p_gs_light2_blocking_start");
            p_shield_light2_blocking_start_hash = Animator.StringToHash("p_shield_light2_blocking_start");
            p_strs_light2_blocking_start_hash = Animator.StringToHash("p_strs_light2_blocking_start");

            /// Light2 Blocking React.
            p_axe_light2_blocking_react_hash = Animator.StringToHash("p_axe_light2_blocking_react");
            p_gs_light2_blocking_react_hash = Animator.StringToHash("p_gs_light2_blocking_react");
            p_shield_light2_blocking_react_hash = Animator.StringToHash("p_shield_light2_blocking_react");
            p_strs_light2_blocking_react_hash = Animator.StringToHash("p_strs_light2_blocking_react");
            #endregion

            #region Damaged.

            #region Axe.
            p_axe_1h_hit_small_f_hash = Animator.StringToHash("p_axe_1h_hit_small_f");
            p_axe_1h_hit_small_b_hash = Animator.StringToHash("p_axe_1h_hit_small_b");
            p_axe_1h_hit_small_l_hash = Animator.StringToHash("p_axe_1h_hit_small_l");
            p_axe_1h_hit_small_r_hash = Animator.StringToHash("p_axe_1h_hit_small_r");

            p_axe_2h_hit_small_f_hash = Animator.StringToHash("p_axe_2h_hit_small_f");
            p_axe_2h_hit_small_b_hash = Animator.StringToHash("p_axe_2h_hit_small_b");
            p_axe_2h_hit_small_l_hash = Animator.StringToHash("p_axe_2h_hit_small_l");
            p_axe_2h_hit_small_r_hash = Animator.StringToHash("p_axe_2h_hit_small_r");
            #endregion

            #region Shield.
            p_shield_1h_hit_small_f_hash = Animator.StringToHash("p_shield_1h_hit_small_f");
            p_shield_1h_hit_small_b_hash = Animator.StringToHash("p_shield_1h_hit_small_b");
            p_shield_1h_hit_small_l_hash = Animator.StringToHash("p_shield_1h_hit_small_l");
            p_shield_1h_hit_small_r_hash = Animator.StringToHash("p_shield_1h_hit_small_r");

            p_shield_2h_hit_small_f_hash = Animator.StringToHash("p_shield_2h_hit_small_f");
            p_shield_2h_hit_small_b_hash = Animator.StringToHash("p_shield_2h_hit_small_b");
            p_shield_2h_hit_small_l_hash = Animator.StringToHash("p_shield_2h_hit_small_l");
            p_shield_2h_hit_small_r_hash = Animator.StringToHash("p_shield_2h_hit_small_r");
            #endregion

            #region Fist.
            p_fist_1h_hit_small_f_hash = Animator.StringToHash("p_fist_1h_hit_small_f");
            p_fist_1h_hit_small_b_hash = Animator.StringToHash("p_fist_1h_hit_small_b");
            p_fist_1h_hit_small_l_hash = Animator.StringToHash("p_fist_1h_hit_small_l");
            p_fist_1h_hit_small_r_hash = Animator.StringToHash("p_fist_1h_hit_small_r");

            p_fist_2h_hit_small_f_hash = Animator.StringToHash("p_fist_2h_hit_small_f");
            p_fist_2h_hit_small_b_hash = Animator.StringToHash("p_fist_2h_hit_small_b");
            p_fist_2h_hit_small_l_hash = Animator.StringToHash("p_fist_2h_hit_small_l");
            p_fist_2h_hit_small_r_hash = Animator.StringToHash("p_fist_2h_hit_small_r");
            #endregion

            #endregion

            #region Fist Two Hand Sheath / UnSheath
            p_fist_th_sheath_hash = Animator.StringToHash("p_fist_th_sheath");
            p_fist_th_unSheath_hash = Animator.StringToHash("p_fist_th_unSheath");
            p_fist_th_sheath_fullBody_hash = Animator.StringToHash("p_fist_th_sheath_fullBody");
            p_fist_th_unSheath_fullBody_hash = Animator.StringToHash("p_fist_th_unSheath_fullBody");
            #endregion

            #region Two Handing Switch Weapon.
            p_switchToRh_hash = Animator.StringToHash("p_switchToRh");
            p_switchToLh_hash = Animator.StringToHash("p_switchToLh");
            p_passToRh_hash = Animator.StringToHash("p_passToRh");
            p_passToLh_hash = Animator.StringToHash("p_passToLh");
            #endregion

            p_item_vessel_empty_hash = Animator.StringToHash("p_item_vessel_empty");
            #endregion

            #region Player Full Body Left Hand Override Layer
            //Player Full Body Left Hand Override Layer ---------------------------------------------------------------------------

            #region Empty.
            p_empty_fullbody_lh_overide_hash = Animator.StringToHash("p_empty_fullbody_lh_overide");
            #endregion

            #region Oppose1 Blocking Break.
            /// Oppose1 Blocking Break.
            p_axe_oppose1_blocking_break_hash = Animator.StringToHash("p_axe_oppose1_blocking_break");
            p_gs_oppose1_blocking_break_hash = Animator.StringToHash("p_gs_oppose1_blocking_break");
            p_shield_oppose1_blocking_break_hash = Animator.StringToHash("p_shield_oppose1_blocking_break");
            p_strs_oppose1_blocking_break_hash = Animator.StringToHash("p_strs_oppose1_blocking_break");
            #endregion

            #region Interaction.
            p_int_pickup_up_hash = Animator.StringToHash("p_int_pickup_up");
            p_int_pickup_mid_hash = Animator.StringToHash("p_int_pickup_mid");
            p_int_pickup_down_hash = Animator.StringToHash("p_int_pickup_down");
            #endregion

            #endregion

            #region Player Full Body Right Hand Override Layer
            //Player Full Body Right Hand Override Layer --------------------------------------------------------------------------

            #region Empty.
            p_empty_fullbody_rh_overide_hash = Animator.StringToHash("p_empty_fullbody_rh_overide");
            #endregion

            #region Interaction.
            p_int_takeChest_hash = Animator.StringToHash("p_int_takeChest");
            #endregion

            #endregion

            #region Player Full Body Override Layer
            //Player Full Body Override Layer ---------------------------------------------------------------------------------------

            #region Empty.
            p_empty_fullBody_override_hash = Animator.StringToHash("p_empty_fullBody_override");
            #endregion

            #region Light2 Blocking Break.
            p_axe_light2_blocking_break_hash = Animator.StringToHash("p_axe_light2_blocking_break");
            p_gs_light2_blocking_break_hash = Animator.StringToHash("p_gs_light2_blocking_break");
            p_shield_light2_blocking_break_hash = Animator.StringToHash("p_shield_light2_blocking_break");
            p_strs_light2_blocking_break_hash = Animator.StringToHash("p_strs_light2_blocking_break");
            #endregion

            #region Heavy1 Charge Enchant.
            p_axe_heavy1_charge_enchant_hash = Animator.StringToHash("p_axe_heavy1_charge_enchant");
            #endregion

            #region Roll.
            p_axe_lockon_rolls_tree_hash = Animator.StringToHash("p_axe_lockon_rolls_tree");
            p_fist_1h_lockon_rolls_tree_hash = Animator.StringToHash("p_fist_1h_lockon_rolls_tree");
            p_fist_2h_lockon_rolls_tree_hash = Animator.StringToHash("p_fist_2h_lockon_rolls_tree");
            p_fist_evade_tree_hash = Animator.StringToHash("p_fist_evade_tree");
            p_gs_lockon_rolls_tree_hash = Animator.StringToHash("p_gs_lockon_rolls_tree");
            p_shield_lockon_rolls_tree_hash = Animator.StringToHash("p_shield_lockon_rolls_tree");
            p_strs_lockon_rolls_tree_hash = Animator.StringToHash("p_strs_lockon_rolls_tree");
            p_backstep_hash = Animator.StringToHash("p_backstep");
            #endregion

            #region Jump.
            p_unarmed_jump_start_hash = Animator.StringToHash("p_unarmed_jump_start");
            p_unarmed_fall_start_hash = Animator.StringToHash("p_unarmed_fall_start");
            p_unarmed_land_hash = Animator.StringToHash("p_unarmed_land");
            p_armed_jump_start_hash = Animator.StringToHash("p_armed_jump_start");
            p_armed_fall_start_hash = Animator.StringToHash("p_armed_fall_start");
            p_armed_land_hash = Animator.StringToHash("p_armed_land");
            #endregion

            #region Parry Received.
            p_parry_received_hash = Animator.StringToHash("p_parry_received");
            #endregion

            #region Sprinting.
            p_fist_sprint_start_hash = Animator.StringToHash("p_fist_sprint_start");
            p_light_sprint_start_hash = Animator.StringToHash("p_light_sprint_start");
            p_sprint_end_hash = Animator.StringToHash("p_sprint_end");
            #endregion

            #region Damaged.

            #region Axe.
            p_axe_1h_hit_big_f_hash = Animator.StringToHash("p_axe_1h_hit_big_f");
            p_axe_1h_hit_big_b_hash = Animator.StringToHash("p_axe_1h_hit_big_b");
            p_axe_1h_hit_big_l_hash = Animator.StringToHash("p_axe_1h_hit_big_l");
            p_axe_1h_hit_big_r_hash = Animator.StringToHash("p_axe_1h_hit_big_r");

            p_axe_1h_knockback_hash = Animator.StringToHash("p_axe_1h_knockback");
            p_axe_1h_death_hash = Animator.StringToHash("p_axe_1h_death");

            p_axe_2h_hit_big_f_hash = Animator.StringToHash("p_axe_2h_hit_big_f");
            p_axe_2h_hit_big_b_hash = Animator.StringToHash("p_axe_2h_hit_big_b");
            p_axe_2h_hit_big_l_hash = Animator.StringToHash("p_axe_2h_hit_big_l");
            p_axe_2h_hit_big_r_hash = Animator.StringToHash("p_axe_2h_hit_big_r");

            p_axe_2h_knockback_hash = Animator.StringToHash("p_axe_2h_knockback");
            p_axe_2h_death_hash = Animator.StringToHash("p_axe_2h_death");
            #endregion

            #region Shield.
            p_shield_1h_hit_big_f_hash = Animator.StringToHash("p_shield_1h_hit_big_f");
            p_shield_1h_hit_big_b_hash = Animator.StringToHash("p_shield_1h_hit_big_b");
            p_shield_1h_hit_big_l_hash = Animator.StringToHash("p_shield_1h_hit_big_l");
            p_shield_1h_hit_big_r_hash = Animator.StringToHash("p_shield_1h_hit_big_r");

            p_shield_1h_knockback_hash = Animator.StringToHash("p_shield_1h_knockback");
            p_shield_1h_death_hash = Animator.StringToHash("p_shield_1h_death");
            
            p_shield_2h_hit_big_f_hash = Animator.StringToHash("p_shield_2h_hit_big_f");
            p_shield_2h_hit_big_b_hash = Animator.StringToHash("p_shield_2h_hit_big_b");
            p_shield_2h_hit_big_l_hash = Animator.StringToHash("p_shield_2h_hit_big_l");
            p_shield_2h_hit_big_r_hash = Animator.StringToHash("p_shield_2h_hit_big_r");

            p_shield_2h_knockback_hash = Animator.StringToHash("p_shield_2h_knockback");
            p_shield_2h_death_hash = Animator.StringToHash("p_shield_2h_death");
            #endregion

            #region Fist.
            p_fist_1h_hit_big_f_hash = Animator.StringToHash("p_fist_1h_hit_big_f");
            p_fist_1h_hit_big_b_hash = Animator.StringToHash("p_fist_1h_hit_big_b");
            p_fist_1h_hit_big_l_hash = Animator.StringToHash("p_fist_1h_hit_big_l");
            p_fist_1h_hit_big_r_hash = Animator.StringToHash("p_fist_1h_hit_big_r");

            p_fist_1h_knockback_hash = Animator.StringToHash("p_fist_1h_knockback");
            p_fist_1h_death_hash = Animator.StringToHash("p_fist_1h_death");

            p_fist_2h_hit_big_f_hash = Animator.StringToHash("p_fist_2h_hit_big_f");
            p_fist_2h_hit_big_b_hash = Animator.StringToHash("p_fist_2h_hit_big_b");
            p_fist_2h_hit_big_l_hash = Animator.StringToHash("p_fist_2h_hit_big_l");
            p_fist_2h_hit_big_r_hash = Animator.StringToHash("p_fist_2h_hit_big_r");

            p_fist_2h_knockback_hash = Animator.StringToHash("p_fist_2h_knockback");
            p_fist_2h_death_hash = Animator.StringToHash("p_fist_2h_death");
            #endregion

            #endregion
            
            #region Interaction.
            p_int_bonfire_start_hash = Animator.StringToHash("p_int_bonfire_start");

            p_int_cantOpen_hash = Animator.StringToHash("p_int_cantOpen");
            p_int_openDoor_hash = Animator.StringToHash("p_int_openDoor");
            p_int_openChest_hash = Animator.StringToHash("p_int_openChest");

            p_int_levelup_end_hash = Animator.StringToHash("p_int_levelup_end");
            #endregion

            #region Parry Execution.
            p_axe_parryExecute_present_hash = Animator.StringToHash("p_axe_parryExecute_present");
            p_fist_parryExecute_present_hash = Animator.StringToHash("p_fist_parryExecute_present");
            p_gs_parryExecute_present_hash = Animator.StringToHash("p_gs_parryExecute_present");
            p_shield_parryExecute_present_hash = Animator.StringToHash("p_shield_parryExecute_present");
            p_strs_parryExecute_present_hash = Animator.StringToHash("p_strs_parryExecute_present");
            #endregion

            #region Getup.

            #region Axe.
            p_axe_1h_getup_faceUp_hash = Animator.StringToHash("p_axe_1h_getup_faceUp");
            p_axe_1h_getup_faceDown_hash = Animator.StringToHash("p_axe_1h_getup_faceDown");
            p_axe_2h_getup_faceUp_hash = Animator.StringToHash("p_axe_2h_getup_faceUp");
            p_axe_2h_getup_faceDown_hash = Animator.StringToHash("p_axe_2h_getup_faceDown");
            #endregion

            #region Shield.
            p_shield_1h_getup_faceUp_hash = Animator.StringToHash("p_shield_1h_getup_faceUp");
            p_shield_1h_getup_faceDown_hash = Animator.StringToHash("p_shield_1h_getup_faceDown");
            p_shield_2h_getup_faceUp_hash = Animator.StringToHash("p_shield_2h_getup_faceUp");
            p_shield_2h_getup_faceDown_hash = Animator.StringToHash("p_shield_2h_getup_faceDown");
            #endregion

            #region Fist.
            p_fist_1h_getup_faceUp_hash = Animator.StringToHash("p_fist_1h_getup_faceUp");
            p_fist_1h_getup_faceDown_hash = Animator.StringToHash("p_fist_1h_getup_faceDown");
            p_fist_2h_getup_faceUp_hash = Animator.StringToHash("p_fist_2h_getup_faceUp");
            p_fist_2h_getup_faceDown_hash = Animator.StringToHash("p_fist_2h_getup_faceDown");
            #endregion

            #endregion

            #region Revive.

            #region Axe.
            p_axe_revive_hash = Animator.StringToHash("p_axe_revive");
            #endregion

            #region Shield.
            p_shield_revive_hash = Animator.StringToHash("p_shield_revive");
            #endregion

            #region Fist.
            p_fist_revive_hash = Animator.StringToHash("p_fist_revive");
            #endregion

            #endregion
            
            #endregion
        }

        public void GenerateHashForVariables(AnimStateVariable[] enemyStateArray, AnimStateVariable[] playerStateArray)
        {
            int enemyArrayLength = enemyStateArray.Length;
            for (int i = 0; i < enemyArrayLength; i++)
            {
                enemyStateArray[i].animStateHash = Animator.StringToHash(enemyStateArray[i].animStateName);
            }

            int playerArrayLength = playerStateArray.Length;
            for (int i = 0; i < playerArrayLength; i++)
            {
                playerStateArray[i].animStateHash = Animator.StringToHash(playerStateArray[i].animStateName);
            }
        }
    }

    [Serializable]
    public class AnimStateVariables
    {
        [HideInInspector] public bool showFoldout;

        public AnimStateVariable[] enemyAnimStates;

        public AnimStateVariable[] playerAnimStates;
    }
}