using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///* Handles Callback Functions like FixedUpdate, Update, LateUpdate for all the AI Managable Classes.

namespace SA
{
    public class AISessionManager : MonoBehaviour
    {
        [Header("AI General Data.")]
        public AIGeneralData _aiGeneralData;

        #region Passive Actions.
        [Header("Passive Actions (Drops).")]
        /// Throwable Weapons.
        public ReacquireFirstThrowable_PA reacquireFirstThrowable;
        public ReacquireSecondThrowable_PA reacquireSecondThrowable;

        /// Dual Weapon Mod.
        public SwitchWeaponReady_PA switchWeaponReady;
        public SwitchToFirstWeapon_PA switchToFirstWeapon;
        public SwitchToSecondWeapon_PA switchToSecondWeapon;

        /// Enemy Interactable Mod.
        public EquipFirstWeaponAfterPW_PA equipFWAfterUsedPowerWeapon = null;
        public EquipPowerWeapon_PA equipPowerWeaponInteractable = null;
        #endregion

        #region Managables.
        [Header("Managable (Drops).")]
        public AIGroupManagable _aiGroupManagable;
        public AIBossManagable _aiBossManagable;
        public AIEmptyManagable _aiEmptyManagable;
        public AI_Managable _currentAIManagable;
        #endregion

        #region Single Dp Array.
        [Header("Single DPs (Drops).")]
        public AI_Single_AreaDamageParticle[] _single_AreaDp_InSession;

        [Space(10)]
        public AI_Gnd_Proj_DamageParticle[] _single_ProjDp_InSession;
        #endregion

        #region Poolable Dps.
        [Header("Poolable DPs (Drops).")]
        public AI_AreaDamageParticlePool _bombExplode_AreaDp_Pool;
        #endregion
        
        #region Poolables AI Bfx Array.
        [Header("Poolable Bfx Handlers (Drops).")]
        public AI_BFX_HandlerPool[] _ai_Bfx_Pools;
        #endregion

        #region Poolable Weapons.
        [Header("Poolable Weapons (Drops).")]
        public ThrowableEnemyRuntimeWeaponPool _bombRuntimeWeaponPool;
        #endregion

        #region AI General Effects.
        [Header("Poolable AI General Effects (Drops).")]
        public JavelinBroken_AIGeneralEffectPool _javelinBroken_pool;
        #endregion

        #region Egil FXs.
        [Header("Egil Phase Change FXs (Drops).")]
        public ParticleSystem _egilPhaseChangeAuraFx;
        public Transform _egilPhaseChangeFxTransform;
        public ParticleSystem _egilPhaseChange_openingFx;
        public ParticleSystem _egilPhaseChange_chargeFx_1;
        public ParticleSystem _egilPhaseChange_chargeFx_2;
        public ParticleSystem _egilPhaseChange_EndFx;
        public Transform _egilDeathFxParentTransform;
        public ParticleSystem _egilDeathFx;
        #endregion

        #region Backpacks.
        [Header("Backpacks (Drops).")]
        public Transform _ai_dpHub_Backpack;
        public Transform _ai_Bfx_Backpack;
        public Transform _ai_generalFx_Backpack;
        #endregion

        #region Deltas.
        [Header("Deltas.")]
        [ReadOnlyInspector] public int _frameCount;
        [ReadOnlyInspector] public float _delta;
        [ReadOnlyInspector] public float _fixedDelta;
        #endregion
        
        #region Active Tick List.
        [Header("Active Weapon List.")]
        [HideInInspector] public List<ThrowableEnemyRuntimeWeaponBase> _activeThrowableWeapons;
        [ReadOnlyInspector] public bool _isActiveThrowableWeaponEmpty;

        [Header("Active Damage Particles List.")]
        [HideInInspector] public List<AI_DamageParticle> _activeDamageParticles;
        [ReadOnlyInspector] public bool _isActiveDamageParticlesEmpty;

        [Header("Active AI Bfx List.")]
        [ReadOnlyInspector] public List<AI_BFX_HandlerBase> _active_AI_Bfxs;
        [ReadOnlyInspector] public bool _isActive_AI_BfxsEmpty;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _playerState;
        [ReadOnlyInspector] public SavableManager _savableManager;
        #endregion

