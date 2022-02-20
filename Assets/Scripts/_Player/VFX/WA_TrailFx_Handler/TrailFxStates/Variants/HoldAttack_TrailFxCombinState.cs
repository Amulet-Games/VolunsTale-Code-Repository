using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/TrailFxCombinState/Hold Attack Combin State")]
    public class HoldAttack_TrailFxCombinState : BaseTrailFxCombinState
    {
        public override void OnBlockingCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnBlockingCombin();
        }

        public override void OnBuffCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnBuffCombin();
        }

        public override void OnChargeAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnChargeAttackCombin();
        }

        public override void OnChargeEnchantCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnChargeEnchantCombin();
        }

        public override void OnHoldAttackCombinState(WA_TrailFx_Handler _handler)
        {
            ///* Irrelvant.
            //Debug.LogError("Trail Fx Handler Error! Current Combin State 'All' of 'OnAllCombinState' Func is called and it's irrelvant.");
        }

        public override void OnHoldReadyCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnHoldReadyCombin();
        }

        public override void OnNormalAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnNormalAttackCombin();
        }

        public override void OnNullCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnNullCombin();
        }

        public override void OnParryCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldAttack_OnParryCombin();
        }
    }
}