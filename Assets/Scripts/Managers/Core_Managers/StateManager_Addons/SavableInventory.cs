using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

namespace SA
{
    [Serializable]
    public class SavableInventory
    {
        #region Inventory Runtimes.
        [Header("Inventory Runtimes.")]
        // Weapons
        [ReadOnlyInspector] public RuntimeWeapon[] rightHandSlots = new RuntimeWeapon[3];
        [ReadOnlyInspector] public RuntimeWeapon[] leftHandSlots = new RuntimeWeapon[3];
        [ReadOnlyInspector] public RuntimeArrow[] arrowSlots = new RuntimeArrow[4];
        // Armors
        [ReadOnlyInspector] public RuntimeHeadArmor headArmorSlot;
        [ReadOnlyInspector] public RuntimeChestArmor chestArmorSlot;
        [ReadOnlyInspector] public RuntimeHandArmor handArmorSlot;
        [ReadOnlyInspector] public RuntimeLegArmor legArmorSlot;
        // Rings
        [ReadOnlyInspector] public RuntimeRing rightRingSlot;
        [ReadOnlyInspector] public RuntimeRing leftRingSlot;
        // Charm
        [ReadOnlyInspector] public RuntimeCharm charmSlot;
        // Powerup
        [ReadOnlyInspector] public RuntimePowerup powerupSlot;
        // Consumables
        [ReadOnlyInspector] public RuntimeConsumable[] consumableSlots = new RuntimeConsumable[10];
        // Spells
        [ReadOnlyInspector] public SpellItem[] spellSlots = new SpellItem[4];
        #endregion

        #region Default Runtimes.
        [Header("Default Runtimes.")]
        // Weapons
        [ReadOnlyInspector] public RuntimeWeapon runtimeUnarmed;
        // Armors
        [ReadOnlyInspector] public RuntimeHeadArmor runtimeDeprivedHead;
        [ReadOnlyInspector] public RuntimeChestArmor runtimeDeprivedChest;
        [ReadOnlyInspector] public RuntimeHandArmor runtimeDeprivedHand;
        [ReadOnlyInspector] public RuntimeLegArmor runtimeDeprivedLeg;
        // Consumables
        [ReadOnlyInspector] public VolunVesselConsumable runtimeVolunVessel;
        [ReadOnlyInspector] public SodurVesselConsumable runtimeSodurVessel;
        // Amulet
        [ReadOnlyInspector] public RuntimeVolunAmulet runtimeAmulet;
        #endregion

        #region Current Runtimes.
        [Header("Current Runtimes.")]
        [ReadOnlyInspector] public RuntimeWeapon _rightHandWeapon;
        [ReadOnlyInspector] public RuntimeWeapon _leftHandWeapon;
        [ReadOnlyInspector] public RuntimeWeapon _twoHandingWeapon;
        [ReadOnlyInspector] public RuntimeConsumable _consumable;
        [ReadOnlyInspector] public SpellItem _spell;
        #endregion

        #region Current Runtimes ReferedItems.
        [ReadOnlyInspector] public WeaponItem _rightHandWeapon_referedItem;
        [ReadOnlyInspector] public WeaponItem _leftHandWeapon_referedItem;
        [ReadOnlyInspector] public WeaponItem _twoHandingWeapon_referedItem;
        [ReadOnlyInspector] public ConsumableItem _consumable_referedItem;
        #endregion

        #region Refs
        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public AnimatorHook _a_hook;
        [ReadOnlyInspector] public PlayerIKHandler _playerIKHandler;
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        [ReadOnlyInspector] public StatusHub _statusHub;
        StringBuilder _inventoryStrBuilder;
        #endregion

        #region Drag and Drop Refs.
        [Header("Drag and Drop Refs.")]
        [SerializeField] ConsumableAction consumableAction = null;
        #endregion

        #region Status.
        [Header("Weapon Status.")]
        [ReadOnlyInspector] public bool _isRightUnarmed;
        [ReadOnlyInspector] public bool _isLeftUnarmed;
        [ReadOnlyInspector] public bool _isBothUnarmed;
        [ReadOnlyInspector] public bool _isWeaponCarryingFilled;
        [ReadOnlyInspector] public bool _isPowerupHideShield;
        [ReadOnlyInspector] public bool _isHidingBothCurrentWeapons;
        [ReadOnlyInspector] public bool _isHideCurrentVisibleShields;
        [ReadOnlyInspector] public bool _isShowCurrentVisibleShields;
        [ReadOnlyInspector] public bool _isHideCurrent_Lh_Shield;
        [ReadOnlyInspector] public bool _isShowCurrent_Lh_Shield;
        [ReadOnlyInspector] public bool _isHideCurrent_Rh_Shield;
        [ReadOnlyInspector] public bool _isShowCurrent_Rh_Shield;
        [ReadOnlyInspector] public bool _hasOffTwoHandingFistInMenu;
        [ReadOnlyInspector] public bool _isTwoHanding_Rh_Fist;
        [ReadOnlyInspector] public bool _isOnDeathDissolved_Visible_RH_Weapon;
        [ReadOnlyInspector] public bool _isOnDeathDissolved_Visible_LH_Weapon;
        [ReadOnlyInspector] public RuntimeWeapon _curVisibleLhWeapon;
        [ReadOnlyInspector] public RuntimeWeapon _curVisibleRhWeapon;
        [ReadOnlyInspector] public RuntimeWeapon _curWeaponToPass;
        [ReadOnlyInspector] public WeaponBackpackOperationTypeEnum _currentWeaponBackpackOperationType;

        [Header("Sheath Status.")]
        [ReadOnlyInspector] public List<RuntimeWeapon> _rhWeaponsToSheath;
        [ReadOnlyInspector] public List<RuntimeWeapon> _lhWeaponsToSheath;

        [Header("Arrow Status.")]
        [ReadOnlyInspector] public bool _isArrowCarryingFilled;

        [Header("Armor Status.")]
        [ReadOnlyInspector] public bool _isHeadCarryingFilled;
        [ReadOnlyInspector] public bool _isChestCarryingFilled;
        [ReadOnlyInspector] public bool _isHandCarryingFilled;
        [ReadOnlyInspector] public bool _isLegCarryingFilled;

        [Header("Charm Status.")]
        [ReadOnlyInspector] public bool _isCharmCarryingFilled;

        [Header("Powerup Status.")]
        [ReadOnlyInspector] public bool _isPowerupCarryingFilled;
        [ReadOnlyInspector] public RuntimePowerup _pickedUpReadyDissolvePowerup;

        [Header("Consumable Status.")]
        [ReadOnlyInspector] public bool _isUsingConsumable;
        [ReadOnlyInspector] public bool _isShow2ndNextConsumable;
        [ReadOnlyInspector] public bool _isShowNextConsumable;
        [ReadOnlyInspector] RuntimeConsumable _consumableToHide;
        [ReadOnlyInspector] public RuntimeConsumableEffect _currentConsumableEffect;

        [Header("Ring Status.")]
        [ReadOnlyInspector] public bool _isRingCarryingFilled;
        [ReadOnlyInspector] public Transform _ringFlyingTrailTransform;

        [Header("QSlot Index.")]
        [ReadOnlyInspector, SerializeField] int r_index = 0;
        [ReadOnlyInspector, SerializeField] int l_index = 0;
        [ReadOnlyInspector, SerializeField] int d_index = 0;
        [ReadOnlyInspector, SerializeField] int u_index = 0;
        #endregion

        #region Private.
        
        #region All Carrying Lists.
        /*[HideInInspector]*/ public List<RuntimeWeapon> allWeaponsPlayerCarrying = new List<RuntimeWeapon>();
        [HideInInspector] public List<RuntimeArrow> allArrowsPlayerCarrying = new List<RuntimeArrow>();

        [HideInInspector] public List<RuntimeHeadArmor> allHeadsPlayerCarrying = new List<RuntimeHeadArmor>();
        [HideInInspector] public List<RuntimeChestArmor> allChestsPlayerCarrying = new List<RuntimeChestArmor>();
        [HideInInspector] public List<RuntimeHandArmor> allHandsPlayerCarrying = new List<RuntimeHandArmor>();
        [HideInInspector] public List<RuntimeLegArmor> allLegsPlayerCarrying = new List<RuntimeLegArmor>();

        /*[HideInInspector]*/ public List<RuntimeRing> allRingsPlayerCarrying = new List<RuntimeRing>();

        [HideInInspector] public List<RuntimeCharm> allCharmsPlayerCarrying = new List<RuntimeCharm>();

        [HideInInspector] public List<RuntimePowerup> allPowerupsPlayerCarrying = new List<RuntimePowerup>();

        /*[HideInInspector]*/ public List<RuntimeConsumable> allConsumablesPlayerCarrying = new List<RuntimeConsumable>();
        #endregion

        #region All Carrying Dicts.
        // Dictionary should only be used when item is stackable, 
        // meaning that each comsumable should be a separate single instance and carrying amount value on them.
        [HideInInspector] public Dictionary<int, RuntimeConsumable> carryingConsumablesDict = new Dictionary<int, RuntimeConsumable>();
        [HideInInspector] public Dictionary<int, RuntimeArrow> carryingArrowsDict = new Dictionary<int, RuntimeArrow>();
        #endregion
        
        #region SlotLength.
        [HideInInspector]
        int rightHandWeaponSlotsLength = 0;
        [HideInInspector]
        int leftHandWeaponSlotsLength = 0;
        [HideInInspector]
        int arrowSlotsLength = 0;
        [HideInInspector]
        int consumableSlotsLength = 0;
        [HideInInspector]
        int spellSlotsLength = 0;
        #endregion

        #region Runtime Backpacks Parents.
        [HideInInspector] public Transform weaponBackpackTransform;
        [HideInInspector] public Transform arrowBackpackTransform;
        [HideInInspector] public Transform armorBackpackTransform;
        [HideInInspector] public Transform ringBackpackTransform;
        [HideInInspector] public Transform charmBackpackTransform;
        [HideInInspector] public Transform powerupBackpackTransform;
        [HideInInspector] public Transform consumableBackpackTransform;
        [HideInInspector] public Transform INV_ItemsEffectsBackpackTransform;
        #endregion

        public Action _showItemInfoCardAction; 

        #endregion

        #region ReadOnly Serialization Files.
        [Header("ReadOnly Serialization Files.")]
        /// Weapon.
        [HideInInspector] public List<SavableWeaponState> _savedWeaponStateList;
        [HideInInspector] public List<SavableArrowState> _savedArrowStateList;
        /// Armor.
        [HideInInspector] public List<SavableHeadState> _savedHeadStateList;
        [HideInInspector] public List<SavableChestState> _savedChestStateList;
        [HideInInspector] public List<SavableHandState> _savedHandStateList;
        [HideInInspector] public List<SavableLegState> _savedLegStateList;
        /// Charm.
        [HideInInspector] public List<SavableCharmState> _savedCharmStateList;
        /// Powerup.
        [HideInInspector] public List<SavablePowerupState> _savedPowerupStateList;
        /// Ring.
        [HideInInspector] public List<SavableRingState> _savedRingStateList;
        /// Consumable.
        [HideInInspector] public List<SavableStatsEffectConsumableState> _savedStatsEffectConsumableStateList;
        [HideInInspector] public List<SavableThrowableConsumableState> _savedThrowableConsumableStateList;
        #endregion

        /// -------------------------------------Methods

        #region Weapon.
        /// INIT DEFAULT.

        public void InitUnarmedDefault(RuntimeWeapon _runtimeUnarmed)
        {
            runtimeUnarmed = _runtimeUnarmed;
        }
        
        public void InitRhWeaponSlot(RuntimeWeapon _runtimeWeapon, int _weaponSlotNum)
        {
            if (!FindFirstRhWeaponFromSlots())
            {
                BringWeaponToRightSlot(_runtimeWeapon, _weaponSlotNum);
                RegisterRhCurrentWeapon(_runtimeWeapon);
                r_index = _weaponSlotNum;

                _playerIKHandler.InitRegisterRhWeaponIK();
                _mainHudManager.RegisterRhWeaponQSlot();
                SetIsRightUnarmedStatus(false);
                HandleWeaponsVisibility();
                UnSheathRhWeapon();
            }
            else
            {
                BringWeaponToRightSlot(_runtimeWeapon, _weaponSlotNum);
                SheathRhWeapon(_runtimeWeapon);
                HandleWeaponsVisibility();
            }
        }

        public void InitLhWeaponSlot(RuntimeWeapon _runtimeWeapon, int _weaponSlotNum)
        {
            if (!FindFirstLhWeaponFromSlots())
            {
                BringWeaponToLeftSlot(_runtimeWeapon, _weaponSlotNum);
                RegisterLhCurrentWeapon(_runtimeWeapon);
                l_index = _weaponSlotNum;

                _mainHudManager.RegisterLhWeaponQSlot();
                SetIsLeftUnarmedStatus(false);
                HandleWeaponsVisibility();
                UnSheathLhWeapon();
            }
            else
            {
                BringWeaponToLeftSlot(_runtimeWeapon, _weaponSlotNum);
                SheathLhWeapon(_runtimeWeapon);
                HandleWeaponsVisibility();
            }
        }

        /// INIT SAVED STATE SLOTS.
        public void InitSavedStateRhWeaponSlot(RuntimeWeapon _runtimeWeapon, int _weaponSlotNum)
        {
            if (_runtimeWeapon.isCurrentRhWeapon)
            {
                BringWeaponToRightSlot(_runtimeWeapon, _weaponSlotNum);
                RegisterRhCurrentWeapon(_runtimeWeapon);
                r_index = _weaponSlotNum;

                _playerIKHandler.InitRegisterRhWeaponIK();
                _mainHudManager.RegisterRhWeaponQSlot();
                SetIsRightUnarmedStatus(false);
                UnSheathRhWeapon();
            }
            else
            {
                BringWeaponToRightSlot(_runtimeWeapon, _weaponSlotNum);
                SheathRhWeapon(_runtimeWeapon);
            }
        }

        public void InitSavedStateLhWeaponSlot(RuntimeWeapon _runtimeWeapon, int _weaponSlotNum)
        {
            if (_runtimeWeapon.isCurrentLhWeapon)
            {
                BringWeaponToLeftSlot(_runtimeWeapon, _weaponSlotNum);
                RegisterLhCurrentWeapon(_runtimeWeapon);
                l_index = _weaponSlotNum;

                _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                _mainHudManager.RegisterLhWeaponQSlot();
                SetIsLeftUnarmedStatus(false);
                HandleWeaponsVisibility();
                UnSheathLhWeapon();
            }
            else
            {
                BringWeaponToLeftSlot(_runtimeWeapon, _weaponSlotNum);
                SheathLhWeapon(_runtimeWeapon);
                HandleWeaponsVisibility();
            }
        }

        /// SETUP EMPTY SLOTS.

        public void SetupRhWeaponEmptySlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            TryUnHide_All_Shields();

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                bool _isPlayerRhLocomotionNeeded = false;
                if (_overwriteWeapon == _twoHandingWeapon)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();
                    _isPlayerRhLocomotionNeeded = true;

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingLeftHandWeapon();
                }

                /// Overwrite Weapon Opposite Slot.
                EmptyLhWeaponSlot(_overwriteWeapon);

                /// Overwrite Weapon RH Slot.
                if (!FindFirstRhWeaponFromSlots())
                {
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterRhCurrentWeapon(_overwriteWeapon);
                    r_index = _weaponSlotNum;

                    _mainHudManager.RegisterRhWeaponQSlot();
                    SetIsRightUnarmedStatus(false);

                    if (!_states._isTwoHanding)
                    {
                        _playerIKHandler.RegisterRhWeaponIK();
                        _isPlayerRhLocomotionNeeded = true;
                    }
                    else
                    {
                        SheathRhWeapon(_overwriteWeapon);
                    }
                }
                else
                {
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);

