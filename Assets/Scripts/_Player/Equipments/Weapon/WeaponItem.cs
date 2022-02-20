using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class WeaponItem : Item
    {
        #region Item Stats.
        [Header("Weapon Item Stats.")]
        public P_Weapon_WeaponTypeEnum weaponType;
        public P_Weapon_ElementTypeEnum weaponMainElementType;
        //public float weight;

        [Header("Guard Absorptions.")]
        public int _guardAbsorpValue;
        public int stability;
        
        [Header("Attribute Bonus% .")]
        /// One of the major factor for BNS DMG caculation.
        /// S : 101%
        /// A : 81% - 100%
        /// B : 61% - 80%
        /// C : 45% - 60%
        /// D : 1% - 44%
        /// - : 0%
        [Range(0, 101)] public int mainAtkAttriScaling;

        //[Header("Attribute Requirement.")]
        //public int strength_require;
        //public int dexterity_require;
        //public int intelligence_require;
        //public int faith_require;

        [Header("Execution damage section.")]
        public Player_ExecutionProfile _executionProfile;
        public bool _alertEnemyFromBeginning;

        [Header("Runtime Weapon Vanilla Stats.")]
        public WeaponVanillaStats weaponVanillaStats;
        #endregion

        #region Weapon Action Sprites.
        [Header("1 Handed Sprite.")]
        public Sprite _1HandedRbActionSprite;
        public Sprite _1HandedRtActionSprite;
        public Sprite _1HandedLbActionSprite;
        public Sprite _1HandedLtActionSprite;

        [Header("2 Handed Sprite.")]
        public Sprite _2HandedRbActionSprite;
        public Sprite _2HandedRtActionSprite;
        public Sprite _2HandedLbActionSprite;
        public Sprite _2HandedLtActionSprite;
        #endregion

        #region Dissolve Values.
        [Header("Dissolve Config.")]
        public float _dissolveSpeed;
        public float _cutOffFullOpaqueValue;
        public float _cutOffFullTransparentValue;
        #endregion
        
        #region Drag and Drop Refs.
        [Header("Runtime Prefab.")]
        public RuntimeWeapon modelPrefab;

        [Header("Transform Datas.")]
        public WeaponOpposedHandTransform opposedHandTransform;
        public PlayerSheathTransform weaponSheathTransform;
        public PlayerSheathTransform opposedSheathTransform;

        [Header("Hold Loop Profile.")]
        public WA_Effect_Profile hold_loop_effect_profile;
        #endregion

        #region Init.
        public RuntimeWeapon GetNewRuntimeWeaponInstance(SavableInventory _inventory)
        {
            RuntimeWeapon _runtimeWeapon = Instantiate(modelPrefab);
            _runtimeWeapon.InitRuntimeWeapon(this, _inventory._states);

            _inventory.ReturnWeaponToBackpack(_runtimeWeapon);
            _inventory.AddWeaponToCarrying(_runtimeWeapon);
            return _runtimeWeapon;
        }

        public RuntimeWeapon GetSaveFileRuntimeWeaponInstance(SavableWeaponState _savableWeaponState, SavableInventory _inventory)
        {
            RuntimeWeapon _runtimeWeapon = Instantiate(modelPrefab);
            _runtimeWeapon.InitRuntimeWeaponFromSave(_savableWeaponState, _inventory._states, this);

            _inventory.AddWeaponToCarrying(_runtimeWeapon);
            return _runtimeWeapon;
        }

        public void InitUnarmedItem(SavableInventory _inventory)
        {
            RuntimeWeapon _runtimeUnarmed = Instantiate(modelPrefab);
            _runtimeUnarmed.InitRuntimeUnarmed(this, _inventory._states);

            _inventory.InitUnarmedDefault(_runtimeUnarmed);
        }
        #endregion

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            GetNewRuntimeWeaponInstance(_inventory);
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeWeaponInstance(_inventory);
        }
        #endregion

        #region Abstract.

        #region Weapon Input.
        public abstract void HandleDominantHandWeaponInput(StateManager _states);

        public abstract void HandleOpposeHandWeaponInput(StateManager _states);

        public abstract void HandleTwoHandingWeaponInput(StateManager _states);
        #endregion

        #region Weapon Action.
        public abstract bool HandleDominantHandWeaponAction(StateManager _states);

        public abstract bool HandleOpposeHandWeaponAction(StateManager _states);

        public abstract bool HandleTwoHandingWeaponAction(StateManager _states);
        #endregion

        #endregion

        #region AnimStates Hash Methods.

        #region Idles.
        public int GetRhLocomotionHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_locomotion_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_locomotion_hash;
                default:
                    return 0;
            }
        }

        public int GetThLocomotionHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_locomotion_th_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_locomotion_th_hash;
                default:
                    return 0;
            }
        }
        #endregion

        #region Sprints.
        public int GetSprintStartHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_sprint_start_hash;
                case P_Weapon_WeaponTypeEnum.Axe:
                case P_Weapon_WeaponTypeEnum.Shield:
                case P_Weapon_WeaponTypeEnum.StraightSword:
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_light_sprint_start_hash;
                default:
                    return 0;
            }
        }
        #endregion

        #region Blocking.

        #region Oppose 1.
        public int GetOppose1BlockingHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_oppose1_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_oppose1_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_oppose1_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_oppose1_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int GetOppose1BlockingReactHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_oppose1_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_oppose1_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_oppose1_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_oppose1_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int GetOppose1BlockingBreakHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_oppose1_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_oppose1_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_oppose1_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_oppose1_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region Light 2.
        public int GetLight2BlockingHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_light2_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_light2_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_light2_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_light2_blocking_start_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int GetLight2BlockingReactHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_light2_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_light2_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_light2_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_light2_blocking_react_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int GetLight2BlockingBreakHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_light2_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_light2_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_light2_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_light2_blocking_break_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #endregion

        #region Sheath / UnSheath.
        public int GetRhUnSheathHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_unSheath_r_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_unSheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_unSheath_r_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_unSheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_unSheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_unSheath_r_hash;
                default:
                    return 0;
            }
        }

        public int GetRhSheathHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_sheath_r_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_sheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_sheath_r_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_sheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_sheath_r_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_sheath_r_hash;
                default:
                    return 0;
            }
        }

        public int GetRhSheathBackpackHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_sheath_r_backpack_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_sheath_r_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_sheath_r_backpack_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_sheath_r_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_sheath_r_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_sheath_r_backpack_hash;
                default:
                    return 0;
            }
        }

        public int GetLhUnSheathHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_unSheath_l_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_unSheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_unSheath_l_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_unSheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_unSheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_unSheath_l_hash;
                default:
                    return 0;
            }
        }

        public int GetLhSheathHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_sheath_l_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_sheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_sheath_l_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_sheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_sheath_l_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_sheath_l_hash;
                default:
                    return 0;
            }
        }

        public int GetLhSheathBackpackHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_sheath_l_backpack_hash;

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_sheath_l_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_sheath_l_backpack_hash;

                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_sheath_l_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Bow:
                    return HashManager.singleton.p_bow_sheath_l_backpack_hash;

                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return HashManager.singleton.p_catalysis_sheath_l_backpack_hash;
                default:
                    return 0;
            }
        }
        #endregion

        #region Damages.

        #region 1H Small.
        public int Get_OneHanded_Small_Hit_F_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Small_Hit_B_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Small_Hit_L_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Small_Hit_R_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region 1H Big.
        public int Get_OneHanded_Big_Hit_F_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Big_Hit_B_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Big_Hit_L_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_Big_Hit_R_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region 1H Knockback.
        public int Get_OneHanded_Knockback_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region 2H Small.
        public int Get_TwoHanded_Small_Hit_F_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_small_f_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Small_Hit_B_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_small_b_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Small_Hit_L_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_small_l_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Small_Hit_R_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_small_r_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region 2H Big.
        public int Get_TwoHanded_Big_Hit_F_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_big_f_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Big_Hit_B_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_big_b_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Big_Hit_L_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_big_l_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Big_Hit_R_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_hit_big_r_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region 2H Knockback.
        public int Get_TwoHanded_Knockback_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_knockback_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #endregion

        #region Death.
        public int Get_OneHanded_Death_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_1h_death_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_1h_death_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_1h_death_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_Death_HashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_2h_death_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_2h_death_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_2h_death_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region Revive.
        public int GetReviveHashByType()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_revive_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return HashManager.singleton.p_fist_revive_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_revive_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region Parry Execute.
        public int GetParryExecutePresentHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_parryExecute_present_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.p_fist_parryExecute_present_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return _hashManager.p_gs_parryExecute_present_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.p_shield_parryExecute_present_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return _hashManager.p_strs_parryExecute_present_hash;
                default:
                    return 0;
            }
        }

        public int GetParryExecuteReceivedHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.e_axe_parryExecute_received_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.e_fist_parryExecute_received_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return _hashManager.e_gs_parryExecute_received_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.e_shield_parryExecute_received_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return _hashManager.e_strs_parryExecute_received_hash;
                default:
                    return 0;
            }
        }
        #endregion

        #region Getup.
        public int Get_OneHanded_FaceUp_GetupHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_1h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.p_fist_1h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.p_shield_1h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_OneHanded_FaceDown_GetupHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_1h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.p_fist_1h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.p_shield_1h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_FaceUp_GetupHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_2h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.p_fist_2h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.p_shield_2h_getup_faceUp_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        public int Get_TwoHanded_FaceDown_GetupHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_2h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return _hashManager.p_fist_2h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return _hashManager.p_shield_2h_getup_faceDown_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #region Charge Enchant.
        public int Get_Heavy1_Charge_EnchantHashByType()
        {
            HashManager _hashManager = HashManager.singleton;

            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return _hashManager.p_axe_heavy1_charge_enchant_hash;
                case P_Weapon_WeaponTypeEnum.Fist:
                    return 0;
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return 0;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }
        #endregion

        #endregion

        #region Enums.
        public enum WeaponGuardAbsorpTypeEnum
        {
            Physical,
            Magical,
            Fire,
            Lightning,
            Dark
        }
        #endregion

        #region Virtual - Get Hold ATK Profiles.
        public virtual WA_Effect_Profile Get_Hold_HalfComp_Effect()
        {
            return null;
        }

        public virtual WA_Effect_Profile Get_Hold_FullComp_Effect()
        {
            return null;
        }
        #endregion

        #region Virtual - Get Charge ATK Profiles.
        public virtual WA_Effect_Profile Get_ChargeEnchant_Effect()
        {
            return null;
        }

        public virtual WA_Effect_Profile Get_ChargeAttack_Effect()
        {
            return null;
        }
        #endregion

        #region Melee Info Detail.
        public string GetSpecialAbilityText()
        {
            switch (weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return "Warcry";
                case P_Weapon_WeaponTypeEnum.Fist:
                    return "Fighter Mode";
                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return "";
                case P_Weapon_WeaponTypeEnum.Shield:
                    return "Parry";
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return "";
                case P_Weapon_WeaponTypeEnum.Bow:
                    return "";
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return "";
                default:
                    return "";
            }
        }

        public string GetMainAtkAttriScalingText()
        {
            if (mainAtkAttriScaling > 61)
            {
                if (mainAtkAttriScaling > 81)
                {
                    if (mainAtkAttriScaling > 101)
                    {
                        return "S";
                    }
                    else
                    {
                        return "A";
                    }
                }
                else
                {
                    return "B";
                }
            }
            else
            {
                if (mainAtkAttriScaling > 45)
                {
                    return "C";
                }
                else
                {
                    return "D";
                }
            }
        }
        #endregion

        public override WeaponItem GetWeaponItem()
        {
            return this;
        }
    }

    [Serializable]
    public class WeaponVanillaStats
    {
        [Header("General.")]
        /// Refine : When Physical Attack Type matches enemy's weakness, Att Power Plus Extra 8% 
        /// Magical : Normally add 5% Att Power to Magical.
        /// Fire : Normally add 5% Att Power to Fire.
        /// Lightning : Normally add 5% Att Power to Lightning.
        /// Dark : Normally add 5% Att Power to Dark.
        public P_Weapon_InfusedElementTypeEnum infusedElementType;
        public float durability;

        [Header("Attack Powers.")]
        public int mainAtkPower;
        public int criticalAtkPower;
        public int range;
        public int spellBuff;

        [Header("Additional Effects.")]
        public int bleed_effect;
        public int poison_effect;
        public int frost_effect;
    }
    
    public enum P_Weapon_WeaponTypeEnum
    {
        Axe,
        Fist,
        GreatSword,
        Shield,
        StraightSword,
        Bow,
        Catalysts,
    }

    public enum P_Weapon_ElementTypeEnum
    {
        Physical,
        Magic,
        Fire,
        Lightning,
        Dark
    }

    public enum P_Weapon_InfusedElementTypeEnum
    {
        None,
        Refined,
        Magic,
        Fire,
        Lightning,
        Dark,
    }

    /// Not In Use.
    //public enum PhysicalAttackDetailTypeEnum
    //{
    //    Strike,
    //    Slash,
    //    Thrust,
    //    Strike_Slash,
    //    Strike_Thrust,
    //    Slash_Thrust,
    //}
}