using UnityEngine;

namespace SA
{
    public class WA_TrailFx_Handler : MonoBehaviour
    {
        [Header("TrailFx Combin State.")]
        ///* Blocking. (Block Dust + BgGlow)
        public Blocking_TrailFxCombinState _blockingCombinState;
        ///* Buff. (Trail + Block Dust + BgGlow)
        public Buff_TrailFxCombinState _buffCombinState;
        ///* Charge Attack. (BgGlow)
        public ChargeAttack_TrailFxCombinState _chargeAttackCombinState;
        ///* Charge Enchant. (Block Dust + BgGlow)
        public ChargeEnchant_TrailFxCombinState _chargeEnchantCombinState;
        ///* Hold Attack. (Trail + ATK Dust + BgGlow)
        public HoldAttack_TrailFxCombinState _holdAttackCombinState;
        ///* Hold Ready. (Block Dust + BgGlow)
        public HoldReady_TrailFxCombinState _holdReadyCombinState;
        ///* Normal Attack. (Trail + ATK Dust)
        public NormalAttack_TrailFxCombinState _normalAttackCombinState;
        ///* Do Nothing.
        public Null_TrailFxCombinState _nullTrailFxCombinState;
        ///* Parry. (Trail + ATK Dust + BgGlow)
        public Parry_TrailFxCombinState _parryCombinState;

        [Header("Current State.")]
        [ReadOnlyInspector, SerializeField] BaseTrailFxCombinState _currentTrailFxCombinState;

        [Header("Trail Fx.")]
        public ParticleSystem _trail_fx;

        [Header("Dust Fx.")]
        public ParticleSystem _block_dust_fx;
        public ParticleSystem _attack_dust_fx;

        [Header("Glow Fx.")]
        public ParticleSystem _bgGlowSmoke_fx;

        [Header("Player State.")]
        public AnimatorHook a_hook;
        
