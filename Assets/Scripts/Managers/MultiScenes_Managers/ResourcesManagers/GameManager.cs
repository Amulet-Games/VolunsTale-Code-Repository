using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GameManager : MonoBehaviour
    {
        #region World Impact Effects.

        #region Poolables.
        [Header("WI Pools.")]
        public Wood_SceneObj_ImpactEffectPool _woodSceneObj_pool;
        public Stone_SceneObj_ImpactEffectPool _stoneSceneObj_pool;
        public Vest_SceneObj_ImpactEffectPool _vestSceneObj_pool;
        #endregion

        #region Singles.
        [Header("WI Singles.")]
        public Levelup_ImpactEffect _levelupImpactEffect;
        public BonfireRest_ImpactEffect _bonfireRestImpactEffect;
        public Parry_ImpactEffect _parryImpactEffect;
        public Block_ImpactEffect _blockImpactEffect;
        #endregion

        #endregion

        #region Weapon Action Effects.
        [Header("WA Effects.")]
        public BaseWeaponActionEffect[] _weaponActionEffects;
        Dictionary<int, BaseWeaponActionEffect> weaponActionDict = new Dictionary<int, BaseWeaponActionEffect>();
        #endregion

        #region Blood FXs.
        [Header("BFX Sticky Pool.")]
        public BFX_StickyUpdaterPool _stickyPool;

        [Header("BFX Handers Pool.")]
        public BFX_HandlerPool[] _bfxHandlerPools;
        Dictionary<int, BFX_HandlerPool> _bfxHandlerDict = new Dictionary<int, BFX_HandlerPool>();

        /// BloodFxUpdater Propery IDs
        [HideInInspector] public int _UseCustomTimeId;
        [HideInInspector] public int _TimeInFramesId;
        [HideInInspector] public int _LightIntencityId;

        /// DecalUpdater Propery IDs
        [HideInInspector] public int cutoutPropertyID;
        [HideInInspector] public int forwardDirPropertyID;
        #endregion

        #region Refs.
        [Header("Backpacks (Drops).")]
        public Transform _WI_Effect_Bp;
        public Transform _WA_Effect_Bp;
        public Transform _BloodFx_Bp;
        #endregion

        #region States.
        [Header("Enemy States.")]
        public State enemyPatrolState;
        public State enemyExitAggroFacedPlayerState;
        public State enemyBossAggroState;
        public State bossWaitForAnimState;
        public State bossIntroSequenceState;

        [Header("Player States.")]
        public State playerIdleState;
        public State playerWaitForAnimState;
        #endregion

        #region Resource Manager.
        [Header("Resources Manager.")]
        public ResourcesManager _resourcesManager;
        #endregion

        #region Vector3s.
        [HideInInspector] public readonly Vector3 vector3Zero = new Vector3(0, 0, 0);
        #endregion
        
        public static GameManager singleton;
        
        #region Init.
        public void Init()
        {
            InitSingleton_DeactivateManager();
            InitResourcesManager();
        }

        void InitSingleton_DeactivateManager()
        {
            if (singleton != null)
                Destroy(gameObject);
            else
                singleton = this;
        }

        void InitResourcesManager()
        {
            _resourcesManager.InitPlayerResources();
            _resourcesManager.InitEnemyResources();
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupSessionManagerImmanentParentRef();
            
            /// WI.
            Setup_Poolable_WorldImpactEffects();
            Setup_Single_WorldImpactEffects();

            /// BFX Handler.
            Setup_BFXHandlers_ProperyIDs();

            void SetupSessionManagerImmanentParentRef()
            {
                SessionManager _sessionManager = SessionManager.singleton;

                _sessionManager.immanent_GameManager = transform.parent.gameObject;
                _sessionManager.immanent_WI_Effects_Bp = _WI_Effect_Bp.gameObject;
                _sessionManager.immanent_WA_Effects_Bp = _WA_Effect_Bp.gameObject;
                _sessionManager.immanent_BloodFx_Bp = _BloodFx_Bp.gameObject;
            }
            
            void Setup_Poolable_WorldImpactEffects()
            {
                // Scene Object.
                _woodSceneObj_pool.PreWarm();
                _stoneSceneObj_pool.PreWarm();
                _vestSceneObj_pool.PreWarm();
            }

            void Setup_Single_WorldImpactEffects()
            {
                _levelupImpactEffect.Setup();
                _bonfireRestImpactEffect.Setup();
                _parryImpactEffect.Setup();
                _blockImpactEffect.Setup();
            }
            
            void Setup_BFXHandlers_ProperyIDs()
            {
                _UseCustomTimeId = Shader.PropertyToID("_UseCustomTime");
                _TimeInFramesId = Shader.PropertyToID("_TimeInFrames");
                _LightIntencityId = Shader.PropertyToID("_LightIntencity");

                cutoutPropertyID = Shader.PropertyToID("_Cutout");
                forwardDirPropertyID = Shader.PropertyToID("_DecalForwardDir");
            }
        }
        #endregion
        
        #region Weapon Actions Effects.
        public void Play_WA_Effect(WA_Effect_Profile _profile)
        {
            weaponActionDict.TryGetValue(_profile._id, out BaseWeaponActionEffect _wa_effect);
            _wa_effect.PlayEffect(_profile);
        }

        public BaseWeaponActionEffect Get_WA_Effect(int _id)
        {
            weaponActionDict.TryGetValue(_id, out BaseWeaponActionEffect _wa_effect);
            return _wa_effect;
        }
        #endregion

        #region BFX Handlers.
        public BFX_Handler GetTargetBfxHandler(int _id)
        {
            _bfxHandlerDict.TryGetValue(_id, out BFX_HandlerPool _bfxHandlerPool);
            return _bfxHandlerPool.Get();
        }

        public BFX_Handler GetFallOffBfxHandler()
        {
            _bfxHandlerDict.TryGetValue(5, out BFX_HandlerPool _bfxHandlerPool);
            return _bfxHandlerPool.Get();
        }
        #endregion

        ///  Savable Inventory - Init.
        public void GetInventoryBackpacksRefs(SavableInventory _inventory)
        {
            Transform _immanentParentTrans = transform.parent;

            _inventory.weaponBackpackTransform = _immanentParentTrans.GetChild(1);
            _inventory.armorBackpackTransform = _immanentParentTrans.GetChild(2);
            _inventory.ringBackpackTransform = _immanentParentTrans.GetChild(3);
            _inventory.charmBackpackTransform = _immanentParentTrans.GetChild(4);
            _inventory.powerupBackpackTransform = _immanentParentTrans.GetChild(5);
            _inventory.consumableBackpackTransform = _immanentParentTrans.GetChild(6);
            _inventory.INV_ItemsEffectsBackpackTransform = _immanentParentTrans.GetChild(7);
        }

        /// State Manager - Set Up.
        public void SetupWeaponActionEffectList(StateManager _states)
        {
            int _WA_EffectsCount = _weaponActionEffects.Length;
            for (int i = 0; i < _WA_EffectsCount; i++)
            {
                weaponActionDict.Add(_weaponActionEffects[i].id, _weaponActionEffects[i]);
                _weaponActionEffects[i].Setup(_states, _WA_Effect_Bp);
            }
        }

        /// State Manager - Set Up.
        public void Setup_BFX_Sticky_Handlers_Dictionary()
        {
            _stickyPool.PreWarm();

            int _bfxHandlerLength = _bfxHandlerPools.Length;
            for (int i = 0; i < _bfxHandlerLength; i++)
            {
                _bfxHandlerDict.Add(_bfxHandlerPools[i].bfxId, _bfxHandlerPools[i]);
                _bfxHandlerPools[i].PreWarm();
            }
        }
    }
}
