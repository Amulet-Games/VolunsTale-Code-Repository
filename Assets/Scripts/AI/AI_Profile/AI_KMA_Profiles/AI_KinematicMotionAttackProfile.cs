using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Profile/KMA Profile")]
    public class AI_KinematicMotionAttackProfile : ScriptableObject
    {
        [Header("Anim State.")]
        public AnimStateVariable KMA_1stHalf_AnimState;
        public AnimStateVariable KMA_2ndHalf_AnimState;
        public bool isApplyRootMotionInAnim;
        public float _2ndHalf_KMA_Offset;
        public float _KMA_diagonalExtraOffset = 0.75f;
        public float _KMA_playerWalkPosOffset = 3;
        public float _KMA_playerRunPosOffset = 5;
        public float _KMA_attackTurningPredict = 25;

        [Header("MSA AICombo.")]
        [Tooltip("The next attack follow up from this attack.")]
        public MSA_AICombo AICombo;

        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        public AI_AttackRefs _aiAttackRefs;
    }
}