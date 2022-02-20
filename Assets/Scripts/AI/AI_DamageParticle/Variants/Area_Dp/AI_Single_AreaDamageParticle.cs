using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_Single_AreaDamageParticle : AI_AreaDamageParticle_Base
    {
        [Header("Dp Id.")]
        public int _area_Dp_ID;

        #region Callback Methods.
        public void OnParticleSystemStopped()
        {
            Off_Single_AreaDp();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            if (_hasHurtPlayer)
                return;

            HandleOverlapSphere();
        }
        #endregion

        #region Off Area Damage Effect.
        void Off_Single_AreaDp()
        {
            // Dp Base.
            RemoveFromActiveDpList();

            // Area Dp Base.
            Off_AreaDp_ResetStatus();
            
            ReturnToBackpack();
        }

        void ReturnToBackpack()
        {
            transform.parent = _aiSessionManager._ai_dpHub_Backpack;
            gameObject.SetActive(false);
        }
        #endregion

        #region Init.
        public override void Init()
        {
            AreaDamageParticle_Init();
        }
        #endregion
    }
}