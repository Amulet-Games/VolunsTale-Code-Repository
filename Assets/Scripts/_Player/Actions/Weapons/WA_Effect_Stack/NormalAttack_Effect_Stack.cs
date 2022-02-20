using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/WA_Effect_Stack/NormalAttack")]
    public class NormalAttack_Effect_Stack : WA_EffectStack
    {
        [Header("1st.")]
        public WA_Effect_Profile _1st_effect_profile;

        [Header("2nd.")]
        public WA_Effect_Profile _2nd_effect_profile;

        [Header("3rd.")]
        public WA_Effect_Profile _3rd_effect_profile;

        [Header("4th.")]
        public WA_Effect_Profile _4th_effect_profile;
    }
}