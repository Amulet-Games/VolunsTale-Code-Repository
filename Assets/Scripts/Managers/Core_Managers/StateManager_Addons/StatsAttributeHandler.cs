using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    [Serializable]
    public class StatsAttributeHandler
    {
        #region General.
        [Header("General.")]
        [Tooltip("The general information of this player character, could be used for new savefile creation.")]
        public string characterName = "";
        #endregion

        #region Attributes.
        [Header("True Attributes.")]
        [Tooltip("The values that affect Base Stats, Attack Powers and more. Each attributes should be used for different catergory calculation.")]
        [ReadOnlyInspector] public int playerLevel = 1;         // All Dmg reduct. (r == 1, physical) (r == 2, fire & lightning & magic) (r == 3, bleed & poison & frost & dark)
        [ReadOnlyInspector] public int voluns;
        [ReadOnlyInspector] public int vigor = 0;               // Hp
        [ReadOnlyInspector] public int adaptation = 0;          // Fp
        [ReadOnlyInspector] public int endurance = 0;           // Stamina, Lightning dmg reduct
        [ReadOnlyInspector] public int vitality = 0;            // EquipLoad , Physical dmg reduct
        [ReadOnlyInspector] public int strength = 0;            // Physical ATK Power, Fire dmg reduct
        [ReadOnlyInspector] public int hexes = 0;               // Dark ATK Power, Fire ATK Power
        [ReadOnlyInspector] public int intelligence = 0;        // Magic ATK Power, Magic dmg reduct
        [ReadOnlyInspector] public int divinity = 0;            // Lightning ATK Power, Dark dmg reduct
        [ReadOnlyInspector] public int fortune = 0;             // Item discovery

        [Header("Attri Attributes.")]
        [HideInInspector] public int attri_vigor = 1;
        [HideInInspector] public int attri_adaptation = 1;
        [HideInInspector] public int attri_endurance = 1;
        [HideInInspector] public int attri_vitality = 1;
        [HideInInspector] public int attri_strength = 1;
        [HideInInspector] public int attri_hexes = 1;
        [HideInInspector] public int attri_intelligence = 1;
        [HideInInspector] public int attri_divinity = 1;
        [HideInInspector] public int attri_fortune = 1;

        [Header("Multi Attributes.")]
        [HideInInspector] public int multi_vigor = 0;
        [HideInInspector] public int multi_adaptation = 0;
        [HideInInspector] public int multi_endurance = 0;
        [HideInInspector] public int multi_vitality = 0;
        [HideInInspector] public int multi_strength = 0;
        [HideInInspector] public int multi_hexes = 0;
        [HideInInspector] public int multi_intelligence = 0;
        [HideInInspector] public int multi_divinity = 0;
        [HideInInspector] public int multi_fortune = 0;
        #endregion

        #region Attributes AddRate / Softcaps.

        #region Vigor.
        [Header("Vigor Levelup Rate.")]
        /// 1st cap => Soft Cap / 2nd cap => Hard Cap.
        /// The goal is to give player more than the half of the maximum health(which generally is on lvl 50 vigor).
        /// ．1st cap starts from lvl 18 vigor, which drops Add Rate from 27 -> 18.
        /// ．2nd cap starts from lvl 36 vigor, which drops Add Rate from 18 -> 8.
        /// In Total (1 - 50) lvl, player will have total (486 + 324 + 112 = 922) of base health without any ring or powerup.
        [ReadOnlyInspector] public int vigor_1st_cap_lvl;
        [ReadOnlyInspector] public int vigor_2nd_cap_lvl;
        [ReadOnlyInspector] public int vigor_inital_add_rate;
        [ReadOnlyInspector] public int vigor_1st_cap_add_rate;
        [ReadOnlyInspector] public int vigor_2nd_cap_add_rate;
        #endregion

        #region Adaptation.
        [Header("Adaptation Levelup Rate.")]
        [ReadOnlyInspector] public int adaptation_1st_cap_lvl = 20;
        [ReadOnlyInspector] public int adaptation_2nd_cap_lvl = 38;
        [ReadOnlyInspector] public int adaptation_inital_add_rate = 24;
        [ReadOnlyInspector] public int adaptation_1st_cap_add_rate = 13;
        [ReadOnlyInspector] public int adaptation_2nd_cap_add_rate = 8;
        #endregion

        #region Endurance.
        [Header("Endurance Levelup Rate.")]
        /// 1st cap => Soft Cap / 2nd cap => Hard Cap.
        /// The goal is to give player more than the half of the maximum stamina(which generally is on lvl 50 endurance).
        /// ．1st cap starts from lvl 15 endurance, which drops Add Rate from 22 -> 11.
        /// ．2nd cap starts from lvl 36 endurance, which drops Add Rate from 11 -> 6.
        /// In Total (1 - 50) lvl, player will have total (330 + 165 + 120 = 615) of base stamina without any ring or powerup.
        [ReadOnlyInspector] public int endurance_1st_cap_lvl;
        [ReadOnlyInspector] public int endurance_2nd_cap_lvl;
        [ReadOnlyInspector] public int endurance_inital_add_rate;
        [ReadOnlyInspector] public int endurance_1st_cap_add_rate;
        [ReadOnlyInspector] public int endurance_2nd_cap_add_rate;
        #endregion

        #region Levelup Requirement.
        [Header("Requirement Volun Add Rate.")]
        [ReadOnlyInspector] public int _base_requirePoints = 200;
        [ReadOnlyInspector] public int _maxLvl_AddRateThershold = 200;
        [ReadOnlyInspector] public float _requirePoints_1st_add_rate = 1.035f;
        [ReadOnlyInspector] public float _requirePoints_2nd_add_rate = 1.07f;
        [ReadOnlyInspector] public float _requirePoints_3rd_add_rate = 1.1f;
        [ReadOnlyInspector] public float _requirePoints_4th_add_rate = 1.05f;
        [ReadOnlyInspector] public float _requirePoints_5th_add_rate = 1.025f;
        #endregion

        #endregion

        #region Initial Base Value.
        [Header("Initial Base.")]
        public int init_b_health;
        public int init_b_stamina = 70;
        [HideInInspector] public float init_total_equip_load = 50;
        [HideInInspector] public float init_item_discover = 5;
        #endregion

        #region Base Stats.
        [Header("BASE.")]
        /// Inital Stats / Progressive Stats.
        /// Under these circumstances only Base Stats can be changed. 
        /// ．Character LevelUp
        /// ．Armor Upgrade
        /// 
        /// ．Register/Unregister Current Armor
        /// ．Register/Unregister Current Ring
        /// ．Register/Unregister Current Charms
        /// ．Register/Unregister Current Powerups
        // GENERAL
        [ReadOnlyInspector] public float attri_hp;
        [ReadOnlyInspector] public float attri_fp;
        [ReadOnlyInspector] public float attri_stamina;

        [ReadOnlyInspector] public float multi_hp;
        [ReadOnlyInspector] public float multi_fp;
        [ReadOnlyInspector] public float multi_stamina;

        [ReadOnlyInspector] public float b_hp;
        [ReadOnlyInspector] public float b_fp;
        [ReadOnlyInspector] public float b_stamina;
        
        [Header("SPEED.")]
        [SerializeField] public float b_walk_speed = 5;
        [SerializeField] public float b_run_speed = 7;
        [SerializeField] public float b_roll_speed = 3;
        [SerializeField] public float b_evade_speed = 3;
        [SerializeField] public float b_fall_speed = 8;
        [SerializeField] public float b_consumable_walk_speed = 3;

        [Header("JUMP.")]
        [SerializeField] public float b_jump_speed = 4;
        [SerializeField] public float b_jump_height = 5;
        [SerializeField] public float b_jump_gravity = 11;
        
        [Header("COST.")]
        [SerializeField] public float b_run_stamina_cost = 80;
        [SerializeField] public float b_roll_stamina_cost = 65;
        [SerializeField] public float b_evade_stamina_cost = 35;
        [SerializeField] public float b_jump_stamina_cost = 95;
        [SerializeField] public float b_fighterMode_attack_stamina_cost_reducePerc = 0.5f;

        [Header("RECOVER.")]
        [SerializeField] public float b_stamina_recover = 80;
        [SerializeField] public float b_stamina_recover_movableRate = 100;
        [SerializeField] public float b_focus_recover = 30;

        [Header("ATTACK POWER MULTIPIER.")] 
        [ReadOnlyInspector] public float b_attPowMulti_charm = 1;
        [ReadOnlyInspector] public float b_attPowMulti_weaponArt = 1;
        [ReadOnlyInspector] public float b_attPowMulti_consumable = 1;
        [ReadOnlyInspector] public float b_attPowMulti_spell = 1;

        [Header("DAMAGE REDUCTION.")] 
        [ReadOnlyInspector] public float attri_physical_reduction;
        [ReadOnlyInspector] public float attri_magic_reduction;
        [ReadOnlyInspector] public float attri_fire_reduction;
        [ReadOnlyInspector] public float attri_lightning_reduction;
        [ReadOnlyInspector] public float attri_dark_reduction;

        [ReadOnlyInspector] public float multi_physical_reduction;
        [ReadOnlyInspector] public float multi_strike_reduction;
        [ReadOnlyInspector] public float multi_slash_reduction;
        [ReadOnlyInspector] public float multi_thrust_reduction;
        [ReadOnlyInspector] public float multi_magic_reduction;
        [ReadOnlyInspector] public float multi_fire_reduction;
        [ReadOnlyInspector] public float multi_lightning_reduction;
        [ReadOnlyInspector] public float multi_dark_reduction;

        [ReadOnlyInspector] public float b_physical_reduction;
        [ReadOnlyInspector] public float b_strike_reduction;
        [ReadOnlyInspector] public float b_slash_reduction;
        [ReadOnlyInspector] public float b_thrust_reduction;
        [ReadOnlyInspector] public float b_magic_reduction;
        [ReadOnlyInspector] public float b_fire_reduction;
        [ReadOnlyInspector] public float b_lightning_reduction;
        [ReadOnlyInspector] public float b_dark_reduction;
        
        [Header("STATUS RESISTANCES.")]
        [ReadOnlyInspector] public float attri_bleed_resistance;
        [ReadOnlyInspector] public float attri_poison_resistance;
        [ReadOnlyInspector] public float attri_frost_resistance;
        [ReadOnlyInspector] public float attri_curse_resistance;

        [ReadOnlyInspector] public float multi_bleed_resistance;
        [ReadOnlyInspector] public float multi_poison_resistance;
        [ReadOnlyInspector] public float multi_frost_resistance;
        [ReadOnlyInspector] public float multi_curse_resistance;

        [ReadOnlyInspector] public float b_bleed_resistance;
        [ReadOnlyInspector] public float b_poison_resistance;
        [ReadOnlyInspector] public float b_frost_resistance;
        [ReadOnlyInspector] public float b_curse_resistance;
        [ReadOnlyInspector] public float b_poise;
        
        [Header("VESSELS.")]
        [SerializeField] public int b_volunVessel_amount;
        [SerializeField] public int b_volunFragment_amount;             // Increase the maximum Volun Vessel amount.
        [SerializeField] public int b_shatteredAmuletPiece_amount;      // Increase the power of Volun Vessel.
        [SerializeField] public int b_sodurVessel_amount;

        // OTHERS
        [ReadOnlyInspector] public float b_total_equip_load;
        [ReadOnlyInspector] public float b_cur_equip_load;
        [ReadOnlyInspector] public float b_item_discover = 111;
        [ReadOnlyInspector] public float b_attunement_slots = 0;
        #endregion

        #region Current Stats.
        /// In-game periodic Stats / Reversible Stats.
        /// Current Stats are stats that frequently changed under these circumstances.
        /// ．Taken Damage
        /// ．Ran, Jumped, Attacked
        /// ．Used Magic
        /// 
        /// ．Debuff Status (Poison, Frostbite, Fire Burn)
        /// ．Buff Status (Magic, Consumable Items)
        [Header("CUR GENERAL")]
        [ReadOnlyInspector] public float _hp;
        [ReadOnlyInspector] public float _fp;
        [ReadOnlyInspector] public float _stamina;

        [Header("CUR SPEED.")]
        [ReadOnlyInspector] public float _walk_speed;
        [ReadOnlyInspector] public float _run_speed;
        [ReadOnlyInspector] public float _jump_speed;
        [ReadOnlyInspector] public float _jump_height;
        [ReadOnlyInspector] public float _roll_speed;
        //[ReadOnlyInspector] public float _fall_speed;
        //[ReadOnlyInspector] public float _consumable_walk_speed = 3;

        [Header("CUR COST.")]
        [ReadOnlyInspector] public float _run_stamina_cost;
        [ReadOnlyInspector] public float _roll_stamina_cost;
        [ReadOnlyInspector] public float _jump_stamina_cost;

        [Header("CUR RECOVER.")]
        [ReadOnlyInspector] public float _stamina_recover;
        [ReadOnlyInspector] public float _focus_recover;
        [ReadOnlyInspector] public float _stamina_recover_movableRate;

        [Header("CUR ATTACK POWER MULTIPIER.")]
        [ReadOnlyInspector] public float _attPowMulti_weaponArt;
        [ReadOnlyInspector] public float _attPowMulti_consumable;
        [ReadOnlyInspector] public float _attPowMulti_spell;
        [ReadOnlyInspector] public float _attPowMulti_weaponAction;
        
        [Header("CUR OTHERS.")]
        [ReadOnlyInspector] public float _weightRatio;
        [ReadOnlyInspector] public float _item_discover;
        #endregion

        #region Effect Max Amounts.
        [Header("Effect Max Amounts.")]
        public int maxStatsEffectAmount = 3;
        public int maxWeaponBuffAmount = 2;
        #endregion

        #region Refs.
        [Header("Ref.")]
        [HideInInspector] public StateManager _states;
        [HideInInspector] public MainHudManager _mainHudManager;
        public StatsEffectJob[] currentStatsEffectJobs = new StatsEffectJob[3];
        public TimedWeaponBuffJob[] currentWeaponBuffJobs = new TimedWeaponBuffJob[2];
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public int _statsEffectJobsAmount;
        [ReadOnlyInspector] public int _weaponBuffJobsAmount;
        [ReadOnlyInspector] public bool isPausingStaminaRecover;
        #endregion

        #region Non Serialized.
        /// This is only used in leveling menu, use for preivew the player stats changes when player certain points applied.
        // ATTRIBUTES
        [NonSerialized] public int _prev_playerLevel;
        [NonSerialized] public int _prev_voluns;
        [NonSerialized] public int _prev_vigor;
        [NonSerialized] public int _prev_adaptation;
        [NonSerialized] public int _prev_endurance;
        [NonSerialized] public int _prev_vitality;
        [NonSerialized] public int _prev_strength;
        [NonSerialized] public int _prev_hexes;
        [NonSerialized] public int _prev_intelligence;
        [NonSerialized] public int _prev_divinity;
        [NonSerialized] public int _prev_fortune;

        // GENERAL
        [NonSerialized] public float _prev_hp;
        [NonSerialized] public float _prev_fp;
        [NonSerialized] public float _prev_stamina;

        // DAMAGE REDUCTION
        [NonSerialized] public float _prev_physical_reduction;
        [NonSerialized] public float _prev_strike_reduction;
        [NonSerialized] public float _prev_slash_reduction;
        [NonSerialized] public float _prev_thrust_reduction;
        [NonSerialized] public float _prev_magic_reduction;
        [NonSerialized] public float _prev_fire_reduction;
        [NonSerialized] public float _prev_lightning_reduction;
        [NonSerialized] public float _prev_dark_reduction;

        // STATUS RESISTANCES
        [NonSerialized] public float _prev_bleed_resistance;
        [NonSerialized] public float _prev_poison_resistance;
        [NonSerialized] public float _prev_frost_resistance;
        [NonSerialized] public float _prev_curse_resistance;

        // OTHERS
        [NonSerialized] public float _prev_total_equip_load;
        [NonSerialized] public float _prev_item_discover;
        [NonSerialized] public float _prev_attunement_slots;
        
        #region Attribute Change Stats Table
        Dictionary<int, int> _levelupRequirementTable = new Dictionary<int, int>();
        Dictionary<int, int> _vigorChangeHealthTable = new Dictionary<int, int>();
        Dictionary<int, int> _attunementChangeFocusTable = new Dictionary<int, int>();
        Dictionary<int, int> _attunementChangeSlotTable = new Dictionary<int, int>();
        Dictionary<int, int> _enduranceChangeStaminaTable = new Dictionary<int, int>();
        Dictionary<int, int> _enduranceChangelightningTable = new Dictionary<int, int>();
        Dictionary<int, float> _vitalityChangeEquipLoadTable = new Dictionary<int, float>();
        Dictionary<int, int> _vitalityChangePhysicalTable = new Dictionary<int, int>();
        Dictionary<int, int> _intelligenceChangeMagicTable = new Dictionary<int, int>();
        Dictionary<int, int> _strengthChangeFireTable = new Dictionary<int, int>();
        Dictionary<int, int> _divinityChangeDarkTable = new Dictionary<int, int>();
        #endregion

        #region Player Lvl Change Stats Table
        Dictionary<int, int> _levelChangePhysicalTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeMagicTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeFireTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeLightningTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeDarkTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeBleedTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangePoisonTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeFrostTable = new Dictionary<int, int>();
        Dictionary<int, int> _levelChangeCurseTable = new Dictionary<int, int>();
        #endregion

        #endregion

        public void Setup()
        {
            SetupLevelupRequirementTable();

            SetupVigorChangeHealthTable();
            SetupAdaptationChangeFocusTable();
            SetupEnduranceChangeStaminaTable();

            SetupAttributesRemainderChangeStatsTable();
            SetupLevelRemainderChangeStatsTable();

            /// Continue Game / Load Game stats is already loaded from save at this point.
            SetAttributesByStarterProfile();
            SetupGetTrueAttributesValue();
            SetupOnAttributesChangeStats();
            SubscribleToCheckpointEvent();
        }
        
        public void Tick()
        {
            MonitorStatsEffectJobs();
            MonitorWeaponBuffJobs();
            RecoverStaminaOverTime();
        }

        #region Local Init.
        void SetAttributesByStarterProfile()
        {
            if (_states._savableManager.isNewGame)
            {
                CreateStartupClassAttributes();
            }
        }

        void CreateStartupClassAttributes()
        {
            StarterAttributesStats _starterAttributesStats = _states._currentProfile.starterAttributeStats;

            attri_vigor = _starterAttributesStats.starter_vigor;
            attri_adaptation = _starterAttributesStats.starter_adaptation;
            attri_endurance = _starterAttributesStats.starter_endurance;
            attri_vitality = _starterAttributesStats.starter_vitality;
            attri_strength = _starterAttributesStats.starter_strength;
            attri_hexes = _starterAttributesStats.starter_hexes;
            attri_intelligence = _starterAttributesStats.starter_intelligence;
            attri_divinity = _starterAttributesStats.starter_divinity;
            attri_fortune = _starterAttributesStats.starter_fortune;
        }

        void SetupGetTrueAttributesValue()
        {
            GetTrue_Vigor_Value();

            GetTrue_Adaptation_Value();

            GetTrue_Endurance_Value();

            GetTrue_Vitality_Value();

            GetTrue_Strength_Value();

            GetTrue_Intelligence_Value();

            GetTrue_Divinity_Value();

            GetTrue_Fortune_Value();

            void GetTrue_Vigor_Value()
            {
                vigor = attri_vigor + multi_vigor;
            }

            void GetTrue_Adaptation_Value()
            {
                adaptation = attri_adaptation + multi_adaptation;
            }

            void GetTrue_Endurance_Value()
            {
                endurance = attri_endurance + multi_endurance;
            }

            void GetTrue_Vitality_Value()
            {
                vitality = attri_vitality + multi_vitality;
            }

            void GetTrue_Strength_Value()
            {
                strength = attri_strength + multi_strength;
            }

            void GetTrue_Intelligence_Value()
            {
                intelligence = attri_intelligence + multi_intelligence;
            }

            void GetTrue_Divinity_Value()
            {
                divinity = attri_divinity + multi_divinity;
            }

            void GetTrue_Fortune_Value()
            {
                fortune = attri_fortune + multi_fortune;
            }
        }

        void SetupOnAttributesChangeStats()
        {
            OnPlayerLevelChangeStats();
            OnVigorChangeStats();
            OnAdaptationChangeStats();
            OnEnduranceChangeStats();
            OnVitalityChangeStats();
            OnStrengthChangeStats();
            OnIntelligenceChangeStats();
            OnDivinityChangeStats();
            OnFortuneChangeStats();
        }
        
        void SubscribleToCheckpointEvent()
        {
            _states.CheckpointRefreshEvent += RefreshPlayerStatsAction;
        }

        #region Init Attributes Change Stats Table.
        void SetupLevelupRequirementTable()
        {
            int _requireRate = _base_requirePoints;

            for (int i = 1; i < _maxLvl_AddRateThershold + 1; i++)
            {
                if (i > 10)
                {
                    if (i < 36)
                    {
                        _requireRate = (int)(_requireRate * _requirePoints_3rd_add_rate);
                        _levelupRequirementTable.Add(i, _requireRate);
                    }
                    else if (i < 50)
                    {
                        _requireRate = (int)(_requireRate * _requirePoints_4th_add_rate);
                        _levelupRequirementTable.Add(i, _requireRate);
                    }
                    else
                    {
                        _requireRate = (int)(_requireRate * _requirePoints_5th_add_rate);
                        _levelupRequirementTable.Add(i, _requireRate);
                    }
                }
                else
                {
                    if (i > 5)
                    {
                        _requireRate = (int)(_requireRate * _requirePoints_2nd_add_rate);
                        _levelupRequirementTable.Add(i, _requireRate);
                    }
                    else
                    {
                        _requireRate = (int)(_requireRate * _requirePoints_1st_add_rate);
                        _levelupRequirementTable.Add(i, _requireRate);
                    }
                }
                
            }
        }

        void SetupVigorChangeHealthTable()
        {
            int _temp_val = init_b_health;

            for (int i = 1; i < 71; i++)
            {
                if (i <= vigor_1st_cap_lvl)
                {
                    _temp_val += vigor_inital_add_rate;
                }
                else if (i <= vigor_2nd_cap_lvl)
                {
                    _temp_val += vigor_1st_cap_add_rate;
                }
                else
                {
                    _temp_val += vigor_2nd_cap_add_rate;
                }

                //Debug.Log(i + ", _temp_val = " + _temp_val);
                _vigorChangeHealthTable.Add(i, _temp_val);
            }
        }

        void SetupAdaptationChangeFocusTable()
        {
            int _temp_val = 0;

            for (int i = 1; i < 71; i++)
            {
                if (i <= adaptation_1st_cap_lvl)
                {
                    _temp_val += adaptation_inital_add_rate;
                }
                else if (i <= adaptation_2nd_cap_lvl)
                {
                    _temp_val += adaptation_1st_cap_add_rate;
                }
                else
                {
                    _temp_val += adaptation_2nd_cap_add_rate;
                }

                _attunementChangeFocusTable.Add(i, _temp_val);
            }
        }

        void SetupEnduranceChangeStaminaTable()
        {
            int _temp_val = init_b_stamina;

            for (int i = 1; i < 71; i++)
            {
                if (i <= endurance_1st_cap_lvl)
                {
                    _temp_val += endurance_inital_add_rate;
                }
                else if (i <= endurance_2nd_cap_lvl)
                {
                    _temp_val += endurance_1st_cap_add_rate;
                }
                else
                {
                    _temp_val += endurance_2nd_cap_add_rate;
                }

                //Debug.Log(i + ", _temp_val = " + _temp_val);
                _enduranceChangeStaminaTable.Add(i, _temp_val);
            }
        }
        
        void SetupAttributesRemainderChangeStatsTable()
        {
            int _temp_second_val = 0;
            int _temp_third_val = 0;
            int _temp_forth_val = 0;

            for (int i = 1; i < 71; i++)
            {
                int _remainder = i % 4;
                if (_remainder >= 2)
                {
                    if (_remainder == 2)
                    {
                        _temp_second_val++;
                    }
                    else
                    {
                        _temp_third_val += 3;
                    }
                }
                else
                {
                    if (_remainder == 0)
                    {
                        _temp_second_val++;
                        _temp_forth_val++;
                    }
                }

                _vitalityChangePhysicalTable.Add(i, _temp_second_val);
                _intelligenceChangeMagicTable.Add(i, _temp_second_val);
                _strengthChangeFireTable.Add(i, _temp_second_val);
                _enduranceChangelightningTable.Add(i, _temp_second_val);
                _divinityChangeDarkTable.Add(i, _temp_second_val);

                _vitalityChangeEquipLoadTable.Add(i, _temp_third_val);

                _attunementChangeSlotTable.Add(i, _temp_forth_val);
            }
        }
        #endregion

        #region Init Level Change Stats Table.
        void SetupLevelRemainderChangeStatsTable()
        {
            int _temp_first_val = 0;
            int _temp_second_val = 0;
            int _temp_third_val = 0;

            for (int i = 1; i < 71; i++)
            {
                int _remainder = i % 3;
                if (_remainder != 2)
                {
                    if (_remainder != 1)
                    {
                        _temp_third_val++;
                    }
                    else
                    {
                        _temp_first_val++;
                    }
                }
                else
                {
                    _temp_second_val++;
                }

                _levelChangePhysicalTable.Add(i, _temp_first_val);

                _levelChangeMagicTable.Add(i, _temp_second_val);
                _levelChangeFireTable.Add(i, _temp_second_val);
                _levelChangeLightningTable.Add(i, _temp_second_val);

                _levelChangeBleedTable.Add(i, _temp_third_val);
                _levelChangePoisonTable.Add(i, _temp_third_val);
                _levelChangeFrostTable.Add(i, _temp_third_val);
                _levelChangeCurseTable.Add(i, _temp_third_val);
                _levelChangeDarkTable.Add(i, _temp_third_val);
            }
        }
        #endregion

        #endregion

        #region Public Init.
        public void InitRuntimeStats_BaseNonChangeable()
        {
            // SPEED.
            _walk_speed = b_walk_speed;
            _run_speed = b_run_speed;
            _jump_speed = b_jump_speed;
            _roll_speed = b_roll_speed;
            _jump_height = b_jump_height;

            // COST
            _run_stamina_cost = b_run_stamina_cost;
            _roll_stamina_cost = b_roll_stamina_cost;
            _jump_stamina_cost = b_jump_stamina_cost;

            // RECOVER
            _stamina_recover = b_stamina_recover;
            _focus_recover = b_focus_recover;

            // ATTACK POWER MULTIPIER
            _attPowMulti_weaponArt = b_attPowMulti_weaponArt;
            _attPowMulti_consumable = b_attPowMulti_consumable;
            _attPowMulti_spell = b_attPowMulti_spell;

            // OTHERS
            _item_discover = b_item_discover;
        }
        
        public void InitRuntimeStats_NewGame_BaseChangeable()
        {
            InitRefreshBaseStats();

            // BASE.
            _hp = b_hp;
            _fp = b_fp;
            _stamina = b_stamina;

            _mainHudManager.ReflectCurrentHealthChanges();
            _mainHudManager.ReflectCurrentFocusChanges();
            _mainHudManager.ReflectCurrentStaminaChanges();
        }
        
        public void InitRuntimeStats_LoadGame_BaseChangeable()
        {
            InitRefreshBaseStats();

            // BASE.
            _stamina = b_stamina;

            _mainHudManager.ReflectCurrentHealthChanges();
            _mainHudManager.ReflectCurrentFocusChanges();
            _mainHudManager.ReflectCurrentStaminaChanges();
        }

        void InitRefreshBaseStats()
        {
            RefreshBaseHealth();
            RefreshBaseFocus();
            RefreshBaseStamina();
            RefreshBaseDmgReductionAndResis();
        }
        #endregion

        #region Tick.
        void MonitorStatsEffectJobs()
        {
            for (int i = 0; i < _statsEffectJobsAmount; i++)
            {
                if (currentStatsEffectJobs[i].ProcessStatsEffectJob(this))
                {
                    /// Job Finished.
                    UnRegisterStatsEffectJobAtIndex(i);
                }
            }
        }

        void MonitorWeaponBuffJobs()
        {
            for (int i = 0; i < _weaponBuffJobsAmount; i++)
            {
                if (currentWeaponBuffJobs[i].ProcessWeaponBuffJob(this))
                {
                    /// Job Finished.
                    UnRegisterWeaponBuffJobAtIndex(i);
                }
            }
        }

        void RecoverStaminaOverTime()
        {
            if (_stamina <= 1)
            {
                _states.SetIsWaitStaminaRecoverStatus(true);
            }

            if (_stamina != b_stamina)
            {
                if (!isPausingStaminaRecover)
                {
                    IncrementPlayerStaminaWhenNotFull();
                }

                if (_stamina > b_stamina)
                {
                    _stamina = b_stamina;
                }
                else if (_stamina < 0)
                {
                    _stamina = 0;
                }
            }

            if (_states._isWaitForStaminaRecover)
            {
                if (_stamina >= b_stamina_recover_movableRate)
                {
                    _states.SetIsWaitStaminaRecoverStatus(false);
                }
            }
        }
        #endregion
        
        #region Refresh Multi Stats / Volun Changes.
        void RefreshBaseHealth()
        {
            b_hp = attri_hp + multi_hp;
            _mainHudManager.UpdateSliderWidthByType(MainHudManager.StatSliderEnum.health);
            //b_hp = 10000;
        }

        void RefreshBaseStamina()
        {
            b_stamina = attri_stamina + multi_stamina;
            _mainHudManager.UpdateSliderWidthByType(MainHudManager.StatSliderEnum.stamina);
        }

        void RefreshBaseFocus()
        {
            b_fp = attri_fp + multi_fp;
            _mainHudManager.UpdateSliderWidthByType(MainHudManager.StatSliderEnum.focus);
        }

        void RefreshBaseDmgReduction_Endurance()
        {
            b_lightning_reduction = attri_lightning_reduction + multi_lightning_reduction;
        }

        void RefreshBaseDmgReduction_Vitality()
        {
            b_physical_reduction = attri_physical_reduction + multi_physical_reduction;
        }

        void RefreshBaseDmgReduction_Strength()
        {
            b_fire_reduction = attri_fire_reduction + multi_fire_reduction;
        }

        void RefreshBaseDmgReduction_Intelligence()
        {
            b_magic_reduction = attri_magic_reduction + multi_magic_reduction;
        }

        void RefreshBaseDmgReduction_Divinity()
        {
            b_dark_reduction = attri_dark_reduction + multi_dark_reduction;
        }

        public void RefreshBaseDmgReductionAndResis()
        {
            b_physical_reduction = attri_physical_reduction + multi_physical_reduction;
            b_strike_reduction = attri_physical_reduction + multi_strike_reduction;
            b_thrust_reduction = attri_physical_reduction + multi_thrust_reduction;
            b_slash_reduction = attri_physical_reduction + multi_slash_reduction;

            b_magic_reduction = attri_magic_reduction + multi_magic_reduction;
            b_fire_reduction = attri_fire_reduction + multi_fire_reduction;
            b_lightning_reduction = attri_lightning_reduction + multi_lightning_reduction;
            b_dark_reduction = attri_dark_reduction + multi_dark_reduction;

            b_bleed_resistance = attri_bleed_resistance + multi_bleed_resistance;
            b_poison_resistance = attri_poison_resistance + multi_poison_resistance;
            b_frost_resistance = attri_frost_resistance + multi_frost_resistance;
            b_curse_resistance = attri_curse_resistance + multi_curse_resistance;
        }

        public void RefreshVolunsWhenAIKilled(int _amount)
        {
            voluns += _amount;
            _mainHudManager.RefreshVolunTextWithEffect();
        }
        
        #region Calculate Voluns Ratio.
        public float GetLevelupRequireRatioProgress()
        {
            _levelupRequirementTable.TryGetValue(playerLevel + 1, out int retVal);
            return Mathf.Clamp(voluns / retVal, 0, 1);
        }
        #endregion

        #endregion

        #region Modify General Stats.

        #region Health.
        public void IncrementPlayerHealth(float _value)
        {
            _hp += _value;
            _hp = _hp > b_hp ? b_hp : _hp;
            _mainHudManager.ReflectCurrentHealthChanges();
        }

        public void DecrementPlayerHealth()
        {
            _hp -= _states._previousGetHitDamage;
            _mainHudManager.ReflectCurrentHealthChanges();

            if (_hp <= 0)
                _states.isDead = true;
        }
        #endregion

        #region Stamina.
        public void IncrementPlayerStamina(float _value)
        {
            _stamina += _value;
            _mainHudManager.ReflectCurrentStaminaChanges();
        }

        public void IncrementPlayerStaminaWhenNotFull()
        {
            _stamina += _stamina_recover * _states._delta;
            _mainHudManager.ReflectCurrentStaminaChanges();
        }
        
        public void DecrementPlayerStamina(float _value)
        {
            _stamina -= _value;
            _mainHudManager.ReflectCurrentStaminaChanges();
        }

        public void DecrementPlayerStaminaWhenAttacking(float _value)
        {
            if (_states.isReduceNextAttackCost)
            {
                DecrementPlayerStamina(_value * (1 - b_fighterMode_attack_stamina_cost_reducePerc));
            }
            else
            {
                DecrementPlayerStamina(_value);
            }

            isPausingStaminaRecover = true;
        }

        public void DecrementPlayerStaminaWhenBlocking(float _staminaTaken)
        {
            DecrementPlayerStamina(_staminaTaken);

            if (_stamina <= 0)
                _states._hasBlockingBroken = true;
        }

        public void DecrementPlayerStaminaWhenSprinting()
        {
            _stamina -= _run_stamina_cost * 1.5f * _states._delta;
            _mainHudManager.ReflectCurrentStaminaChanges();
        }

        public void DecrementPlayerStaminaWhenRunning()
        {
            _stamina -= _run_stamina_cost * _states._delta;
            _mainHudManager.ReflectCurrentStaminaChanges();
        }

        public void DecrementPlayerStaminaWhenRolling()
        {
            _stamina -= _roll_stamina_cost;
            _mainHudManager.ReflectCurrentStaminaChanges();
            isPausingStaminaRecover = true;
        }

        public void DecrementPlayerStaminaWhenJumping()
        {
            _stamina -= _jump_stamina_cost;
            _mainHudManager.ReflectCurrentStaminaChanges();
            isPausingStaminaRecover = true;
        }

        public void DecrementPlayerStaminaWhenEvading()
        {
            _stamina -= b_evade_stamina_cost;
            _mainHudManager.ReflectCurrentStaminaChanges();
            isPausingStaminaRecover = true;
        }
        #endregion

        #region Focus.
        public void IncrementPlayerFocus(float _value)
        {
            _fp += _value;
            _fp = _fp > b_fp ? b_fp : _fp;
            _mainHudManager.ReflectCurrentFocusChanges();
        }

        public void DecrementPlayerFocus(float _value)
        {
            _fp -= _value;
            _mainHudManager.ReflectCurrentFocusChanges();
        }
        #endregion

        #endregion

        #region Modify Multi Values.
        public void Modify_Multi_Hp_Value(float _value)
        {
            multi_hp += _value;
            RefreshBaseHealth();

            if (_hp > b_hp)
            {
                _hp = b_hp;
                _mainHudManager.ReflectCurrentHealthChanges();
            }
        }

        public void Modify_Multi_Stamina_Value(float _value)
        {
            multi_stamina += _value;
            RefreshBaseStamina();

            if (_stamina > b_stamina)
            {
                _stamina = b_stamina;
                _mainHudManager.ReflectCurrentStaminaChanges();
            }
        }

        public void Modify_Multi_Fp_Value(float _value)
        {
            multi_fp += _value;
            RefreshBaseFocus();
        }

        public void Modify_Multi_Vigor_Value(int value)
        {
            multi_vigor += value;
            vigor = attri_vigor + multi_vigor;

            OnVigorChangeStats();
            RefreshBaseHealth();
        }

        public void Modify_Multi_Adaptation_Value(int value)
        {
            multi_adaptation += value;
            adaptation = attri_adaptation + multi_adaptation;

            OnAdaptationChangeStats();
            RefreshBaseFocus();
        }

        public void Modify_Multi_Endurance_Value(int value)
        {
            multi_endurance += value;
            endurance = attri_endurance + multi_endurance;

            OnEnduranceChangeStats();
            RefreshBaseStamina();
            RefreshBaseDmgReduction_Endurance();
        }

        public void Modify_Multi_Vitality_Value(int value)
        {
            multi_vitality += value;
            vitality = attri_vitality + multi_vitality;

            OnVitalityChangeStats();
            RefreshBaseDmgReduction_Vitality();
        }

        public void Modify_Multi_Strength_Value(int value)
        {
            multi_strength += value;
            strength = attri_strength + multi_strength;

            OnStrengthChangeStats();
            RefreshBaseDmgReduction_Strength();
        }

        public void Modify_Multi_Intelligence_Value(int value)
        {
            multi_intelligence += value;
            intelligence = attri_intelligence + multi_intelligence;

            OnIntelligenceChangeStats();
            RefreshBaseDmgReduction_Intelligence();
        }

        public void Modify_Multi_Divinity_Value(int value)
        {
            multi_divinity += value;
            divinity = attri_divinity + multi_divinity;

            OnDivinityChangeStats();
            RefreshBaseDmgReduction_Divinity();
        }

        public void Modify_Multi_Hexes_Value(int value)
        {
            multi_hexes += value;
            hexes = attri_hexes + multi_hexes;
        }
        #endregion

        #region Refresh Attribute Changes.
        void OnPlayerLevelChangeStats()
        {
            attri_physical_reduction = GetValueFrom_LevelChangePhysicalTable(playerLevel);
            attri_magic_reduction = GetValueFrom_LevelChangeMagicTable(playerLevel);
            attri_fire_reduction = GetValueFrom_LevelChangeFireTable(playerLevel);
            attri_lightning_reduction = GetValueFrom_LevelChangeLightningTable(playerLevel);
            attri_dark_reduction = GetValueFrom_LevelChangeDarkTable(playerLevel);
            attri_bleed_resistance = GetValueFrom_LevelChangeBleedTable(playerLevel);
            attri_poison_resistance = GetValueFrom_LevelChangePoisonTable(playerLevel);
            attri_frost_resistance = GetValueFrom_LevelChangeFrostTable(playerLevel);
            attri_curse_resistance = GetValueFrom_LevelChangeCurseTable(playerLevel);
        }

        void OnVigorChangeStats()
        {
            attri_hp = GetValueFrom_VigorChangeHealthTable(vigor);
        }

        void OnAdaptationChangeStats()
        {
            b_attunement_slots = GetValueFrom_AdaptationChangeSlotTable(adaptation);
            attri_fp = GetValueFrom_AdaptationChangeFocusTable(adaptation);
        }

        void OnEnduranceChangeStats()
        {
            attri_stamina = GetValueFrom_EnduranceChangeStaminaTable(endurance);
            attri_lightning_reduction += GetValueFrom_EnduranceChangeLightningTable(endurance);
        }

        void OnVitalityChangeStats()
        {
            b_total_equip_load = GetValueFrom_VitalityChangeEquipLoadTable(vitality) + init_total_equip_load;
            attri_physical_reduction += GetValueFrom_VitalityChangePhysicalTable(vitality);
        }

        void OnStrengthChangeStats()
        {
            attri_fire_reduction += GetValueFrom_StrengthChangeFireTable(strength);
        }

        void OnIntelligenceChangeStats()
        {
            attri_magic_reduction += GetValueFrom_IntelligenceChangeMagicTable(intelligence);
        }

        void OnDivinityChangeStats()
        {
            attri_dark_reduction += GetValueFrom_DivinityChangeDarkTable(divinity);
        }

        void OnFortuneChangeStats()
        {
            b_item_discover = fortune + init_item_discover;
        }
        #endregion

        #region Get Values From Tables.
        public int GetValueFrom_VigorChangeHealthTable(int _vigor)
        {
            _vigorChangeHealthTable.TryGetValue(_vigor, out int retVal);
            return retVal;
        }

        public int GetValueFrom_AdaptationChangeFocusTable(int attunement)
        {
            _attunementChangeFocusTable.TryGetValue(attunement, out int retVal);
            return retVal;
        }

        public int GetValueFrom_EnduranceChangeStaminaTable(int _endurance)
        {
            _enduranceChangeStaminaTable.TryGetValue(_endurance, out int retVal);
            return retVal;
        }

        public float GetValueFrom_VitalityChangeEquipLoadTable(int _vitality)
        {
            _vitalityChangeEquipLoadTable.TryGetValue(_vitality, out float retVal);
            return retVal;
        }

        public int GetValueFrom_VitalityChangePhysicalTable(int _vitality)
        {
            _vitalityChangePhysicalTable.TryGetValue(_vitality, out int retVal);
            return retVal;
        }

        public int GetValueFrom_IntelligenceChangeMagicTable(int _intelligence)
        {
            _intelligenceChangeMagicTable.TryGetValue(_intelligence, out int retVal);
            return retVal;
        }

        public int GetValueFrom_StrengthChangeFireTable(int _strength)
        {
            _strengthChangeFireTable.TryGetValue(_strength, out int retVal);
            return retVal;
        }

        public int GetValueFrom_EnduranceChangeLightningTable(int _endurance)
        {
            _enduranceChangelightningTable.TryGetValue(_endurance, out int retVal);
            return retVal;
        }

        public int GetValueFrom_DivinityChangeDarkTable(int _divinity)
        {
            _divinityChangeDarkTable.TryGetValue(_divinity, out int retVal);
            return retVal;
        }

        public int GetValueFrom_AdaptationChangeSlotTable(int _attunement)
        {
            _attunementChangeSlotTable.TryGetValue(_attunement, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangePhysicalTable(int _level)
        {
            _levelChangePhysicalTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeMagicTable(int _level)
        {
            _levelChangeMagicTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeFireTable(int _level)
        {
            _levelChangeFireTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeLightningTable(int _level)
        {
            _levelChangeLightningTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeDarkTable(int _level)
        {
            _levelChangeDarkTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeBleedTable(int _level)
        {
            _levelChangeBleedTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangePoisonTable(int _level)
        {
            _levelChangePoisonTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeFrostTable(int _level)
        {
            _levelChangeFrostTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetValueFrom_LevelChangeCurseTable(int _level)
        {
            _levelChangeCurseTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        #region Levelup Require.
        public int GetLevelUpRequireFromTable(int _level)
        {
            _levelupRequirementTable.TryGetValue(_level, out int retVal);
            return retVal;
        }

        public int GetNextPreviewLevelupRequireFromTable()
        {
            _levelupRequirementTable.TryGetValue(_prev_playerLevel + 1, out int retVal);
            return retVal;
        }

        public int GetLevelupRequireAfterNextLevelFromTable()
        {
            _levelupRequirementTable.TryGetValue(_prev_playerLevel + 2, out int retVal);
            return retVal;
        }
        #endregion
        
        #endregion

        #region Leveling Menu.
        public void OnLevelingMenu_RefreshPreviewAttributes()
        {
            // ATTRIBUTES
            _prev_playerLevel = playerLevel;
            _prev_voluns = voluns;
            _prev_vigor = vigor;
            _prev_adaptation = adaptation;
            _prev_endurance = endurance;
            _prev_vitality = vitality;
            _prev_strength = strength;
            _prev_hexes = hexes;
            _prev_intelligence = intelligence;
            _prev_divinity = divinity;
            _prev_fortune = fortune;

            // GENERAL
            _prev_hp = b_hp;
            _prev_fp = b_fp;
            _prev_stamina = b_stamina;

            // DAMAGE REDUCTION
            _prev_physical_reduction = b_physical_reduction;
            _prev_strike_reduction = b_strike_reduction;
            _prev_thrust_reduction = b_thrust_reduction;
            _prev_slash_reduction = b_slash_reduction;
            _prev_magic_reduction = b_magic_reduction;
            _prev_fire_reduction = b_fire_reduction;
            _prev_lightning_reduction = b_lightning_reduction;
            _prev_dark_reduction = b_dark_reduction;

            // STATUS RESISTANCES
            _prev_bleed_resistance = b_bleed_resistance;
            _prev_poison_resistance = b_poison_resistance;
            _prev_frost_resistance = b_frost_resistance;
            _prev_curse_resistance = b_curse_resistance;

            // OTHERS
            _prev_total_equip_load = b_total_equip_load;
            _prev_item_discover = b_item_discover;
            _prev_attunement_slots = b_attunement_slots;
        }
        
        public void OnLevelingConfirm_OverwriteBaseAttributes()
        {
            // TRUE ATTRIBUTES
            playerLevel = _prev_playerLevel;
            voluns = _prev_voluns;
            vigor = _prev_vigor;
            adaptation = _prev_adaptation;
            endurance = _prev_endurance;
            vitality = _prev_vitality;
            strength = _prev_strength;
            hexes = _prev_hexes;
            intelligence = _prev_intelligence;
            divinity = _prev_divinity;
            fortune = _prev_fortune;

            // ATTRI ATTRIBUTES
            attri_vigor = vigor - multi_vigor;
            attri_adaptation = adaptation - multi_adaptation;
            attri_endurance = endurance - multi_endurance;
            attri_vitality = vitality - multi_vitality;
            attri_strength = strength - multi_strength;
            attri_hexes = hexes - multi_hexes;
            attri_intelligence = intelligence - multi_intelligence;
            attri_divinity = divinity - multi_divinity;
            attri_fortune = fortune - multi_fortune;

            // GENERAL
            b_hp = _prev_hp;
            _hp = b_hp;
            b_fp = _prev_fp;
            _fp = b_fp;
            b_stamina = _prev_stamina;
            _stamina = b_stamina;

            // DAMAGE REDUCTION
            b_physical_reduction = _prev_physical_reduction;
            b_strike_reduction = _prev_strike_reduction;
            b_thrust_reduction = _prev_thrust_reduction;
            b_slash_reduction = _prev_slash_reduction;
            b_magic_reduction = _prev_magic_reduction;
            b_fire_reduction = _prev_fire_reduction;
            b_lightning_reduction = _prev_lightning_reduction;
            b_dark_reduction = _prev_dark_reduction;

            // STATUS RESISTANCES
            b_bleed_resistance = _prev_bleed_resistance;
            b_poison_resistance = _prev_poison_resistance;
            b_frost_resistance = _prev_frost_resistance;
            b_curse_resistance = _prev_curse_resistance;

            // OTHERS
            b_total_equip_load = _prev_total_equip_load;
            b_item_discover = _prev_item_discover;
            b_attunement_slots = _prev_attunement_slots;

            _mainHudManager.OnLevelingConfirm_RefreshAllSliders();
        }

        #region Modifly Preview Values.
        public void Modifly_Prev_Health()
        {
            attri_hp = GetValueFrom_VigorChangeHealthTable(_prev_vigor);
            _prev_hp = attri_hp + multi_hp;
        }

        public void Modifly_Prev_Focus()
        {
            attri_fp = GetValueFrom_AdaptationChangeFocusTable(_prev_adaptation);
            _prev_fp = attri_fp + multi_fp;
        }

        public void Modifly_Prev_Stamina()
        {
            attri_stamina = GetValueFrom_EnduranceChangeStaminaTable(_prev_endurance);
            _prev_stamina = attri_stamina + multi_stamina;
        }

        public void Modifly_Prev_EquipLoad()
        {
            _prev_total_equip_load = GetValueFrom_VitalityChangeEquipLoadTable(_prev_vitality) + init_total_equip_load;
        }

        public void Modifly_Prev_PhyReduct()
        {
            attri_physical_reduction = GetValueFrom_VitalityChangePhysicalTable(_prev_vitality) + GetValueFrom_LevelChangePhysicalTable(_prev_playerLevel);

            _prev_physical_reduction = attri_physical_reduction + multi_physical_reduction;
            _prev_strike_reduction = attri_physical_reduction + multi_strike_reduction;
            _prev_thrust_reduction = attri_physical_reduction + multi_thrust_reduction;
            _prev_slash_reduction = attri_physical_reduction + multi_slash_reduction;
        }

        public void Modifly_Prev_MagicReduct()
        {
            attri_magic_reduction = GetValueFrom_IntelligenceChangeMagicTable(_prev_intelligence) + GetValueFrom_LevelChangeMagicTable(_prev_playerLevel);
            _prev_magic_reduction = attri_magic_reduction + multi_magic_reduction;
        }

        public void Modifly_Prev_FireReduct()
        {
            attri_fire_reduction = GetValueFrom_StrengthChangeFireTable(_prev_strength) + GetValueFrom_LevelChangeFireTable(_prev_playerLevel);
            _prev_fire_reduction = attri_fire_reduction + multi_fire_reduction;
        }

        public void Modifly_Prev_LightningReduct()
        {
            attri_lightning_reduction = GetValueFrom_EnduranceChangeLightningTable(_prev_endurance) + GetValueFrom_LevelChangeLightningTable(_prev_playerLevel);
            _prev_lightning_reduction = attri_lightning_reduction + multi_lightning_reduction;
        }

        public void Modifly_Prev_DarkReduct()
        {
            attri_dark_reduction = GetValueFrom_DivinityChangeDarkTable(_prev_divinity) + GetValueFrom_LevelChangeDarkTable(_prev_playerLevel);
            _prev_dark_reduction = attri_dark_reduction + multi_dark_reduction;
        }

        public void Modifly_Prev_BleedResis()
        {
            attri_bleed_resistance = GetValueFrom_LevelChangeBleedTable(_prev_playerLevel);
            _prev_bleed_resistance = attri_bleed_resistance + multi_bleed_resistance;
        }

        public void Modifly_Prev_PoisonResis()
        {
            attri_poison_resistance = GetValueFrom_LevelChangePoisonTable(_prev_playerLevel);
            _prev_poison_resistance = attri_poison_resistance + multi_poison_resistance;
        }

        public void Modifly_Prev_FrostResis()
        {
            attri_frost_resistance = GetValueFrom_LevelChangeFrostTable(_prev_playerLevel);
            _prev_frost_resistance = attri_frost_resistance + multi_frost_resistance;
        }

        public void Modifly_Prev_CurseResis()
        {
            attri_curse_resistance = GetValueFrom_LevelChangeCurseTable(_prev_playerLevel);
            _prev_curse_resistance = attri_curse_resistance + multi_curse_resistance;
        }

        public void Modifly_Prev_attunementSlots()
        {
            _prev_attunement_slots = GetValueFrom_AdaptationChangeSlotTable(_prev_adaptation);
        }

        public void ModiflyLevelAffectedStatsPreview()
        {
            Modifly_Prev_PhyReduct();

            Modifly_Prev_MagicReduct();
            Modifly_Prev_FireReduct();
            Modifly_Prev_LightningReduct();

            Modifly_Prev_DarkReduct();
            Modifly_Prev_BleedResis();
            Modifly_Prev_PoisonResis();
            Modifly_Prev_FrostResis();
            Modifly_Prev_CurseResis();
        }
        #endregion

        #region Increment Value Changes.
        public void Increment_Prev_phyReduct()
        {
            _prev_physical_reduction++;
            _prev_strike_reduction++;
            _prev_thrust_reduction++;
            _prev_slash_reduction++;
        }
        
        public void Increment_Prev_magicReduct()
        {
            _prev_magic_reduction++;
        }
        
        public void Increment_Prev_fireReduct()
        {
            _prev_fire_reduction++;
        }
        
        public void Increment_Prev_lightningReduct()
        {
            _prev_lightning_reduction++;
        }
        
        public void Increment_Prev_darkReduct()
        {
            _prev_dark_reduction++;
        }

        public void Increment_Prev_bleedResis()
        {
            _prev_bleed_resistance++;
        }

        public void Increment_Prev_poisonResis()
        {
            _prev_poison_resistance++;
        }

        public void Increment_Prev_frostResis()
        {
            _prev_frost_resistance++;
        }

        public void Increment_Prev_curseResis()
        {
            _prev_curse_resistance++;
        }

        public void Increment_Prev_equipLoad()
        {
            _prev_total_equip_load += 3;
        }

        public void Increment_Prev_itemDiscover()
        {
            _prev_item_discover++;
        }

        public void Increment_Prev_attunementSlots()
        {
            _prev_attunement_slots++;
        }
        #endregion

        #region Decrement Value Changes.
        public void Decrement_Prev_itemDiscover()
        {
            _prev_item_discover--;
        }
        #endregion

        #endregion

        #region StatsEffect Jobs.
        public void RegisterNewStatsEffectJob(StatsEffectConsumable _consumable)
        {
            StatsEffectJob _existedStatsEffectJob = FindStatsEffectJobById(_consumable._referedStatsEffectItem.statsEffectJobId);
            if (_existedStatsEffectJob != null)
            {
                _existedStatsEffectJob._durationTimer = 0;
            }
            else
            {
                if (_statsEffectJobsAmount == maxStatsEffectAmount)
                    UnRegisterFirstStatsEffectJob();

                currentStatsEffectJobs[_statsEffectJobsAmount].RegisterNewStatsEffectJob(_consumable);
                _mainHudManager.RegisterStatsEffectIcon(_statsEffectJobsAmount, _consumable._referedStatsEffectItem.statsEffectIcon);;
                _statsEffectJobsAmount++;
            }
        }

        void UnRegisterStatsEffectJobAtIndex(int _discardJobIndex)
        {
            currentStatsEffectJobs[_discardJobIndex]._statsEffectConsumable.OnCompleteReverseEffect(this);
            currentStatsEffectJobs[_discardJobIndex]._statsEffectConsumable.DestroyConsumableAfterJob();

            ShiftingStatsEffectJobPosition(_discardJobIndex);
            _statsEffectJobsAmount--;
        }

        void UnRegisterFirstStatsEffectJob()
        {
            currentStatsEffectJobs[0]._statsEffectConsumable.OnCompleteReverseEffect(this);
            currentStatsEffectJobs[0]._statsEffectConsumable.DestroyConsumableAfterJob();

            ShiftingStatsEffectJobPosition(0);
            _statsEffectJobsAmount--;
        }

        void ShiftingStatsEffectJobPosition(int _discardJobIndex)
        {
            int _tailIndex = _statsEffectJobsAmount - 1;

            /// No need to shift.
            if (_discardJobIndex == _tailIndex)
            {
                /// UnRegister job.
                currentStatsEffectJobs[_tailIndex].UnRegisterStatsEffectJob();
                _mainHudManager.UnRegisterStatsEffectIcon(_tailIndex);;
            }
            else
            {
                int _shiftAmount = _tailIndex - _discardJobIndex;
                for (int i = 0; i < _shiftAmount; i++)
                {
                    StatsEffectJob _nextJob = currentStatsEffectJobs[_discardJobIndex + i + 1];

                    currentStatsEffectJobs[_discardJobIndex + i].OverrideStatsEffectJobRefs(_nextJob);
                    _mainHudManager.statsEffectIcons[_discardJobIndex + i].SwitchEffectIcon(_nextJob._referedConsumableItem.statsEffectIcon);
                    
                    /// If this is the last shift.
                    if (_shiftAmount - i == 1)
                    {
                        /// UnRegister the last job.
                        _nextJob.UnRegisterStatsEffectJob();
                        _mainHudManager.UnRegisterStatsEffectIcon(_tailIndex);;
                    }
                }
            }
        }

        StatsEffectJob FindStatsEffectJobById(int _statsEffectJobId)
        {
            for (int i = 0; i < _statsEffectJobsAmount; i++)
            {
                if (currentStatsEffectJobs[i]._statsEffectJobId == _statsEffectJobId)
                    return currentStatsEffectJobs[i];
            }

            return null;
        }
        #endregion

        #region Weapon Buff Jobs.
        public void RegisterNewWeaponBuffJob(TimedWeaponBuffAction _weaponBuffAction)
        {
            TimedWeaponBuffJob _existedWeawponBuffAction = FindWeaponBuffJobById(_weaponBuffAction.weaponBuffId);
            if (_existedWeawponBuffAction != null)
            {
                _existedWeawponBuffAction._durationTimer = 0;
            }
            else
            {
                if (_weaponBuffJobsAmount == maxWeaponBuffAmount)
                    UnRegisterFirstWeaponBuffJob();

                currentWeaponBuffJobs[_weaponBuffJobsAmount].RegisterNewWeaponBuffJob(_weaponBuffAction);
                _mainHudManager.RegisterWeaponBuffEffectIcon(_weaponBuffJobsAmount, _weaponBuffAction.weaponBuffIcon);/*.weaponBuffEffectIcons[_weaponBuffJobsAmount].RegisterEffectIcon(_weaponBuffAction.weaponBuffIcon)*/;
                _weaponBuffJobsAmount++;
            }
        }

        void UnRegisterWeaponBuffJobAtIndex(int _discardJobIndex)
        {
            currentWeaponBuffJobs[_discardJobIndex]._weaponBuffAction.OnCompleteReverseEffect(this);

            ShiftingWeaponBuffJobPosition(_discardJobIndex);
            _weaponBuffJobsAmount--;
        }

        void UnRegisterFirstWeaponBuffJob()
        {
            currentWeaponBuffJobs[0]._weaponBuffAction.OnCompleteReverseEffect(this);

            ShiftingWeaponBuffJobPosition(0);
            _weaponBuffJobsAmount--;
        }

        void ShiftingWeaponBuffJobPosition(int _discardJobIndex)
        {
            int _tailIndex = _weaponBuffJobsAmount - 1;

            if (_discardJobIndex == _tailIndex)
            {
                /// No need to shift.
                currentWeaponBuffJobs[_tailIndex].UnRegisterWeaponBuffJob();
                _mainHudManager.UnRegisterWeaponBuffEffectIcon(_tailIndex);;
            }
            else
            {
                int _shiftAmount = _tailIndex - _discardJobIndex;
                for (int i = 0; i < _shiftAmount; i++)
                {
                    TimedWeaponBuffJob _nextJob = currentWeaponBuffJobs[_discardJobIndex + i + 1];

                    currentWeaponBuffJobs[_discardJobIndex + i].OverrideWeaponBuffJobRefs(_nextJob);
                    _mainHudManager.RegisterWeaponBuffEffectIcon(_discardJobIndex + i, _nextJob._weaponBuffAction.weaponBuffIcon);;

                    /// If this is the last shift.
                    if (_shiftAmount - i == 1)
                    {
                        /// UnRegister the last job.
                        _nextJob.UnRegisterWeaponBuffJob();
                        _mainHudManager.UnRegisterWeaponBuffEffectIcon(_tailIndex);
                    }
                }
            }
        }

        TimedWeaponBuffJob FindWeaponBuffJobById(int _weaponBuffId)
        {
            for (int i = 0; i < _weaponBuffJobsAmount; i++)
            {
                if (currentWeaponBuffJobs[i]._weaponBuffId == _weaponBuffId)
                    return currentWeaponBuffJobs[i];
            }

            return null;
        }
        #endregion
        
        #region Checkpoint Event.
        public void RefreshPlayerStatsAction()
        {
            _hp = b_hp;
            _fp = b_fp;
            _stamina = b_stamina;

            _mainHudManager.OnCheckpointEven_RefillFullPlayerStats();

            for (int i = 0; i < _statsEffectJobsAmount; i++)
            {
                UnRegisterStatsEffectJobAtIndex(i);
            }

            for (int i = 0; i < _weaponBuffJobsAmount; i++)
            {
                UnRegisterWeaponBuffJobAtIndex(i);
            }

            _attPowMulti_weaponAction = 1;
        }
        #endregion

        #region Serialization.
        /// SAVE (NEW)
        public SavableStatusState SaveStateToSave()
        {
            SavableStatusState _savablePlayerStats = new SavableStatusState();

            /// General.
            _savablePlayerStats.savablePlayerName = characterName;
            _savablePlayerStats.savablePlayerLevel = playerLevel;
            _savablePlayerStats.savableVolun = voluns;

            /// Attributes.
            _savablePlayerStats.savableVigor = attri_vigor;
            _savablePlayerStats.savableAdaptation = attri_adaptation;
            _savablePlayerStats.savableEndurance = attri_endurance;
            _savablePlayerStats.savableVitality = attri_vitality;
            _savablePlayerStats.savableStrength = attri_strength;
            _savablePlayerStats.savableHexes = attri_hexes;
            _savablePlayerStats.savableIntelligence = attri_intelligence;
            _savablePlayerStats.savableDivinity = attri_divinity;
            _savablePlayerStats.savableFortune = attri_fortune;

            /// Stats.
            _savablePlayerStats.savable_hp = _hp;
            _savablePlayerStats.savable_fp = _fp;
            
            /// Vessels.
            _savablePlayerStats.savable_volunVessel_amount = b_volunVessel_amount;
            _savablePlayerStats.savable_volunFragment_amount = b_volunFragment_amount;
            _savablePlayerStats.savable_shatteredAmuletPiece_amount = b_shatteredAmuletPiece_amount;
            _savablePlayerStats.savable_sodurVessel_amount = b_sodurVessel_amount;
            
            return _savablePlayerStats;
        }

        /// SAVE (EXIST)
        public void OverwriteStateToSave()
        {
            SavableStatusState _overwriteState = _states._current_main_saveFile.savedStatusState;

            /// General.
            _overwriteState.savablePlayerName = characterName;
            _overwriteState.savablePlayerLevel = playerLevel;
            _overwriteState.savableVolun = voluns;

            /// Attributes.
            _overwriteState.savableVigor = attri_vigor;
            _overwriteState.savableAdaptation = attri_adaptation;
            _overwriteState.savableEndurance = attri_endurance;
            _overwriteState.savableVitality = attri_vitality;
            _overwriteState.savableStrength = attri_strength;
            _overwriteState.savableHexes = attri_hexes;
            _overwriteState.savableIntelligence = attri_intelligence;
            _overwriteState.savableDivinity = attri_divinity;
            _overwriteState.savableFortune = attri_fortune;

            /// Stats.
            _overwriteState.savable_hp = _hp;
            _overwriteState.savable_fp = _fp;

            /// Vessels.
            _overwriteState.savable_volunVessel_amount = b_volunVessel_amount;
            _overwriteState.savable_volunFragment_amount = b_volunFragment_amount;
            _overwriteState.savable_shatteredAmuletPiece_amount = b_shatteredAmuletPiece_amount;
            _overwriteState.savable_sodurVessel_amount = b_sodurVessel_amount;
        }

        /// LOAD
        public void LoadStatsFromSave(SavableStatusState _savablePlayerStats)
        {
            /// General.
            characterName = _savablePlayerStats.savablePlayerName;
            playerLevel = _savablePlayerStats.savablePlayerLevel;
            voluns = _savablePlayerStats.savableVolun;

            /// Attributes.
            attri_vigor = _savablePlayerStats.savableVigor;
            attri_adaptation = _savablePlayerStats.savableAdaptation;
            attri_endurance = _savablePlayerStats.savableEndurance;
            attri_vitality = _savablePlayerStats.savableVitality;
            attri_strength = _savablePlayerStats.savableStrength;
            attri_hexes = _savablePlayerStats.savableHexes;
            attri_intelligence = _savablePlayerStats.savableIntelligence;
            attri_divinity = _savablePlayerStats.savableDivinity;
            attri_fortune = _savablePlayerStats.savableFortune;

            /// Stats.
            _hp = _savablePlayerStats.savable_hp;
            _fp = _savablePlayerStats.savable_fp;
            
            /// Vessels.
            b_volunVessel_amount = _savablePlayerStats.savable_volunVessel_amount;
            b_volunFragment_amount = _savablePlayerStats.savable_volunFragment_amount;
            b_shatteredAmuletPiece_amount = _savablePlayerStats.savable_shatteredAmuletPiece_amount;
            b_sodurVessel_amount = _savablePlayerStats.savable_sodurVessel_amount;
        }
        #endregion
    }

    [Serializable]
    public class StatsEffectJob
    {
        [ReadOnlyInspector]
        public StatsEffectConsumable _statsEffectConsumable;
        [ReadOnlyInspector]
        public StatsEffectConsumableItem _referedConsumableItem;
        [ReadOnlyInspector]
        public float _durationTimer;
        [ReadOnlyInspector]
        public int _statsEffectJobId;
        [ReadOnlyInspector]
        public bool _hasExecuteOnce;

        public void RegisterNewStatsEffectJob(StatsEffectConsumable _statsEffectConsumable)
        {
            this._statsEffectConsumable = _statsEffectConsumable;
            _referedConsumableItem = _statsEffectConsumable._referedStatsEffectItem;
            _durationTimer = 0;
            _statsEffectJobId = _referedConsumableItem.statsEffectJobId;
        }

        public void UnRegisterStatsEffectJob()
        {
            /// If _statsEffectConsumable is null, probaby it's been destroyed.
            if (_statsEffectConsumable != null)
            {
                _statsEffectConsumable = null;
                _referedConsumableItem = null;
            }

            _durationTimer = 0;
            _statsEffectJobId = 0;
            _hasExecuteOnce = false;
        }

        public bool ProcessStatsEffectJob(StatsAttributeHandler _statsHandler)
        {
            _durationTimer += _statsHandler._states._delta;
            if (_durationTimer >= _referedConsumableItem.durationalRate)
            {
                return true;
            }

            if (_referedConsumableItem.isRepeatable)
            {
                _statsEffectConsumable.ExecuteStatsEffect(_statsHandler);
            }
            else
            {
                if (!_hasExecuteOnce)
                {
                    _statsEffectConsumable.ExecuteStatsEffect(_statsHandler);
                    _hasExecuteOnce = true;
                }
            }

            return false;
        }

        public void OverrideStatsEffectJobRefs(StatsEffectJob _overwriteStatsEffectJob)
        {
            _statsEffectConsumable = _overwriteStatsEffectJob._statsEffectConsumable;
            _referedConsumableItem = _overwriteStatsEffectJob._referedConsumableItem;
            _durationTimer = _overwriteStatsEffectJob._durationTimer;
            _statsEffectJobId = _overwriteStatsEffectJob._statsEffectJobId;
            _hasExecuteOnce = _overwriteStatsEffectJob._hasExecuteOnce;
        }
    }

    [Serializable]
    public class TimedWeaponBuffJob
    {
        [ReadOnlyInspector]
        public TimedWeaponBuffAction _weaponBuffAction;
        [ReadOnlyInspector]
        public float _durationTimer;
        [ReadOnlyInspector]
        public int _weaponBuffId;

        public void RegisterNewWeaponBuffJob(TimedWeaponBuffAction _weaponBuffAction)
        {
            this._weaponBuffAction = _weaponBuffAction;
            _weaponBuffId = _weaponBuffAction.weaponBuffId;
            _durationTimer = 0;
        }

        public void UnRegisterWeaponBuffJob()
        {
            _weaponBuffAction = null;
            _weaponBuffId = 0;
            _durationTimer = 0;
        }

        public bool ProcessWeaponBuffJob(StatsAttributeHandler _statsHandler)
        {
            _durationTimer += _statsHandler._states._delta;
            if (_durationTimer >= _weaponBuffAction.durationRate)
            {
                return true;
            }

            _weaponBuffAction.ExecuteWeaponBuffEffect(_statsHandler);
            return false;
        }

        public void OverrideWeaponBuffJobRefs(TimedWeaponBuffJob _overwriteWeaponBuffJob)
        {
            _weaponBuffAction = _overwriteWeaponBuffJob._weaponBuffAction;
            _durationTimer = _overwriteWeaponBuffJob._durationTimer;
        }
    }
}

//void GetTruncateAndRemainder(out int _truncate, out double _remainder, int _level)
//{
//    /// dmg reduction and ele resistance are generally increse 1 by every 3 level of player.
//    float _deci = _level / 3;

//    _truncate = (int)_deci;
//    _remainder = _level - (Math.Round(_deci) * 3);
//}

//public int GetAttriPhysicalReductValueByNum(int _vitality, int _level)
//{
//    /// General Level.
//    int _truncate = 0;
//    double _remainder = 0;
//    int _incrementPoints = 0;

//    GetTruncateAndRemainder(out _truncate, out _remainder, _level);
//    _incrementPoints += _truncate;

//    /// Extra point when remainder is 1.
//    if (_remainder == 1)
//        _incrementPoints += 1;

//    /// Vitality.
//    _incrementPoints += (int)(_vitality / 2.0f);

//    return _incrementPoints;
//}

//public int GetAttriFireReductValueByNum(int _strength, int _level)
//{
//    /// General Level.
//    int _truncate = 0;
//    double _remainder = 0;
//    int _incrementPoints = 0;

//    GetTruncateAndRemainder(out _truncate, out _remainder, _level);
//    _incrementPoints += _truncate;

//    /// Extra point when remainder is 2.
//    if (_remainder == 2)
//        _incrementPoints += 1;

//    /// Strength.
//    _incrementPoints += (int)(_strength / 2.0f);

//    return _incrementPoints;
//}

//public int GetAttriLightningReductValueByNum(int _endurance, int _level)
//{
//    /// General Level.
//    int _truncate = 0;
//    double _remainder = 0;
//    int _incrementPoints = 0;

//    GetTruncateAndRemainder(out _truncate, out _remainder, _level);
//    _incrementPoints += _truncate;

//    /// Extra point when remainder is 2.
//    if (_remainder == 2)
//        _incrementPoints += 1;

//    /// Endurance.
//    _incrementPoints += (int)(_endurance / 2.0f);

//    return _incrementPoints;
//}

//[ObsoleteAttribute]
//public int GetBaseHealthValueByNum(int _vigor)
//{
//    int _temp_val = 0;
//    for (int i = 1; i <= _vigor; i++)
//    {
//        if (i <= vigor_1st_cap_lvl)
//        {
//            _temp_val += vigor_inital_add_rate;
//        }
//        else if (i <= vigor_2nd_cap_lvl)
//        {
//            _temp_val += vigor_1st_cap_add_rate;
//        }
//        else
//        {
//            _temp_val += vigor_2nd_cap_add_rate;
//        }
//    }

//    return _temp_val;
//}

//[ObsoleteAttribute]
//public int GetBaseStaminaValueByNum(int _endurance)
//{
//    int _temp_val = 0;
//    for (int i = 0; i < _endurance; i++)
//    {
//        if (i <= endurance_1st_cap_lvl)
//        {
//            _temp_val += endurance_inital_add_rate;
//        }
//        else if (i <= endurance_2nd_cap_lvl)
//        {
//            _temp_val += endurance_1st_cap_add_rate;
//        }
//        else
//        {
//            _temp_val += endurance_2nd_cap_add_rate;
//        }
//    }

//    return _temp_val;
//}