        #region Non Serialzied.
        int _activeThrowableWeaponAmount;
        int _activeDamageParticlesAmount;
        int _active_AI_BfxsAmount;
        
        Dictionary<int, AI_AreaDamageParticle_Base> _singlesAreaDpDict = new Dictionary<int, AI_AreaDamageParticle_Base>();
        Dictionary<int, AI_Gnd_Proj_DamageParticle> _singlesProjDpDict = new Dictionary<int, AI_Gnd_Proj_DamageParticle>();

        Dictionary<int, AI_BFX_HandlerPool> _ai_Bfx_PoolDict = new Dictionary<int, AI_BFX_HandlerPool>();
        #endregion

        public static AISessionManager singleton;
        void Awake()
        {
            InitSingleton();
            
            InitGetManagerRefs();
            
            Init_Singles_Dps_Dict();

            Init_AI_BfxHandlers_Dict();

            Init_Prewarm_Pools_Dps();

            Init_Prewarm_Pools_AIGenerals();

            InitActiveLists();

            InitManagables();
        }

        void Start()
        {
            SetupManagables();
            SetupSetCurrentManagable();
        }

        void FixedUpdate()
        {
            UpdateFixedDeltas();

            ManagableFixedTick();
        }

        void Update()
        {
            UpdateDeltas();

            ActiveThrowableWeaponsTick();

            ActiveDamageParticlesTick();

            Active_AI_BfxHandler_Tick();

            ManagableTick();
        }

        void LateUpdate()
        {
            ManagableLateTick();
        }

        #region Awake.
        void InitSingleton()
        {
            if (singleton != null)
                Destroy(this);
            else
                singleton = this;
        }

        void InitGetManagerRefs()
        {
            _playerState = SessionManager.singleton._states;
            _playerState._aiGroupManagable = _aiGroupManagable;

            _savableManager = SavableManager.singleton;
            _savableManager._aIBossManagable = _aiBossManagable;
        }

        void Init_Singles_Dps_Dict()
        {
            #region Singles Area Dp.
            for (int i = 0; i < _single_AreaDp_InSession.Length; i++)
            {
                _singlesAreaDpDict.Add(_single_AreaDp_InSession[i]._area_Dp_ID, _single_AreaDp_InSession[i]);
                _single_AreaDp_InSession[i].Init();
            }
            #endregion

            #region Singles Projectile Dp.
            for (int i = 0; i < _single_ProjDp_InSession.Length; i++)
            {
                _singlesProjDpDict.Add(_single_ProjDp_InSession[i]._proj_Dp_ID, _single_ProjDp_InSession[i]);
                _single_ProjDp_InSession[i].Init();
            }
            #endregion
        }

        void Init_AI_BfxHandlers_Dict()
        {
            for (int i = 0; i < _ai_Bfx_Pools.Length; i++)
            {
                _ai_Bfx_PoolDict.Add(_ai_Bfx_Pools[i]._ai_Bfx_ID, _ai_Bfx_Pools[i]);
                _ai_Bfx_Pools[i].PreWarm();
            }
        }

        void Init_Prewarm_Pools_Dps()
        {
            _bombExplode_AreaDp_Pool.PreWarm();
        }

        void Init_Prewarm_Pools_AIGenerals()
        {
            _javelinBroken_pool.PreWarm();
        }

        void InitActiveLists()
        {
            _activeThrowableWeapons = new List<ThrowableEnemyRuntimeWeaponBase>();
            _isActiveThrowableWeaponEmpty = true;

            _activeDamageParticles = new List<AI_DamageParticle>();
            _isActiveDamageParticlesEmpty = true;

            _active_AI_Bfxs = new List<AI_BFX_HandlerBase>();
            _isActive_AI_BfxsEmpty = true;
        }
        
        void InitManagables()
        {
            _aiGroupManagable.Init(this);
            _aiBossManagable.Init(this);
        }
        #endregion

