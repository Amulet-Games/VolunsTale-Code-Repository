using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace SA
{
    public class AIBossManagable : MonoBehaviour, AI_Managable
    {
        [Header("AI Session Manager.")]
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;

        [Header("AI Boss Trigger (Drops).")]
        public AIBossTrigger _aiBossTrigger;

        [Header("AI Boss Gate (Drops).")]
        public GameObject _bossGatesParentObject;

        [Header("AI Boss (Drops).")]
        public AIStateManager _bossInSession;
        public Material _bossMat;
        public float _boss_cutOff_initalLength = 1.64f;

        [Header("Boss Weapon (Drops).")]
        public NonThrowableEnemyRuntimeWeapon _bossRuntimeWeapon;
        public NonThrowableEnemyWeapon _bossEnemyWeapon;
        public Animator _bossWeaponAnimator;
        public MeshRenderer _bossWeaponMeshRenderer;
        public AISheathTransform _bossWeaponSequenceTransform;

        [Header("Boss Amulet Prop (Drops).")]
        public float _initalCutOffHeight = 3;
        public MeshRenderer _egilAmuletDissolveMesh;
        public GameObject _egilAmuletProp;
        [ReadOnlyInspector] public Material[] _amuletMaterials;

        [Header("Boss Defeated Dissolve Props / Mats.")]
        public Material _bossWeaponDissolveHideMat;
        public float _boss_defeated_DissolveSpeed = 2.5f;

        [Header("Sequence Director.")]
        public PlayableDirector _bossSequenceDirector;
        public PlayableAsset _bossOutroAsset;

        [Header("Status. (Serialized)")]
        [ReadOnlyInspector] public bool _isBossSequenceTriggered;
        [ReadOnlyInspector] public bool _isBossKilled;

        [HideInInspector] public int _cutoffHeightPropertyId;
        [HideInInspector] public int _cutoffLengthPropertyId;

        #region Init.
        public void Init(AISessionManager _aiSessionManager)
        {
            //Debug.Log("AI Boss Managable Init.");
            this._aiSessionManager = _aiSessionManager;

            InitSavableManagerRefs();

            InitCheckSavedBossState();

            InitBossInSession();
        }

        void InitSavableManagerRefs()
        {
            _aiSessionManager._savableManager._aIBossManagable = this;
        }

        void InitCheckSavedBossState()
        {
            SavableManager _savableManager = _aiSessionManager._savableManager;
            if (_savableManager.isContinueGame)
            {
                LoadSavedBossStateFromSave(_savableManager._prev_MainSavedFile.savedBossState);
            }
            else if (_savableManager.isLoadGame)
            {
                LoadSavedBossStateFromSave(_savableManager._spec_MainSavedFile.savedBossState);
            }
        }

        void InitBossInSession()
        {
            if (!_isBossKilled)
            {
                _bossInSession.BossInSessionInit(_aiSessionManager);
            }
            else
            {
                InitHideBoss_WhenKilled();
                InitHideBossProps_WhenKilled();
                InitDeactivateSequenceDirector_WhenKilled();
            }
        }

        void InitHideBoss_WhenKilled()
        {
            _bossInSession.gameObject.SetActive(false);
        }

        void InitHideBossProps_WhenKilled()
        {
            _bossRuntimeWeapon.gameObject.SetActive(false);
            _egilAmuletProp.SetActive(false);
        }

        void InitDeactivateSequenceDirector_WhenKilled()
        {
            _bossSequenceDirector.gameObject.SetActive(false);
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            if (!_isBossKilled)
            {
                SetupGetDissolveAmuletMats();
                SetupBossInSession_1st_Time();
                SetupBossTrigger();
            }
        }

        void SetupGetDissolveAmuletMats()
        {
            _cutoffHeightPropertyId = Shader.PropertyToID("_CutoffHeight");
            _cutoffLengthPropertyId = Shader.PropertyToID("_CutoffLength");

            _amuletMaterials = _egilAmuletDissolveMesh.materials;
            _amuletMaterials[0].SetFloat(_cutoffHeightPropertyId, _initalCutOffHeight);
            _amuletMaterials[1].SetFloat(_cutoffHeightPropertyId, _initalCutOffHeight);

            _bossMat.SetFloat(_cutoffLengthPropertyId, _boss_cutOff_initalLength);
        }

        void SetupBossInSession_1st_Time()
        {
            _bossInSession._1st_SetupBossAIManager();
            SetupActivateBossWeapon();

            void SetupActivateBossWeapon()
            {
                _bossInSession.aiManager._1st_SetupBossFirstWeapon(_bossRuntimeWeapon);
                _bossRuntimeWeapon.SetupBossNonThrowableRuntimeWeapon(_bossEnemyWeapon);
                DisableBossWeaponAnimator();
                
                _bossWeaponMeshRenderer.material.SetFloat(_cutoffHeightPropertyId, 6.35f);
            }
        }

        void SetupBossTrigger()
        {
            ActivateBossTrigger();
            _aiBossTrigger.Setup(this);
        }
        #endregion

        #region FixedTick.
        public void FixedTick()
        {
            _bossInSession.FixedTick();
        }
        #endregion

        #region Tick.
        public void Tick()
        {
            _bossInSession.Tick();
        }
        #endregion

        #region LateTick.
        public void LateTick()
        {
            _bossInSession.LateTick();
        }
        #endregion

        #region Trigger Boss Sequence.
        public void TriggerBossSequence()
        {
            _aiSessionManager.OnBossSequenceStart();

            PlayBossSequence();
            DeactivateBossTrigger();
            ActivateBossGate();
        }

        void PlayBossSequence()
        {
            EnableBossWeaponAnimator();
            _bossSequenceDirector.Play();
        }
        #endregion

        #region On Sequence Show Boss UI.
        public void OnSequenceShowBossUI()
        {
            _bossInSession.aiDisplayManager.OnLockon();
        }
        #endregion

        #region On Boss Sequence End.
        public void OnBossSequenceEnd()
        {
            /// Show Boss Health Bar UI;
            SetupBossInSession_2nd_Time();
            DisableBossWeaponAnimator();
        }

        void SetupBossInSession_2nd_Time()
        {
            _bossInSession._2nd_SetupBossInSession();
        }
        #endregion

        #region On Boss Sequence Parent Weapon.
        public void ParentWeaponToBoss()
        {
            _bossRuntimeWeapon.ParentEnemyWeaponUnderHand();
        }
        #endregion

        #region Activate / Deactivate Trigger.
        public void ActivateBossTrigger()
        {
            _aiBossTrigger.gameObject.SetActive(true);
        }

        public void DeactivateBossTrigger()
        {
            _aiBossTrigger.gameObject.SetActive(false);
        }
        #endregion

        #region Activate Boss Gate.
        public void ActivateBossGate()
        {
            _bossGatesParentObject.SetActive(true);
        }
        #endregion

        #region Weapon Animator.
        void EnableBossWeaponAnimator()
        {
            _bossWeaponAnimator.enabled = true;
        }

        void DisableBossWeaponAnimator()
        {
            _bossWeaponAnimator.enabled = false;
        }
        #endregion

        #region Hide Amulet Prop.
        public void Egil3rdPhaseChange_SwitchAmuletAndPlayOpeningFX()
        {
            /// Switch Amulet.
            _egilAmuletProp.SetActive(false);
            _egilAmuletDissolveMesh.gameObject.SetActive(true);

            /// Play Opening FX.
            _aiSessionManager.ShowEgilPhaseChangeOpeningFx();
        }
        #endregion

        #region On Boss Death.
        public void OnBossDeath()
        {
            OnBossDeathSetStatus();
            BeginBossDeathDissolveEffectWait();

            void OnBossDeathSetStatus()
            {
                _isBossKilled = true;
            }

            void BeginBossDeathDissolveEffectWait()
            {
                LeanTween.value(0, 1, 0.28f).setOnComplete(OnCompleteCounter);
                
                void OnCompleteCounter()
                {
                    _aiSessionManager._playerState._mainHudManager.ShowBossVictoryScreen();
                    LevelAreaFxManager.singleton.PlayDefaultSnowFx();

                    LeanTween.value(0, 1, 1.5f).setOnComplete(BeginBossDeathDissolveEffect);
                }
            }
        }
        
        void BeginBossDeathDissolveEffect()
        {
            OnBossDeathDissolveEgil();
            OnBossDeathDissolveWeapon();
            OnBossDeathDissolveArena();

            void OnBossDeathDissolveEgil()
            {
                LeanTween.value(1.66f, -1.48f, _boss_defeated_DissolveSpeed).setEaseOutSine().setOnUpdate((value) => _bossMat.SetFloat(_cutoffLengthPropertyId, value));
            }

            void OnBossDeathDissolveWeapon()
            {
                _bossWeaponMeshRenderer.material = _bossWeaponDissolveHideMat;
                
                LeanTween.value(2.06f, -0.37f, _boss_defeated_DissolveSpeed - 1).setEaseOutSine().setOnUpdate((value) => _bossWeaponMeshRenderer.material.SetFloat(_cutoffLengthPropertyId, value));
            }

            void OnBossDeathDissolveArena()
            {
                _bossSequenceDirector.playableAsset = _bossOutroAsset;
                _bossSequenceDirector.Play();
            }
        }

        public void OnOutroSequenceEnd()
        {
            _bossGatesParentObject.SetActive(false);
            _bossSequenceDirector.gameObject.SetActive(false);
        }
        #endregion

        #region On Checkpoint Refresh.
        public void OnBossCheckpointRefresh()
        {
            ResetBossTrigger();
            RefreshBossWeapon();
            RefreshAmulet();
            ResetGatesParentObject();

            void ResetBossTrigger()
            {
                ActivateBossTrigger();
            }

            void RefreshBossWeapon()
            {
                ResetBossWeapon_Rb_Collider();

                ResetBossWeaponAnimator();

                ResetBossWeaponDissolveValue();

                ResetBossWeaponToSequenceTransform();

                void ResetBossWeapon_Rb_Collider()
                {
                    _bossRuntimeWeapon.rb.isKinematic = true;
                    _bossRuntimeWeapon.e_hook.SetColliderStatusToFalse();
                }

                void ResetBossWeaponAnimator()
                {
                    _bossWeaponAnimator.enabled = false;
                }

                void ResetBossWeaponDissolveValue()
                {
                    _bossWeaponMeshRenderer.material.SetFloat(_cutoffHeightPropertyId, 6.35f);
                }

                void ResetBossWeaponToSequenceTransform()
                {
                    Transform _bossRuntimeWeaponTransform = _bossRuntimeWeapon.transform;

                    _bossRuntimeWeaponTransform.parent = null;
                    _bossRuntimeWeaponTransform.localPosition = _bossWeaponSequenceTransform.pos;
                    _bossRuntimeWeaponTransform.localEulerAngles = _bossWeaponSequenceTransform.eulers;
                    _bossRuntimeWeaponTransform.localScale = _bossWeaponSequenceTransform.scale;
                }
            }

            void RefreshAmulet()
            {
                _egilAmuletProp.gameObject.SetActive(true);

                _egilAmuletDissolveMesh.transform.parent = _bossInSession.anim.GetBoneTransform(HumanBodyBones.RightHand);
                _amuletMaterials[0].SetFloat(_cutoffHeightPropertyId, _initalCutOffHeight);
                _amuletMaterials[1].SetFloat(_cutoffHeightPropertyId, _initalCutOffHeight);
            }

            void ResetGatesParentObject()
            {
                _bossGatesParentObject.SetActive(false);
            }
        }
        #endregion

        #region Serialization.
        public SavableBossState SaveBossStateToSave()
        {
            SavableBossState _savableBossState = new SavableBossState();
            _savableBossState.savableIsBossSequenceTriggered = _isBossSequenceTriggered;
            _savableBossState.savableIsBossKilled = _isBossKilled;
            return _savableBossState;
        }

        void LoadSavedBossStateFromSave(SavableBossState _savableBossState)
        {
            _isBossKilled = _savableBossState.savableIsBossKilled;
            _isBossSequenceTriggered = _savableBossState.savableIsBossSequenceTriggered;
        }
        #endregion
    }
}