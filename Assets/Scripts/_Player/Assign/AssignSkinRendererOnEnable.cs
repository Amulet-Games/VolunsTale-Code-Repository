using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AssignSkinRendererOnEnable : MonoBehaviour
    {
        [Header("Config.")]
        public SkinRendererAssignTypeEnum targetAssignType;
        public int assignTypeArrayIndex;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _states;
        
        void OnEnable()
        {
            _states = SessionManager.singleton._states;
            _states.GetModularSkinRenderersEvent += ReturnModularSkinRendererAction;
        }

        void OnDisable()
        {
            _states.GetModularSkinRenderersEvent -= ReturnModularSkinRendererAction;
            _states = null;
        }

        public void ReturnModularSkinRendererAction()
        {
            switch (targetAssignType)
            {
                case SkinRendererAssignTypeEnum.Head:
                    _states.headPiecesRenderer[assignTypeArrayIndex] = GetComponent<SkinnedMeshRenderer>();
                    break;

                case SkinRendererAssignTypeEnum.Chest:
                    _states.chestPiecesRenderer[assignTypeArrayIndex] = GetComponent<SkinnedMeshRenderer>();
                    break;

                case SkinRendererAssignTypeEnum.Hands:
                    _states.handPiecesRenderer[assignTypeArrayIndex] = GetComponent<SkinnedMeshRenderer>();
                    break;

                case SkinRendererAssignTypeEnum.Legs:
                    _states.legPiecesRenderer[assignTypeArrayIndex] = GetComponent<SkinnedMeshRenderer>();
                    break;

                case SkinRendererAssignTypeEnum.Back:
                    _states.backPiecesRenderer = GetComponent<SkinnedMeshRenderer>();
                    break;
            }

            enabled = false;
        }

        public enum SkinRendererAssignTypeEnum
        {
            Head,
            Chest,
            Hands,
            Legs,
            Back
        }
    }
}