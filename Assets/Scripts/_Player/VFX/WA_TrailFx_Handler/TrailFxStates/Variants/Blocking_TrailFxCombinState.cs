using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/TrailFxCombinState/Blocking Combin State")]
    public class Blocking_TrailFxCombinState : BaseTrailFxCombinState
    {
        public override void OnBlockingCombinState(WA_TrailFx_Handler _handler)
        {
            ///* Irrelvant.
            //Debug.LogError("Trail Fx Handler Error! Current Combin State 'BgGlow Dust' of 'OnBgGlowDustCombinState' Func is called and it's irrelvant.");
        }

        public override void OnBuffCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnBuffCombin();
        }

        public override void OnChargeAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnChargeAttackCombin();
        }

        public override void OnChargeEnchantCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnChargeEnchantCombin();
        }

        public override void OnHoldAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnHoldAttackCombin();
        }

        public override void OnHoldReadyCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnHoldReadyCombin();
        }

        public override void OnNormalAttackCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnNormalAttackCombin();
        }

        public override void OnNullCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnNullCombin();
        }

        public override void OnParryCombinState(WA_TrailFx_Handler _handler)
        {
            _handler.Blocking_OnParryCombin();
        }
    }
}