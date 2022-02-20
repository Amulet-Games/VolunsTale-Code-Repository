using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Enumerable Phase/Egil 3rd Phase Change Dissolve Amulet Passive Action")]
    public class Egil3rdPhaseChangeDissolveAmulet : EP_AI_PassiveAction
    {
        [Header("Amulet Values.")]
        public float _initalCutOffHeight = 3;
        public float _amuletMaxVisibleValue = 4.58f;
        public float _amuletDissolvedValue = 2.5f;

        [Header("Leather Values.")]
        public float _leatherMaxVisibleValue = 3.8f;
        public float _leatherDissolvedValue = 2.9f;

        [Header("Tween Config.")]
        public float _dissolveCompleteTime;

        [Header("3rd Phase Chain Anim.")]
        public AnimStateVariable _egil_changePhase2_chain_4;
        
        int _cutoffHeightPropertyId;
        Material[] _amuletMaterials;

        public override void Init(AIManager _ai)
        {
            AIBossManagable _aiBossManagable = _ai.aISessionManager._aiBossManagable;
            _cutoffHeightPropertyId = _aiBossManagable._cutoffHeightPropertyId;
            _amuletMaterials = _aiBossManagable._amuletMaterials;
        }
        
        public override void Execute(AIManager ai)
        {
            AISessionManager _aiSessionManager = ai.aISessionManager;
            _aiSessionManager.ShowEgilPhaseChangeChargeFx();

            LeanTween.value(_amuletMaxVisibleValue, _amuletDissolvedValue, _dissolveCompleteTime).setEaseInSine().setOnUpdate((value) => _amuletMaterials[0].SetFloat(_cutoffHeightPropertyId, value));
            LeanTween.value(_leatherMaxVisibleValue, _leatherDissolvedValue, _dissolveCompleteTime).setEaseInSine().setOnUpdate((value) => _amuletMaterials[1].SetFloat(_cutoffHeightPropertyId, value)).setOnComplete(OnCompleteDissolveAmulet);

            LeanTween.value(1, 0, 0.5f).setOnComplete(_aiSessionManager.ShowEgilPhaseChangeAuraFx);

            void OnCompleteDissolveAmulet()
            {
                LeanTween.value(1, 0, 0.05f).setOnComplete(OnCompleteCounter);
                _aiSessionManager.HideDissovledAmulet();
            }

            void OnCompleteCounter()
            {
                ai.PlayAnimationCrossFade(_egil_changePhase2_chain_4.animStateHash, 0.15f, true);
                _aiSessionManager.HideEgilPhaseChangeFx();
            }
        }
    }
}