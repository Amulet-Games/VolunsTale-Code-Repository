using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/TrailFxCombinState/Hold Ready Combin State")]
    public class HoldReady_TrailFxCombinState : BaseTrailFxCombinState
    {
        public override void OnBlockingCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnBlockingCombin();
        }

        public override void OnBuffCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnBuffCombin();
        }

        public override void OnChargeAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnChargeAttackCombin();
        }

        public override void OnChargeEnchantCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnChargeEnchantCombin();
        }

        public override void OnHoldAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnHoldAttackCombin();
        }

        public override void OnHoldReadyCombinState(WA_TrailFx_Handler _handler)
        {
            ///* Irrelvant.
            //Debug.LogError("Trail Fx Handler Error! Current Combin State 'Null' of 'OffAllCombinState' Func is called and it's irrelvant.");
        }

        public override void OnNormalAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.NullState_OnNormalAttackCombin();
        }

        public override void OnNullCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnNullCombin();
        }

        public override void OnParryCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.HoldReady_OnParryCombin();
        }
    }
}