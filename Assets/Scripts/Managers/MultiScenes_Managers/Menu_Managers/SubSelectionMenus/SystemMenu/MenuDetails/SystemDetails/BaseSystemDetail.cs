using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public abstract class BaseSystemDetail : MonoBehaviour, ISystemMenuDetailUpdatable
    {
        [Header("Slots.")]
        public BaseOptionSlot[] _optionSlots;

        [Header("Dual Option Mid Pos Thershold.")]
        public float _dualOptionMidPosThershold = 1800;

        [Header("Status.")]
        [ReadOnlyInspector] public int _slotsLength;
        [ReadOnlyInspector] public int _raycastImageLength;
        [ReadOnlyInspector] public int _slotIndex;
        [ReadOnlyInspector] public BaseOptionSlot _currentSlot;
        [ReadOnlyInspector] public DualOptionSlot _currentDualOptionSlot;
        [ReadOnlyInspector] public bool _hasChangedStats;

        [Header("Refs.")]
        [ReadOnlyInspector] public Canvas _systemDetailCanvas;
        [ReadOnlyInspector] public SystemMenuManager _systemMenuManager;
        [ReadOnlyInspector] public List<Image> _slotRaycastImages;

        #region On Detail Open.
        public abstract void OnDetailOpen();

        protected void OnDetailOpenSetCurrentSlot()
        {
            _slotIndex = 0;
            _currentSlot = _optionSlots[0];

            OnCurrentSlot();
        }

        protected void OnDetailOpenEnableExtraInputs()
        {
            _systemMenuManager._isUpdateExtraInputs = true;
        }
        #endregion

        #region On Detail Close.
        public abstract void OnDetailClose();

        protected void OnDetailCloseResetSlot()
        {
            _currentSlot.OnDetailCloseResetSlot();
        }

        protected void OnDetailCloseResetHasChangedStats()
        {
            SetHasChangedStatsStatusToFalse();
        }

        protected void OnDetailCloseDisableExtraInputs()
        {
            _systemMenuManager.DisableExtraInputs();
        }
        #endregion

        public abstract void Tick();

        #region Get Current Slot By Cursor.
        public void GetCurrentSlotByPointerEvent(BaseOptionSlot _targetSlot)
        {
            if (_currentSlot != _targetSlot)
            {
                _slotIndex = _targetSlot._slotIndex;
                SetCurrentSlot(_targetSlot);
            }
        }

        public void GetCurrentDualSlotOptionByPointerEvent(float _pointerPosX)
        {
            if (_currentSlot != _currentDualOptionSlot)
            {
                GetCorrectOptionFromOutsideSlot();
            }
            else
            {
                GetCorrectOptionWithinDualSlot();
            }

            void GetCorrectOptionWithinDualSlot()
            {
                /// Pointer Hovered on 1st Option.
                if (!_currentDualOptionSlot._isInFirstOption)
                {
                    if (_pointerPosX < _dualOptionMidPosThershold)
                    {
                        _slotIndex = _currentDualOptionSlot._slotIndex;

                        OffCurrentSlot();
                        _currentSlot = _currentDualOptionSlot;
                        _currentDualOptionSlot.PointerEventSet_1st_OptionCurrent();
                    }
                }
                /// Pointer Hovered on 2nd Option.
                else
                {
                    if (_pointerPosX > _dualOptionMidPosThershold)
                    {
                        _slotIndex = _currentDualOptionSlot._slotIndex;

                        OffCurrentSlot();
                        _currentSlot = _currentDualOptionSlot;
                        _currentDualOptionSlot.PointerEventSet_2nd_OptionCurrent();
                    }
                }
            }

            void GetCorrectOptionFromOutsideSlot()
            {
                /// Pointer Hovered on 1st Option.
                if (_pointerPosX < _dualOptionMidPosThershold)
                {
                    _slotIndex = _currentDualOptionSlot._slotIndex;

                    OffCurrentSlot();
                    _currentSlot = _currentDualOptionSlot;
                    _currentDualOptionSlot.PointerEventSet_1st_OptionCurrent();
                }
                /// Pointer Hovered on 2nd Option.
                else
                {
                    _slotIndex = _currentDualOptionSlot._slotIndex;

                    OffCurrentSlot();
                    _currentSlot = _currentDualOptionSlot;
                    _currentDualOptionSlot.PointerEventSet_2nd_OptionCurrent();
                }
            }
        }
        #endregion

        #region Set Current Slot.
        public abstract void SetCurrentSlot(BaseOptionSlot _optionSlot);

        public abstract void OnCurrentSlot();

        public abstract void OffCurrentSlot();
        #endregion

        #region On Slot Select Tween.
        public abstract void OnSelectChangeShadowColor();
        #endregion

        #region Enable / Disable Pointer Event Image RaycastTarget.
        public void EnableSlotsRaycastable()
        {
            for (int i = 0; i < _raycastImageLength; i++)
            {
                _slotRaycastImages[i].raycastTarget = true;
            }
        }

        public void DisableSlotsRaycastable()
        {
            for (int i = 0; i < _raycastImageLength; i++)
            {
                _slotRaycastImages[i].raycastTarget = false;
            }
        }

        public void AddRaycastableImage(Image _image)
        {
            _slotRaycastImages.Add(_image);
            _raycastImageLength++;
        }
        #endregion

        #region Enable / Disable Horizontal Scrollable.
        public void EnableHorizontalScrollable()
        {
            _systemMenuManager._horizontalScrollHandler._isForbiddenToScroll = false;
        }

        public void DisableHorizontalScrollable()
        {
            _systemMenuManager._horizontalScrollHandler._isForbiddenToScroll = true;
        }
        #endregion

        #region Set Detail As Current Updatable.
        public void SetDetailAsCurrentUpdatable()
        {
            _systemMenuManager._currentDetailUpdatable = this;
        }
        #endregion

        #region Set Status.
        public void SetHasChangedStatsStatusToTrue()
        {
            if (!_hasChangedStats)
            {
                _hasChangedStats = true;
                _currentDualOptionSlot.OnHasChangedStatsStatus();
            }
        }

        public void SetHasChangedStatsStatusToFalse()
        {
            if (_hasChangedStats)
            {
                _hasChangedStats = false;
                _currentDualOptionSlot.OffHasChangedStatsStatus();
            }
        }
        #endregion

        #region Setup.
        public abstract void Setup(SystemMenuManager _systemMenuManager);

        public abstract void SetupSlots();

        protected void BaseSetup()
        {
            SetupBaseDetailCanvas();
        }

        void SetupBaseDetailCanvas()
        {
            _systemDetailCanvas = GetComponent<Canvas>();
        }
        
        /// Graphic Detail Group.
        public virtual GraphicSystemDetail GetGraphicSystemDetail()
        {
            return null;
        }
        #endregion
    }
}