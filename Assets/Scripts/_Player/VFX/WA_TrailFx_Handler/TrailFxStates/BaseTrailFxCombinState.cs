using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseTrailFxCombinState : ScriptableObject
    {
        public abstract void OnBlockingCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnBuffCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnChargeAttackCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnChargeEnchantCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnHoldAttackCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnHoldReadyCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnNormalAttackCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnNullCombinState(WA_TrailFx_Handler _handler);

        public abstract void OnParryCombinState(WA_TrailFx_Handler _handler);
    }
}