        #region Start.
        void SetupManagables()
        {
            _aiGroupManagable.Setup();
            _aiBossManagable.Setup();
        }

        void SetupSetCurrentManagable()
        {
            _currentAIManagable = _aiGroupManagable;
        }
        #endregion

        #region Fixed Update.
        void UpdateFixedDeltas()
        {
            _fixedDelta = Time.fixedDeltaTime;
        }

        void ManagableFixedTick()
        {
            _currentAIManagable.FixedTick();
        }
        #endregion

        #region Update.
        void UpdateDeltas()
        {
            _delta = Time.deltaTime;
            _frameCount = Time.frameCount;
        }

        void ActiveThrowableWeaponsTick()
        {
            if (!_isActiveThrowableWeaponEmpty)
            {
                for (int i = 0; i < _activeThrowableWeaponAmount; i++)
                {
                    _activeThrowableWeapons[i].ReturnToPoolTick();
                }
            }
        }

        void ActiveDamageParticlesTick()
        {
            if (!_isActiveDamageParticlesEmpty)
            {
                for (int i = 0; i < _activeDamageParticlesAmount; i++)
                {
                    _activeDamageParticles[i].Tick();
                }
            }
        }

        void Active_AI_BfxHandler_Tick()
        {
            if (!_isActive_AI_BfxsEmpty)
            {
                for (int i = 0; i < _active_AI_BfxsAmount; i++)
                {
                    _active_AI_Bfxs[i].Tick();
                }
            }
        }

        void ManagableTick()
        {
            _currentAIManagable.Tick();
        }
        #endregion

        #region LateUpdate.
        void ManagableLateTick()
        {
            _currentAIManagable.LateTick();
        }
        #endregion

        #region Switch Managable.
        public void SwitchCurrentManagable_To_AIGroupManagable()
        {
            _currentAIManagable = _aiGroupManagable;
        }

        public void SwitchCurrentManagable_To_AIBossManagable()
        {
            _currentAIManagable = _aiBossManagable;
        }

        public void SwitchCurrentManagable_To_AIEmptyManagable()
        {
            _currentAIManagable = _aiEmptyManagable;
        }
        #endregion

        #region Add / Remove Active ThrowableWeapon.
        public void AddThrowableToActiveList(ThrowableEnemyRuntimeWeaponBase _weaponToAdd)
        {
            _activeThrowableWeaponAmount++;
            _activeThrowableWeapons.Add(_weaponToAdd);

            _isActiveThrowableWeaponEmpty = false;
        }

        public void RemoveThrowableFromActiveList(ThrowableEnemyRuntimeWeaponBase _weaponToRemove)
        {
            _activeThrowableWeaponAmount--;
            _activeThrowableWeapons.Remove(_weaponToRemove);

            if (_activeThrowableWeaponAmount < 1)
            {
                _isActiveThrowableWeaponEmpty = true;
            }
        }
        #endregion

        #region Add / Remove Active DP.
        public void AddDamageParticleToActiveList(AI_DamageParticle _dp)
        {
            _activeDamageParticlesAmount++;
            _activeDamageParticles.Add(_dp);

            _isActiveDamageParticlesEmpty = false;
        }

        public void RemoveDamageParticleToActiveList(AI_DamageParticle _dp)
        {
            _activeDamageParticlesAmount--;
            _activeDamageParticles.Remove(_dp);

            if (_activeDamageParticlesAmount < 1)
            {
                _isActiveDamageParticlesEmpty = true;
            }
        }
        #endregion

        #region Get DP By ID.
        public AI_AreaDamageParticle_Base GetSinglesAreaDP_ById(int _id)
        {
            _singlesAreaDpDict.TryGetValue(_id, out AI_AreaDamageParticle_Base _singletonAreaDp);
            return _singletonAreaDp;
        }
        
        public AI_Gnd_Proj_DamageParticle GetSingletonProjDP_ById(int _id)
        {
            _singlesProjDpDict.TryGetValue(_id, out AI_Gnd_Proj_DamageParticle _singletonProjDp);
            return _singletonProjDp;
        }
        #endregion

