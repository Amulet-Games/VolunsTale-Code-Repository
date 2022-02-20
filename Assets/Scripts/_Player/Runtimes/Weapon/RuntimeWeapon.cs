using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeWeapon : RuntimeItem
    {
        [Header("Weapon Modifiable Stats.")]
        public WeaponModifiableStats weaponModifiableStats;

        [Header("Status.")]
        [ReadOnlyInspector] public P_Weapon_SlotSideTypeEnum currentSlotSide;
        [ReadOnlyInspector] public bool isCurrentRhWeapon;
        [ReadOnlyInspector] public bool isCurrentLhWeapon;
        [ReadOnlyInspector] public bool isShield;

        [Header("Refs.")]
        [ReadOnlyInspector] public Rigidbody rb;
        [ReadOnlyInspector] public WeaponHook weaponHook;
        [ReadOnlyInspector] public WeaponItem _referedWeaponItem;
        [ReadOnlyInspector] public StateManager _states;
        [NonSerialized] StatsAttributeHandler _statsHandler;
        [NonSerialized] Material _weaponMaterial;

        [Header("Drag and Drop.")]
        public WA_TrailFx_Handler _trailFxHandler;

        /// INIT

        #region Vanilla Init.
        public void InitRuntimeWeapon(WeaponItem _referedWeaponItem, StateManager _states)
        {
            this._referedWeaponItem = _referedWeaponItem;
            this._states = _states;

            InitRuntimeItem();
            InitReferences();
            InitTrailFxHandler();

            InitRuntimeWeaponHook();
            InitRuntimeRigidbody();
            InitRuntimeDissolved();

            InitDefaultStatus();
            InitModifiableStats();
            InitRuntimeName();
        }

        void InitRuntimeWeaponHook()
        {
            if (weaponHook == null)
            {
                weaponHook = GetComponent<WeaponHook>();
                weaponHook.Init(this);
            }
        }

        void InitRuntimeRigidbody()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }

        void InitReferences()
        {
            _statsHandler = _states.statsHandler;
            weaponModifiableStats._statsHandler = _statsHandler;
        }

        void InitTrailFxHandler()
        {
            _trailFxHandler.a_hook = _states.a_hook;
            _trailFxHandler.Init();
        }

        void InitDefaultStatus()
        {
            isShield = _referedWeaponItem.weaponType == P_Weapon_WeaponTypeEnum.Shield ? true : false;
        }

        void InitModifiableStats()
        {
            WeaponVanillaStats _vanillaStats = _referedWeaponItem.weaponVanillaStats;

            weaponModifiableStats._infusedElementType = P_Weapon_InfusedElementTypeEnum.None;
            weaponModifiableStats._fortifiedLevel = 0;
            weaponModifiableStats._durability = _vanillaStats.durability;

            weaponModifiableStats._mainAttackPowerType = _referedWeaponItem.weaponMainElementType;
            weaponModifiableStats._mainAtkPower = _vanillaStats.mainAtkPower;
            weaponModifiableStats._criticalAtkPower = _vanillaStats.criticalAtkPower;
            weaponModifiableStats._range = _vanillaStats.range;
            weaponModifiableStats._spellBuff = _vanillaStats.spellBuff;

            weaponModifiableStats._bleed_effect = _vanillaStats.bleed_effect;
            weaponModifiableStats._poison_effect = _vanillaStats.poison_effect;
            weaponModifiableStats._frost_effect = _vanillaStats.frost_effect;

            weaponModifiableStats._mainAtkAttriScaling = _referedWeaponItem.mainAtkAttriScaling;
        }

        void InitRuntimeName()
        {
            runtimeName = _states._savableInventory.GetFortifiedWeaponName(this);
        }
        #endregion

        #region Unarmed Init.
        public void InitRuntimeUnarmed(WeaponItem _referedWeaponItem, StateManager _states)
        {
            this._referedWeaponItem = _referedWeaponItem;
            this._states = _states;

            //InitRuntimeWeaponHook();
            InitRuntimeRigidbody();
            InitReferences();
            
            InitUnarmedDefaultStatus();
            InitModifiableStats();
        }

        void InitUnarmedDefaultStatus()
        {
            currentSlotSide = P_Weapon_SlotSideTypeEnum.Unarmed;
            runtimeName = _referedWeaponItem.itemName;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeWeaponFromSave(SavableWeaponState _savableWeaponState, StateManager _states, WeaponItem _referedWeaponItem)
        {
            this._referedWeaponItem = _referedWeaponItem;
            this._states = _states;
            
            InitRuntimeWeaponHook();
            InitRuntimeRigidbody();
            InitRuntimeDissolved();

            InitReferences();
            InitTrailFxHandler();

            InitDefaultStatus();
            LoadStatsFromSavable(_savableWeaponState);
            InitRuntimeName();
        }

        public SavableWeaponState SaveWeaponStateToSave()
        {
            SavableWeaponState _savableWeaponState = new SavableWeaponState();

            /// Runtime General.
            _savableWeaponState.savableWeaponName = _referedWeaponItem.itemName;
            _savableWeaponState.savableWeaponUniqueId = uniqueId;
            _savableWeaponState.savableWeaponSlotNumber = slotNumber;

            /// Weapon General.
            _savableWeaponState.savableWeaponSlotSide = (int)currentSlotSide;
            _savableWeaponState.savableIsCurrentRhWeapon = isCurrentRhWeapon;
            _savableWeaponState.savableIsCurrentLhWeapon = isCurrentLhWeapon;
            weaponModifiableStats.SaveModifiableStatsToSave(_savableWeaponState);

            return _savableWeaponState;
        }

        void LoadStatsFromSavable(SavableWeaponState _savableWeaponState)
        {
            /// Runtime General.
            uniqueId = _savableWeaponState.savableWeaponUniqueId;
            slotNumber = _savableWeaponState.savableWeaponSlotNumber;

            /// Weapon General.
            currentSlotSide = (P_Weapon_SlotSideTypeEnum)_savableWeaponState.savableWeaponSlotSide;
            isCurrentRhWeapon = _savableWeaponState.savableIsCurrentRhWeapon;
            isCurrentLhWeapon = _savableWeaponState.savableIsCurrentLhWeapon;

            weaponModifiableStats.LoadModifiableStatsFromSave(_savableWeaponState);
            weaponModifiableStats._mainAttackPowerType = _referedWeaponItem.weaponMainElementType;
        }
        #endregion

        #region Return Damage Method.
        public double ReturnWeaponTotalAttackPower(AIManager _ai)
        {
            double retVal = weaponModifiableStats.GetRawAttackPowerValue();

            retVal *= _statsHandler.b_attPowMulti_charm;
            retVal *= _statsHandler._attPowMulti_weaponArt;
            retVal *= _statsHandler._attPowMulti_consumable;
            //retVal *= _statsHandler._attPowMulti_spell;

            retVal *= weaponModifiableStats.GetExtraDamageFromElementalWeakness(_ai.currentElementalType);
            retVal *= _statsHandler._attPowMulti_weaponAction;

            return retVal;
        }
        
        public float ReturnFinalExecutionPower()
        {
            float retVal = weaponModifiableStats._criticalAtkPower;

            retVal *= _statsHandler.b_attPowMulti_charm;
            retVal *= _statsHandler._attPowMulti_weaponArt;
            retVal *= _statsHandler._attPowMulti_consumable;
            //retVal *= _statsHandler._attPowMulti_spell;

            retVal *= weaponModifiableStats.GetExtraDamageFromElementalWeakness(_states._currentExecutingTarget.currentElementalType);

            return retVal;
        }
        #endregion

        #region Preview Only.
        public double ReturnWeaponPreviewAttackPower()
        {
            double retVal = weaponModifiableStats.GetRawAttackPowerValue();

            retVal *= _statsHandler.b_attPowMulti_charm;
            retVal *= _statsHandler._attPowMulti_weaponArt;
            retVal *= _statsHandler._attPowMulti_consumable;
            //retVal *= _statsHandler._attPowMulti_spell;
            
            return retVal;
        }
        #endregion

        /// Anim Hash.
        public int GetRollsTreeHashByType()
        {
            switch (_referedWeaponItem.weaponType)
            {
                case P_Weapon_WeaponTypeEnum.Axe:
                    return HashManager.singleton.p_axe_lockon_rolls_tree_hash;

                case P_Weapon_WeaponTypeEnum.Fist:
                    if (_states._isTwoHanding)
                    {
                        return HashManager.singleton.p_fist_2h_lockon_rolls_tree_hash;
                    }
                    {
                        return HashManager.singleton.p_fist_1h_lockon_rolls_tree_hash;
                    }

                case P_Weapon_WeaponTypeEnum.GreatSword:
                    return HashManager.singleton.p_gs_lockon_rolls_tree_hash;
                case P_Weapon_WeaponTypeEnum.Shield:
                    return HashManager.singleton.p_shield_lockon_rolls_tree_hash;
                case P_Weapon_WeaponTypeEnum.StraightSword:
                    return HashManager.singleton.p_strs_lockon_rolls_tree_hash;
                case P_Weapon_WeaponTypeEnum.Bow:
                    return 0;
                case P_Weapon_WeaponTypeEnum.Catalysts:
                    return 0;
                default:
                    return 0;
            }
        }

        /// Override
        public override Item GetReferedItem()
        {
            return _referedWeaponItem;
        }

        /// Is Unarmed.
        public bool IsUnarmed()
        {
            return currentSlotSide == P_Weapon_SlotSideTypeEnum.Unarmed;
        }

        /// Dissolved.
        void InitRuntimeDissolved()
        {
            _weaponMaterial = GetComponentInChildren<MeshRenderer>().material;
            _weaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, _referedWeaponItem._cutOffFullOpaqueValue);
        }
        
        public void DissolveOutWeapon()
        {
            LeanTween.value(_referedWeaponItem._cutOffFullOpaqueValue, _referedWeaponItem._cutOffFullTransparentValue, _referedWeaponItem._dissolveSpeed).setOnUpdate((value) => _weaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, value));
        }

        public void DissolveInWeapon()
        {
            LeanTween.value(_referedWeaponItem._cutOffFullTransparentValue, _referedWeaponItem._cutOffFullOpaqueValue, _referedWeaponItem._dissolveSpeed).setOnUpdate((value) => _weaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, value));
        }

        public void SetWeaponToOpaque()
        {
            _weaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, _referedWeaponItem._cutOffFullOpaqueValue);
        }

        public void SetWeaponToTransparent()
        {
            _weaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, _referedWeaponItem._cutOffFullTransparentValue);
        }

        /// Enum.
        public enum P_Weapon_SlotSideTypeEnum
        {
            Right,
            Left,
            Backpack,
            Unarmed
        }
    }

    [Serializable]
    public class WeaponModifiableStats
    {
        [Header("General.")]
        [ReadOnlyInspector] public P_Weapon_InfusedElementTypeEnum _infusedElementType;
        [ReadOnlyInspector] public bool _isInfused;
        [ReadOnlyInspector] public int _fortifiedLevel;
        [ReadOnlyInspector] public float _durability;

        [Header("Attack Powers.")]
        [ReadOnlyInspector] public P_Weapon_ElementTypeEnum _mainAttackPowerType;
        [ReadOnlyInspector] public int _mainAtkPower;
        [ReadOnlyInspector] public double _mainAtkBonus;
        [ReadOnlyInspector] public int _criticalAtkPower;
        [ReadOnlyInspector] public int _range;
        [ReadOnlyInspector] public int _spellBuff;
        
        [Header("Additional Effects.")]
        [ReadOnlyInspector] public int _bleed_effect;
        [ReadOnlyInspector] public int _poison_effect;
        [ReadOnlyInspector] public int _frost_effect;

        [Header("Attribute Bonus% .")]
        [ReadOnlyInspector] public int _mainAtkAttriScaling;

        [NonSerialized] public StatsAttributeHandler _statsHandler;

        /// DAMAGE CALCULATE ALGORITHMS

        public float GetExtraDamageFromElementalWeakness(AIElementalTypeEnum _enemyElementType)
        {
            switch (_enemyElementType)
            {
                case AIElementalTypeEnum.Magical:

                    if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Physical)
                    {
                        return 1.3f;
                    }
                    else if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Magic)
                    {
                        return 0.7f;
                    }
                    return 1;

                case AIElementalTypeEnum.Fire:

                    if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Lightning)
                    {
                        return 1.3f;
                    }
                    else if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Dark)
                    {
                        return 0.7f;
                    }
                    return 1;

                case AIElementalTypeEnum.Lightning:

                    if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Dark)
                    {
                        return 1.3f;
                    }
                    else if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Fire)
                    {
                        return 0.7f;
                    }
                    return 1;

                case AIElementalTypeEnum.Dark:

                    if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Fire)
                    {
                        return 1.3f;
                    }
                    else if(_mainAttackPowerType == P_Weapon_ElementTypeEnum.Lightning)
                    {
                        return 0.7f;
                    }
                    return 1;

                case AIElementalTypeEnum.Physical:

                    if (_mainAttackPowerType == P_Weapon_ElementTypeEnum.Magic)
                    {
                        return 1.3f;
                    }
                    else if(_mainAttackPowerType == P_Weapon_ElementTypeEnum.Physical)
                    {
                        return 0.7f;
                    }
                    return 1;

                default:
                    return 1;
            }
        }

        public double GetRawAttackPowerValue()
        {
            RefreshTrueBonusDamageValue();
            return _mainAtkPower + _mainAtkBonus;
        }

        public void RefreshTrueBonusDamageValue()
        {
            switch (_mainAttackPowerType)
            {
                case P_Weapon_ElementTypeEnum.Physical:

                    _mainAtkBonus = _mainAtkPower * (_mainAtkAttriScaling * 0.01f) * GetBonusPercentageValue(_statsHandler.strength);
                    break;

                case P_Weapon_ElementTypeEnum.Magic:

                    _mainAtkBonus = _mainAtkPower * (_mainAtkAttriScaling * 0.01f) * GetBonusPercentageValue(_statsHandler.intelligence);
                    break;

                case P_Weapon_ElementTypeEnum.Fire:

                    _mainAtkBonus = _mainAtkPower * (_mainAtkAttriScaling * 0.01f) * GetBonusPercentageValue(_statsHandler.hexes);
                    break;

                case P_Weapon_ElementTypeEnum.Lightning:

                    _mainAtkBonus = _mainAtkPower * (_mainAtkAttriScaling * 0.01f) * GetBonusPercentageValue(_statsHandler.divinity);
                    break;

                case P_Weapon_ElementTypeEnum.Dark:

                    _mainAtkBonus = _mainAtkPower * (_mainAtkAttriScaling * 0.01f) * GetBonusPercentageValue(_statsHandler.hexes);
                    break;
            }
        }

        double GetBonusPercentageValue(int _playerAttributeVal)
        {
            double retVal = 0;

            if (_playerAttributeVal > 60)
            {
                retVal = 100;
            }
            else if (_playerAttributeVal <= 60 && _playerAttributeVal >= 41)
            {
                // forth group.
                retVal = 75 + (100 - 75) * ((_playerAttributeVal - 41) / 19f);
            }
            else if (_playerAttributeVal <= 40 && _playerAttributeVal >= 32)
            {
                // third group.
                retVal = 60 + (75 - 60) * ((_playerAttributeVal - 32) / 8f);
            }
            else if (_playerAttributeVal <= 31 && _playerAttributeVal >= 21)
            {
                // second group.
                retVal = 30 + (60 - 30) * ((_playerAttributeVal - 21) / 10f);
            }
            else
            {
                // first group.
                retVal = 30 * (_playerAttributeVal / 20f);
            }

            retVal = Math.Round(retVal, MidpointRounding.AwayFromZero);
            return retVal * 0.01f;
        }

        #region Serializable.
        public void SaveModifiableStatsToSave(SavableWeaponState _savableWeaponState)
        {
            /// Weapon Modifiable Stats.
            _savableWeaponState.savableInfusedElementType = (int)_infusedElementType;
            _savableWeaponState.savableWeaponFortifiedLevel = _fortifiedLevel;
            _savableWeaponState.savableWeaponDurability = _durability;

            /// Weapon Attack Powers.
            _savableWeaponState.savableMainAttackPower = _mainAtkPower;
            _savableWeaponState.savableMainAttackBonus = _mainAtkBonus;
            _savableWeaponState.savableCriticalAttPower = _criticalAtkPower;
            _savableWeaponState.savableRange = _range;
            _savableWeaponState.savableSpellBuff = _spellBuff;
            
            /// Weapon Additional Effects.
            _savableWeaponState.savableBleedEffect = _bleed_effect;
            _savableWeaponState.savablePoisonEffect = _poison_effect;
            _savableWeaponState.savableFrostEffect = _frost_effect;
        }

        public void LoadModifiableStatsFromSave(SavableWeaponState _savableWeaponState)
        {
            /// Weapon Modifiable Stats.
            _infusedElementType = (P_Weapon_InfusedElementTypeEnum)_savableWeaponState.savableInfusedElementType;
            _isInfused = _infusedElementType == P_Weapon_InfusedElementTypeEnum.None ? true : false;
            _fortifiedLevel = _savableWeaponState.savableWeaponFortifiedLevel;
            _durability = _savableWeaponState.savableWeaponDurability;

            /// Weapon Attack Powers.
            _mainAtkPower = _savableWeaponState.savableMainAttackPower;
            _mainAtkBonus = _savableWeaponState.savableMainAttackBonus;
            _criticalAtkPower = _savableWeaponState.savableCriticalAttPower;
            _range = _savableWeaponState.savableRange;
            _spellBuff = _savableWeaponState.savableSpellBuff;
            
            /// Weapon Additional Effects.
            _bleed_effect = _savableWeaponState.savableBleedEffect;
            _poison_effect = _savableWeaponState.savablePoisonEffect;
            _frost_effect = _savableWeaponState.savableFrostEffect;
        }
        #endregion
    }
}