using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/TrailFxCombinState/Charge Attack Combin State")]
    public class ChargeAttack_TrailFxCombinState : BaseTrailFxCombinState
    {
        public override void OnBlockingCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnBlockingCombin();
        }

        public override void OnBuffCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnBuffCombin();
        }

        public override void OnChargeAttackCombinState(WA_TrailFx_Handler _handler)
        {
            ///* Irrelvant.
            //Debug.LogError("Trail Fx Handler Error! Current Combin State 'Trail Dust' of 'OnTrailDustCombinState' Func is called and it's irrelvant.");
        }

        public override void OnChargeEnchantCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnChargeEnchantCombin();
        }

        public override void OnHoldAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnHoldAttackCombin();
        }

        public override void OnHoldReadyCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnHoldReadyCombin();
        }

        public override void OnNormalAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnNormalAttackCombin();
        }

        public override void OnNullCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnNullCombin();
        }

        public override void OnParryCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.ChargeAttack_OnParryCombin();
        }
    }
}