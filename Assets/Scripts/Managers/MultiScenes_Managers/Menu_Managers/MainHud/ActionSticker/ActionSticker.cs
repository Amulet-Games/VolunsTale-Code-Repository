using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class ActionSticker : MonoBehaviour
    {
        [Header("Id Within Group.")]
        /// This has to match the order of each side of weapon group.
        /// Positions for each sticker is, 
        /// LEFT SIDE: (Icon) RB LB RT LT.
        /// RIGHT SIDE: LT, RT, LB, RB (Icon).
        public float _1h_stickPosX;
        public float _2h_stickPosX;

        [Header("Drag and Drop Refs.")]
        public Image _stickerIcon;
        public RectTransform _stickerRect;
        public Canvas _stickerCanvas;

        public void MoveTo_1H_Position()
        {
            Vector3 _temp_vector3 = _stickerRect.localPosition;
            _temp_vector3.x = _1h_stickPosX;
            _stickerRect.localPosition = _temp_vector3;
        }

        public void MoveTo_2H_Position()
        {
            Vector3 _temp_vector3 = _stickerRect.localPosition;
            _temp_vector3.x = _2h_stickPosX;
            _stickerRect.localPosition = _temp_vector3;
        }
    }
}