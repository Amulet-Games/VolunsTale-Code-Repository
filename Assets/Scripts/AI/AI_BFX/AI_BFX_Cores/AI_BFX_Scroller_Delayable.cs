using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_BFX_Scroller_Delayable : MonoBehaviour
    {
        [Header("Config.")]
        public float Fps = 50;
        public float StartDelay;
        
        [Header("Refs (Drops).")]
        public Renderer _renderer;
        public AI_BFX_HandlerBase ai_BfxHandler;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] bool canUpdate;
        [ReadOnlyInspector] public bool isLastUpdatable;

        [Space(10)]
        [ReadOnlyInspector, SerializeField] float animationStartTime;
        [ReadOnlyInspector, SerializeField] int cur_Index;
        
        #region Non Serialized.
        Vector2 temp_offset;
        float orig_size_x;
        float orig_size_y;
        float _oneMinusOrigSizeY;

        Material mat;

        [HideInInspector]
        public int _mainTextId;
        #endregion

        public void StartScrollerDelayCount()
        {
            Invoke("OnScrollerDelayStart", StartDelay);
        }

        public void OnScrollerDelayStart()
        {
            mat.SetTextureOffset(_mainTextId, ai_BfxHandler.orig_offset);
            animationStartTime = Time.time;
            canUpdate = true;
        }

        public void Tick()
        {
            if (!canUpdate)
                return;

            UpdateScroller();
        }

        void UpdateScroller()
        {
            cur_Index = (int)((Time.time - animationStartTime) * Fps);
            cur_Index = cur_Index % 32;     /// 32 is the total frame number caculated by Columns * Rows.

            temp_offset.x = (cur_Index % 4) * orig_size_x;
            temp_offset.y = _oneMinusOrigSizeY - (cur_Index / 4) * orig_size_y;

            mat.SetTextureOffset(_mainTextId, temp_offset);

            if (cur_Index > 30)
            {
                OnScrollerEnd();
            }
        }

        void OnScrollerEnd()
        {
            canUpdate = false;

            if (isLastUpdatable)
            {
                ai_BfxHandler.End_AI_Bfx();
            }
        }

        public void Setup()
        {
            Vector2 orig_size;

            SetupGetMaterial();
            SetupGetOriginalSize();
            SetupSetMaterialOriginalSize();

            void SetupGetMaterial()
            {
                mat = _renderer.material;
            }

            void SetupGetOriginalSize()
            {
                orig_size = new Vector2(1f / 4, 1f / 8);
                orig_size_x = orig_size.x;
                orig_size_y = orig_size.y;
                _oneMinusOrigSizeY = 1.0f - orig_size_y;
            }

            void SetupSetMaterialOriginalSize()
            {
                mat.SetTextureScale(_mainTextId, orig_size);
            }
        }
    }
}