        #region Add / Remove Active AI Bfx.
        public void OnActiveListAdd_AI_Bfx_Handler(AI_BFX_HandlerBase _aiBfxHandler)
        {
            _active_AI_BfxsAmount++;
            _active_AI_Bfxs.Add(_aiBfxHandler);

            _isActive_AI_BfxsEmpty = false;
        }

        public void OnActiveListRemove_AI_Bfx_Handler(AI_BFX_HandlerBase _aiBfxHandler)
        {
            _active_AI_BfxsAmount--;
            _active_AI_Bfxs.Remove(_aiBfxHandler);

            if (_active_AI_BfxsAmount < 1)
            {
                _isActive_AI_BfxsEmpty = true;
            }
        }
        #endregion

        #region Get AI Bfx By ID.
        public AI_Poolable_BFX_Handler GetPoolableAIBfxHandler_ById(int _id)
        {
            _ai_Bfx_PoolDict.TryGetValue(_id, out AI_BFX_HandlerPool _aiBfxPool);
            return _aiBfxPool.Get();
        }
        #endregion

        #region On Player Death.
        public void OnPlayerDeath()
        {
            OnPlayerDeathSwitchManagable();

            void OnPlayerDeathSwitchManagable()
            {
                _currentAIManagable = _aiEmptyManagable;
            }
        }
        #endregion

        #region Egil Phase Change FXs.
        public void ShowEgilPhaseChangeOpeningFx()
        {
            AIStateManager _bossState = _aiBossManagable._bossInSession;
            _egilPhaseChangeFxTransform.parent = _bossState.mTransform;
            _egilPhaseChangeFxTransform.localPosition = _bossState.vector3Zero;
            _egilPhaseChangeFxTransform.localEulerAngles = _bossState.vector3Zero;
            _egilPhaseChangeFxTransform.localScale = _bossState.aiManager.vector3One;
            _egilPhaseChangeFxTransform.gameObject.SetActive(true);

            _egilPhaseChange_openingFx.Play();
        }

        public void ShowEgilPhaseChangeChargeFx()
        {
            _egilPhaseChange_chargeFx_1.Play();
            _egilPhaseChange_chargeFx_2.Play();
        }

        public void HideEgilPhaseChangeFx()
        {
            HidePhaseChangeFx();

            LeanTween.value(1, 0, 2.5f).setOnComplete(OnCompleteReparentPhaseChangeFx);

            void HidePhaseChangeFx()
            {
                _egilPhaseChange_openingFx.Stop();
                _egilPhaseChange_chargeFx_1.Stop();
                _egilPhaseChange_chargeFx_2.Stop();
                LeanTween.value(0, 1, 1f).setOnComplete(_egilPhaseChange_EndFx.Play);
            }

            void OnCompleteReparentPhaseChangeFx()
            {
                _egilPhaseChangeFxTransform.parent = transform;
                _egilPhaseChangeFxTransform.gameObject.SetActive(false);
            }
        }

        public void HideDissovledAmulet()
        {
            _aiBossManagable._egilAmuletDissolveMesh.gameObject.SetActive(false);
            _aiBossManagable._egilAmuletDissolveMesh.transform.parent = transform;
        }

        public void ShowEgilPhaseChangeAuraFx()
        {
            AIStateManager _bossState = _aiBossManagable._bossInSession;
            Transform _auraTransform = _egilPhaseChangeAuraFx.transform;

            _auraTransform.parent =  _bossState.anim.GetBoneTransform(HumanBodyBones.Chest);
            _auraTransform.localPosition = _bossState.aiManager.vector3Zero;
            _auraTransform.localEulerAngles = _bossState.vector3Zero;
            _auraTransform.localScale = _bossState.aiManager.vector3One;
            _auraTransform.gameObject.SetActive(true);

            _egilPhaseChangeAuraFx.Play();
        }

        public void HideEgil3rdPhaseAura()
        {
            if (_egilPhaseChangeAuraFx.isPlaying)
            {
                _egilPhaseChangeAuraFx.Stop();
                LeanTween.value(0, 1, 0.75f).setOnComplete(OnCompleteFxStoppingWait);
            }

            void OnCompleteFxStoppingWait()
            {
                _egilPhaseChangeAuraFx.gameObject.SetActive(false);
            }
        }
        #endregion

