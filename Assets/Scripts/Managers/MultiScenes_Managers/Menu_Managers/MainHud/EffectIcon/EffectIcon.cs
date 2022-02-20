using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class EffectIcon : MonoBehaviour
    {
        public Image _iconImage;
        public RectTransform _iconRect;

        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] Canvas _iconCanvas;
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        
        public void SwitchEffectIcon(Sprite _statsEffectIcon)
        {
            _iconImage.sprite = _statsEffectIcon;
        }
        
        public void EnableIcon()
        {
            _iconCanvas.enabled = true;
        }

        public void DisableIcon()
        {
            _iconImage.sprite = null;
            _iconCanvas.enabled = false;
        }

        public void Init()
        {
            _iconCanvas = _iconImage.GetComponent<Canvas>();
            DisableIcon();
        }
    }
}