        public void Play_Block_TrailFx()
        {
            _currentTrailFxCombinState.OnBlockingCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_Buff_TrailFx()
        {
            _currentTrailFxCombinState.OnBuffCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_ChargeEnchant_TrailFx()
        {
            _currentTrailFxCombinState.OnChargeEnchantCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_ChargeAttack_TrailFx()
        {
            _currentTrailFxCombinState.OnChargeAttackCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_HoldATK_TrailFx()
        {
            _currentTrailFxCombinState.OnHoldAttackCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_HoldReady_TrailFx()
        {
            _currentTrailFxCombinState.OnHoldReadyCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_NormalATK_TrailFx()
        {
            _currentTrailFxCombinState.OnNormalAttackCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }
        
        public void Play_NullState_TrailFx()
        {
            _currentTrailFxCombinState.OnNullCombinState(this);

            a_hook._states._isUsedTrailFx = false;
        }
        
        public void Play_Parry_TrailFx()
        {
            _currentTrailFxCombinState.OnParryCombinState(this);

            a_hook.currentTrailFxHandler = this;
            a_hook._states._isUsedTrailFx = true;
        }

        #region Init.
        public void Init()
        {
            InitCurrentTrailFxCombinState();

            void InitCurrentTrailFxCombinState()
            {
                _currentTrailFxCombinState = _nullTrailFxCombinState;
            }
        }
        #endregion

        #region Blocking Combin State.
        public void Blocking_OnBuffCombin()
        {
            _trail_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void Blocking_OnChargeAttackCombin()
        {
            _block_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void Blocking_OnChargeEnchantCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void Blocking_OnHoldAttackCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void Blocking_OnHoldReadyCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void Blocking_OnNormalAttackCombin()
        {
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void Blocking_OnNullCombin()
        {
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void Blocking_OnParryCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Buff Combin State.
        public void Buff_OnBlockingCombin()
        {
            _trail_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void Buff_OnChargeAttackCombin()
        {
            _trail_fx.Stop();
            _block_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void Buff_OnChargeEnchantCombin()
        {
            _trail_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void Buff_OnHoldAttackCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void Buff_OnHoldReadyCombin()
        {
            _trail_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void Buff_OnNormalAttackCombin()
        {
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            _attack_dust_fx.Play();
            
            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void Buff_OnNullCombin()
        {
            _trail_fx.Stop();
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void Buff_OnParryCombin()
        {
            _block_dust_fx.Stop();
            
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Charge Attack Combin State.
        public void ChargeAttack_OnBlockingCombin()
        {
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void ChargeAttack_OnBuffCombin()
        {
            _trail_fx.Play();
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void ChargeAttack_OnChargeEnchantCombin()
        {
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void ChargeAttack_OnHoldAttackCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void ChargeAttack_OnHoldReadyCombin()
        {
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void ChargeAttack_OnNormalAttackCombin()
        {
            _bgGlowSmoke_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void ChargeAttack_OnNullCombin()
        {
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void ChargeAttack_OnParryCombin()
        {
            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Charge Enchant Combin State.
        public void ChargeEnchant_OnBlockingCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void ChargeEnchant_OnBuffCombin()
        {
            _trail_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void ChargeEnchant_OnChargeAttackCombin()
        {
            _block_dust_fx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void ChargeEnchant_OnHoldAttackCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void ChargeEnchant_OnHoldReadyCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void ChargeEnchant_OnNormalAttackCombin()
        {
            _bgGlowSmoke_fx.Stop();
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void ChargeEnchant_OnNullCombin()
        {
            _bgGlowSmoke_fx.Stop();
            _block_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void ChargeEnchant_OnParryCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Hold Attack Combin State.
        public void HoldAttack_OnBlockingCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void HoldAttack_OnBuffCombin()
        {
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void HoldAttack_OnChargeAttackCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void HoldAttack_OnChargeEnchantCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void HoldAttack_OnHoldReadyCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void HoldAttack_OnNormalAttackCombin()
        {
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void HoldAttack_OnNullCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void HoldAttack_OnParryCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Hold Ready Combin State.
        public void HoldReady_OnBlockingCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void HoldReady_OnBuffCombin()
        {
            _trail_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void HoldReady_OnChargeAttackCombin()
        {
            _block_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }
        
        public void HoldReady_OnChargeEnchantCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void HoldReady_OnHoldAttackCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void HoldReady_NormalAttackCombin()
        {
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void HoldReady_OnNullCombin()
        {
            _block_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }

        public void HoldReady_OnParryCombin()
        {
            _block_dust_fx.Stop();

            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Normal Attack Combin State.
        public void NormalAttack_OnBlockingCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void NormalAttack_OnBuffCombin()
        {
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void NormalAttack_OnChargeAttackCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void NormalAttack_OnChargeEnchantCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _bgGlowSmoke_fx.Play();
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void NormalAttack_OnHoldAttackCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void NormalAttack_OnHoldReadyCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void NormalAttack_OnParryCombin()
        {
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }

        public void NormalAttack_OnNullCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }
        #endregion

        #region Null Combin State.
        public void NullState_OnBlockingCombin()
        {
            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void NullState_OnBuffCombin()
        {
            _trail_fx.Play();
            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void NullState_OnChargeAttackCombin()
        {
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void NullState_OnChargeEnchantCombin()
        {
            _bgGlowSmoke_fx.Play();
            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void NullState_OnHoldAttackCombin()
        {
            _trail_fx.Play();
            _attack_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void NullState_OnHoldReadyCombin()
        {
            _block_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void NullState_OnNormalAttackCombin()
        {
            _trail_fx.Play();
            _attack_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void NullState_OnParryCombin()
        {
            _trail_fx.Play();
            _attack_dust_fx.Play();
            _bgGlowSmoke_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _parryCombinState;
        }
        #endregion

        #region Parry State.
        public void Parry_OnBlockingCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _blockingCombinState;
        }

        public void Parry_OnBuffCombin()
        {
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _buffCombinState;
        }

        public void Parry_OnChargeAttackCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeAttackCombinState;
        }

        public void Parry_OnChargeEnchantCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _chargeEnchantCombinState;
        }

        public void Parry_OnHoldAttackCombin()
        {
            /// Set Current State.
            _currentTrailFxCombinState = _holdAttackCombinState;
        }

        public void Parry_OnHoldReadyCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();

            _block_dust_fx.Play();

            /// Set Current State.
            _currentTrailFxCombinState = _holdReadyCombinState;
        }

        public void Parry_OnNormalAttackCombin()
        {
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _normalAttackCombinState;
        }

        public void Parry_OnNullCombin()
        {
            _trail_fx.Stop();
            _attack_dust_fx.Stop();
            _bgGlowSmoke_fx.Stop();

            /// Set Current State.
            _currentTrailFxCombinState = _nullTrailFxCombinState;
        }
        #endregion
    }
}