        #region On Boss Sequence Start / End.
        public void OnBossSequenceStart()
        {
            _aiGroupManagable.OnBossSequenceStart();
            SwitchCurrentManagable_To_AIEmptyManagable();
        }
        #endregion

        #region On Boss Death.
        public void OnBossDeathShowEgilDeathFx()
        {
            AIStateManager _bossState = _aiBossManagable._bossInSession;

            _egilDeathFxParentTransform.parent = _bossState.anim.transform;
            _egilDeathFxParentTransform.localPosition = _bossState.vector3Zero;
            _egilDeathFxParentTransform.localEulerAngles = _bossState.vector3Zero;
            _egilDeathFxParentTransform.localScale = _bossState.aiManager.vector3One;

            _egilDeathFx.gameObject.SetActive(true);
            _egilDeathFx.Play();
        }

        public void OnBossDeath_SetManagablesStatus()
        {
            _aiBossManagable.OnBossDeath();
            _aiGroupManagable.OnBossFightEnded_ActivateGroups();
            SwitchCurrentManagable_To_AIGroupManagable();
        }
        #endregion

        #region On Boss Fight Ended.
        public void OnBossFightEnded_SetManagablesStatus()
        {
            _aiGroupManagable.OnBossFightEnded_ActivateGroups();
            SwitchCurrentManagable_To_AIGroupManagable();
        }
        #endregion

        #region On Boss Checkpoint Refresh.
        public void OnBossCheckpointRefresh()
        {
            _aiBossManagable.OnBossCheckpointRefresh();

            LevelAreaFxManager.singleton.BossFightEnded_ReverseSnowFx();

            CheckpointRefreshResetBossFx();

            void CheckpointRefreshResetBossFx()
            {
                /// Aura.
                if (_egilPhaseChangeAuraFx.isPlaying)
                    _egilPhaseChangeAuraFx.Stop();

                _egilPhaseChangeAuraFx.gameObject.SetActive(false);
                _egilPhaseChangeAuraFx.transform.parent = transform;

                /// Phase Change Fx.
                if (_egilPhaseChange_openingFx.isPlaying)
                {
                    _egilPhaseChange_openingFx.Stop();
                    _egilPhaseChange_chargeFx_1.Stop();
                    _egilPhaseChange_chargeFx_2.Stop();
                }

                _egilPhaseChangeFxTransform.gameObject.SetActive(false);
                _egilPhaseChangeFxTransform.parent = transform;
            }
        }
        #endregion
    }

    public interface AI_Managable
    {
        void Init(AISessionManager _aiSessionManager);

        void Setup();

        void FixedTick();

        void Tick();

        void LateTick();
    }

    [System.Serializable]
    public class AIGeneralData
    {
        [Header("AI Volun Drops.")]
        public int _warriorVolunDrops = 289;
        public int _bomberVolunDrops = 275;
        public int _marksmanVolunDrops = 261;
        public int _swordsmanVolunDrops = 600;
        public int _shieldmanVolunDrops = 570;
        public int _lancerVolunDrops = 630;
        public int _egilVolunDrops = 630;

        public int GetVolunDropAmount(EnemyTypeEnum _enemyType)
        {
            switch (_enemyType)
            {
                case EnemyTypeEnum.Egil:
                    return 0;
                case EnemyTypeEnum.Bomber:
                    return _bomberVolunDrops;
                case EnemyTypeEnum.Warrior:
                    return _warriorVolunDrops;
                case EnemyTypeEnum.Marksman:
                    return _marksmanVolunDrops;
                case EnemyTypeEnum.Swordsman:
                    return _swordsmanVolunDrops;
                case EnemyTypeEnum.Shieldman:
                    return _shieldmanVolunDrops;
                case EnemyTypeEnum.Lancer:
                    return _lancerVolunDrops;
                case EnemyTypeEnum.Thief:
                    return 0;
                case EnemyTypeEnum.Assassin:
                    return 0;
                default:
                    return 0;
            }
        }
    }
}