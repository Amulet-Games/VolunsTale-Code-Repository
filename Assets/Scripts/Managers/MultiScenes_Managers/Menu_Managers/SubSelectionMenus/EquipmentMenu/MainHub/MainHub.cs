using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class MainHub : MonoBehaviour
    {
        [Header("MainHub Tween.")]
        public LeanTweenType _mainHubEaseType;
        public float _mainHubFadeInSpeed;
        public float _mainHubFadeOutSpeed;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isInEquipHub;

        [Header("ReviewHub (Drops).")]
        [SerializeField] CanvasGroup reviewHubGroup;
        [SerializeField] Canvas reviewHubCanvas;

        [Header("EquipHub (Drops).")]
        [SerializeField] CanvasGroup equipHubGroup;
        [SerializeField] Canvas equipHubCanvas;
        
        [Header("Manager Refs.")]
        [ReadOnlyInspector] public EquipmentMenuManager _equipmentMenuManager;
        
        [Header("Text Detail.")]
        public Text quickSlotPositionText;
        public Text currentItemNameText;

        [Header("ReviewSlot Detail")]
        public ItemsReviewSlotDetail itemsReviewSlotDetail;

        [Header("EquipSlot Details")]
        /// Weapons.
        public WeaponEquipSlotsDetail weaponEquipSlotsDetail;
        public ArrowEquipSlotsDetail arrowEquipSlotsDetail;
        /// Armors.
        public HeadArmorEquipSlotsDetail headArmorEquipSlotsDetail;
        public ChestArmorEquipSlotsDetail chestArmorEquipSlotsDetail;
        public HandArmorEquipSlotsDetail handArmorEquipSlotsDetail;
        public LegArmorEquipSlotsDetail legArmorEquipSlotsDetail;
        /// Accessories.
        public CharmEquipSlotsDetail charmEquipSlotsDetail;
        public PowerupEquipSlotsDetail powerupEquipSlotsDetail;
        public RingEquipSlotsDetail ringEquipSlotsDetail;
        /// Consumables.
        public ConsumableEquipSlotsDetail consumableEquipSlotsDetail;
        [ReadOnlyInspector] public ItemEquipSlotsDetail _currentEquipSlotsDetail;

        int _cur_mainHubTweenId;

        public void Tick()
        {
            if (isInEquipHub)
            {
                _currentEquipSlotsDetail.Tick();
            }
            else
            {
                itemsReviewSlotDetail.Tick();
            }
        }
        
        public void OnReviewHub()
        {
            isInEquipHub = false;
            itemsReviewSlotDetail.OnReviewDetail();
            OnTextDetail();
            ShowReviewHub();
        }

        public void OnEquipHub()
        {
            isInEquipHub = true;
            itemsReviewSlotDetail.OffReviewDetail();
            OffTextDetail();
            ShowEquipHub();
        }

        #region Reset Hub OnMenuOpen / OnMenuClose.
        public void ResetHubOnMenuOpen()
        {
            isInEquipHub = false;
            itemsReviewSlotDetail.OnReviewHub_ResetHubOnMenuOpen();
            OnTextDetail();
            ShowReviewHub();
        }

        public void ResetHubOnMenuClose()
        {
            itemsReviewSlotDetail.OffReviewDetail();
        }
        #endregion

        #region Show / Hide MainHubs.
        void ShowReviewHub()
        {
            if (equipHubCanvas.enabled)
            {
                HideEquipHub(true);
            }
            else
            {
                TweenOnReviewHub();
            }
        }

        void ShowEquipHub()
        {
            if (reviewHubCanvas.enabled)
            {
                HideReviewHub(true);
            }
            else
            {
                TweenOnEquipHub();
            }
        }

        void HideReviewHub(bool _tweenOnEquipHubOnComplete)
        {
            CancelUnFinishTweeningJob();

            if (!_tweenOnEquipHubOnComplete)
            {
                _cur_mainHubTweenId = LeanTween.alphaCanvas(reviewHubGroup, 0, _mainHubFadeOutSpeed).setEase(_mainHubEaseType).setOnComplete(OnCompleteHideReviewHub).id;
            }
            else
            {
                _cur_mainHubTweenId = LeanTween.alphaCanvas(reviewHubGroup, 0, _mainHubFadeOutSpeed).setEase(_mainHubEaseType).setOnComplete(OnCompleteTweenOnEquipHub).id;
            }
        }

        void OnCompleteHideReviewHub()
        {
            reviewHubCanvas.enabled = false;
        }

        void OnCompleteTweenOnEquipHub()
        {
            reviewHubCanvas.enabled = false;
            TweenOnEquipHub();
        }

        void HideEquipHub(bool _tweenOnReviewHubOnComplete)
        {
            CancelUnFinishTweeningJob();

            if (!_tweenOnReviewHubOnComplete)
            {
                _cur_mainHubTweenId = LeanTween.alphaCanvas(equipHubGroup, 0, _mainHubFadeOutSpeed).setEase(_mainHubEaseType).setOnComplete(OnCompleteHideEquipHub).id;
            }
            else
            {
                _cur_mainHubTweenId = LeanTween.alphaCanvas(equipHubGroup, 0, _mainHubFadeOutSpeed).setEase(_mainHubEaseType).setOnComplete(OnCompleteTweenOnReviewHub).id;
            }
        }

        void OnCompleteHideEquipHub()
        {
            equipHubCanvas.enabled = false;
        }

        void OnCompleteTweenOnReviewHub()
        {
            equipHubCanvas.enabled = false;
            TweenOnReviewHub();
        }

        void TweenOnReviewHub()
        {
            reviewHubCanvas.enabled = true;
            _cur_mainHubTweenId = LeanTween.alphaCanvas(reviewHubGroup, 1, _mainHubFadeInSpeed).setEase(_mainHubEaseType).id;
        }

        void TweenOnEquipHub()
        {
            equipHubCanvas.enabled = true;
            _cur_mainHubTweenId = LeanTween.alphaCanvas(equipHubGroup, 1, _mainHubFadeInSpeed).setEase(_mainHubEaseType).id;
        }

        void CancelUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_mainHubTweenId))
                LeanTween.cancel(_cur_mainHubTweenId);
        }
        #endregion

        #region On / Off Text Detail.
        void OnTextDetail()
        {
            currentItemNameText.enabled = true;
        }

        void OffTextDetail()
        {
            currentItemNameText.enabled = false;
        }
        #endregion

        #region Init.
        public void Setup(EquipmentMenuManager _equipmentMenuManager)
        {
            this._equipmentMenuManager = _equipmentMenuManager;
            
            SetupReviewSlotDetail();

            SetupEquipSlotDetails();
        }
        
        void SetupReviewSlotDetail()
        {
            itemsReviewSlotDetail.Setup(_equipmentMenuManager);
        }

        void SetupEquipSlotDetails()
        {
            /// Weapons.
            weaponEquipSlotsDetail.Setup(_equipmentMenuManager);
            arrowEquipSlotsDetail.Setup(_equipmentMenuManager);

            /// Armors.
            headArmorEquipSlotsDetail.Setup(_equipmentMenuManager);
            chestArmorEquipSlotsDetail.Setup(_equipmentMenuManager);
            handArmorEquipSlotsDetail.Setup(_equipmentMenuManager);
            legArmorEquipSlotsDetail.Setup(_equipmentMenuManager);

            /// Accessories.
            charmEquipSlotsDetail.Setup(_equipmentMenuManager);
            powerupEquipSlotsDetail.Setup(_equipmentMenuManager);
            ringEquipSlotsDetail.Setup(_equipmentMenuManager);

            /// Consumables.
            consumableEquipSlotsDetail.Setup(_equipmentMenuManager);
        }
        #endregion
    }
}