                    ///* Delete this line of code if you don't want player to sheath _overwriteWeapon for the second time.
                    ///* First time is when 'EmptyLhWeaponSlot(_overwriteWeapon)'.
                    SheathRhWeapon(_overwriteWeapon);
                }

                if (_hasOffTwoHandingFistInMenu)
                {
                    if (!_isLeftUnarmed)
                        UnSheathLhWeapon();
                }

                if (_isPlayerRhLocomotionNeeded)
                {
                    UnSheathRhWeapon();
                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                }

                HandleWeaponsVisibility();
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                if (_overwriteWeapon == _twoHandingWeapon)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingRightHandWeapon();
                }

                rightHandSlots[_overwriteWeapon.slotNumber] = null;
                BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                
                if (!_hasOffTwoHandingFistInMenu)
                {
                    SheathRhWeapon(_overwriteWeapon);
                }
                else
                {
                    if (!_isLeftUnarmed)
                        UnSheathLhWeapon();
                }

                if (_overwriteWeapon.isCurrentRhWeapon)
                {
                    r_index = _weaponSlotNum;
                    UnSheathRhWeapon();
                }

                HandleWeaponsVisibility();
            }
            else
            {
                if (!FindFirstRhWeaponFromSlots() || _weaponSlotNum == r_index)
                {
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterRhCurrentWeapon(_overwriteWeapon);
                    r_index = _weaponSlotNum;

                    _mainHudManager.RegisterRhWeaponQSlot();
                    SetIsRightUnarmedStatus(false);
                    HandleWeaponsVisibility();

                    if (!_states._isTwoHanding)
                    {
                        _playerIKHandler.RegisterRhWeaponIK();

                        if (_hasOffTwoHandingFistInMenu)
                        {
                            if (!_isLeftUnarmed)
                                UnSheathLhWeapon();
                        }

                        UnSheathRhWeapon();
                        _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                    }
                    else
                    {
                        SheathRhWeapon(_overwriteWeapon);
                    }
                }
                else
                {
                    if (_hasOffTwoHandingFistInMenu)
                    {
                        if (!_isLeftUnarmed)
                            UnSheathLhWeapon();
                    }

                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    SheathRhWeapon(_overwriteWeapon);
                    HandleWeaponsVisibility();
                }
            }
            
            _states.OnSetupEmptyWeaponWait();
        }

        public void SetupLhWeaponEmptySlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            TryUnHide_All_Shields();

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                if (_overwriteWeapon == _twoHandingWeapon)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();
                    
                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingRightHandWeapon();
                }

                /// Overwrite Weapon Opposite Slot.
                EmptyRhWeaponSlot(_overwriteWeapon);

                /// Overwrite Weapon LH Slot.
                if (!FindFirstLhWeaponFromSlots())
                {
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterLhCurrentWeapon(_overwriteWeapon);
                    l_index = _weaponSlotNum;

                    _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                    _mainHudManager.RegisterLhWeaponQSlot();
                    SetIsLeftUnarmedStatus(false);

                    if (!_states._isTwoHanding)
                    {
                        if (_hasOffTwoHandingFistInMenu)
                        {
                            if (!_isRightUnarmed)
                                UnSheathRhWeapon();
                        }

                        UnSheathLhWeapon();
                    }
                }
                else
                {
                    if (_hasOffTwoHandingFistInMenu)
                    {
                        if (!_isRightUnarmed)
                            UnSheathRhWeapon();
                    }

                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    SheathLhWeapon(_overwriteWeapon);
                }
                
                HandleWeaponsVisibility();
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                if (_overwriteWeapon == _twoHandingWeapon)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();
                    
                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingLeftHandWeapon();
                }

                leftHandSlots[_overwriteWeapon.slotNumber] = null;
                BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);

                if (!_hasOffTwoHandingFistInMenu)
                {
                    SheathLhWeapon(_overwriteWeapon);
                }
                else
                {
                    if (!_isRightUnarmed)
                        UnSheathRhWeapon();
                }

                if (_overwriteWeapon.isCurrentLhWeapon)
                {
                    l_index = _weaponSlotNum;
                    UnSheathLhWeapon();
                }

                HandleWeaponsVisibility();
            }
            else
            {
                if (!FindFirstLhWeaponFromSlots() || _weaponSlotNum == l_index)
                {
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterLhCurrentWeapon(_overwriteWeapon);
                    l_index = _weaponSlotNum;

                    _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                    _mainHudManager.RegisterLhWeaponQSlot();
                    SetIsLeftUnarmedStatus(false);
                    HandleWeaponsVisibility();

                    if (!_states._isTwoHanding)
                    {
                        if (_hasOffTwoHandingFistInMenu)
                        {
                            if (!_isRightUnarmed)
                                UnSheathRhWeapon();
                        }

                        UnSheathLhWeapon();
                    }
                    else
                    {
                        SheathLhWeapon(_overwriteWeapon);
                    }
                }
                else
                {
                    if (_hasOffTwoHandingFistInMenu)
                    {
                        if (!_isRightUnarmed)
                            UnSheathRhWeapon();
                    }

                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    SheathLhWeapon(_overwriteWeapon);
                    HandleWeaponsVisibility();
                }
            }
            
            _states.OnSetupEmptyWeaponWait();
        }

        /// SETUP TAKEN SLOTS.

        // This is when player is Two Handing RH weapon and _overwrite is taking over the TwoHanding RightHand Slot.
        public void Setup_TH_RhWeaponTakenSlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            if (_overwriteWeapon == _twoHandingWeapon)
                return;
            
            TryUnHide_All_Shields();

            UnRegisterThCurrentWeapon();
            _states.OffTwoHandingWhenSetupSlot();

            /// UnRegister MainHud Two Handing.
            _mainHudManager.OffTwoHandingRightHandWeapon();

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                if (_overwriteWeapon.isCurrentLhWeapon)
                {
                    /// Overwrite Weapon.
                    _leftHandWeapon.isCurrentLhWeapon = false;

                    /// Taken Weapon.
                    _rightHandWeapon.isCurrentRhWeapon = false;
                    BringWeaponToLeftSlot(_rightHandWeapon, _overwriteWeapon.slotNumber);
                    RegisterLhCurrentWeapon(_rightHandWeapon);

                    _curWeaponToPass = _rightHandWeapon;
                    _a_hook.RegisterNewAnimationJob(_states.p_passToLh_hash, false);
                    _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, _states.vector3Zero);
                    
                    _mainHudManager.RegisterLhWeaponQSlot();
                }
                else
                {
                    BringWeaponToLeftSlot(_rightHandWeapon, _overwriteWeapon.slotNumber);
                    SheathLhWeapon(_rightHandWeapon);
                }

                EquipOverwriteWeaponRegular();
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                /// Overwrite Weapon.
                _rightHandWeapon.isCurrentRhWeapon = false;
                BringWeaponToRightSlot(_rightHandWeapon, _overwriteWeapon.slotNumber);

                SheathRhWeapon(_rightHandWeapon);

                EquipOverwriteWeaponRegular();
            }
            else
            {
                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                RequestSwitchCurrentInBackpack_AllRightHand(false, _overwriteWeapon);
                EquipOverwriteWeaponInAnim();
            }

            void EquipOverwriteWeaponInAnim()
            {
                BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);

                _states.OnSetupTakenWeaponWait();
                _mainHudManager.OffTwoHandingRightHandWeapon();
            }

            void EquipOverwriteWeaponRegular()
            {
                BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                RegisterRhCurrentWeapon(_overwriteWeapon);

                _playerIKHandler.RegisterRhWeaponIK();
                _mainHudManager.RegisterRhWeaponQSlot();
                HandleWeaponsVisibility();
                UnSheathRhWeapon();
                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);

                _states.OnSetupTakenWeaponWait();
                _mainHudManager.OffTwoHandingRightHandWeapon();
            }
        }

        // This is when player is Two Handing LH weapon and _overwrite is taking over the TwoHanding LeftHand Slot.
        public void Setup_TH_LhWeaponTakenSlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            if (_overwriteWeapon == _twoHandingWeapon)
                return;
            
            TryUnHide_All_Shields();

            UnRegisterThCurrentWeapon();
            _states.OffTwoHandingWhenSetupSlot();

            /// UnRegister MainHud Two Handing.
            _mainHudManager.OffTwoHandingLeftHandWeapon();

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                if (_overwriteWeapon.isCurrentRhWeapon)
                {
                    /// Overwrite Weapon.
                    _rightHandWeapon.isCurrentRhWeapon = false;

                    /// Taken Weapon.
                    _leftHandWeapon.isCurrentLhWeapon = false;
                    BringWeaponToRightSlot(_leftHandWeapon, _overwriteWeapon.slotNumber);
                    RegisterRhCurrentWeapon(_leftHandWeapon);

                    _playerIKHandler.RegisterRhWeaponIK();
                    _mainHudManager.RegisterRhWeaponQSlot();
                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                }
                else
                {
                    BringWeaponToRightSlot(_leftHandWeapon, _overwriteWeapon.slotNumber);
                    SheathRhWeapon(_leftHandWeapon);
                }

                EquipOverwriteWeaponRegular();
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                /// Overwrite Weapon.
                _leftHandWeapon.isCurrentLhWeapon = false;
                BringWeaponToLeftSlot(_leftHandWeapon, _overwriteWeapon.slotNumber);

                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                SheathRhWeapon(_leftHandWeapon);

                EquipOverwriteWeaponRegular();
            }
            else
            {
                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                RequestSwitchCurrentInBackpack_AllRightHand(true, _overwriteWeapon);
                EquipOverwriteWeaponInAnim();
            }

            void EquipOverwriteWeaponInAnim()
            {
                BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);

                _states.OnSetupTakenWeaponWait();
                _mainHudManager.OffTwoHandingLeftHandWeapon();
            }

            void EquipOverwriteWeaponRegular()
            {
                BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                RegisterLhCurrentWeapon(_overwriteWeapon);

                _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                _mainHudManager.RegisterLhWeaponQSlot();
                HandleWeaponsVisibility();
                UnSheathLhWeapon();

                _states.OnSetupTakenWeaponWait();
                _mainHudManager.OffTwoHandingLeftHandWeapon();
            }
        }

        public void SetupRhWeaponTakenSlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            /// Taken Weapon can't be two handing weapon, because if it is it already falls to another catergory.
            RuntimeWeapon _takenSlotWeapon = rightHandSlots[_weaponSlotNum];

            if (_takenSlotWeapon == _overwriteWeapon)
                return;

            TryUnHide_All_Shields();

            int _overwriteWeaponSlotNumber = _overwriteWeapon.slotNumber;
            bool _isOverWriteWeaponCurrentTwoHanding = _overwriteWeapon == _twoHandingWeapon ? true : false;
            bool _isSwitchRhCurrentToBackpackInAnim = false;

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                if (_isOverWriteWeaponCurrentTwoHanding)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingLeftHandWeapon();

                    /// Overwrite Slot Weapon.
                    _overwriteWeapon.isCurrentLhWeapon = false;
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterRhCurrentWeapon(_overwriteWeapon);
                    r_index = _weaponSlotNum;

                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);

                    /// Taken Slot Weapon.
                    _takenSlotWeapon.isCurrentRhWeapon = false;
                    BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                    RegisterLhCurrentWeapon(_takenSlotWeapon);

                    _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                    _mainHudManager.RegisterLhWeaponQSlot();
                    UnSheathLhWeapon();
                }
                else
                {
                    if (_overwriteWeapon.isCurrentLhWeapon)
                    {
                        if (_takenSlotWeapon.isCurrentRhWeapon)
                        {
                            UnRegisterRhCurrentWeapon_local();

                            /// Overwrite Slot Weapon.
                            PassLhWeaponToRightHand();

                            _overwriteWeapon.isCurrentLhWeapon = false;
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                            RegisterRhCurrentWeapon(_overwriteWeapon);
                            
                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentRhWeapon = false;
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterLhCurrentWeapon(_takenSlotWeapon);

                            _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                            _mainHudManager.RegisterLhWeaponQSlot();
                            UnSheathLhWeapon();
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }
                        else
                        {
                            /// Overwrite Slot Weapon.
                            SheathLhWeapon(_overwriteWeapon);

                            _overwriteWeapon.isCurrentLhWeapon = false;
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);

                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentRhWeapon = false;
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterLhCurrentWeapon(_takenSlotWeapon);

                            _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                            _mainHudManager.RegisterLhWeaponQSlot();
                            UnSheathLhWeapon();
                        }
                    }
                    else
                    {
                        if (_takenSlotWeapon.isCurrentRhWeapon)
                        {
                            /// Taken Slot Weapon.
                            UnRegisterRhCurrentWeapon_local();
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                            /// Overwrite Slot Weapon.
                            _overwriteWeapon.isCurrentRhWeapon = true;
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                            RegisterRhCurrentWeapon(_overwriteWeapon);
                            UnSheathRhWeapon();
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }
                        else
                        {
                            /// Overwrite Slot Weapon.
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                            SheathRhWeapon(_overwriteWeapon);

                            /// Taken Slot Weapon.
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                        }
                    }
                }
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                if (_isOverWriteWeaponCurrentTwoHanding)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingLeftHandWeapon();

                    /// Overwrite Slot Weapon.
                    _overwriteWeapon.isCurrentRhWeapon = false;
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);

                    SheathRhWeapon(_overwriteWeapon);

                    /// Taken Slot Weapon.
                    _takenSlotWeapon.isCurrentRhWeapon = true;
                    BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                    RegisterRhCurrentWeapon(_takenSlotWeapon);
                    UnSheathRhWeapon();
                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                }
                else
                {
                    if (_takenSlotWeapon.isCurrentRhWeapon)
                    {
                        /// Taken Slot Weapon.
                        _takenSlotWeapon.isCurrentRhWeapon = false;
                        BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                        if (!_hasOffTwoHandingFistInMenu)
                            SheathRhWeapon(_takenSlotWeapon);

                        /// Overwrite Slot Weapon.
                        _overwriteWeapon.isCurrentRhWeapon = true;
                        BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                        RegisterRhCurrentWeapon(_overwriteWeapon);
                        UnSheathRhWeapon();
                        _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                    }
                    else
                    {
                        if (_overwriteWeapon.isCurrentRhWeapon)
                        {
                            /// Overwrite Slot Weapon.
                            _overwriteWeapon.isCurrentRhWeapon = false;
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);

                            SheathRhWeapon(_overwriteWeapon);

                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentRhWeapon = true;
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterRhCurrentWeapon(_takenSlotWeapon);
                            UnSheathRhWeapon();
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }
                        else
                        {
                            /// Taken Slot Weapon.
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                            /// Overwrite Slot Weapon.
                            BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                        }
                    }
                }
            }
            else
            {
                /// Taken Slot Weapon.
                if (_takenSlotWeapon.isCurrentRhWeapon)
                {
                    _isSwitchRhCurrentToBackpackInAnim = true;
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    RequestSwitchCurrentInBackpack(false, _overwriteWeapon);
                }
                else
                {
                    ReturnWeaponToBackpack(_takenSlotWeapon);

                    /// Overwrite Slot Weapon.
                    BringWeaponToRightSlot(_overwriteWeapon, _weaponSlotNum);
                    SheathRhWeapon(_overwriteWeapon);
                }
            }

            if (!_isSwitchRhCurrentToBackpackInAnim)
            {
                _playerIKHandler.RegisterRhWeaponIK();
                _mainHudManager.RegisterRhWeaponQSlot();

                HandleWeaponsVisibility();
            }

            _states.OnSetupTakenWeaponWait();

            void UnRegisterRhCurrentWeapon_local()
            {
                SheathRhWeapon(_takenSlotWeapon);
                _takenSlotWeapon.isCurrentRhWeapon = false;
            }

            void PassLhWeaponToRightHand()
            {
                _curWeaponToPass = _leftHandWeapon;
                _a_hook.RegisterNewAnimationJob(_states.p_passToRh_hash, false);
                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, _states.vector3Zero);
            }
        }

        public void SetupLhWeaponTakenSlot(RuntimeWeapon _overwriteWeapon, int _weaponSlotNum)
        {
            /// Taken Weapon can't be two handing weapon, because if it is it already falls to another catergory.
            RuntimeWeapon _takenSlotWeapon = leftHandSlots[_weaponSlotNum];

            if (_takenSlotWeapon == _overwriteWeapon)
                return;

            TryUnHide_All_Shields();

            int _overwriteWeaponSlotNumber = _overwriteWeapon.slotNumber;
            bool _isOverWriteWeaponCurrentTwoHanding = _overwriteWeapon == _twoHandingWeapon ? true : false;
            bool _isSwitchLhCurrentToBackpackInAnim = false;

            if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right)
            {
                if (_isOverWriteWeaponCurrentTwoHanding)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingRightHandWeapon();

                    /// Overwrite Slot Weapon.
                    PassRhWeaponToLeftHand();

                    /// Overwrite Slot Weapon.
                    _overwriteWeapon.isCurrentRhWeapon = false;
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    RegisterLhCurrentWeapon(_overwriteWeapon);
                    l_index = _weaponSlotNum;
                    
                    /// Taken Slot Weapon.
                    _takenSlotWeapon.isCurrentLhWeapon = false;
                    BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                    RegisterRhCurrentWeapon(_takenSlotWeapon);

                    _playerIKHandler.RegisterRhWeaponIK();
                    _mainHudManager.RegisterRhWeaponQSlot();
                    UnSheathRhWeapon();
                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                }
                else
                {
                    if (_overwriteWeapon.isCurrentRhWeapon)
                    {
                        if (_takenSlotWeapon.isCurrentLhWeapon)
                        {
                            UnRegisterLhCurrentWeapon_local();

                            /// Overwrite Slot Weapon.
                            PassRhWeaponToLeftHand();

                            _overwriteWeapon.isCurrentRhWeapon = false;
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                            RegisterLhCurrentWeapon(_overwriteWeapon);

                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentLhWeapon = false;
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterRhCurrentWeapon(_takenSlotWeapon);

                            _playerIKHandler.RegisterRhWeaponIK();
                            _mainHudManager.RegisterRhWeaponQSlot();
                            UnSheathRhWeapon();
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }
                        else
                        {
                            /// Overwrite Slot Weapon.
                            SheathRhWeapon(_overwriteWeapon);

                            _overwriteWeapon.isCurrentRhWeapon = false;
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);

                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentLhWeapon = false;
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterRhCurrentWeapon(_takenSlotWeapon);

                            _playerIKHandler.RegisterRhWeaponIK();
                            _mainHudManager.RegisterRhWeaponQSlot();
                            UnSheathRhWeapon();
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }
                    }
                    else
                    {
                        if (_takenSlotWeapon.isCurrentLhWeapon)
                        {
                            /// Taken Slot Weapon.
                            UnRegisterLhCurrentWeapon_local();
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                            /// Overwrite Slot Weapon.
                            _overwriteWeapon.isCurrentLhWeapon = true;
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                            RegisterLhCurrentWeapon(_overwriteWeapon);
                            UnSheathLhWeapon();
                        }
                        else
                        {
                            /// Overwrite Slot Weapon.
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                            SheathLhWeapon(_overwriteWeapon);

                            /// Taken Slot Weapon.
                            BringWeaponToRightSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                        }
                    }
                }
            }
            else if (_overwriteWeapon.currentSlotSide == RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left)
            {
                if (_isOverWriteWeaponCurrentTwoHanding)
                {
                    /// Off Two Handing.
                    UnRegisterThCurrentWeapon();
                    _states.OffTwoHandingWhenSetupSlot();

                    /// UnRegister MainHud Two Handing.
                    _mainHudManager.OffTwoHandingLeftHandWeapon();

                    /// Overwrite Slot Weapon.
                    _overwriteWeapon.isCurrentLhWeapon = false;
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);

                    SheathLhWeapon(_overwriteWeapon);

                    /// Taken Slot Weapon.
                    _takenSlotWeapon.isCurrentLhWeapon = true;
                    BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                    RegisterLhCurrentWeapon(_takenSlotWeapon);
                    UnSheathLhWeapon();
                }
                else
                {
                    if (_takenSlotWeapon.isCurrentLhWeapon)
                    {
                        if (_takenSlotWeapon == _twoHandingWeapon)
                        {
                            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                        }

                        /// Taken Slot Weapon.
                        _takenSlotWeapon.isCurrentLhWeapon = false;
                        BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                        if (!_hasOffTwoHandingFistInMenu)
                            SheathLhWeapon(_takenSlotWeapon);

                        /// Overwrite Slot Weapon.
                        _overwriteWeapon.isCurrentLhWeapon = true;
                        BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                        RegisterLhCurrentWeapon(_overwriteWeapon);
                        UnSheathLhWeapon();
                    }
                    else
                    {
                        if (_overwriteWeapon.isCurrentLhWeapon)
                        {
                            /// Overwrite Slot Weapon.
                            _overwriteWeapon.isCurrentLhWeapon = false;
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);

                            SheathLhWeapon(_overwriteWeapon);

                            /// Taken Slot Weapon.
                            _takenSlotWeapon.isCurrentLhWeapon = true;
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);
                            RegisterLhCurrentWeapon(_takenSlotWeapon);
                            UnSheathLhWeapon();
                        }
                        else
                        {
                            /// Taken Slot Weapon.
                            BringWeaponToLeftSlot(_takenSlotWeapon, _overwriteWeaponSlotNumber);

                            /// Overwrite Slot Weapon.
                            BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                        }
                    }
                }
            }
            else
            {
                /// Taken Slot Weapon.
                if (_takenSlotWeapon.isCurrentLhWeapon)
                {
                    _isSwitchLhCurrentToBackpackInAnim = true;
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    RequestSwitchCurrentInBackpack(true, _overwriteWeapon);
                }
                else
                {
                    ReturnWeaponToBackpack(_takenSlotWeapon);

                    /// Overwrite Slot Weapon.
                    BringWeaponToLeftSlot(_overwriteWeapon, _weaponSlotNum);
                    SheathLhWeapon(_overwriteWeapon);
                }
            }

            if (!_isSwitchLhCurrentToBackpackInAnim)
            {
                _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                _mainHudManager.RegisterLhWeaponQSlot();

                HandleWeaponsVisibility();
            }

            _states.OnSetupTakenWeaponWait();

            void UnRegisterLhCurrentWeapon_local()
            {
                SheathLhWeapon(_takenSlotWeapon);
                _takenSlotWeapon.isCurrentLhWeapon = false;
            }

            void PassRhWeaponToLeftHand()
            {
                _curWeaponToPass = _rightHandWeapon;
                _a_hook.RegisterNewAnimationJob(_states.p_passToLh_hash, false);
                _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.NotUseIK, -1, _states.vector3Zero);
            }
        }

        /// EMPTY SLOTS.

        public void InMenu_EmptyLhSlot(int lhSlotNumber)
        {
            if (_states._isTwoHanding)
            {
                Empty_2H_LhWeaponSlotInMenu();
            }
            else
            {
                Empty_1H_LhWeaponSlotInMenu();
            }

            void Empty_2H_LhWeaponSlotInMenu()
            {
                bool _isTwoHandingCurrent = _twoHandingWeapon.isCurrentLhWeapon ? true : false;

                if (_states._isInTwoHandFist)
                {
                    /// If the current two handing weapon is fist, Shout it now.
                    _states.OffTwoHandingFistInMenuBeforeSetupSlot();
                }
                else
                {
                    /// If the current two handing weapon is current RH, Sheath the Weapon Now.
                    if (_isTwoHandingCurrent)
                    {
                        RequestSheathCurrentInBackpack_AllRightHand(true);
                        _states.OnRemove_TH_WeaponWait();
                    }

                    _states.OffTwoHandingWhenSetupSlot();
                }

                /// UnRegister MainHud RH Two Handing.
                _mainHudManager.OffTwoHandingLeftHandWeapon();

                /// If player has LH Weapon, he can UnSheath it now.
                if (_rightHandWeapon && !_isRightUnarmed)
                {
                    UnSheathRhWeapon();
                }
                
                /// If player is hiding the Rh shield, it can be shown in anim now.
                if (_isHideCurrent_Rh_Shield)
                {
                    _isHideCurrent_Rh_Shield = false;
                    _isShowCurrent_Rh_Shield = true;
                }
                /// Otherwise, check is hiding visible shields.
                else if (_isHideCurrentVisibleShields)
                {
                    _isHideCurrentVisibleShields = false;
                    _isShowCurrentVisibleShields = true;
                }
                
                /// Player can play Rh Locomotion again.
                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);

                if (!_isTwoHandingCurrent)
                {
                    RequestSheathWeaponInBackpack(true, leftHandSlots[lhSlotNumber]);
                    _states.OnRemoveWeaponWait();
                }

                /// Set Two Handed Weapon To Null.
                UnRegisterThCurrentWeapon();
                leftHandSlots[lhSlotNumber] = null;
            }

            void Empty_1H_LhWeaponSlotInMenu()
            {
                RuntimeWeapon _weaponToEmpty = leftHandSlots[lhSlotNumber];
                leftHandSlots[lhSlotNumber] = null;

                if (_weaponToEmpty.isCurrentLhWeapon)
                {
                    RequestSheathCurrentInBackpack(true);
                    _states.OnRemoveWeaponWait();
                }
                else
                {
                    RequestSheathWeaponInBackpack(true, _weaponToEmpty);
                }
            }
        }

        public void InMenu_EmptyRhSlot(int rhSlotNumber)
        {
            if (_states._isTwoHanding)
            {
                Empty_2H_RhWeaponSlotInMenu();
            }
            else
            {
                Empty_1H_RhWeaponSlotInMenu();
            }

            void Empty_2H_RhWeaponSlotInMenu()
            {
                bool _isTwoHandingCurrent = _twoHandingWeapon.isCurrentRhWeapon ? true : false;

                if (_states._isInTwoHandFist)
                {
                    /// If the current two handing weapon is fist, Shout it now.
                    _states.OffTwoHandingFistInMenuBeforeSetupSlot();
                }
                else
                {
                    /// If the current two handing weapon is current RH, Sheath the Weapon Now.
                    if (_isTwoHandingCurrent)
                    {
                        RequestSheathCurrentInBackpack_AllRightHand(false);
                        _states.OnSetupEmptyWeaponWait();
                    }

                    _states.OffTwoHandingWhenSetupSlot();
                }

                /// UnRegister MainHud RH Two Handing.
                _mainHudManager.OffTwoHandingRightHandWeapon();

                /// If player has LH Weapon, he can UnSheath it now.
                if (_leftHandWeapon && !_isLeftUnarmed)
                {
                    UnSheathLhWeapon();
                }

                /// If player is hiding the Lh shield, it can be shown in anim now.
                if (_isHideCurrent_Lh_Shield)
                {
                    _isHideCurrent_Lh_Shield = false;
                    _isShowCurrent_Lh_Shield = true;
                }
                /// Otherwise, check is hiding visible shields.
                else if (_isHideCurrentVisibleShields)
                {
                    _isHideCurrentVisibleShields = false;
                    _isShowCurrentVisibleShields = true;
                }

                if (_isTwoHandingCurrent)
                {
                    /// Player can play Rh Unarmed Fist Locomotion again.
                    _states.CrossFadeAnimWithMoveDir(runtimeUnarmed._referedWeaponItem.GetRhLocomotionHashByType(), false, false);
                }
                else
                {
                    /// Player can play Rh Locomotion again.
                    _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);

                    RequestSheathWeaponInBackpack(false, rightHandSlots[rhSlotNumber]);
                    _states.OnRemoveWeaponWait();
                }

                /// Set Two Handed Weapon To Null.
                UnRegisterThCurrentWeapon();
                rightHandSlots[rhSlotNumber] = null;
            }

            void Empty_1H_RhWeaponSlotInMenu()
            {
                RuntimeWeapon _weaponToEmpty = rightHandSlots[rhSlotNumber];
                rightHandSlots[rhSlotNumber] = null;

                if (_weaponToEmpty.isCurrentRhWeapon)
                {
                    RequestSheathCurrentInBackpack(false);
                    _states.OnRemoveWeaponWait();
                }
                else
                {
                    RequestSheathWeaponInBackpack(false, _weaponToEmpty);
                }
            }
        }

        public void EmptyRhWeaponSlot(RuntimeWeapon _weaponToEmpty)
        {
            rightHandSlots[_weaponToEmpty.slotNumber] = null;

            if (_weaponToEmpty.isCurrentRhWeapon)
            {
                if (!_hasOffTwoHandingFistInMenu)
                {
                    SheathRhWeapon(_rightHandWeapon);
                }

                UnRegisterRhCurrentWeapon();
                SetIsRightUnarmedStatus(true);
            }
        }
        
        public void EmptyLhWeaponSlot(RuntimeWeapon _weaponToEmpty)
        {
            leftHandSlots[_weaponToEmpty.slotNumber] = null;

            if (_weaponToEmpty.isCurrentLhWeapon)
            {
                if (!_hasOffTwoHandingFistInMenu)
                {
                    SheathLhWeapon(_leftHandWeapon);
                }

                UnRegisterLhCurrentWeapon();
                SetIsLeftUnarmedStatus(true);
            }
        }

        public void ReturnWeaponToBackpack(RuntimeWeapon _weaponToReturn)
        {
            ParentWeaponUnderBackpack(_weaponToReturn.transform);
            _weaponToReturn.currentSlotSide = RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Backpack;
            _weaponToReturn.gameObject.SetActive(false);
        }

        void BringWeaponToLeftSlot(RuntimeWeapon _weaponToBringOut, int _targetSlotNum)
        {
            leftHandSlots[_targetSlotNum] = _weaponToBringOut;
            _weaponToBringOut.slotNumber = _targetSlotNum;
            _weaponToBringOut.currentSlotSide = RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left;
        }

        void BringWeaponToRightSlot(RuntimeWeapon _weaponToBringOut, int _targetSlotNum)
        {
            rightHandSlots[_targetSlotNum] = _weaponToBringOut;
            _weaponToBringOut.slotNumber = _targetSlotNum;
            _weaponToBringOut.currentSlotSide = RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right;
        }
        
        public void ReturnRhCurrentInAnim()
        {
            ReturnWeaponToBackpack(_rightHandWeapon);
            _rightHandWeapon.isCurrentRhWeapon = false;
            _rightHandWeapon = null;
            _rightHandWeapon_referedItem = null;
            SetIsRightUnarmedStatus(true);
        }

        public void ReturnLhCurrentInAnim()
        {
            ReturnWeaponToBackpack(_leftHandWeapon);
            _leftHandWeapon.isCurrentLhWeapon = false;
            _leftHandWeapon = null;
            _leftHandWeapon_referedItem = null;
            SetIsLeftUnarmedStatus(true);
        }

        public void ReturnWeaponInAnim()
        {
            ReturnWeaponToBackpack(_curWeaponToPass);
        }
        
        public void SwitchRhCurrentInAnim()
        {
            int _rhSlotNumber = _rightHandWeapon.slotNumber;

            _rightHandWeapon.isCurrentRhWeapon = false;
            ReturnWeaponToBackpack(_rightHandWeapon);
            
            RegisterRhCurrentWeapon(_curWeaponToPass);
            UnSheathRhWeapon();
            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);

            _playerIKHandler.RegisterRhWeaponIK();
            _mainHudManager.RegisterRhWeaponQSlot();

            HandleWeaponsVisibility();
        }

        public void SwitchLhCurrentInAnim()
        {
            int _lhSlotNumber = _leftHandWeapon.slotNumber;

            _leftHandWeapon.isCurrentLhWeapon = false;
            ReturnWeaponToBackpack(_leftHandWeapon);
            
            RegisterLhCurrentWeapon(_curWeaponToPass);
            UnSheathLhWeapon();

            _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
            _mainHudManager.RegisterLhWeaponQSlot();

            HandleWeaponsVisibility();
        }

        /// HANDLE VISIBLITY.

        public void HandleWeaponsVisibility()
        {
            HandleRhSlotWeaponsVisibility();
            HandleLhSlotWeaponsVisibility();
        }

        void HandleRhSlotWeaponsVisibility()
        {
            if (_rightHandWeapon)
            {
                if (_rightHandWeapon.isShield)
                {
                    if (!_isPowerupHideShield)
                    {
                        _rightHandWeapon.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (_states._isTwoHanding)
                        {
                            _rightHandWeapon.gameObject.SetActive(false);
                        }
                        else
                        {
                            _isShowCurrent_Rh_Shield = true;
                        }
                    }
                }
                else
                {
                    _rightHandWeapon.gameObject.SetActive(true);
                }
            }

            RuntimeWeapon _2ndWeaponNextToCurrent = Get2ndRhWeaponNextToCurrent();
            RuntimeWeapon _3rdWeaponNextToCurrent = Get3rdRhWeaponNextToCurrent();

            if (_2ndWeaponNextToCurrent != null)
            {
                if (_2ndWeaponNextToCurrent.isShield)
                {
                    if (_isPowerupHideShield || _isHideCurrentVisibleShields)
                    {
                        #region Hide 2nd weapon, check 3rd weapon is shield.
                        _2ndWeaponNextToCurrent.gameObject.SetActive(false);
                        if (_3rdWeaponNextToCurrent != null)
                        {
                            if (_3rdWeaponNextToCurrent.isShield)
                            {
                                _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                                _curVisibleRhWeapon = null;
                            }
                            else
                            {
                                _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                                _curVisibleRhWeapon = _3rdWeaponNextToCurrent;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region Show 2nd weapon, Set 2nd weapon as current visible rh weapon.
                        // Set 2nd Weapon GameObject as current visible rh weapon
                        _2ndWeaponNextToCurrent.gameObject.SetActive(true);
                        _curVisibleRhWeapon = _2ndWeaponNextToCurrent;
                        #endregion

                        #region Hide 3rd weapon.
                        if (_3rdWeaponNextToCurrent != null)
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                        #endregion
                    }
                }
                else
                {
                    #region Show 2nd weapon, Set 2nd weapon as current visible rh weapon.
                    _2ndWeaponNextToCurrent.gameObject.SetActive(true);
                    _curVisibleRhWeapon = _2ndWeaponNextToCurrent;
                    #endregion

                    #region Hide 3rd Weapon if it's not Fist.
                    if (_3rdWeaponNextToCurrent != null)
                    {
                        if (_isRightUnarmed)
                        {
                            _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                        }
                        else
                        {
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                if (_3rdWeaponNextToCurrent != null)
                {
                    if (_3rdWeaponNextToCurrent.isShield)
                    {
                        if (_isPowerupHideShield || _isHideCurrentVisibleShields)
                        {
                            #region Hide 3rd weapon, set current visible rh weapon to null.
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                            _curVisibleRhWeapon = null;
                            #endregion
                        }
                        else
                        {
                            #region Show 3rd weapon, Set 3rd weapon as current visible rh weapon.
                            _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                            _curVisibleRhWeapon = _3rdWeaponNextToCurrent;
                            #endregion
                        }
                    }
                    else
                    {
                        #region Show 3rd weapon, Set 3rd weapon as current visible rh weapon.
                        _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                        _curVisibleRhWeapon = _3rdWeaponNextToCurrent;
                        #endregion
                    }
                }
                else
                {
                    _curVisibleRhWeapon = null;
                }
            }
        }

        void HandleLhSlotWeaponsVisibility()
        {
            if (_leftHandWeapon)
            {
                if (_leftHandWeapon.isShield)
                {
                    if (!_isPowerupHideShield)
                    {
                        _leftHandWeapon.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (_states._isTwoHanding)
                        {
                            _leftHandWeapon.gameObject.SetActive(false);
                        }
                        else
                        {
                            _isShowCurrent_Lh_Shield = true;
                        }
                    }
                }
                else
                {
                    _leftHandWeapon.gameObject.SetActive(true);
                }
            }
            
            RuntimeWeapon _2ndWeaponNextToCurrent = Get2ndLhWeaponNextToCurrent();
            RuntimeWeapon _3rdWeaponNextToCurrent = Get3rdLhWeaponNextToCurrent();

            if (_2ndWeaponNextToCurrent != null)
            {
                if (_2ndWeaponNextToCurrent.isShield)
                {
                    if (_isPowerupHideShield || _isHideCurrentVisibleShields)
                    {
                        #region Hide 2nd weapon, check 3rd weapon is shield.
                        _2ndWeaponNextToCurrent.gameObject.SetActive(false);
                        if (_3rdWeaponNextToCurrent != null)
                        {
                            if (_3rdWeaponNextToCurrent.isShield)
                            {
                                _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                                _curVisibleLhWeapon = null;
                            }
                            else
                            {
                                _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                                _curVisibleLhWeapon = _3rdWeaponNextToCurrent;
                            }
                        }
                        #endregion
                    }
                    else if (_curVisibleRhWeapon != null)
                    {
                        if (_curVisibleRhWeapon.isShield)
                        {
                            #region Hide 2nd weapon, check 3rd weapon is shield.
                            _2ndWeaponNextToCurrent.gameObject.SetActive(false);
                            if (_3rdWeaponNextToCurrent != null)
                            {
                                if (_3rdWeaponNextToCurrent.isShield)
                                {
                                    _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                                    _curVisibleLhWeapon = null;
                                }
                                else
                                {
                                    _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                                    _curVisibleLhWeapon = _3rdWeaponNextToCurrent;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Hide 3rd weapon, set 2nd weapon as current visible lh weapon.
                            // Set 2nd Weapon GameObject as current visible lh weapon
                            _2ndWeaponNextToCurrent.gameObject.SetActive(true);
                            _curVisibleLhWeapon = _2ndWeaponNextToCurrent;

                            // Set 3rd Weapon GameObject Active to false.
                            if (_3rdWeaponNextToCurrent != null)
                                _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                            #endregion
                        }
                    }
                    else
                    {
                        #region Hide 3rd weapon, set 2nd weapon as current visible lh weapon.
                        // Set 2nd Weapon GameObject as current visible lh weapon
                        _2ndWeaponNextToCurrent.gameObject.SetActive(true);
                        _curVisibleLhWeapon = _2ndWeaponNextToCurrent;

                        // Set 3rd Weapon GameObject Active to false.
                        if (_3rdWeaponNextToCurrent != null)
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                        #endregion
                    }
                }
                else
                {
                    #region Hide 3rd weapon, set 2nd weapon as current visible lh weapon.
                    // Set 2nd Weapon GameObject as current visible lh weapon
                    _2ndWeaponNextToCurrent.gameObject.SetActive(true);
                    _curVisibleLhWeapon = _2ndWeaponNextToCurrent;

                    // Set 3rd Weapon GameObject Active to false.
                    if (_3rdWeaponNextToCurrent != null)
                    {
                        if (_isLeftUnarmed)
                        {
                            _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                        }
                        else
                        {
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                if (_3rdWeaponNextToCurrent != null)
                {
                    if (_3rdWeaponNextToCurrent.isShield)
                    {
                        if (_isPowerupHideShield || _isHideCurrentVisibleShields)
                        {
                            #region Hide 3rd weapon, set current visible lh weapon to null.
                            _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                            _curVisibleLhWeapon = null;
                            #endregion
                        }
                        else if (_curVisibleRhWeapon != null)
                        {
                            if (_curVisibleRhWeapon.isShield)
                            {
                                #region Hide 3rd weapon, set current visible lh weapon to null.
                                _3rdWeaponNextToCurrent.gameObject.SetActive(false);
                                _curVisibleLhWeapon = null;
                                #endregion
                            }
                            else
                            {
                                // Set 3nd Weapon GameObject as current visible lh weapon
                                _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                                _curVisibleLhWeapon = _3rdWeaponNextToCurrent;
                            }
                        }
                        else
                        {
                            // Set 3nd Weapon GameObject as current visible lh weapon
                            _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                            _curVisibleLhWeapon = _3rdWeaponNextToCurrent;
                        }
                    }
                    else
                    {
                        // Set 3nd Weapon GameObject as current visible lh weapon
                        _3rdWeaponNextToCurrent.gameObject.SetActive(true);
                        _curVisibleLhWeapon = _3rdWeaponNextToCurrent;
                    }
                }
                else
                {
                    _curVisibleLhWeapon = null;
                }
            }
        }

        void TryUnHide_All_Shields()
        {
            if (_isHideCurrent_Rh_Shield)
            {
                _isHideCurrent_Rh_Shield = false;

                if (!_isPowerupHideShield)
                {
                    _rightHandWeapon.gameObject.SetActive(true);
                }
                else
                {
                    _isShowCurrent_Rh_Shield = true;
                }
            }
            else if (_isHideCurrent_Lh_Shield)
            {
                _isHideCurrent_Lh_Shield = false;

                if (!_isPowerupHideShield)
                {
                    _leftHandWeapon.gameObject.SetActive(true);
                }
                else
                {
                    _isShowCurrent_Lh_Shield = true;
                }
            }
            else if (_isHideCurrentVisibleShields)
            {
                if (_curVisibleLhWeapon && _curVisibleRhWeapon)
                {
                    if (_curVisibleLhWeapon.isShield)
                    {
                        if (!_curVisibleRhWeapon.isShield)
                        {
                            _isHideCurrentVisibleShields = false;
                        }
                    }
                    else
                    {
                        _isHideCurrentVisibleShields = false;
                    }
                }
                else
                {
                    _isHideCurrentVisibleShields = false;
                }
            }
        }

        void TryUnHide_Visible_Shields()
        {
            if (_isHideCurrentVisibleShields)
            {
                if (_curVisibleLhWeapon && _curVisibleRhWeapon)
                {
                    if (_curVisibleLhWeapon.isShield)
                    {
                        if (!_curVisibleRhWeapon.isShield)
                        {
                            _isHideCurrentVisibleShields = false;
                        }
                    }
                    else
                    {
                        _isHideCurrentVisibleShields = false;
                    }
                }
                else
                {
                    _isHideCurrentVisibleShields = false;
                }
            }
        }

        #region Anim Events.
        public void HideCurrentVisibleShields()
        {
            if (_isHideCurrentVisibleShields)
            {
                if (_curVisibleRhWeapon)
                {
                    if (_curVisibleRhWeapon.isShield)
                    {
                        _curVisibleRhWeapon.gameObject.SetActive(false);
                    }
                }

                if (_curVisibleLhWeapon)
                {
                    if (_curVisibleLhWeapon.isShield)
                    {
                        _curVisibleLhWeapon.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void ShowCurrentVisibleShields()
        {
            if (_isShowCurrentVisibleShields)
            {
                _isShowCurrentVisibleShields = false;

                if (_curVisibleRhWeapon)
                {
                    if (_curVisibleRhWeapon.isShield)
                    {
                        _curVisibleRhWeapon.gameObject.SetActive(true);
                    }
                }

                if (_curVisibleLhWeapon)
                {
                    if (_curVisibleLhWeapon.isShield)
                    {
                        _curVisibleLhWeapon.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void Hide_Lh_CurrentShield()
        {
            if (_isHideCurrent_Lh_Shield)
            {
                _leftHandWeapon.gameObject.SetActive(false);
            }
        }

        public void Show_Lh_CurrentShield()
        {
            if (_isShowCurrent_Lh_Shield)
            {
                _isShowCurrent_Lh_Shield = false;
                _leftHandWeapon.gameObject.SetActive(true);
            }
        }

        public void Hide_Rh_CurrentShield()
        {
            if (_isHideCurrent_Rh_Shield)
            {
                _rightHandWeapon.gameObject.SetActive(false);
            }
        }

        public void Show_Rh_CurrentShield()
        {
            if (_isShowCurrent_Rh_Shield)
            {
                _isShowCurrent_Rh_Shield = false;
                _rightHandWeapon.gameObject.SetActive(true);
            }
            
        }

        public void HideBothCurrentWeapons()
        {
            if (!_isHidingBothCurrentWeapons)
            {
                if (_states._isTwoHanding)
                {
                    _twoHandingWeapon.gameObject.SetActive(false);
                }
                else
                {
                    if (!_isRightUnarmed)
                        _rightHandWeapon.gameObject.SetActive(false);

                    if (!_isLeftUnarmed)
                        _leftHandWeapon.gameObject.SetActive(false);
                }
            }

            _isHidingBothCurrentWeapons = true;
        }

        public void ShowBothCurrentWeapons()
        {
            if (_isHidingBothCurrentWeapons)
            {
                if (_states._isTwoHanding)
                {
                    _twoHandingWeapon.gameObject.SetActive(true);
                }
                else
                {
                    if (!_isRightUnarmed)
                        _rightHandWeapon.gameObject.SetActive(true);

                    if (!_isLeftUnarmed)
                        _leftHandWeapon.gameObject.SetActive(true);
                }
            }

            _isHidingBothCurrentWeapons = false;
        }

        public void CheckIfBothCurrentWeaponIsHidden()
        {
            if (_isHidingBothCurrentWeapons)
            {
                ShowBothCurrentWeapons();
            }
        }

        public void Hide_Rh_Th_CurrentWeapon()
        {
            if (_states._isTwoHanding)
            {
                _twoHandingWeapon.gameObject.SetActive(false);
            }
            else
            {
                if (!_isRightUnarmed)
                    _rightHandWeapon.gameObject.SetActive(false);
            }
        }

        public void Show_Rh_Th_CurrentWeapon()
        {
            if (_states._isTwoHanding)
            {
                _twoHandingWeapon.gameObject.SetActive(true);
            }
            else
            {
                if (!_isRightUnarmed)
                    _rightHandWeapon.gameObject.SetActive(true);
            }
        }

        public void HideLhCurrentWeapon()
        {
            if (!_isLeftUnarmed && !_states._isTwoHanding)
            {
                _leftHandWeapon.gameObject.SetActive(false);
            }
        }

        public void ShowLhCurrentWeapon()
        {
            if (!_isLeftUnarmed && !_states._isTwoHanding)
            {
                _leftHandWeapon.gameObject.SetActive(true);
            }
        }
        #endregion

        RuntimeWeapon Get2ndLhWeaponNextToCurrent()
        {
            int _2ndWeaponIndex = l_index + 1 == leftHandWeaponSlotsLength ? 0 : l_index + 1;
            return leftHandSlots[_2ndWeaponIndex];
        }

        RuntimeWeapon Get3rdLhWeaponNextToCurrent()
        {
            int _3rdWeaponIndex = 0;
            int _3rd_l_index = l_index + 2;

            if (_3rd_l_index >= leftHandWeaponSlotsLength)
            {
                if (_3rd_l_index > leftHandWeaponSlotsLength)
                {
                    _3rdWeaponIndex = 1;
                }
                else
                {
                    _3rdWeaponIndex = 0;
                }
            }
            else
            {
                _3rdWeaponIndex = _3rd_l_index;
            }
            
            return leftHandSlots[_3rdWeaponIndex];
        }

        RuntimeWeapon GetPreviousWeaponBeforeRhUnarmed()
        {
            int _weaponBeforeUnarmedIndex = r_index - 1 < 0 ? rightHandWeaponSlotsLength - 1 : r_index - 1;
            return rightHandSlots[_weaponBeforeUnarmedIndex];
        }

        RuntimeWeapon Get2ndRhWeaponNextToCurrent()
        {
            int _2ndWeaponIndex = r_index + 1 == rightHandWeaponSlotsLength ? 0 : r_index + 1;
            return rightHandSlots[_2ndWeaponIndex];
        }

        RuntimeWeapon Get3rdRhWeaponNextToCurrent()
        {
            int _3rdWeaponIndex = 0;
            int _3rd_r_index = r_index + 2;

            if (_3rd_r_index >= rightHandWeaponSlotsLength)
            {
                if (_3rd_r_index > rightHandWeaponSlotsLength)
                {
                    _3rdWeaponIndex = 1;
                }
                else
                {
                    _3rdWeaponIndex = 0;
                }
            }
            else
            {
                _3rdWeaponIndex = _3rd_r_index;
            }
            
            return rightHandSlots[_3rdWeaponIndex];
        }

        /// REGISTER CURRENTS.

        public void RegisterRhCurrentWeapon(RuntimeWeapon _runtimeWeapon)
        {
            _runtimeWeapon.isCurrentRhWeapon = true;
            _rightHandWeapon = _runtimeWeapon;
            _rightHandWeapon_referedItem = _rightHandWeapon._referedWeaponItem;
        }

        public void RegisterLhCurrentWeapon(RuntimeWeapon _runtimeWeapon)
        {
            _runtimeWeapon.isCurrentLhWeapon = true;
            _leftHandWeapon = _runtimeWeapon;
            _leftHandWeapon_referedItem = _leftHandWeapon._referedWeaponItem;
        }
        
        void RegisterRhCurrentUnarmed()
        {
            _rightHandWeapon = runtimeUnarmed;
            _rightHandWeapon_referedItem = runtimeUnarmed._referedWeaponItem;

            _mainHudManager.RegisterRhWeaponQSlot();

            if (!_states._isTwoHanding)
            {
                _playerIKHandler.RegisterRhWeaponIK();
                ParentRhCurrentUnderHand();
            }
        }

        void RegisterLhCurrentUnarmed()
        {
            _leftHandWeapon = runtimeUnarmed;
            _leftHandWeapon_referedItem = runtimeUnarmed._referedWeaponItem;

            _mainHudManager.RegisterLhWeaponQSlot();

            if (!_states._isTwoHanding)
                ParentLhCurrentUnderHand();
        }

        void RegisterRhCurrentTwoHanding()
        {
            _twoHandingWeapon = _rightHandWeapon;
            _twoHandingWeapon_referedItem = _rightHandWeapon_referedItem;
        }

        void RegisterLhCurrentTwoHanding()
        {
            _twoHandingWeapon = _leftHandWeapon;
            _twoHandingWeapon_referedItem = _leftHandWeapon_referedItem;
        }

        /// UNREGISTER WEAPON

        public void UnRegisterLhCurrentWeapon()
        {
            _leftHandWeapon.isCurrentLhWeapon = false;
            _leftHandWeapon = null;
            _leftHandWeapon_referedItem = null;
        }

        public void UnRegisterRhCurrentWeapon()
        {
            _rightHandWeapon.isCurrentRhWeapon = false;
            _rightHandWeapon = null;
            _rightHandWeapon_referedItem = null;
        }
        
        public void UnRegisterThCurrentWeapon()
        {
            _twoHandingWeapon = null;
            _twoHandingWeapon_referedItem = null;
        }

        /// PARENT TRANSFORMS.

        public void ParentLhCurrentUnderHand()
        {
            Transform _runtimeWeaponTransform = _leftHandWeapon.transform;
            _runtimeWeaponTransform.parent = _states.leftHandTransform;

            _runtimeWeaponTransform.localPosition = _leftHandWeapon_referedItem.opposedHandTransform.pos;
            _runtimeWeaponTransform.localEulerAngles = _leftHandWeapon_referedItem.opposedHandTransform.eulers;
            _runtimeWeaponTransform.localScale = _states.vector3One;
        }

        public void ParentRhCurrentUnderHand()
        {
            Transform _runtimeWeaponTransform = _rightHandWeapon.transform;
            _runtimeWeaponTransform.parent = _states.rightHandTransform;

            _runtimeWeaponTransform.localPosition = _states.vector3Zero;
            _runtimeWeaponTransform.localEulerAngles = _states.vector3Zero;
            _runtimeWeaponTransform.localScale = _states.vector3One;

            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
        }

        public void ParentRhListWeaponUnderSheath()
        {
            PlayerSheathTransform _playerSheathTransform = _rhWeaponsToSheath[0]._referedWeaponItem.weaponSheathTransform;
            Transform _rightHandSheathTransform = _rhWeaponsToSheath[0].transform;

            switch (_playerSheathTransform.parentBoneType)
            {
                case PlayerSheathTransform.SheathParentBoneTypeEnum.Spine:
                    _rightHandSheathTransform.parent = _states.spineTransform;
                    break;

                case PlayerSheathTransform.SheathParentBoneTypeEnum.Hips:
                    _rightHandSheathTransform.parent = _states.hipTransform;
                    break;
            }

            _rightHandSheathTransform.localPosition = _playerSheathTransform.pos;
            _rightHandSheathTransform.localEulerAngles = _playerSheathTransform.eulers;
            _rightHandSheathTransform.localScale = _states.vector3One;

            _rhWeaponsToSheath.Remove(_rhWeaponsToSheath[0]);
        }

        public void ParentLhListWeaponUnderSheath()
        {
            PlayerSheathTransform _playerSheathTransform = _lhWeaponsToSheath[0]._referedWeaponItem.opposedSheathTransform;
            Transform _leftHandSheathTransform = _lhWeaponsToSheath[0].transform;

            switch (_playerSheathTransform.parentBoneType)
            {
                case PlayerSheathTransform.SheathParentBoneTypeEnum.Spine:
                    _leftHandSheathTransform.parent = _states.spineTransform;
                    break;

                case PlayerSheathTransform.SheathParentBoneTypeEnum.Hips:
                    _leftHandSheathTransform.parent = _states.hipTransform;
                    break;
            }

            _leftHandSheathTransform.localPosition = _playerSheathTransform.pos;
            _leftHandSheathTransform.localEulerAngles = _playerSheathTransform.eulers;
            _leftHandSheathTransform.localScale = _states.vector3One;

            _lhWeaponsToSheath.Remove(_lhWeaponsToSheath[0]);
        }

        public void BringOpposeWeaponToRightHand()
        {
            Transform _runtimeWeaponTransform = _leftHandWeapon.transform;
            _runtimeWeaponTransform.parent = _states.rightHandTransform;
            _runtimeWeaponTransform.localPosition = _states.vector3Zero;
            _runtimeWeaponTransform.localEulerAngles = _states.vector3Zero;
            _runtimeWeaponTransform.localScale = _states.vector3One;
        }
        
        public void PassRhWeaponToLeftHand()
        {
            Transform _runtimeWeaponTransform = _curWeaponToPass.transform;
            _runtimeWeaponTransform.parent = _states.leftHandTransform;
            _runtimeWeaponTransform.localPosition = _curWeaponToPass._referedWeaponItem.opposedHandTransform.pos;
            _runtimeWeaponTransform.localEulerAngles = _curWeaponToPass._referedWeaponItem.opposedHandTransform.eulers;
            _runtimeWeaponTransform.localScale = _states.vector3One;

            _curWeaponToPass = null;
        }

        public void PassLhWeaponToRightHand()
        {
            Transform _runtimeWeaponTransform = _curWeaponToPass.transform;
            _runtimeWeaponTransform.parent = _states.rightHandTransform;
            _runtimeWeaponTransform.localPosition = _states.vector3Zero;
            _runtimeWeaponTransform.localEulerAngles = _states.vector3Zero;
            _runtimeWeaponTransform.localScale = _states.vector3One;
        }

        void ParentWeaponUnderBackpack(Transform _runtimeWeaponTransform)
        {
            _runtimeWeaponTransform.parent = weaponBackpackTransform;

            _runtimeWeaponTransform.localPosition = _states.vector3Zero;
            _runtimeWeaponTransform.localEulerAngles = _states.vector3Zero;
            _runtimeWeaponTransform.localScale = _states.vector3One;
        }

        /// SHEATH / UNSHEATH

        public void UnSheathRhWeapon()
        {
            _a_hook.RegisterNewAnimationJob(_rightHandWeapon_referedItem.GetRhUnSheathHashByType(), false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void SheathRhWeapon(RuntimeWeapon _runtimeWeapon)
        {
            _rhWeaponsToSheath.Add(_runtimeWeapon);
            _a_hook.RegisterNewAnimationJob(_runtimeWeapon._referedWeaponItem.GetRhSheathHashByType(), false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }
        
        public void UnSheathLhWeapon()
        {
            _a_hook.RegisterNewAnimationJob(_leftHandWeapon_referedItem.GetLhUnSheathHashByType(), false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void SheathLhWeapon(RuntimeWeapon _runtimeWeapon)
        {
            _lhWeaponsToSheath.Add(_runtimeWeapon);
            _a_hook.RegisterNewAnimationJob(_runtimeWeapon._referedWeaponItem.GetLhSheathHashByType(), false);
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }
        
        /// SET UNARMS.

        public void SetIsRightUnarmedStatus(bool _isRightUnarmed)
        {
            if (_isRightUnarmed)
            {
                if (!this._isRightUnarmed)
                {
                    this._isRightUnarmed = true;
                    RegisterRhCurrentUnarmed();

                    if (_isLeftUnarmed)
                        _isBothUnarmed = true;
                }
            }
            else
            {
                if (this._isRightUnarmed)
                {
                    this._isRightUnarmed = false;
                    _isBothUnarmed = false;
                }
            }
        }

        public void SetIsLeftUnarmedStatus(bool _isLeftUnarmed)
        {
            if (_isLeftUnarmed)
            {
                if (!this._isLeftUnarmed)
                {
                    this._isLeftUnarmed = true;
                    RegisterLhCurrentUnarmed();

                    if (_isRightUnarmed)
                        _isBothUnarmed = true;
                }
            }
            else
            {
                if (this._isLeftUnarmed)
                {
                    this._isLeftUnarmed = false;
                    _isBothUnarmed = false;
                }
            }
        }
        
        /// SET TWO HANDING.
        
        public void SetIsTwoHandingStatusToFalseByType()
        {
            switch (_twoHandingWeapon.currentSlotSide)
            {
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right:
                    BackFromRhTwoHanding();
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left:
                    BackFromLhTwoHanding();
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Unarmed:
                    if (_isTwoHanding_Rh_Fist)
                    {
                        BackFromRhTwoHanding();
                    }
                    else
                    {
                        BackFromLhTwoHanding();
                    }
                    break;
            }
        }

        public void TwoHandingRhWeapon()
        {
            bool _isFistWeapon = _rightHandWeapon_referedItem.weaponType == P_Weapon_WeaponTypeEnum.Fist ? true : false;
            if (_isFistWeapon)
            {
                _isTwoHanding_Rh_Fist = true;
            }

            bool _isSheathingNeeded = false;
            if (_leftHandWeapon && !_isLeftUnarmed)
            {
                SheathLhWeapon(_leftHandWeapon);
                _isSheathingNeeded = true;
            }

            if (_isPowerupHideShield)
            {
                _isHideCurrent_Lh_Shield = true;
            }
            else
            {
                _isHideCurrentVisibleShields = true;
            }

            _states.OnTwoHandingRhWeapon(_rightHandWeapon_referedItem, _isFistWeapon);
            _states.OnTwoHandingWeaponWait(false, _isSheathingNeeded, _isFistWeapon);

            if (_playerIKHandler._isHandleIKJobsEmpty)
            {
                _states.ResumeLocoIKStateTick();
            }

            RegisterRhCurrentTwoHanding();

            _mainHudManager.OnTwoHandingRightHandWeapon();
        }

        public void TwoHandingLhWeapon()
        {
            bool _isFistWeapon = _leftHandWeapon_referedItem.weaponType == P_Weapon_WeaponTypeEnum.Fist ? true : false;

            bool _isSheathingNeeded = false;
            if (_rightHandWeapon && !_isRightUnarmed)
            {
                SheathRhWeapon(_rightHandWeapon);
                _isSheathingNeeded = true;
            }

            if (_isPowerupHideShield)
            {
                _isHideCurrent_Rh_Shield = true;
            }
            else
            {
                _isHideCurrentVisibleShields = true;
            }

            _states.OnTwoHandingLhWeapon(_leftHandWeapon_referedItem, _isFistWeapon);
            _states.OnTwoHandingWeaponWait(true, _isSheathingNeeded, _isFistWeapon);

            RegisterLhCurrentTwoHanding();

            _mainHudManager.OnTwoHandingLeftHandWeapon();
        }

        public void BackFromRhTwoHanding()
        {
            bool _isFistWeapon = _rightHandWeapon_referedItem.weaponType == P_Weapon_WeaponTypeEnum.Fist ? true : false;
            bool _isSheathingNeeded = false;

            if (_isFistWeapon)
            {
                _states.OffTwoHandingFistBeforeUnSheathLhWeapon();
                _isTwoHanding_Rh_Fist = false;
            }

            if (_leftHandWeapon && !_isLeftUnarmed)
            {
                UnSheathLhWeapon();
                _isSheathingNeeded = true;
            }

            _states.OffTwoHandingRhWeapon(_rightHandWeapon_referedItem);
            _states.OnTwoHandingWeaponWait(false, _isSheathingNeeded, _isFistWeapon);

            if (_isHideCurrent_Lh_Shield)
            {
                _isHideCurrent_Lh_Shield = false;
                _isShowCurrent_Lh_Shield = true;
            }

            if (_isHideCurrentVisibleShields)
            {
                _isHideCurrentVisibleShields = false;
                _isShowCurrentVisibleShields = true;
            }

            if (_playerIKHandler._isHandleIKJobsEmpty)
            {
                _states.ResumeLocoIKStateTick();
            }

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingRightHandWeapon();
        }

        public void BackFromLhTwoHanding()
        {
            bool _isFistWeapon = _leftHandWeapon_referedItem.weaponType == P_Weapon_WeaponTypeEnum.Fist ? true : false;
            bool _isSheathingNeeded = false;

            _states.OffTwoHandingLhWeapon(_leftHandWeapon_referedItem , _isFistWeapon);
            
            if (_rightHandWeapon && !_isRightUnarmed)
            {
                UnSheathRhWeapon();
                _isSheathingNeeded = true;
            }

            _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
            _states.OnTwoHandingWeaponWait(true, _isSheathingNeeded, _isFistWeapon);

            if (_isHideCurrent_Rh_Shield)
            {
                _isHideCurrent_Rh_Shield = false;
                _isShowCurrent_Rh_Shield = true;
            }

            if (_isHideCurrentVisibleShields)
            {
                _isHideCurrentVisibleShields = false;
                _isShowCurrentVisibleShields = true;
            }

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingLeftHandWeapon();
        }

        public void OffTwoHandingInMenuBeforeSetupSlot()
        {
            _hasOffTwoHandingFistInMenu = false;
            if (_states._isInTwoHandFist)
            {
                /// UnRegister MainHud Two Handing.
                if (_isTwoHanding_Rh_Fist)
                {
                    _mainHudManager.OffTwoHandingRightHandWeapon();
                }
                else
                {
                    _mainHudManager.OffTwoHandingLeftHandWeapon();
                }

                _hasOffTwoHandingFistInMenu = true;
                _states.OffTwoHandingFistInMenuBeforeSetupSlot();
                _states.CrossFadeAnimWithMoveDir(_rightHandWeapon_referedItem.GetRhLocomotionHashByType(), false, false);
                UnRegisterThCurrentWeapon();
            }
        }

        #region Revive Version.
        public void OnReviveOffTwoHanding()
        {
            switch (_twoHandingWeapon.currentSlotSide)
            {
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right:
                    BackFromRhTwoHanding_Revive();
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left:
                    BackFromLhTwoHanding_Revive();
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Unarmed:
                    if (_isTwoHanding_Rh_Fist)
                    {
                        BackFromRhTwoHanding_Fist_Revive();
                    }
                    else
                    {
                        BackFromLhTwoHanding_Fist_Revive();
                    }
                    break;
            }

            _states.anim.Play(_rightHandWeapon_referedItem.GetRhLocomotionHashByType());
        }

        void BackFromRhTwoHanding_Revive()
        {
            if (!_isRightUnarmed)
                ParentRhCurrentUnderHand();

            if (!_isLeftUnarmed)
                ParentLhCurrentUnderHand();

            if (_isHideCurrent_Lh_Shield)
            {
                _isHideCurrent_Lh_Shield = false;
                _leftHandWeapon.gameObject.SetActive(true);
            }

            _states.OffTwoHandingWhenSetupSlot();

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingRightHandWeapon();
        }

        void BackFromRhTwoHanding_Fist_Revive()
        {
            _states.OffTwoHandingFistOnRevive();

            if (!_isLeftUnarmed)
                ParentLhCurrentUnderHand();

            if (_isHideCurrent_Lh_Shield)
            {
                _isHideCurrent_Lh_Shield = false;
                _leftHandWeapon.gameObject.SetActive(true);
            }

            _states.OffTwoHandingWhenSetupSlot();

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingRightHandWeapon();
        }

        void BackFromLhTwoHanding_Revive()
        {
            if (!_isRightUnarmed)
                ParentRhCurrentUnderHand();

            if (!_isLeftUnarmed)
                ParentLhCurrentUnderHand();

            if (_isHideCurrent_Rh_Shield)
            {
                _isHideCurrent_Rh_Shield = false;
                _rightHandWeapon.gameObject.SetActive(true);
            }

            _states.OffTwoHandingWhenSetupSlot();

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingLeftHandWeapon();
        }

        void BackFromLhTwoHanding_Fist_Revive()
        {
            Debug.Log("BackFromLhTwoHanding_Fist_Revive");
            _states.OffTwoHandingFistOnRevive();

            if (!_isRightUnarmed)
                ParentRhCurrentUnderHand();

            if (_isHideCurrent_Rh_Shield)
            {
                _isHideCurrent_Rh_Shield = false;
                _rightHandWeapon.gameObject.SetActive(true);
            }

            _states.OffTwoHandingWhenSetupSlot();

            UnRegisterThCurrentWeapon();

            _mainHudManager.OffTwoHandingLeftHandWeapon();
        }
        #endregion

        /// Sprinting.

        public void BeginSprinting()
        { 
            if (!_states._isTwoHanding)
            {
                /// If Lh weapon is not unarmed
                if (!_isLeftUnarmed)
                {
                    /// If Lh weapon is a shield
                    if (_leftHandWeapon.isShield)
                    {
                        /// If player is equipping Powerup
                        if (_isPowerupHideShield)
                        {
                            _isHideCurrent_Lh_Shield = true;
                        }

                        /// Doesn't matter if player equipping Powerup or not, visible shields need to be hidden.
                        _isHideCurrentVisibleShields = true;
                    }

                    SheathLhWeapon(_leftHandWeapon);
                    LeanTween.value(0, 1, 0.5f).setOnComplete(_states.BeginSpritingChangeVelocity);
                }
                else
                {
                    _states.BeginSpritingChangeVelocity();
                }
            }
            else
            {
                _states.BeginSpritingChangeVelocity();
            }

            _states.anim.SetBool(_states.p_IsSprinting_hash, true);
            _a_hook.RegisterNewAnimationJob(_rightHandWeapon_referedItem.GetSprintStartHashByType(), false);
        }

        public void EndSprinting()
        {
            _a_hook.RegisterNewAnimationJob(_states.p_sprint_end_hash, true);
            _states.anim.SetBool(_states.p_IsSprinting_hash, false);
            _states.applySprintEndMotion = true;

            if (!_states._isTwoHanding)
            {
                if (!_isLeftUnarmed)
                {
                    UnSheathLhWeapon();

                    /// If Lh weapon is a shield
                    if (_leftHandWeapon.isShield)
                    {
                        /// If player Lh shield is hidden and Powerup is Equipped
                        if (_isHideCurrent_Lh_Shield)
                        {
                            _isHideCurrent_Lh_Shield = false;
                            _isShowCurrent_Lh_Shield = true;
                        }

                        if (!_isPowerupHideShield)
                        {
                            _isHideCurrentVisibleShields = false;
                            _isShowCurrentVisibleShields = true;
                        }
                    }
                }
            }
        }

        /// UTILITY.

        public RuntimeWeapon FindFirstRhWeaponFromSlots()
        {
            for (int i = 0; i < rightHandWeaponSlotsLength; i++)
            {
                if (rightHandSlots[i] != null)
                {
                    return rightHandSlots[i];
                }
            }

            return null;
        }

        public RuntimeWeapon FindFirstLhWeaponFromSlots()
        {
            for (int i = 0; i < leftHandWeaponSlotsLength; i++)
            {
                if (leftHandSlots[i] != null)
                {
                    return leftHandSlots[i];
                }
            }

            return null;
        }

        public RuntimeWeapon GetNextRhWeaponFromSlots()
        {
            int tempIndex = r_index;
            r_index++;
            r_index = (r_index == rightHandWeaponSlotsLength) ? 0 : r_index;

            if (rightHandSlots[r_index] == null)
            {
                r_index++;
                r_index = (r_index == rightHandWeaponSlotsLength) ? 0 : r_index;
                if (rightHandSlots[r_index] != null)
                {
                    return rightHandSlots[r_index];
                }
                else
                {
                    r_index = tempIndex;
                    return null;
                }
            }
            
            return rightHandSlots[r_index];
        }

        public RuntimeWeapon GetNextLhWeaponFromSlots()
        {
            int tempIndex = l_index;
            l_index++;
            l_index = (l_index == leftHandWeaponSlotsLength) ? 0 : l_index;

            if (leftHandSlots[l_index] == null)
            {
                l_index++;
                l_index = (l_index == leftHandWeaponSlotsLength) ? 0 : l_index;
                if (leftHandSlots[l_index] != null)
                {
                    return leftHandSlots[l_index];
                }
                else
                {
                    l_index = tempIndex;
                    return null;
                }
            }
            
            return leftHandSlots[l_index];
        }

        /// ADD / REMOVE CARRYING.

        public void AddWeaponToCarrying(RuntimeWeapon _runtimeWeapon)
        {
            allWeaponsPlayerCarrying.Add(_runtimeWeapon);
            _isWeaponCarryingFilled = true;
        }

        public void RemoveWeaponFromCarrying(RuntimeWeapon _runtimeWeapon)
        {
            allWeaponsPlayerCarrying.Remove(_runtimeWeapon);
            if (allWeaponsPlayerCarrying.Count == 0)
            {
                _isWeaponCarryingFilled = false;
            }
        }

        /// ON PLAYER DEATH.
        
        public void OnDeathTurnOffDamageCollider(bool _isTwoHanding)
        {
            if (_isTwoHanding)
            {
                if (_twoHandingWeapon.IsUnarmed())
                {
                    _states.OnDeathTurnOffPublieColliders();
                }
                else
                {
                    _twoHandingWeapon.weaponHook.SetColliderStatusToFalse();
                }
            }
            else
            {
                if (_isRightUnarmed)
                {
                    _states.OnDeathTurnOffPublieColliders();

                    if (!_isLeftUnarmed)
                    {
                        _leftHandWeapon.weaponHook.SetColliderStatusToFalse();
                    }
                }
                else if (_isLeftUnarmed)
                {
                    _states.OnDeathTurnOffPublieColliders();

                    if (!_isRightUnarmed)
                    {
                        _rightHandWeapon.weaponHook.SetColliderStatusToFalse();
                    }
                }
                else
                {
                    _leftHandWeapon.weaponHook.SetColliderStatusToFalse();
                    _rightHandWeapon.weaponHook.SetColliderStatusToFalse();
                }
            }
        }

        /// GET FORTIFIED NAME.
        
        public string GetFortifiedWeaponName(RuntimeWeapon _runtimeWeapon)
        {
            _inventoryStrBuilder.Clear();

            switch (_runtimeWeapon.weaponModifiableStats._infusedElementType)
            {
                case P_Weapon_InfusedElementTypeEnum.Refined:
                    _inventoryStrBuilder.Append("Refined ");
                    break;
                case P_Weapon_InfusedElementTypeEnum.Magic:
                    _inventoryStrBuilder.Append("Magical ");
                    break;
                case P_Weapon_InfusedElementTypeEnum.Fire:
                    _inventoryStrBuilder.Append("Fire ");
                    break;
                case P_Weapon_InfusedElementTypeEnum.Lightning:
                    _inventoryStrBuilder.Append("Lightning ");
                    break;
                case P_Weapon_InfusedElementTypeEnum.Dark:
                    _inventoryStrBuilder.Append("Dark ");
                    break;
            }

            _inventoryStrBuilder.Append(_runtimeWeapon._referedWeaponItem.itemName);

            if (_runtimeWeapon.weaponModifiableStats._fortifiedLevel != 0)
                _inventoryStrBuilder.Append(" + ").Append(_runtimeWeapon.weaponModifiableStats._fortifiedLevel);

            return _inventoryStrBuilder.ToString();
        }

        /// WEAPON ACTION.
        
        public bool HandleWeaponActions(bool _isTwoHanding)
        {
            HandleWeaponInputs(_isTwoHanding);

            if (_isTwoHanding)
            {
                if (_twoHandingWeapon_referedItem.HandleTwoHandingWeaponAction(_states))
                    return true;
            }
            else
            {
                if (_rightHandWeapon_referedItem.HandleDominantHandWeaponAction(_states))
                {
                    return true;
                }
                else if (_leftHandWeapon_referedItem.HandleOpposeHandWeaponAction(_states))
                {
                    return true;
                }
            }

            return false;
        }

        /// ON DEATH / REVIVE DISSOLVE.

        public void OnDeathDissolveOutWeawpons()
        {
            if (!_isRightUnarmed)
                _rightHandWeapon.DissolveOutWeapon();

            if (!_isLeftUnarmed)
                _leftHandWeapon.DissolveOutWeapon();
        }

        public void OnReviveDissolveInWeapons()
        {
            if (!_isRightUnarmed)
                _rightHandWeapon.DissolveInWeapon();

            if (!_isLeftUnarmed)
                _leftHandWeapon.DissolveInWeapon();
        }
        #endregion

        #region Arrow.
        /// SETUP EMPTY SLOTS.

        public void SetupArrowEmptySlot(RuntimeArrow _runtimeArrow)
        {
        }

        /// SETUP TAKEN SLOTS.
        
        public void SetupArrowTakenSlot(RuntimeArrow _runtimeArrow)
        {
        }

        /// EMPTY SLOTS.
        
        public void EmptyArrowSlot()
        {
        }
        #endregion

        #region Armor.

        /// INIT EMPTY SLOTS.
        public void InitHeadArmorEmptySlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            headArmorSlot = _runtimeHeadArmor;
            RegisterHeadArmor();
        }

        public void InitChestArmorEmptySlot(RuntimeChestArmor _runtimeChestArmor)
        {
            chestArmorSlot = _runtimeChestArmor;
            RegisterChestArmor();
        }

        public void InitHandArmorEmptySlot(RuntimeHandArmor _runtimeHandArmor)
        {
            handArmorSlot = _runtimeHandArmor;
            RegisterHandArmor();
        }

        public void InitLegArmorEmptySlot(RuntimeLegArmor _runtimeLegArmor)
        {
            legArmorSlot = _runtimeLegArmor;
            RegisterLegArmor();
        }

        /// SETUP EMPTY SLOTS.

        public void SetupHeadArmorEmptySlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            headArmorSlot = _runtimeHeadArmor;
            RegisterHeadArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupChestArmorEmptySlot(RuntimeChestArmor _runtimeChestArmor)
        {
            chestArmorSlot = _runtimeChestArmor;
            RegisterChestArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupHandArmorEmptySlot(RuntimeHandArmor _runtimeHandArmor)
        {
            handArmorSlot = _runtimeHandArmor;
            RegisterHandArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupLegArmorEmptySlot(RuntimeLegArmor _runtimeLegArmor)
        {
            legArmorSlot = _runtimeLegArmor;
            RegisterLegArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        /// SETUP TAKEN SLOTS.

        public void SetupHeadArmorTakenSlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            UnRegisterHeadArmor();

            headArmorSlot = _runtimeHeadArmor;
            RegisterHeadArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupChestArmorTakenSlot(RuntimeChestArmor _runtimeChestArmor)
        {
            UnRegisterChestArmor();

            chestArmorSlot = _runtimeChestArmor;
            RegisterChestArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupHandArmorTakenSlot(RuntimeHandArmor _runtimeHandArmor)
        {
            UnRegisterHandArmor();

            handArmorSlot = _runtimeHandArmor;
            RegisterHandArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void SetupLegArmorTakenSlot(RuntimeLegArmor _runtimeLegArmor)
        {
            UnRegisterLegArmor();

            legArmorSlot = _runtimeLegArmor;
            RegisterLegArmor();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        /// EMPTY SLOTS.

        public void EmptyHeadArmorSlot()
        {
            UnRegisterHeadArmor();

            headArmorSlot = null;
            RegisterDeprivedHead();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void EmptyChestArmorSlot()
        {
            UnRegisterChestArmor();

            chestArmorSlot = null;
            RegisterDeprivedChest();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void EmptyHandArmorSlot()
        {
            UnRegisterHandArmor();

            handArmorSlot = null;
            RegisterDeprivedHand();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void EmptyLegArmorSlot()
        {
            UnRegisterLegArmor();

            legArmorSlot = null;
            RegisterDeprivedLeg();
            _states.statsHandler.RefreshBaseDmgReductionAndResis();
        }

        public void ReturnHeadArmorToBackpack(RuntimeHeadArmor _runtimeHeadArmor)
        {
            ParentArmorUnderBackpack(_runtimeHeadArmor.transform);
            _runtimeHeadArmor.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void ReturnChestArmorToBackpack(RuntimeChestArmor _runtimeChestArmor)
        {
            ParentArmorUnderBackpack(_runtimeChestArmor.transform);
            _runtimeChestArmor.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void ReturnHandArmorToBackpack(RuntimeHandArmor _runtimeHandArmor)
        {
            ParentArmorUnderBackpack(_runtimeHandArmor.transform);
            _runtimeHandArmor.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void ReturnLegArmorToBackpack(RuntimeLegArmor _runtimeLegArmor)
        {
            ParentArmorUnderBackpack(_runtimeLegArmor.transform);
            _runtimeLegArmor.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        /// REGISTER ARMORS.

        void RegisterHeadArmor()
        {
            _states.UnCombineSkinnedMesh();

            switch (headArmorSlot._referedArmorItem.headArmorType)
            {
                case RuntimeHeadArmor.HeadArmorTypeEnum.Cover:
                    _states.headPiecesRenderer[0].enabled = true;
                    _states.headPiecesRenderer[0].sharedMesh = headArmorSlot.headMesh[0];
                    _states.headPiecesRenderer[1].sharedMesh = runtimeDeprivedHead.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = false;
                    break;

                case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet:
                    _states.headPiecesRenderer[0].enabled = false;
                    _states.headPiecesRenderer[1].sharedMesh = headArmorSlot.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = false;
                    break;

                case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet_Attachment:
                    _states.headPiecesRenderer[0].enabled = false;
                    _states.headPiecesRenderer[1].sharedMesh = headArmorSlot.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = true;
                    _states.headPiecesRenderer[2].sharedMesh = headArmorSlot.headMesh[1];
                    break;
            }

            headArmorSlot._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            headArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Head;
        }

        void RegisterChestArmor()
        {
            _states.UnCombineSkinnedMesh();

            for (int i = 0; i < 2; i++)
            {
                _states.chestPiecesRenderer[i].sharedMesh = chestArmorSlot.chestMeshes[i];
            }

            chestArmorSlot._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            chestArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Chest;
        }

        void RegisterHandArmor()
        {
            _states.UnCombineSkinnedMesh();

            switch (handArmorSlot._referedArmorItem.handArmorType)
            {
                case RuntimeHandArmor.HandArmorTypeEnum.No_Attachment:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }

                    for (int i = 6; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.All_Attachment:
                    
                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }

                    for (int i = 6; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.Shoulder_Attachment_Only:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }

                    for (int i = 6; i < 8; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }

                    for (int i = 8; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.Elbow_Attachment_Only:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                    }

                    for (int i = 6; i < 8; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }

                    for (int i = 8; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i - 2];
                    }
                    break;
            }

            handArmorSlot._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            handArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Hand;
        }

        void RegisterLegArmor()
        {
            _states.UnCombineSkinnedMesh();

            switch (legArmorSlot._referedArmorItem.legArmorTypeEnum)
            {
                case RuntimeLegArmor.LegArmorTypeEnum.No_Attachment:

                    for (int i = 0; i < 2; i++)
                    {
                        _states.legPiecesRenderer[i].sharedMesh = legArmorSlot.legMeshes[i];
                    }

                    for (int i = 2; i < 2; i++)
                    {
                        _states.legPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeLegArmor.LegArmorTypeEnum.Kneel_Attachment:

                    for (int i = 0; i < 4; i++)
                    {
                        _states.legPiecesRenderer[i].sharedMesh = legArmorSlot.legMeshes[i];
                    }
                    break;
            }

            legArmorSlot._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            legArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Leg;
        }

        /// REGISTER DEPRIVEDS.

        public void RegisterDeprivedHead()
        {
            _states.UnCombineSkinnedMesh();

            _states.headPiecesRenderer[0].enabled = false;
            _states.headPiecesRenderer[1].sharedMesh = runtimeDeprivedHead.headMesh[0];
            _states.headPiecesRenderer[2].enabled = false;

            runtimeDeprivedHead._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            runtimeDeprivedHead.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Head;
        }

        public void RegisterDeprivedChest()
        {
            _states.UnCombineSkinnedMesh();

            for (int i = 0; i < 2; i++)
            {
                _states.chestPiecesRenderer[i].sharedMesh = runtimeDeprivedChest.chestMeshes[i];
            }

            runtimeDeprivedChest._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            runtimeDeprivedChest.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Chest;
        }

        public void RegisterDeprivedHand()
        {
            _states.UnCombineSkinnedMesh();

            for (int i = 0; i < 6; i++)
            {
                _states.handPiecesRenderer[i].sharedMesh = runtimeDeprivedHand.handMeshes[i];
            }

            for (int i = 6; i < 10; i++)
            {
                _states.handPiecesRenderer[i].enabled = false;
            }

            runtimeDeprivedHand._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            runtimeDeprivedHand.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Hand;
        }

        public void RegisterDeprivedLeg()
        {
            _states.UnCombineSkinnedMesh();

            for (int i = 0; i < 2; i++)
            {
                _states.legPiecesRenderer[i].sharedMesh = runtimeDeprivedLeg.legMeshes[i];
            }

            for (int i = 2; i < 4; i++)
            {
                _states.legPiecesRenderer[i].enabled = false;
            }

            runtimeDeprivedLeg._referedArmorItem.ChangeStatsWithArmor(_states.statsHandler);
            runtimeDeprivedLeg.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Leg;
        }

        /// UNREGISTER ARMORS.
        
        public void UnRegisterHeadArmor()
        {
            headArmorSlot._referedArmorItem.UndoArmorStatsChanges(_states.statsHandler);
            headArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void UnRegisterChestArmor()
        {
            chestArmorSlot._referedArmorItem.UndoArmorStatsChanges(_states.statsHandler);
            chestArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void UnRegisterHandArmor()
        {
            handArmorSlot._referedArmorItem.UndoArmorStatsChanges(_states.statsHandler);
            handArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        public void UnRegisterLegArmor()
        {
            legArmorSlot._referedArmorItem.UndoArmorStatsChanges(_states.statsHandler);
            legArmorSlot.currentSlotSideType = RuntimeArmor.ArmorSlotSideTypeEnum.Backpack;
        }

        /// PREVIEW ARMORS.

        public void PreviewHeadArmor(RuntimeHeadArmor _previewHeadArmor)
        {
            switch (_previewHeadArmor._referedArmorItem.headArmorType)
            {
                case RuntimeHeadArmor.HeadArmorTypeEnum.Cover:
                    _states.headPiecesRenderer[0].enabled = true;
                    _states.headPiecesRenderer[0].sharedMesh = _previewHeadArmor.headMesh[0];
                    _states.headPiecesRenderer[1].sharedMesh = runtimeDeprivedHead.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = false;
                    break;

                case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet:
                    _states.headPiecesRenderer[0].enabled = false;
                    _states.headPiecesRenderer[1].sharedMesh = _previewHeadArmor.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = false;
                    break;

                case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet_Attachment:
                    _states.headPiecesRenderer[0].enabled = false;
                    _states.headPiecesRenderer[1].sharedMesh = _previewHeadArmor.headMesh[0];
                    _states.headPiecesRenderer[2].enabled = true;
                    _states.headPiecesRenderer[2].sharedMesh = _previewHeadArmor.headMesh[1];
                    break;
            }
        }

        public void PreviewChestArmor(RuntimeChestArmor _previewChestArmor)
        {
            for (int i = 0; i < 2; i++)
            {
                _states.chestPiecesRenderer[i].sharedMesh = _previewChestArmor.chestMeshes[i];
            }
        }

        public void PreviewHandArmor(RuntimeHandArmor _previewHandArmor)
        {
            switch (_previewHandArmor._referedArmorItem.handArmorType)
            {
                case RuntimeHandArmor.HandArmorTypeEnum.No_Attachment:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }

                    for (int i = 6; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.All_Attachment:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }

                    for (int i = 6; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.Shoulder_Attachment_Only:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }

                    for (int i = 6; i < 8; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }

                    for (int i = 8; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeHandArmor.HandArmorTypeEnum.Elbow_Attachment_Only:

                    for (int i = 0; i < 6; i++)
                    {
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i];
                    }

                    for (int i = 6; i < 8; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = false;
                    }

                    for (int i = 8; i < 10; i++)
                    {
                        _states.handPiecesRenderer[i].enabled = true;
                        _states.handPiecesRenderer[i].sharedMesh = _previewHandArmor.handMeshes[i - 2];
                    }
                    break;
            }
        }

        public void PreviewLegArmor(RuntimeLegArmor _previewLegArmor)
        {
            switch (_previewLegArmor._referedArmorItem.legArmorTypeEnum)
            {
                case RuntimeLegArmor.LegArmorTypeEnum.No_Attachment:

                    for (int i = 0; i < 2; i++)
                    {
                        _states.legPiecesRenderer[i].sharedMesh = _previewLegArmor.legMeshes[i];
                    }

                    for (int i = 2; i < 2; i++)
                    {
                        _states.legPiecesRenderer[i].enabled = false;
                    }
                    break;

                case RuntimeLegArmor.LegArmorTypeEnum.Kneel_Attachment:

                    for (int i = 0; i < 4; i++)
                    {
                        _states.legPiecesRenderer[i].sharedMesh = _previewLegArmor.legMeshes[i];
                    }
                    break;
            }
        }

        /// REVERSE PREVIEW ARMORS.

        public void ReversePreviewArmors()
        {
            ReversePreviewHeadArmor();
            ReversePreviewChestArmor();
            ReversePreviewHandArmor();
            ReversePreviewLegArmor();

            runtimeAmulet.PlayArmorAbsorbFx();
        }

        void ReversePreviewHeadArmor()
        {
            if (headArmorSlot != null)
            {
                ReversePreviewHeadArmor();
            }
            else
            {
                ReversePreviewDeprivedHead();
            }

            void ReversePreviewHeadArmor()
            {
                switch (headArmorSlot._referedArmorItem.headArmorType)
                {
                    case RuntimeHeadArmor.HeadArmorTypeEnum.Cover:
                        _states.headPiecesRenderer[0].enabled = true;
                        _states.headPiecesRenderer[0].sharedMesh = headArmorSlot.headMesh[0];
                        _states.headPiecesRenderer[1].sharedMesh = runtimeDeprivedHead.headMesh[0];
                        _states.headPiecesRenderer[2].enabled = false;
                        break;

                    case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet:
                        _states.headPiecesRenderer[0].enabled = false;
                        _states.headPiecesRenderer[1].sharedMesh = headArmorSlot.headMesh[0];
                        _states.headPiecesRenderer[2].enabled = false;
                        break;

                    case RuntimeHeadArmor.HeadArmorTypeEnum.Helmet_Attachment:
                        _states.headPiecesRenderer[0].enabled = false;
                        _states.headPiecesRenderer[1].sharedMesh = headArmorSlot.headMesh[0];
                        _states.headPiecesRenderer[2].enabled = true;
                        _states.headPiecesRenderer[2].sharedMesh = headArmorSlot.headMesh[1];
                        break;
                }
            }

            void ReversePreviewDeprivedHead()
            {
                _states.headPiecesRenderer[0].enabled = false;
                _states.headPiecesRenderer[1].sharedMesh = runtimeDeprivedHead.headMesh[0];
                _states.headPiecesRenderer[2].enabled = false;
            }
        }

        void ReversePreviewChestArmor()
        {
            if (chestArmorSlot != null)
            {
                ReversePreviewChestArmor();
            }
            else
            {
                ReversePreviewDeprivedChest();
            }

            void ReversePreviewChestArmor()
            {
                for (int i = 0; i < 2; i++)
                {
                    _states.chestPiecesRenderer[i].sharedMesh = chestArmorSlot.chestMeshes[i];
                }
            }
            
            void ReversePreviewDeprivedChest()
            {
                for (int i = 0; i < 2; i++)
                {
                    _states.chestPiecesRenderer[i].sharedMesh = runtimeDeprivedChest.chestMeshes[i];
                }
            }
        }

        void ReversePreviewHandArmor()
        {
            if (handArmorSlot != null)
            {
                ReversePreviewHandArmor();
            }
            else
            {
                ReversePreviewDeprivedHand();
            }

            void ReversePreviewHandArmor()
            {
                switch (handArmorSlot._referedArmorItem.handArmorType)
                {
                    case RuntimeHandArmor.HandArmorTypeEnum.No_Attachment:

                        for (int i = 0; i < 6; i++)
                        {
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }

                        for (int i = 6; i < 10; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = false;
                        }
                        break;

                    case RuntimeHandArmor.HandArmorTypeEnum.All_Attachment:

                        for (int i = 0; i < 6; i++)
                        {
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }

                        for (int i = 6; i < 10; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = true;
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }
                        break;

                    case RuntimeHandArmor.HandArmorTypeEnum.Shoulder_Attachment_Only:

                        for (int i = 0; i < 6; i++)
                        {
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }

                        for (int i = 6; i < 8; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = true;
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }

                        for (int i = 8; i < 10; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = false;
                        }
                        break;

                    case RuntimeHandArmor.HandArmorTypeEnum.Elbow_Attachment_Only:

                        for (int i = 0; i < 6; i++)
                        {
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i];
                        }

                        for (int i = 6; i < 8; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = false;
                        }

                        for (int i = 8; i < 10; i++)
                        {
                            _states.handPiecesRenderer[i].enabled = true;
                            _states.handPiecesRenderer[i].sharedMesh = handArmorSlot.handMeshes[i - 2];
                        }
                        break;
                }
            }

            void ReversePreviewDeprivedHand()
            {
                for (int i = 0; i < 6; i++)
                {
                    _states.handPiecesRenderer[i].sharedMesh = runtimeDeprivedHand.handMeshes[i];
                }

                for (int i = 6; i < 10; i++)
                {
                    _states.handPiecesRenderer[i].enabled = false;
                }
            }
        }

        void ReversePreviewLegArmor()
        {
            if (legArmorSlot != null)
            {
                ReversePreviewLegArmor();
            }
            else
            {
                ReversePreviewDeprivedLeg();
            }

            void ReversePreviewLegArmor()
            {
                switch (legArmorSlot._referedArmorItem.legArmorTypeEnum)
                {
                    case RuntimeLegArmor.LegArmorTypeEnum.No_Attachment:

                        for (int i = 0; i < 2; i++)
                        {
                            _states.legPiecesRenderer[i].sharedMesh = legArmorSlot.legMeshes[i];
                        }

                        for (int i = 2; i < 4; i++)
                        {
                            _states.legPiecesRenderer[i].enabled = false;
                        }
                        break;

                    case RuntimeLegArmor.LegArmorTypeEnum.Kneel_Attachment:

                        for (int i = 0; i < 4; i++)
                        {
                            _states.legPiecesRenderer[i].sharedMesh = legArmorSlot.legMeshes[i];
                        }
                        break;
                }
            }

            void ReversePreviewDeprivedLeg()
            {
                for (int i = 0; i < 2; i++)
                {
                    _states.legPiecesRenderer[i].sharedMesh = runtimeDeprivedLeg.legMeshes[i];
                }

                for (int i = 2; i < 4; i++)
                {
                    _states.legPiecesRenderer[i].enabled = false;
                }
            }
        }

        /// PARENT TRANSFORMS.

        void ParentArmorUnderBackpack(Transform _runtimeArmorTransform)
        {
            _runtimeArmorTransform.gameObject.SetActive(false);
            _runtimeArmorTransform.parent = armorBackpackTransform;

            _runtimeArmorTransform.localPosition = _states.vector3Zero;
            _runtimeArmorTransform.localEulerAngles = _states.vector3Zero;
            _runtimeArmorTransform.localScale = _states.vector3One;
        }

        /// ADD / REMOVE CARRYING.

        #region Head.
        public void AddHeadArmorToCarrying(RuntimeHeadArmor _runtimeHeadArmor)
        {
            allHeadsPlayerCarrying.Add(_runtimeHeadArmor);
            _isHeadCarryingFilled = true;
        }

        public void RemoveHeadAmorFromCarrying(RuntimeHeadArmor _runtimeHeadArmor)
        {
            allHeadsPlayerCarrying.Remove(_runtimeHeadArmor);
            if (allHeadsPlayerCarrying.Count == 0)
            {
                _isHeadCarryingFilled = false;
            }
        }
        #endregion 

        #region Chest.
        public void AddChestArmorToCarrying(RuntimeChestArmor _runtimeChestArmor)
        {
            allChestsPlayerCarrying.Add(_runtimeChestArmor);
            _isChestCarryingFilled = true;
        }

        public void RemoveChestAmorFromCarrying(RuntimeChestArmor _runtimeChestArmor)
        {
            allChestsPlayerCarrying.Remove(_runtimeChestArmor);
            if (allChestsPlayerCarrying.Count == 0)
            {
                _isChestCarryingFilled = false;
            }
        }
        #endregion 

        #region Hand.
        public void AddHandArmorToCarrying(RuntimeHandArmor _runtimeHandArmor)
        {
            allHandsPlayerCarrying.Add(_runtimeHandArmor);
            _isHandCarryingFilled = true;
        }

        public void RemoveHandAmorFromCarrying(RuntimeHandArmor _runtimeHandArmor)
        {
            allHandsPlayerCarrying.Remove(_runtimeHandArmor);
            if (allHandsPlayerCarrying.Count == 0)
            {
                _isHandCarryingFilled = false;
            }
        }
        #endregion

        #region Legs.
        public void AddLegArmorToCarrying(RuntimeLegArmor _runtimeLegArmor)
        {
            allLegsPlayerCarrying.Add(_runtimeLegArmor);
            _isLegCarryingFilled = true;
        }

        public void RemoveLegAmorFromCarrying(RuntimeLegArmor _runtimeLegArmor)
        {
            allLegsPlayerCarrying.Remove(_runtimeLegArmor);
            if (allLegsPlayerCarrying.Count == 0)
            {
                _isLegCarryingFilled = false;
            }
        }
        #endregion 

        #endregion

        #region Charm.
        /// SETUP EMPTY SLOTS.

        public void SetupCharmEmptySlot(RuntimeCharm _runtimeCharm)
        {
            charmSlot = _runtimeCharm;
            RegisterCharm();
        }

        /// SETUP TAKEN SLOTS.

        public void SetupCharmTakenSlot(RuntimeCharm _runtimeCharm)
        {
            UnRegisterCharm();

            charmSlot = _runtimeCharm;
            RegisterCharm();
        }

        /// EMPTY SLOTS.

        public void EmptyCharmSlot()
        {
            UnRegisterCharm();
            charmSlot = null;
        }

        public void ReturnCharmToBackpack(RuntimeCharm _runtimeCharm)
        {
            ParentCharmUnderBackpack(_runtimeCharm.transform);
            _runtimeCharm.gameObject.SetActive(false);
            _runtimeCharm.currentSlotSide = RuntimeCharm.CharmSlotSideTypeEnum.Backpack;
        }

        /// REGISTER CHARMS.

        void RegisterCharm()
        {
            charmSlot.ChangeStatsWithCharm(_states);
            charmSlot.currentSlotSide = RuntimeCharm.CharmSlotSideTypeEnum.Slot;
        }

        /// UNREGISTER CHARMS.

        void UnRegisterCharm()
        {
            charmSlot.UndoCharmStatsChanges(_states);
            charmSlot.currentSlotSide = RuntimeCharm.CharmSlotSideTypeEnum.Backpack;
        }

        /// PARENT TRANSFORMS.

        public void ParentCharmUnderBackpack(Transform _runtimeCharmTransform)
        {
            _runtimeCharmTransform.parent = charmBackpackTransform;

            _runtimeCharmTransform.localPosition = _states.vector3Zero;
            _runtimeCharmTransform.localEulerAngles = _states.vector3Zero;
            _runtimeCharmTransform.localScale = _states.vector3One;
        }

        /// ADD / REMOVE CARRYING.

        public void AddCharmToCarrying(RuntimeCharm _runtimeCharm)
        {
            allCharmsPlayerCarrying.Add(_runtimeCharm);
            _isCharmCarryingFilled = true;
        }

        public void RemoveCharmFromCarrying(RuntimeCharm _runtimeCharm)
        {
            allCharmsPlayerCarrying.Remove(_runtimeCharm);
            if (allCharmsPlayerCarrying.Count == 0)
            {
                _isCharmCarryingFilled = false;
            }
        }
        #endregion

        #region Powerup.
        /// SETUP EMPTY SLOTS.

        public void SetupPowerupEmptySlot(RuntimePowerup _runtimePowerup)
        {
            powerupSlot = _runtimePowerup;

            _states.UnCombineSkinnedMesh();
            RegisterCurrentPowerup();
        }

        /// SETUP TAKEN SLOTS.
        
        public void SetupPowerupTakenSlot(RuntimePowerup _runtimePowerup)
        {
            _states.UnCombineSkinnedMesh();
            UnRegisterCurrentPowerup();

            powerupSlot = _runtimePowerup;
            RegisterCurrentPowerup();
        }

        /// EMPTY SLOTS.
        
        public void EmptyPowerupSlot()
        {
            _states.UnCombineSkinnedMesh();
            UnRegisterCurrentPowerup();

            powerupSlot = null;
        }

        public void ReturnPowerupToBackpack(RuntimePowerup _runtimePowerup)
        {
            ParentPowerupUnderBackpack(_runtimePowerup.transform);
            _runtimePowerup.currentSlotSide = RuntimePowerup.PowerupSlotSideTypeEnum.Backpack;
        }

        /// REGISTER POWERUPS.
        
        public void RegisterCurrentPowerup()
        {
            _states.backPiecesRenderer.enabled = true;
            _states.backPiecesRenderer.sharedMesh = powerupSlot.powerupMesh;
            
            _isPowerupHideShield = true;
            HandleWeaponsVisibility();

            powerupSlot.ChangeStatsWithPowerup(_states.statsHandler);
            powerupSlot.currentSlotSide = RuntimePowerup.PowerupSlotSideTypeEnum.Slot;
        }

        /// UNREGISTER POWERUPS.

        public void UnRegisterCurrentPowerup()
        {
            _states.backPiecesRenderer.enabled = false;

            _isPowerupHideShield = false;
            HandleWeaponsVisibility();

            powerupSlot.UndoPowerupStatsChanges(_states.statsHandler);
            powerupSlot.currentSlotSide = RuntimePowerup.PowerupSlotSideTypeEnum.Backpack;
        }

        /// PREVIEW POWERUPS.

        public void PreviewPowerup()
        {
            /// If player isn't equipping any Powerup yet.
            if (!powerupSlot)
            {
                _states.backPiecesRenderer.enabled = true;

                _isPowerupHideShield = true;
                TryUnHide_Visible_Shields();
                HandleWeaponsVisibility();
            }

            _states.backPiecesRenderer.sharedMesh = _pickedUpReadyDissolvePowerup.powerupMesh;
        }

        /// REVERSE PREVIEW POWERUPS.
        
        public void OnCompleteDissolvePreviewPowerup()
        {
            /// If player isn't equipping any Powerup yet.
            if (!powerupSlot)
            {
                _isPowerupHideShield = false;
                HandleWeaponsVisibility();

                _states.OnCompleteDissolvePowerup_WithoutPowerupEquip();
            }
            
            runtimeAmulet.PlayPowerupAbsorbFx();
        }

        /// PARENT TRANSFORMS.

        void ParentPowerupUnderBackpack(Transform _runtimePowerupTransform)
        {
            _runtimePowerupTransform.gameObject.SetActive(false);
            _runtimePowerupTransform.parent = powerupBackpackTransform;

            _runtimePowerupTransform.localPosition = _states.vector3Zero;
            _runtimePowerupTransform.localEulerAngles = _states.vector3Zero;
            _runtimePowerupTransform.localScale = _states.vector3One;
        }

        /// ADD / REMOVE CARRYING.

        public void AddPowerupToCarrying(RuntimePowerup _runtimePowerup)
        {
            allPowerupsPlayerCarrying.Add(_runtimePowerup);
            _isPowerupCarryingFilled = true;
        }

        public void RemovePowerupFromCarrying(RuntimePowerup _runtimePowerup)
        {
            allPowerupsPlayerCarrying.Remove(_runtimePowerup);
            if (allPowerupsPlayerCarrying.Count == 0)
            {
                _isPowerupCarryingFilled = false;
            }
        }
        #endregion

        #region Ring.
        /// SETUP EMPYT SLOTS.

        public void InitRightRingSlot(RuntimeRing _runtimeRing)
        {
            rightRingSlot = _runtimeRing;
            RegisterRightRing();
        }

        public void InitLeftRingSlot(RuntimeRing _runtimeRing)
        {
            leftRingSlot = _runtimeRing;
            RegisterLeftRing();
        }

        public void SetupRightRingEmptySlot(RuntimeRing _overwriteRing)
        {
            if (_overwriteRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Left)
                EmptyLeftRingSlot();

            rightRingSlot = _overwriteRing;
            RegisterRightRing();
        }

        public void SetupLeftRingEmptySlot(RuntimeRing _overwriteRing)
        {
            if (_overwriteRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Right)
                EmptyRightRingSlot();

            leftRingSlot = _overwriteRing;
            RegisterLeftRing();
        }
        
        /// SETUP TAKEN SLOTS.

        public void SetupRightRingTakenSlot(RuntimeRing _runtimeRing)
        {
            if (_runtimeRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Backpack)
            {
                UnRegisterRightRing();

                rightRingSlot = _runtimeRing;
                RegisterRightRing();
            }
            else
            {
                if (_runtimeRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Left)
                {
                    RuntimeRing _takenSlotRing = rightRingSlot;
                    EmptyRightRingSlot();

                    leftRingSlot = _takenSlotRing;
                    RegisterLeftRing();

                    rightRingSlot = _runtimeRing;
                    RegisterRightRing();
                }
            }
        }

        public void SetupLeftRingTakenSlot(RuntimeRing _runtimeRing)
        {
            if (_runtimeRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Backpack)
            {
                EmptyLeftRingSlot();

                leftRingSlot = _runtimeRing;
                RegisterLeftRing();
            }
            else
            {
                if (_runtimeRing.currentSlotSide == RuntimeRing.RingSlotSideTypeEnum.Right)
                {
                    RuntimeRing _takenSlotRing = leftRingSlot;
                    EmptyLeftRingSlot();

                    rightRingSlot = _takenSlotRing;
                    RegisterRightRing();

                    leftRingSlot = _runtimeRing;
                    RegisterLeftRing();
                }
            }
        }

        /// EMPTY SLOTS.

        public void EmptyRightRingSlot()
        {
            UnRegisterRightRing();
            rightRingSlot = null;
        }

        public void EmptyLeftRingSlot()
        {
            UnRegisterLeftRing();
            leftRingSlot = null;
        }

        /// REGISTER RINGS.

        void RegisterRightRing()
        {
            ParentRingUnderRightFinger(rightRingSlot.transform);

            rightRingSlot.gameObject.SetActive(true);
            rightRingSlot.runtimeRingParticle.gameObject.SetActive(true);

            rightRingSlot.ChangeStatsWithRing(_states.statsHandler);
            rightRingSlot.currentSlotSide = RuntimeRing.RingSlotSideTypeEnum.Right;
        }

        void RegisterLeftRing()
        {
            ParentRingUnderLeftFinger(leftRingSlot.transform);

            leftRingSlot.gameObject.SetActive(true);
            leftRingSlot.runtimeRingParticle.gameObject.SetActive(true);

            leftRingSlot.ChangeStatsWithRing(_states.statsHandler);
            leftRingSlot.currentSlotSide = RuntimeRing.RingSlotSideTypeEnum.Left;
        }

        public void ReturnRingToBackpack(RuntimeRing _runtimeRing)
        {
            ParentRingUnderBackpack(_runtimeRing.transform);
            _runtimeRing.currentSlotSide = RuntimeRing.RingSlotSideTypeEnum.Backpack;
        }

        /// UNREGISTER RINGS.

        void UnRegisterRightRing()
        {
            rightRingSlot.UndoRingStatsChanges(_states.statsHandler);
            ReturnRingToBackpack(rightRingSlot);
        }

        void UnRegisterLeftRing()
        {
            leftRingSlot.UndoRingStatsChanges(_states.statsHandler);
            ReturnRingToBackpack(leftRingSlot);
        }

        /// PREVIEWS RINGS.
        
        public void PreviewRing(Transform _ringTrailTransform)
        {
            _ringFlyingTrailTransform = _ringTrailTransform;

            if (rightRingSlot != null)
            {
                rightRingSlot.gameObject.SetActive(false);
                rightRingSlot.runtimeRingParticle.gameObject.SetActive(false);
            }

            ParentRingUnderRightFinger(_ringFlyingTrailTransform);
        }

        /// REVERSE PREVIEW RINGS.
        
        public void ReversePreviewRing()
        {
            _ringFlyingTrailTransform.parent = INV_ItemsEffectsBackpackTransform;

            if (rightRingSlot != null)
            {
                rightRingSlot.gameObject.SetActive(true);
                rightRingSlot.runtimeRingParticle.gameObject.SetActive(true);
            }
        }

        /// PARENT TRANSFORMS.

        void ParentRingUnderRightFinger(Transform _runtimeRingTransform)
        {
            _runtimeRingTransform.parent = _states.rightIndexIntermediateTransform;

            _runtimeRingTransform.localPosition = _states.vector3Zero;
            _runtimeRingTransform.localEulerAngles = _states.vector3Zero;
            _runtimeRingTransform.localScale = _states.vector3One;
        }

        void ParentRingUnderLeftFinger(Transform _runtimeRingTransform)
        {
            _runtimeRingTransform.parent = _states.leftIndexIntermediateTransform;

            _runtimeRingTransform.localPosition = _states.vector3Zero;
            _runtimeRingTransform.localEulerAngles = _states.vector3Zero;
            _runtimeRingTransform.localScale = _states.vector3One;
        }

        void ParentRingUnderBackpack(Transform _runtimeRingTransform)
        {
            _runtimeRingTransform.gameObject.SetActive(false);
            _runtimeRingTransform.parent = ringBackpackTransform;

            _runtimeRingTransform.localPosition = _states.vector3Zero;
            _runtimeRingTransform.localEulerAngles = _states.vector3Zero;
            _runtimeRingTransform.localScale = _states.vector3One;
        }

        /// GET FORTIFIED NAME.

        public string GetFortifiedRingName(RuntimeRing _runtimeRing)
        {
            _inventoryStrBuilder.Clear();
            _inventoryStrBuilder.Append(_runtimeRing._referedRingItem.itemName);
            int _fortifiedLevel = _runtimeRing._fortifiedLevel;
            if (_fortifiedLevel != 0)
            {
                _inventoryStrBuilder.Append(" + ").Append(_fortifiedLevel);
            }
            
            return _inventoryStrBuilder.ToString();
        }

        /// ADD / REMOVE CARRYING.

        public void AddRingToCarrying(RuntimeRing _runtimeRing)
        {
            allRingsPlayerCarrying.Add(_runtimeRing);
            _isRingCarryingFilled = true;
        }

        public void RemoveRingFromCarrying(RuntimeRing _runtimeRing)
        {
            allRingsPlayerCarrying.Remove(_runtimeRing);
            if (allRingsPlayerCarrying.Count == 0)
            {
                _isRingCarryingFilled = false;
            }
        }
        #endregion

        #region Consumable.
        /// SETUP DEFAULT.
        
        public void SetupVolunVesselsDefault(VolunVesselConsumable _volunVessel)
        {
            runtimeVolunVessel = _volunVessel;
            BringConsumableToSlot(_volunVessel, 0);
            RegisterCurrentConsumable(runtimeVolunVessel);

            SetupVolunCarryStoredAmount();

            void SetupVolunCarryStoredAmount()
            {
                int _amount = _states.statsHandler.b_volunVessel_amount;
                runtimeVolunVessel.curCarryingAmount = _amount;
                runtimeVolunVessel.curStoredAmount = _amount;
            }
        }

        public void SetupSodurVesselsDefault(SodurVesselConsumable _sodurVessel)
        {
            runtimeSodurVessel = _sodurVessel;
            BringConsumableToSlot(_sodurVessel, 1);

            SetupSodurCarryStoredAmount();

            void SetupSodurCarryStoredAmount()
            {
                int _amount = _states.statsHandler.b_sodurVessel_amount;
                runtimeSodurVessel.curCarryingAmount = _amount;
                runtimeSodurVessel.curStoredAmount = _amount;
            }
        }

        void RefreshVesselAmount()
        {
            runtimeVolunVessel.RefillCarryingVolunAmount(_states.statsHandler.b_volunVessel_amount);
            runtimeSodurVessel.RefillCarryingSodurAmount(_states.statsHandler.b_sodurVessel_amount);
            RefreshNextTwoConsumables();
        }

        /// SETUP SLOTS.

        public void InitConsumableSlot(RuntimeConsumable _runtimeConsumable, int _consumSlotNum)
        {
            BringConsumableToSlot(_runtimeConsumable, _consumSlotNum);
        }

        public void LoadSavedConsumableSlot(RuntimeConsumable _runtimeConsumable, int _consumSlotNum)
        {
            BringConsumableToSlot(_runtimeConsumable, _consumSlotNum);

            if (_runtimeConsumable._isCurrentConsumable)
            {
                d_index = _consumSlotNum;
                RegisterCurrentConsumable(_runtimeConsumable);
            }
        }

        void BringConsumableToSlot(RuntimeConsumable _consumableToBringOut, int _targetSlotNum)
        {
            _consumableToBringOut.slotNumber = _targetSlotNum;
            consumableSlots[_targetSlotNum] = _consumableToBringOut;
            _consumableToBringOut.currentSlotSide = RuntimeConsumable.ConsumableSlotSideTypeEnum.Slot;
        }

        /// SETUP EMPTY SLOTS.

        public void SetupConsumableEmptySlot(RuntimeConsumable _overwriteConsumable, int _consumSlotNum)
        {
            if (_overwriteConsumable.currentSlotSide == RuntimeConsumable.ConsumableSlotSideTypeEnum.Slot)
            {
                consumableSlots[_overwriteConsumable.slotNumber] = null;
            }

            bool _isConsumableSlotsEmpty = !FindFirstConsumableFromSlots();

            BringConsumableToSlot(_overwriteConsumable, _consumSlotNum);

            if (_isConsumableSlotsEmpty || _overwriteConsumable._isCurrentConsumable)
            {
                d_index = _consumSlotNum;
                RegisterCurrentConsumable(_overwriteConsumable);
            }

            RefreshNextTwoConsumables();
        }

        /// SETUP TAKEN SLOTS.

        public void SetupConsumableTakenSlot(RuntimeConsumable _overwriteConsumable, int _consumSlotNum)
        {
            /// Check if the taken slot consumable is current consumable.
            bool _isTakenSlotCurrent = consumableSlots[_consumSlotNum]._isCurrentConsumable;

            RuntimeConsumable _takenSlotConsumable = consumableSlots[_consumSlotNum];
            EmptyConsumableSlot(_consumSlotNum);

            if (_overwriteConsumable.currentSlotSide == RuntimeConsumable.ConsumableSlotSideTypeEnum.Backpack)
            {
                /// If overwrite consumable is from backpack, return taken slot consumable to backpack.
                SetConsumableSlotSideToBackpack(_takenSlotConsumable);
            }
            else
            {
                /// If overwrite consumable is from other slot, set taken slot consumable to overwrite consumable slots.
                BringConsumableToSlot(_takenSlotConsumable, _overwriteConsumable.slotNumber);
            }

            /// Bring overwrite consumable to taken slot, and set it as current consumable if the previous consumable was current.
            BringConsumableToSlot(_overwriteConsumable, _consumSlotNum);
            if (_isTakenSlotCurrent)
            {
                d_index = _consumSlotNum;
                RegisterCurrentConsumable(_overwriteConsumable);
            }

            RefreshNextTwoConsumables();
        }
        
        /// EMPTY SLOTS.
        
        public void EmptyConsumableSlot(int _consumSlotNum)
        {
            RuntimeConsumable _consumableToReturn = consumableSlots[_consumSlotNum];
            consumableSlots[_consumSlotNum] = null;

            if (_consumableToReturn._isCurrentConsumable)
                UnRegisterCurrentConsumable();

            RegisterNextConsumableAsCurrent();
        }

        public void EmptyConsumableSlotInMenu(int _consumSlotNum)
        {
            RuntimeConsumable _consumableToReturn = consumableSlots[_consumSlotNum];
            consumableSlots[_consumSlotNum] = null;

            SetConsumableSlotSideToBackpack(_consumableToReturn);

            if (_consumableToReturn._isCurrentConsumable)
                UnRegisterCurrentConsumable();

            RegisterNextConsumableAsCurrent();
        }

        public void SetConsumableSlotSideToBackpack(RuntimeConsumable _consumableToReturn)
        {
            _consumableToReturn.currentSlotSide = RuntimeConsumable.ConsumableSlotSideTypeEnum.Backpack;
        }

        public void TransferConsumableToStorage(RuntimeConsumable _consumableToTransfer)
        {
            ParentConsumableUnderBackpack(_consumableToTransfer.transform);
            _consumableToTransfer.currentSlotSide = RuntimeConsumable.ConsumableSlotSideTypeEnum.Storage;
        }

        void RegisterNextConsumableAsCurrent()
        {
            RuntimeConsumable _nexConsumable = FindFirstConsumableFromSlots();
            if (_nexConsumable != null)
            {
                d_index = _nexConsumable.slotNumber;
                RegisterCurrentConsumable(_nexConsumable);
                RefreshNextTwoConsumables();
            }
        }
        
        /// REGISTER CURRENTS.

        public void RegisterCurrentConsumable(RuntimeConsumable _runtimeConsumable)
        {
            _consumable = _runtimeConsumable;
            _runtimeConsumable._isCurrentConsumable = true;
            _consumable_referedItem = _consumable._baseConsumableItem;
            _mainHudManager.RegisterConsumQSlotIcon();
        }

        public void RefreshNextTwoConsumables()
        {
            #region Next Consumable.
            RuntimeConsumable _nextConsumable = FindNextConsumable();
            if (_nextConsumable != null)
            {
                SetIsShowNextConsumable(true, _nextConsumable);

                #region 2nd Next Consumable.
                RuntimeConsumable _2ndNextConsumable = Find2ndNextConsumable(_nextConsumable.slotNumber);
                if (_2ndNextConsumable != null)
                {
                    SetIsShow2ndNextConsumable(true, _2ndNextConsumable);
                }
                else
                {
                    SetIsShow2ndNextConsumable(false);
                }
                #endregion

            }
            else
            {
                SetIsShowNextConsumable(false);
                SetIsShow2ndNextConsumable(false);
            }
            #endregion
        }

        void SetIsShow2ndNextConsumable(bool _isShow2ndNextConsumable, RuntimeConsumable _2ndNextConsumable = null)
        {
            if (_isShow2ndNextConsumable)
            {
                if (!this._isShow2ndNextConsumable)
                {
                    this._isShow2ndNextConsumable = true;
                    _mainHudManager.Show2ndNextConsumableQSlot(_2ndNextConsumable);
                }
                else
                {
                    _mainHudManager.RegisterPreConsumableQSlotIcon(_2ndNextConsumable);
                }
            }
            else
            {
                if (this._isShow2ndNextConsumable)
                {
                    this._isShow2ndNextConsumable = false;
                    _mainHudManager.Hide2ndNextConsumableQSlot();
                }
            }
        }

        void SetIsShowNextConsumable(bool _isShowNextConsumable, RuntimeConsumable _nextConsumable = null)
        {
            if (_isShowNextConsumable)
            {
                if (!this._isShowNextConsumable)
                {
                    this._isShowNextConsumable = true;
                    _mainHudManager.ShowNextConsumableQSlot(_nextConsumable);
                }
                else
                {
                    _mainHudManager.RegisterNextConsumableQSlotIcon(_nextConsumable);
                }
            }
            else
            {
                if (this._isShowNextConsumable)
                {
                    this._isShowNextConsumable = false;
                    _mainHudManager.HideNextConsumableQSlot();
                }
            }
        }

        /// UNREGISTER CURRENTS.

        public void UnRegisterCurrentConsumable()
        {
            _consumable._isCurrentConsumable = false;
            _consumable = null;
            _consumable_referedItem = null;
            _mainHudManager.EmptyConsumQSlotIcon();

            SetIsShow2ndNextConsumable(false);
            SetIsShowNextConsumable(false);
        }

        /// PARENT TRANSFORMS.

        void ParentConsumableUnderHand(Transform _consumableTransform)
        {
            _consumableTransform.parent = _states.rightHandTransform;

            _consumableTransform.localPosition = _states.vector3Zero;
            _consumableTransform.localEulerAngles = _states.vector3Zero;
        }

        public void ParentConsumableUnderBackpack(Transform _consumableTransform)
        {
            _consumableTransform.gameObject.SetActive(false);
            _consumableTransform.parent = consumableBackpackTransform;

            _consumableTransform.localPosition = _states.vector3Zero;
            _consumableTransform.localEulerAngles = _states.vector3Zero;
            _consumableTransform.localScale = _states.vector3One;
        }

        /// UTILITY.

        public RuntimeConsumable FindFirstConsumableFromSlots()
        {
            for (int i = 0; i < consumableSlotsLength; i++)
            {
                if (consumableSlots[i] != null)
                {
                    return consumableSlots[i];
                }
            }

            return null;
        }

        /// This Method Will Overwrite d_index.
        public RuntimeConsumable GetNextConsumableFromSlots()
        {
            int tempIndex = d_index;
            d_index++;
            d_index = (d_index == consumableSlotsLength) ? 0 : d_index;

            if (consumableSlots[d_index] == null)
            {
                int j = d_index;
                for (int i = 0; i < consumableSlotsLength - 1; i++)
                {
                    j++;
                    j = (j == consumableSlotsLength) ? 0 : j;

                    if (consumableSlots[j] != null)
                    {
                        d_index = j;
                        return consumableSlots[j];
                    }

                    if (j == d_index)
                    {
                        d_index = tempIndex;
                        return null;
                    }
                }
            }
            return consumableSlots[d_index];
        }

        /// This Method Will Not Overwrite d_index.
        public RuntimeConsumable FindNextConsumable()
        {
            int tempIndex = d_index;
            tempIndex++;
            tempIndex = (tempIndex == consumableSlotsLength) ? 0 : tempIndex;

            if (consumableSlots[tempIndex] == null)
            {
                for (int i = 0; i < consumableSlotsLength - 1; i++)
                {
                    tempIndex++;
                    tempIndex = (tempIndex == consumableSlotsLength) ? 0 : tempIndex;
                    
                    if (tempIndex == d_index)
                    {
                        return null;
                    }
                    else
                    {
                        if (consumableSlots[tempIndex] != null)
                        {
                            return consumableSlots[tempIndex];
                        }
                    }
                }
            }

            return consumableSlots[tempIndex];
        }

        public RuntimeConsumable FindPreConsumable()
        {
            int tempIndex = d_index;
            tempIndex--;
            tempIndex = (tempIndex < 0) ? consumableSlotsLength - 1 : tempIndex;

            if (consumableSlots[tempIndex] == null)
            {
                for (int i = 0; i < consumableSlotsLength - 1; i++)
                {
                    tempIndex--;
                    tempIndex = (tempIndex < 0) ? consumableSlotsLength - 1 : tempIndex;
                    
                    if (tempIndex == d_index)
                    {
                        return null;
                    }
                    else
                    {
                        if (consumableSlots[tempIndex] != null)
                        {
                            return consumableSlots[tempIndex];
                        }
                    }
                }
            }

            return consumableSlots[tempIndex];
        }

        public RuntimeConsumable Find2ndNextConsumable(int _nextConsumableIndex)
        {
            int tempIndex = _nextConsumableIndex;
            tempIndex++;
            tempIndex = (tempIndex == consumableSlotsLength) ? 0 : tempIndex;

            if (consumableSlots[tempIndex] == null)
            {
                for (int i = 0; i < consumableSlotsLength - 1; i++)
                {
                    tempIndex++;
                    tempIndex = (tempIndex == consumableSlotsLength) ? 0 : tempIndex;

                    if (tempIndex == d_index)
                    {
                        return null;
                    }
                    else
                    {
                        if (consumableSlots[tempIndex] != null)
                        {
                            return consumableSlots[tempIndex];
                        }
                    }
                }
            }
            else if (tempIndex == d_index)
            {
                for (int i = 0; i < consumableSlotsLength - 1; i++)
                {
                    tempIndex++;
                    tempIndex = (tempIndex == consumableSlotsLength) ? 0 : tempIndex;

                    if (tempIndex == _nextConsumableIndex)
                    {
                        return null;
                    }
                    else
                    {
                        if (consumableSlots[tempIndex] != null)
                        {
                            return consumableSlots[tempIndex];
                        }
                    }
                }
            }

            return consumableSlots[tempIndex];
        }

        /// CONSUMABLE ACTION.
        public void HandleConsumableAction()
        {
            if (_states.p_useConsum_input)
            {
                if (_consumable != null)
                {
                    if (_consumable.curCarryingAmount > 0)
                    {
                        _states._currentNeglectInputAction = consumableAction;
                    }
                    else
                    {
                        if (runtimeVolunVessel == _consumable || runtimeSodurVessel == _consumable)
                        {
                            PrepareEmptyVessel();
                        }
                    }
                }
            }
        }

        public void PrepareConsumable()
        {
            SetIsUsingConsumableStatusToTrue();
            CheckCanMoveWhileUsingConsumable();

            _states.PlayConsumableAnimation();

            _playerIKHandler.HandleLookAtIKWhenUsingConsumable(_consumable_referedItem);

            _consumable._referedEffect.PrepareConsumableEffect();
            _consumableToHide = _consumable;
        }

        public void UseConsumableInAnim()
        {
            _consumable.curCarryingAmount--;
            if (_consumable.isStatsEffectConsumable)
            {
                StatsEffectConsumable _statsEffectConsumable = _consumable.GetStatsEffectConsumable();
                if (!_statsEffectConsumable._referedStatsEffectItem.isDurational)
                {
                    _statsEffectConsumable.ExecuteStatsEffect(_states.statsHandler);
                }
                else
                {
                    /// Register Stats Effect Job.
                    _states.statsHandler.RegisterNewStatsEffectJob(_statsEffectConsumable);
                }
            }

            _currentConsumableEffect.PlayConsumableEffect();
        }

        public void HideCurrentConsumableInAnim()
        {
            ParentConsumableUnderBackpack(_consumableToHide.transform);
            _consumableToHide.gameObject.SetActive(false);
        }

        public void PrepareEmptyVessel()
        {
            SetIsUsingConsumableStatusToTrue();

            _states.canMoveWhileNeglect = false;
            _states.PlayEmptyVesselAnimation();
        }

        /// SET STATUS.

        void SetIsUsingConsumableStatusToTrue()
        {
            _isUsingConsumable = true;
            
            /// Hide RH Weapons.
            if (_states._isTwoHanding)
            {
                if (!_states._isInTwoHandFist)
                {
                    _twoHandingWeapon.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!_isRightUnarmed)
                {
                    _rightHandWeapon.gameObject.SetActive(false);
                }
            }

            ParentConsumableUnderHand(_consumable.transform);
            _consumable.gameObject.SetActive(true);
        }

        void CheckCanMoveWhileUsingConsumable()
        {
            if (_consumable_referedItem.canMoveWhileUsing)
            {
                _states.canMoveWhileNeglect = true;
            }
            else
            {
                _states.OnConsumableResetInputsStatus();
            }
        }

        public void SetIsUsingConsumableStatusToFalse()
        {
            _isUsingConsumable = false;
            
            if (_states._isTwoHanding)
            {
                /// Show TH Weapons Again.
                if (!_states._isInTwoHandFist)
                {
                    _twoHandingWeapon.gameObject.SetActive(true);
                }
            }
            else
            {
                /// Show RH Weapons Again.
                if (!_isRightUnarmed)
                {
                    _rightHandWeapon.gameObject.SetActive(true);
                }
            }
            
            _consumable.CheckIsCarryingEmpty();
        }

        #region GET FORTIFIED NAME.
        public string GetFortifiedConsumableName(StatsEffectConsumable _statsEffectConsumable)
        {
            _inventoryStrBuilder.Clear();
            _inventoryStrBuilder.Append(_statsEffectConsumable._referedStatsEffectItem.itemName);
            if (_statsEffectConsumable._fortifiedLevel != 0)
            {
                _inventoryStrBuilder.Append(" + ").Append(_statsEffectConsumable._fortifiedLevel);
            }

            return _inventoryStrBuilder.ToString();
        }
        #endregion

        #region DESTROY CONSUMABLES.
        public void OnDestroyConsumable(RuntimeConsumable _consumableToDestroy)
        {
            // Clear refs inside savableInventory.
            EmptyConsumableSlot(_consumableToDestroy.slotNumber);

            // Clear refs inside dictionary and allConsumablesCarrying.
            RemoveConsumableFromCarrying(_consumableToDestroy);
        }
        #endregion

        #region ADD / REMOVE CARRYING.
        public RuntimeConsumable GetConsumableFromDict(int _id)
        {
            if (carryingConsumablesDict.TryGetValue(_id, out RuntimeConsumable _result))
            {
                return _result;
            }
            else
            {
                return null;
            }
        }

        public void AddCarryConsumableToDictionary(RuntimeConsumable _runtimeConsumable)
        {
            carryingConsumablesDict.Add(_runtimeConsumable._stackId, _runtimeConsumable);
            allConsumablesPlayerCarrying.Add(_runtimeConsumable);
        }

        void RemoveConsumableFromCarrying(RuntimeConsumable _runtimeConsumable)
        {
            if (carryingConsumablesDict.ContainsKey(_runtimeConsumable._stackId))
            {
                carryingConsumablesDict.Remove(_runtimeConsumable._stackId);
                allConsumablesPlayerCarrying.Remove(_runtimeConsumable);
            }
        }
        #endregion

        #endregion

        #region Spell
        public void SetSpell(SpellItem s, bool updateReferences)
        {
            _spell = s;
            if (updateReferences)
            {
                /// MainHud Update Spell Icon.
            }
        }

        public SpellItem GetNextSpellOnSlot()
        {
            SpellItem[] spell = spellSlots;
            u_index++;
            if(u_index > spellSlots.Length - 1)
            {
                u_index = 0;
            }

            int targetIndex = u_index;
            if (spell[targetIndex] == null)
            {
                for (int i = targetIndex + 1; i <= spell.Length; i++)
                {
                    i = (i == 4) ? 0 : i;

                    if (spell[i] != null)
                    {
                        u_index = i;
                        return spell[i];
                    }

                    if (i == targetIndex)
                        return null;
                }
            }

            return spell[targetIndex];           
        }

        public void SetSpellActive()
        {
            for (int i = 0; i < spellSlots.Length; i++)
            {
                if(spellSlots[i] != null)
                {
                    if (spellSlots[i].spellParticleInstance != null)
                        spellSlots[i].spellParticleInstance.SetActive(false);
                }
            }
        }
        #endregion

        #region Amulet.
        public void RefreshAmuletEmissionWhenAIKilled()
        {
            runtimeAmulet.PlayVolunAbsorbFx();
        }

        public void RefreshAmuletEmissionWhenPickupWeapon()
        {
            runtimeAmulet.PlayWeaponAbsorbFx();
        }

        public void RefreshAmuletEmissionWithPickupConsums()
        {
            runtimeAmulet.PlayConsumableAbsorbFx();
        }
        #endregion
        
        #region Neglect Inputs.
        public void HandleWeaponInputs(bool _isTwoHanding)
        {
            if (_isTwoHanding)
            {
                _twoHandingWeapon_referedItem.HandleTwoHandingWeaponInput(_states);
            }
            else
            {
                _rightHandWeapon_referedItem.HandleDominantHandWeaponInput(_states);
                _leftHandWeapon_referedItem.HandleOpposeHandWeaponInput(_states);
            }
        }
        #endregion
        
        #region Switch QSlot.
        public void SwitchLeftWeapon()
        {
            RuntimeWeapon nextRuntimeWeapon = GetNextLhWeaponFromSlots();
            if (nextRuntimeWeapon != null)
            {
                if (_leftHandWeapon && !_isLeftUnarmed)
                {
                    SheathLhWeapon(_leftHandWeapon);
                    UnRegisterLhCurrentWeapon();
                }

                RegisterLhCurrentWeapon(nextRuntimeWeapon);

                _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();
                _mainHudManager.RegisterLhWeaponQSlot();
                SetIsLeftUnarmedStatus(false);
                HandleWeaponsVisibility();
                UnSheathLhWeapon();
            }
            else
            {
                if (!_leftHandWeapon)
                {
                    SetIsLeftUnarmedStatus(true);
                }
            }
        }

        public void SwitchRightWeapon()
        {
            RuntimeWeapon nextRuntimeWeapon = GetNextRhWeaponFromSlots();
            if (nextRuntimeWeapon != null)
            {
                if (_rightHandWeapon && !_isRightUnarmed)
                {
                    SheathRhWeapon(_rightHandWeapon);
                    UnRegisterRhCurrentWeapon();
                }

                RegisterRhCurrentWeapon(nextRuntimeWeapon);

                _playerIKHandler.RegisterRhWeaponIK();
                _mainHudManager.RegisterRhWeaponQSlot();
                SetIsRightUnarmedStatus(false);
                HandleWeaponsVisibility();
                UnSheathRhWeapon();
            }
            else
            {
                if (!_rightHandWeapon)
                {
                    SetIsRightUnarmedStatus(true);
                }
            }
        }

        public void SwitchConsumable()
        {
            RuntimeConsumable nextRuntimeConsumable = GetNextConsumableFromSlots();
            if (nextRuntimeConsumable != null)
            {
                if (_consumable)
                {
                    _consumable._isCurrentConsumable = false;
                }

                RegisterCurrentConsumable(nextRuntimeConsumable);
                RefreshNextTwoConsumables();
            }
        }
        #endregion
        
        #region Get Execution Anim Hash / Weapon.
        public int GetParryExecutePresentHashByType(bool _isTwoHanding)
        {
            if (_isTwoHanding)
            {
               return _twoHandingWeapon_referedItem.GetParryExecutePresentHashByType();
            }
            else
            {
                return _rightHandWeapon_referedItem.GetParryExecutePresentHashByType();
            }
        }
        #endregion

        #region On Death Dissolve Out Equipment.
        public void OnDeathDissolveOutEquipments()
        {
            /// Dissolve Out Powerup.
            if (powerupSlot != null)
                _states.DissolveOutPowerup();

            /// Dissolve Out Visual Weapons.
            if (_curVisibleLhWeapon != null)
            {
                if (_curVisibleLhWeapon.isShield)
                {
                    if (!_isHideCurrentVisibleShields && !_isPowerupHideShield)
                    {
                        _curVisibleLhWeapon.DissolveOutWeapon();
                        _isOnDeathDissolved_Visible_LH_Weapon = true;
                    }
                }
                else
                {
                    _curVisibleLhWeapon.DissolveOutWeapon();
                    _isOnDeathDissolved_Visible_LH_Weapon = true;
                }
            }
            
            if (_curVisibleRhWeapon != null)
            {
                if (_curVisibleRhWeapon.isShield)
                {
                    if (!_isHideCurrentVisibleShields && !_isPowerupHideShield)
                    {
                        _curVisibleRhWeapon.DissolveOutWeapon();
                        _isOnDeathDissolved_Visible_RH_Weapon = true;
                    }
                }
                else
                {
                    _curVisibleRhWeapon.DissolveOutWeapon();
                    _isOnDeathDissolved_Visible_RH_Weapon = true;
                }
            }

            /// Dissolve Out Weapons.
            OnDeathDissolveOutWeawpons();

            /// Deactivate Amulet. (Future maybe dissolve)
            DeactivateAmulet_AfterWait();

            void DeactivateAmulet_AfterWait()
            {
                LeanTween.value(0, 1, 0.2f).setOnComplete(OnWaitComplete);

                void OnWaitComplete()
                {
                    runtimeAmulet.gameObject.SetActive(false);
                }
            }
        }
        #endregion

        #region On Revive Dissolve In Equipment.
        public void OnReviveDissolveInEquipments()
        {
            /// Dissolve In Powerup.
            if (powerupSlot != null)
                _states.DissolveInPowerup();

            /// Dissolve In Visual Weapons.
            if (_isOnDeathDissolved_Visible_LH_Weapon)
            {
                _isOnDeathDissolved_Visible_LH_Weapon = false;
                _curVisibleLhWeapon.DissolveInWeapon();
            }
            
            if (_isOnDeathDissolved_Visible_RH_Weapon)
            {
                _isOnDeathDissolved_Visible_RH_Weapon = false;
                _curVisibleRhWeapon.DissolveInWeapon();
            }

            /// Dissolve In Weapons.
            OnReviveDissolveInWeapons();

            /// Activate Amulet. (Future maybe dissolve)
            ActivateAmulet_AfterWait();

            void ActivateAmulet_AfterWait()
            {
                LeanTween.value(0, 1, 0.2f).setOnComplete(OnWaitComplete);

                void OnWaitComplete()
                {
                    runtimeAmulet.gameObject.SetActive(true);
                }
            }
        }
        #endregion

        #region Checkpoint Event.
        public void RefreshPlayerStatsAction()
        {
            RefreshVesselAmount();
        }
        #endregion

        #region Serialization.

        #region SAVE.

        #region Weapon.
        public List<SavableWeaponState> SaveWeaponStateToSave()
        {
            List<SavableWeaponState> _savableWeaponStateList = new List<SavableWeaponState>();
            int allWeaponPlayerCarryingCount = allWeaponsPlayerCarrying.Count;
            for (int i = 0; i < allWeaponPlayerCarryingCount; i++)
            {
                _savableWeaponStateList.Add(allWeaponsPlayerCarrying[i].SaveWeaponStateToSave());
            }

            return _savableWeaponStateList;
        }
        #endregion

        #region Arrow.
        public List<SavableArrowState> SaveArrowStateToSave()
        {
            List<SavableArrowState> _savableArrowStateList = new List<SavableArrowState>();
            return _savableArrowStateList;
        }
        #endregion

        #region Armor.
        public List<SavableHeadState> SaveHeadStateToSave()
        {
            List<SavableHeadState> _savableHeadStateList = new List<SavableHeadState>();

            int allHeadsPlayerCarryingCount = allHeadsPlayerCarrying.Count;
            for (int i = 0; i < allHeadsPlayerCarryingCount; i++)
            {
                _savableHeadStateList.Add(allHeadsPlayerCarrying[i].SaveHeadStateToSave());
            }

            return _savableHeadStateList;
        }

        public List<SavableChestState> SaveChestStateToSave()
        {
            List<SavableChestState> _savableChestStateList = new List<SavableChestState>();

            int allChestsPlayerCarryingCount = allChestsPlayerCarrying.Count;
            for (int i = 0; i < allChestsPlayerCarryingCount; i++)
            {
                _savableChestStateList.Add(allChestsPlayerCarrying[i].SaveChestStateToSave());
            }

            return _savableChestStateList;
        }

        public List<SavableHandState> SaveHandStateToSave()
        {
            List<SavableHandState> _savableHandStateList = new List<SavableHandState>();

            int allHandsPlayerCarryingCount = allHandsPlayerCarrying.Count;
            for (int i = 0; i < allHandsPlayerCarryingCount; i++)
            {
                _savableHandStateList.Add(allHandsPlayerCarrying[i].SaveHandStateToSave());
            }

            return _savableHandStateList;
        }

        public List<SavableLegState> SaveLegStateToSave()
        {
            List<SavableLegState> _savableLegStateList = new List<SavableLegState>();

            int allLegsPlayerCarryingCount = allLegsPlayerCarrying.Count;
            for (int i = 0; i < allLegsPlayerCarryingCount; i++)
            {
                _savableLegStateList.Add(allLegsPlayerCarrying[i].SaveLegStateToSave());
            }

            return _savableLegStateList;
        }
        #endregion

        #region Charm.
        public List<SavableCharmState> SaveCharmStateToSave()
        {
            List<SavableCharmState> _savableCharmStateList = new List<SavableCharmState>();
            int allCharmsPlayerCarryCount = allCharmsPlayerCarrying.Count;
            for (int i = 0; i < allCharmsPlayerCarryCount; i++)
            {
                _savableCharmStateList.Add(allCharmsPlayerCarrying[i].SaveCharmStateToSave());
            }

            return _savableCharmStateList;
        }
        #endregion

        #region Powerup.
        public List<SavablePowerupState> SavePowerupStateToSave()
        {
            List<SavablePowerupState> _savablePowerupStateList = new List<SavablePowerupState>();
            int allPowerupsPlayerCarryCount = allPowerupsPlayerCarrying.Count;
            for (int i = 0; i < allPowerupsPlayerCarryCount; i++)
            {
                _savablePowerupStateList.Add(allPowerupsPlayerCarrying[i].SavePowerupStateToSave());
            }

            return _savablePowerupStateList;
        }
        #endregion

        #region Ring.
        public List<SavableRingState> SaveRingStateToSave()
        {
            List<SavableRingState> _savableRingStateList = new List<SavableRingState>();
            int allRingsPlayerCarryCount = allRingsPlayerCarrying.Count;
            for (int i = 0; i < allRingsPlayerCarryCount; i++)
            {
                _savableRingStateList.Add(allRingsPlayerCarrying[i].SaveRingStateToSave());
            }

            return _savableRingStateList;
        }
        #endregion

        #region Consumable.
        public void SaveConsumableStateToSave(MainSaveFile _savefile)
        {
            List<SavableStatsEffectConsumableState> _savableStatsEffectStateList = new List<SavableStatsEffectConsumableState>();
            List<SavableThrowableConsumableState> _savableThrowableStateList = new List<SavableThrowableConsumableState>();

            int allConsumablesPlayerCarryCount = allConsumablesPlayerCarrying.Count;
            for (int i = 0; i < allConsumablesPlayerCarryCount; i++)
            {
                if (allConsumablesPlayerCarrying[i].isStatsEffectConsumable)
                {
                    StatsEffectConsumable _statsEffectConsumable = allConsumablesPlayerCarrying[i].GetStatsEffectConsumable();
                    if (!_statsEffectConsumable._isDestroyAfterStatsEffectJob)
                    {
                        _savableStatsEffectStateList.Add(_statsEffectConsumable.SaveConsumableStateToSave());
                    }
                }
                else
                {
                    _savableThrowableStateList.Add(allConsumablesPlayerCarrying[i].GetThrowableConsumable().SaveConsumableStateToSave());
                }
            }

            _savefile.savedStatesEffectConsumableStates = _savableStatsEffectStateList;
            _savefile.savedThrowableConsumableStates = _savableThrowableStateList;
        }
        #endregion

        #endregion

        #region LOAD.
        public void LoadStateFromSave(MainSaveFile _saveFile)
        {
            /// Weapon.
            _savedWeaponStateList = _saveFile.savedWeaponStates;

            /// Armor.
            _savedHeadStateList = _saveFile.savedHeadStates;
            _savedChestStateList = _saveFile.savedChestStates;
            _savedHandStateList = _saveFile.savedHandStates;
            _savedLegStateList = _saveFile.savedLegStates;

            /// Charm.
            _savedCharmStateList = _saveFile.savedCharmStates;
            /// Powerup.
            _savedPowerupStateList = _saveFile.savedPowerupStates;
            /// Ring.
            _savedRingStateList = _saveFile.savedRingStates;
            /// Consumable.
            _savedStatsEffectConsumableStateList = _saveFile.savedStatesEffectConsumableStates;
            _savedThrowableConsumableStateList = _saveFile.savedThrowableConsumableStates;
        }
        #endregion

        #endregion

        #region Setup Player Gear Action.
        public void LoadItemsFromSave(ResourcesManager rm)
        {
            #region Weapon.
            int _savedWeaponStateCount = _savedWeaponStateList.Count;
            for (int i = 0; i < _savedWeaponStateCount; i++)
            {
                WeaponItem _weaponItem = rm.GetPlayerWeaponById(RemoveItemNameWhiteSpace(_savedWeaponStateList[i].savableWeaponName));
                HandleSavedWeaponPosition(_weaponItem.GetSaveFileRuntimeWeaponInstance(_savedWeaponStateList[i], this));
            }

            /// Set Oppose1 Defense IKProfile has be done after all the weapons are loaded.
            _playerIKHandler.SetCurrent_Oppose1_Defense_IKProfile();

            /// Calling Handle Weapons Visibility here is to save performance.
            HandleWeaponsVisibility();
            #endregion

            #region Head.
            int _savedHeadStateListCount = _savedHeadStateList.Count;
            for (int i = 0; i < _savedHeadStateListCount; i++)
            {
                HeadArmorItem _headArmorItem = rm.GetPlayerHeadArmorById(RemoveItemNameWhiteSpace(_savedHeadStateList[i].savableHeadName));
                HandleSavedHeadPosition(_headArmorItem.GetSaveFileRuntimeHeadInstance(_savedHeadStateList[i], this));
            }
            #endregion

            #region Chest.
            int _savedChestStateListCount = _savedChestStateList.Count;
            for (int i = 0; i < _savedChestStateListCount; i++)
            {
                ChestArmorItem _chestArmorItem = rm.GetPlayerChestArmorById(RemoveItemNameWhiteSpace(_savedChestStateList[i].savableChestName));
                HandleSavedChestPosition(_chestArmorItem.GetSaveFileRuntimeChestInstance(_savedChestStateList[i], this));
            }
            #endregion

            #region Hand.
            int _savedHandStateListCount = _savedHandStateList.Count;
            for (int i = 0; i < _savedHandStateListCount; i++)
            {
                HandArmorItem _handArmorItem = rm.GetPlayerHandArmorById(RemoveItemNameWhiteSpace(_savedHandStateList[i].savableHandName));
                HandleSavedHandPosition(_handArmorItem.GetSaveFileRuntimeHandInstance(_savedHandStateList[i], this));
            }
            #endregion

            #region Leg.
            int _savedLegStateListCount = _savedLegStateList.Count;
            for (int i = 0; i < _savedLegStateListCount; i++)
            {
                LegArmorItem _legArmorItem = rm.GetPlayerLegArmorById(RemoveItemNameWhiteSpace(_savedLegStateList[i].savableLegName));
                HandleSavedLegPosition(_legArmorItem.GetSaveFileRuntimeLegInstance(_savedLegStateList[i], this));
            }
            #endregion

            #region Charm.
            int _savedCharmStateListCount = _savedCharmStateList.Count;
            for (int i = 0; i < _savedCharmStateListCount; i++)
            {
                CharmItem _charmItem = rm.GetPlayerCharmById(RemoveItemNameWhiteSpace(_savedCharmStateList[i].savableCharmName));
                HandleSavedCharmPosition(_charmItem.GetSaveFileRuntimeCharmInstance(_savedCharmStateList[i], this));
            }
            #endregion

            #region Powerup.
            int _savedPowerupStateListCount = _savedPowerupStateList.Count;
            for (int i = 0; i < _savedPowerupStateListCount; i++)
            {
                PowerupItem _powerupItem = rm.GetPlayerPowerupById(RemoveItemNameWhiteSpace(_savedPowerupStateList[i].savablePowerupName));
                HandleSavedPowerupPosition(_powerupItem.GetSaveFileRuntimePowerupInstance(_savedPowerupStateList[i], this));
            }
            #endregion

            #region Ring.
            int _savedRingStateListCount = _savedRingStateList.Count;
            for (int i = 0; i < _savedRingStateListCount; i++)
            {
                RingItem _ringItem = rm.GetPlayerRingById(RemoveItemNameWhiteSpace(_savedRingStateList[i].savableRingName));
                HandleSavedRingPosition(_ringItem.GetSaveFileRuntimeRingInstance(_savedRingStateList[i], this));
            }
            #endregion

            #region Consumable.
            int _savedStatsEffectStateListCount = _savedStatsEffectConsumableStateList.Count;
            for (int i = 0; i < _savedStatsEffectStateListCount; i++)
            {
                /// If consumable is not vessel.
                if (!_savedStatsEffectConsumableStateList[i].savableStatsEffectIsVessel)
                {
                    StatsEffectConsumableItem _statsEffectConsumableItem = rm.GetPlayerStatsEffectConsumableById(RemoveItemNameWhiteSpace(_savedStatsEffectConsumableStateList[i].savableStatsEffectName));
                    StatsEffectConsumable _statsEffectConsumable = _statsEffectConsumableItem.GetSaveFileStatsEffectConsumableInstance(_savedStatsEffectConsumableStateList[i], this);

                    HandleSavedConsumablePosition(_statsEffectConsumable);
                    AddCarryConsumableToDictionary(_statsEffectConsumable);
                }
                else
                {
                    /// If consumable is volun vessel.
                    if (_savedStatsEffectConsumableStateList[i].savableStatsEffectIsVolun)
                    {
                        rm.volunVesselConsumableItem.GetSaveFileVesselsIntance(_savedStatsEffectConsumableStateList[i], this);
                        HandleSavedConsumablePosition(runtimeVolunVessel);
                        AddCarryConsumableToDictionary(runtimeVolunVessel);
                    }
                    else
                    {
                        rm.sodurVesselConsumableItem.GetSaveFileVesselsIntance(_savedStatsEffectConsumableStateList[i], this);
                        HandleSavedConsumablePosition(runtimeSodurVessel);
                        AddCarryConsumableToDictionary(runtimeSodurVessel);
                    }
                }
            }

            int _savedThrowableConsumableStateListCount = _savedThrowableConsumableStateList.Count;
            for (int i = 0; i < _savedThrowableConsumableStateListCount; i++)
            {
                ThrowableConsumableItem _throwableConsumableItem = rm.GetPlayerThrowableConsumableById(RemoveItemNameWhiteSpace(_savedThrowableConsumableStateList[i].savableThrowableName));
                ThrowableConsumable _throwableConsumable = _throwableConsumableItem.GetSaveFileThrowableConsumableInstance(_savedThrowableConsumableStateList[i], this);

                HandleSavedConsumablePosition(_throwableConsumable);
                AddCarryConsumableToDictionary(_throwableConsumable);
            }
            #endregion

            string RemoveItemNameWhiteSpace(string _itemName)
            {
                _inventoryStrBuilder.Clear();
                _inventoryStrBuilder.Append(_itemName);
                _inventoryStrBuilder.Replace(" ", string.Empty);
                return _inventoryStrBuilder.ToString();
            }
        }

        #region Handle Saved Item Position.

        #region Weapon.
        void HandleSavedWeaponPosition(RuntimeWeapon _runtimeWeapon)
        {
            switch (_runtimeWeapon.currentSlotSide)
            {
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Right:
                    InitSavedStateRhWeaponSlot(_runtimeWeapon, _runtimeWeapon.slotNumber);
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Left:
                    InitSavedStateLhWeaponSlot(_runtimeWeapon, _runtimeWeapon.slotNumber);
                    break;
                case RuntimeWeapon.P_Weapon_SlotSideTypeEnum.Backpack:
                    ReturnWeaponToBackpack(_runtimeWeapon);
                    break;
            }
        }
        #endregion
        
        #region Armor.
        void HandleSavedHeadPosition(RuntimeHeadArmor _runtimeHeadArmor)
        {
            switch (_runtimeHeadArmor.currentSlotSideType)
            {
                case RuntimeArmor.ArmorSlotSideTypeEnum.Head:
                    ParentArmorUnderBackpack(_runtimeHeadArmor.transform);
                    InitHeadArmorEmptySlot(_runtimeHeadArmor);
                    break;
                case RuntimeArmor.ArmorSlotSideTypeEnum.Backpack:
                    ReturnHeadArmorToBackpack(_runtimeHeadArmor);
                    break;
            }
        }

        void HandleSavedChestPosition(RuntimeChestArmor _runtimeChestArmor)
        {
            switch (_runtimeChestArmor.currentSlotSideType)
            {
                case RuntimeArmor.ArmorSlotSideTypeEnum.Chest:
                    ParentArmorUnderBackpack(_runtimeChestArmor.transform);
                    InitChestArmorEmptySlot(_runtimeChestArmor);
                    break;
                case RuntimeArmor.ArmorSlotSideTypeEnum.Backpack:
                    ReturnChestArmorToBackpack(_runtimeChestArmor);
                    break;
            }
        }

        void HandleSavedHandPosition(RuntimeHandArmor _runtimeHandArmor)
        {
            switch (_runtimeHandArmor.currentSlotSideType)
            {
                case RuntimeArmor.ArmorSlotSideTypeEnum.Hand:
                    ParentArmorUnderBackpack(_runtimeHandArmor.transform);
                    InitHandArmorEmptySlot(_runtimeHandArmor);
                    break;
                case RuntimeArmor.ArmorSlotSideTypeEnum.Backpack:
                    ReturnHandArmorToBackpack(_runtimeHandArmor);
                    break;
            }
        }

        void HandleSavedLegPosition(RuntimeLegArmor _runtimeLegArmor)
        {
            switch (_runtimeLegArmor.currentSlotSideType)
            {
                case RuntimeArmor.ArmorSlotSideTypeEnum.Leg:
                    ParentArmorUnderBackpack(_runtimeLegArmor.transform);
                    InitLegArmorEmptySlot(_runtimeLegArmor);
                    break;
                case RuntimeArmor.ArmorSlotSideTypeEnum.Backpack:
                    ReturnLegArmorToBackpack(_runtimeLegArmor);
                    break;
            }
        }
        #endregion

        #region Charm.
        void HandleSavedCharmPosition(RuntimeCharm _runtimeCharm)
        {
            switch (_runtimeCharm.currentSlotSide)
            {
                case RuntimeCharm.CharmSlotSideTypeEnum.Slot:
                    ParentCharmUnderBackpack(_runtimeCharm.transform);
                    SetupCharmEmptySlot(_runtimeCharm);
                    break;
                case RuntimeCharm.CharmSlotSideTypeEnum.Backpack:
                    ReturnCharmToBackpack(_runtimeCharm);
                    break;
            }
        }
        #endregion

        #region Powerup.
        void HandleSavedPowerupPosition(RuntimePowerup _runtimePowerup)
        {
            if (_runtimePowerup.currentSlotSide == RuntimePowerup.PowerupSlotSideTypeEnum.Slot)
            {
                SetupPowerupEmptySlot(_runtimePowerup);
            }
        }
        #endregion

        #region Ring.
        void HandleSavedRingPosition(RuntimeRing _runtimeRing)
        {
            switch (_runtimeRing.currentSlotSide)
            {
                case RuntimeRing.RingSlotSideTypeEnum.Right:
                    InitRightRingSlot(_runtimeRing);
                    break;
                case RuntimeRing.RingSlotSideTypeEnum.Left:
                    InitLeftRingSlot(_runtimeRing);
                    break;
                case RuntimeRing.RingSlotSideTypeEnum.Backpack:
                    ReturnRingToBackpack(_runtimeRing);
                    break;
            }
        }
        #endregion

        #region Consumable.
        void HandleSavedConsumablePosition(RuntimeConsumable _runtimeConsumable)
        {
            ParentConsumableUnderBackpack(_runtimeConsumable.transform);

            switch (_runtimeConsumable.currentSlotSide)
            {
                case RuntimeConsumable.ConsumableSlotSideTypeEnum.Slot:
                    LoadSavedConsumableSlot(_runtimeConsumable, _runtimeConsumable.slotNumber);
                    break;
                case RuntimeConsumable.ConsumableSlotSideTypeEnum.Backpack:
                    SetConsumableSlotSideToBackpack(_runtimeConsumable);
                    break;
            }
        }
        #endregion

        #endregion

        #endregion

        public void Init()
        {
            _a_hook = _states.a_hook;
            _a_hook._inventory = this;

            _playerIKHandler = _a_hook.playerIKHandler;
            _playerIKHandler._inventory = this;

            _states.CheckpointRefreshEvent += RefreshPlayerStatsAction;
            _inventoryStrBuilder = _states._strBuilder;

            GameManager.singleton.GetInventoryBackpacksRefs(this);

            rightHandWeaponSlotsLength = rightHandSlots.Length;
            leftHandWeaponSlotsLength = leftHandSlots.Length;
            arrowSlotsLength = arrowSlots.Length;
            consumableSlotsLength = consumableSlots.Length;
            spellSlotsLength = spellSlots.Length;

            r_index = 0;
            l_index = 0;
            d_index = 0;
            u_index = 0;
        }

        public void RequestSheathCurrentInBackpack(bool _isLeft)
        {
            if (_isLeft)
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathLhCurrentToBackpack;
                _a_hook.RegisterNewAnimationJob(_leftHandWeapon_referedItem.GetLhSheathBackpackHashByType(), false);
            }
            else
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathRhCurrentToBackpack;
                _a_hook.RegisterNewAnimationJob(_rightHandWeapon_referedItem.GetRhSheathBackpackHashByType(), false);
            }
            
            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void RequestSheathCurrentInBackpack_AllRightHand(bool _isLeft)
        {
            if (_isLeft)
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathLhCurrentToBackpack;
                _a_hook.RegisterNewAnimationJob(_leftHandWeapon_referedItem.GetRhSheathBackpackHashByType(), false);
            }
            else
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathRhCurrentToBackpack;
                _a_hook.RegisterNewAnimationJob(_rightHandWeapon_referedItem.GetRhSheathBackpackHashByType(), false);
            }

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void RequestSheathWeaponInBackpack(bool _isLeft, RuntimeWeapon _weaponToSheath)
        {
            _curWeaponToPass = _weaponToSheath;

            if (_isLeft)
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathLhWeaponToBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetLhSheathBackpackHashByType(), false);
            }
            else
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SheathRhWeaponToBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetRhSheathBackpackHashByType(), false);
            }

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void RequestSwitchCurrentInBackpack(bool _isLeft, RuntimeWeapon _weaponToSwitchTo)
        {
            _curWeaponToPass = _weaponToSwitchTo;

            if (_isLeft)
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SwitchLhWeaponInBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetLhSheathBackpackHashByType(), false);
            }
            else
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SwitchRhWeaponInBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetRhSheathBackpackHashByType(), false);
            }

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void RequestSwitchCurrentInBackpack_AllRightHand(bool _isLeft, RuntimeWeapon _weaponToSwitchTo)
        {
            _curWeaponToPass = _weaponToSwitchTo;

            if (_isLeft)
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SwitchLhWeaponInBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetRhSheathBackpackHashByType(), false);
            }
            else
            {
                _currentWeaponBackpackOperationType = WeaponBackpackOperationTypeEnum.SwitchRhWeaponInBackpack;
                _a_hook.RegisterNewAnimationJob(_curWeaponToPass._referedWeaponItem.GetRhSheathBackpackHashByType(), false);
            }

            _playerIKHandler.RegisterNewHandleIKJob(IKUsageType.UpperBody, 0.4f, _states.vector3Zero);
        }

        public void ExecuteBackpackOperationInAnim()
        {
            switch (_currentWeaponBackpackOperationType)
            {
                case WeaponBackpackOperationTypeEnum.SheathRhCurrentToBackpack:
                    ReturnRhCurrentInAnim();
                    break;
                case WeaponBackpackOperationTypeEnum.SheathLhCurrentToBackpack:
                    ReturnLhCurrentInAnim();
                    break;
                case WeaponBackpackOperationTypeEnum.SheathRhWeaponToBackpack:
                case WeaponBackpackOperationTypeEnum.SheathLhWeaponToBackpack:
                    ReturnWeaponInAnim();
                    break;
                case WeaponBackpackOperationTypeEnum.SwitchRhWeaponInBackpack:
                    SwitchRhCurrentInAnim();
                    break;
                case WeaponBackpackOperationTypeEnum.SwitchLhWeaponInBackpack:
                    SwitchLhCurrentInAnim();
                    break;
            }
        }

        public enum WeaponBackpackOperationTypeEnum
        {
            SheathRhCurrentToBackpack,
            SheathLhCurrentToBackpack,
            SheathRhWeaponToBackpack,
            SheathLhWeaponToBackpack,
            SwitchRhWeaponInBackpack,
            SwitchLhWeaponInBackpack
        }
    }
}