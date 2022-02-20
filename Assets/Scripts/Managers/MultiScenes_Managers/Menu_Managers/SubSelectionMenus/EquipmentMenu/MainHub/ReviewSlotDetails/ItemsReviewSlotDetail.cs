using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class ItemsReviewSlotDetail : MonoBehaviour
    {
        [Header("Canvas (Drops).")]
        [SerializeField] Canvas detailCanvas;

        #region Review Slots.
        [Header("Weapon ReviewSlots.")]
        public Rh_WeaponReviewSlot[] rhWeaponReviewSlots;
        public Lh_WeaponReviewSlot[] lhWeaponReviewSlots;
        public ArrowReviewSlot[] arrowReviewSlots;

        [Header("Armor ReviewSlots.")]
        public HeadArmorReviewSlot headArmorReviewSlot;
        public ChestArmorReviewSlot chestArmorReviewSlot;
        public HandArmorReviewSlot handArmorReviewSlot;
        public LegArmorReviewSlot legArmorReviewSlot;
        
        [Header("Accessories ReviewSlots.")]
        public CharmReviewSlot charmReviewSlot;
        public PowerupReviewSlot powerupReviewSlot;
        public R_RingReviewSlot rightRingReviewSlot;
        public L_RingReviewSlot leftRingReviewSlot;

        [Header("Consumables ReviewSlots.")]
        public ConsumableReviewSlot[] consumableReviewSlots;
        #endregion

        #region Status.
        [Header("Detail Status.")]
        [ReadOnlyInspector, SerializeField] ItemReviewSlot _currentReviewSlot;
        [ReadOnlyInspector, SerializeField] int detailIndex_x;
        [ReadOnlyInspector, SerializeField] int detailIndex_y;
        [ReadOnlyInspector] public bool isCursorOverSelection;
        #endregion
        
        #region Refs.
        [Header("Manager Refs.")]
        [ReadOnlyInspector] public EquipmentMenuManager equipmentMenuManager;
        [NonSerialized] public SavableInventory _inventory;
        InputManager _inp;
        #endregion

        #region Private.
        int rhWeaponReviewSlotsLength;
        int lhWeaponReviewSlotsLength;
        int arrowReviewSlotsLength;
        int consumableReviewSlotsLength;
        [HideInInspector] public ItemReviewSlot[,] reviewSlots2dArray = new ItemReviewSlot[6, 5];
        #endregion
        
        #region Tick.
        public void Tick()
        {
            GetCurrentReviewSlotByInput();
            
            EmptyCurrentSlotByInput();

            QuitReviewSlotDetailByInput();

            SelectCurrentReviewSlotByInput();

            SelectCurrentReviewSlotByCursor();

            equipmentMenuManager.ReviewSlot_HighlighterTick();
        }

        void GetCurrentReviewSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                detailIndex_x++;
                detailIndex_x = (detailIndex_x == 6) ? 0 : detailIndex_x;

                CheckRightInputExceptions();
                SetCurrentReviewSlot();
            }
            else if (_inp.menu_left_input)
            {
                detailIndex_x--;
                detailIndex_x = (detailIndex_x < 0) ? 5 : detailIndex_x;

                CheckLeftInputExceptions();
                SetCurrentReviewSlot();
            }
            else if (_inp.menu_up_input)
            {
                detailIndex_y--;
                detailIndex_y = (detailIndex_y < 0) ? 4 : detailIndex_y;

                CheckUpInputExceptions();
                SetCurrentReviewSlot();
            }
            else if (_inp.menu_down_input)
            {
                detailIndex_y++;
                detailIndex_y = (detailIndex_y == 5) ? 0 : detailIndex_y;

                CheckDownInputExceptions();
                SetCurrentReviewSlot();
            }
        }

        public void GetCurrentReviewSlotByPointerEvent(ItemReviewSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (_currentReviewSlot != _targetSlot)
            {
                detailIndex_x = _targetSlot.x_pos;
                detailIndex_y = _targetSlot.y_pos;

                SetCurrentReviewSlot();
            }
        }
        
        void SetCurrentReviewSlot()
        {
            _currentReviewSlot.OffCurrentSlot();
            _currentReviewSlot = reviewSlots2dArray[detailIndex_x, detailIndex_y];
            _currentReviewSlot.OnCurrentSlot();
        }

        void EmptyCurrentSlotByInput()
        {
            if (_inp.menu_remove_input)
            {
                if (!_currentReviewSlot._isSlotEmpty)
                {
                    _currentReviewSlot.EmptyInventorySlot();
                }
            }
        }

        void QuitReviewSlotDetailByInput()
        {
            if (_inp.menu_quit_input)
            {
                _inp.SetIsInEquipmentMenuStatus(false);
            }
        }

        void SelectCurrentReviewSlotByInput()
        {
            if (_inp.menu_select_input)
            {
                SelectCurrentReviewSlot();
            }
        }

        void SelectCurrentReviewSlotByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (isCursorOverSelection)
                {
                    SelectCurrentReviewSlot();
                }
            }
        }

        void SelectCurrentReviewSlot()
        {
            _currentReviewSlot.DrawEquipHubSlots();
        }

        #region Check Input Exceptions
        void CheckUpInputExceptions()
        {
            if (detailIndex_x == 5)
            {
                if (detailIndex_y == 1)
                {
                    detailIndex_x = 4;
                    detailIndex_y = 1;
                }
            }
        }

        void CheckDownInputExceptions()
        {
            if (detailIndex_x == 5)
            {
                if (detailIndex_y == 0)
                {
                    detailIndex_x = 5;
                    detailIndex_y = 2;
                }
            }
        }

        void CheckLeftInputExceptions()
        {
            if (detailIndex_x == 5)
            {
                if (detailIndex_y == 1)
                {
                    detailIndex_x = 4;
                    detailIndex_y = 1;
                }
                else if (detailIndex_y == 0)
                {
                    detailIndex_x = 4;
                    detailIndex_y = 0;
                }
            }
        }

        void CheckRightInputExceptions()
        {
            if (detailIndex_x == 5)
            {
                if (detailIndex_y == 1)
                {
                    detailIndex_x = 0;
                    detailIndex_y = 1;
                }
                else if (detailIndex_y == 0)
                {
                    detailIndex_x = 0;
                    detailIndex_y = 0;
                }
            }
        }
        #endregion

        #endregion

        #region On / Off Detail
        public void OnReviewHub_ResetHubOnMenuOpen()
        {
            ShowDetail();
            SetFirstIndexSlotAsCurrentSlot();

            void SetFirstIndexSlotAsCurrentSlot()
            {
                detailIndex_x = 0;
                detailIndex_y = 0;

                _currentReviewSlot = reviewSlots2dArray[0, 0];
                _currentReviewSlot.OnCurrentSlot();
            }
        }

        public void OnReviewDetail()
        {
            ShowDetail();
            SetCurrentIndexSlotAsCurrentSlot();
            
            void SetCurrentIndexSlotAsCurrentSlot()
            {
                _currentReviewSlot = reviewSlots2dArray[detailIndex_x, detailIndex_y];
                _currentReviewSlot.OnCurrentSlot();
            }
        }

        public void OffReviewDetail()
        {
            _currentReviewSlot.OffCurrentSlot();

            HideDetail();
            ResetCursorEventStatus();
        }
        
        void ResetCursorEventStatus()
        {
            isCursorOverSelection = false;
        }

        void ShowDetail()
        {
            detailCanvas.enabled = true;
            LoadReviewSlots();
        }

        void HideDetail()
        {
            detailCanvas.enabled = false;
        }
        #endregion

        #region Register Review Slots
        void LoadReviewSlots()
        {
            /// Weapons.
            LoadWeaponReviewSlots();
            LoadArrowReviewSlot();

            /// Armors.
            LoadHeadReviewSlot();
            LoadChestReviewSlot();
            LoadHandReviewSlot();
            LoadLegReviewSlot();

            /// Accessories.
            LoadCharmReviewSlot();
            LoadPowerupReviewSlot();
            LoadRingReviewSlot();

            /// Consumables.
            LoadConsumableReviewSlot();
        }

        void LoadWeaponReviewSlots()
        {
            for (int i = 0; i < rhWeaponReviewSlotsLength; i++)
            {
                if (_inventory.rightHandSlots[i] != null)
                {
                    rhWeaponReviewSlots[i].RegisterWeaponReviewSlot(_inventory.rightHandSlots[i]);
                }
                else
                {
                    rhWeaponReviewSlots[i].UnRegisterWeaponReviewSlot();
                }
            }

            for (int i = 0; i < lhWeaponReviewSlotsLength; i++)
            {
                if (_inventory.leftHandSlots[i] != null)
                {
                    lhWeaponReviewSlots[i].RegisterWeaponReviewSlot(_inventory.leftHandSlots[i]);
                }
                else
                {
                    lhWeaponReviewSlots[i].UnRegisterWeaponReviewSlot();
                }
            }
        }

        void LoadArrowReviewSlot()
        {
            for (int i = 0; i < arrowReviewSlotsLength; i++)
            {
                if (_inventory.arrowSlots[i] != null)
                {
                    arrowReviewSlots[i].RegisterArrowReviewSlot(_inventory.arrowSlots[i]);
                }
                else
                {
                    arrowReviewSlots[i].UnRegisterArrowReviewSlot();
                }
            }
        }

        void LoadHeadReviewSlot()
        {
            if (_inventory.headArmorSlot != null)
            {
                headArmorReviewSlot.RegisterHeadArmorReviewSlot(_inventory.headArmorSlot);
            }
            else
            {
                headArmorReviewSlot.UnRegisterHeadArmorReviewSlot();
            }
        }

        void LoadChestReviewSlot()
        {
            if (_inventory.chestArmorSlot != null)
            {
                chestArmorReviewSlot.RegisterChestArmorReviewSlot(_inventory.chestArmorSlot);
            }
            else
            {
                chestArmorReviewSlot.UnRegisterChestArmorReviewSlot();
            }
        }

        void LoadHandReviewSlot()
        {
            if (_inventory.handArmorSlot != null)
            {
                handArmorReviewSlot.RegisterHandArmorReviewSlot(_inventory.handArmorSlot);
            }
            else
            {
                handArmorReviewSlot.UnRegisterHandArmorReviewSlot();
            }
        }

        void LoadLegReviewSlot()
        {
            if (_inventory.legArmorSlot != null)
            {
                legArmorReviewSlot.RegisterLegArmorReviewSlot(_inventory.legArmorSlot);
            }
            else
            {
                legArmorReviewSlot.UnRegisterLegArmorReviewSlot();
            }
        }

        void LoadCharmReviewSlot()
        {
            if (_inventory.charmSlot != null)
            {
                charmReviewSlot.RegisterCharmReviewSlot(_inventory.charmSlot);
            }
            else
            {
                charmReviewSlot.UnRegisterCharmReviewSlot();
            }
        }

        void LoadPowerupReviewSlot()
        {
            if (_inventory.powerupSlot != null)
            {
                powerupReviewSlot.RegisterPowerupReviewSlot(_inventory.powerupSlot);
            }
            else
            {
                powerupReviewSlot.UnRegisterPowerupReviewSlot();
            }
        }

        void LoadRingReviewSlot()
        {
            if (_inventory.rightRingSlot != null)
            {
                rightRingReviewSlot.RegisterRightRingReviewSlot(_inventory.rightRingSlot);
            }
            else
            {
                rightRingReviewSlot.UnRegisterRightRingReviewSlot();
            }

            if (_inventory.leftRingSlot != null)
            {
                leftRingReviewSlot.RegisterLeftRingReviewSlot(_inventory.leftRingSlot);
            }
            else
            {
                leftRingReviewSlot.UnRegisterLeftRingReviewSlot();
            }
        }

        void LoadConsumableReviewSlot()
        {
            for (int i = 0; i < consumableReviewSlotsLength; i++)
            {
                if (_inventory.consumableSlots[i] != null)
                {
                    consumableReviewSlots[i].RegisterConsumableReviewSlot(_inventory.consumableSlots[i]);
                }
                else
                {
                    consumableReviewSlots[i].UnRegisterConsumableReviewSlot();
                }
            }
        }
        #endregion

        #region Setup.
        public void Setup(EquipmentMenuManager _equipmentMenuManager)
        {
            equipmentMenuManager = _equipmentMenuManager;

            SetupRefs();
            
            SetupReviewSlots();
        }

        void SetupRefs()
        {
            _inp = equipmentMenuManager._inp;
            _inventory = _inp._states._savableInventory;
        }
        
        void SetupReviewSlots()
        {
            /// Weapons.
            rhWeaponReviewSlotsLength = rhWeaponReviewSlots.Length;
            for (int i = 0; i < rhWeaponReviewSlotsLength; i++)
            {
                rhWeaponReviewSlots[i].Setup(this);
            }

            lhWeaponReviewSlotsLength = lhWeaponReviewSlots.Length;
            for (int i = 0; i < lhWeaponReviewSlotsLength; i++)
            {
                lhWeaponReviewSlots[i].Setup(this);
            }

            arrowReviewSlotsLength = arrowReviewSlots.Length;
            for (int i = 0; i < arrowReviewSlotsLength; i++)
            {
                arrowReviewSlots[i].Setup(this);
            }

            /// Armors.
            headArmorReviewSlot.Setup(this);
            chestArmorReviewSlot.Setup(this);
            handArmorReviewSlot.Setup(this);
            legArmorReviewSlot.Setup(this);

            /// Accessories.
            charmReviewSlot.Setup(this);
            powerupReviewSlot.Setup(this);
            rightRingReviewSlot.Setup(this);
            leftRingReviewSlot.Setup(this);

            /// Consumables.
            consumableReviewSlotsLength = consumableReviewSlots.Length;
            for (int i = 0; i < consumableReviewSlotsLength; i++)
            {
                consumableReviewSlots[i].Setup(this);
            }
        }
        #endregion
    }
}