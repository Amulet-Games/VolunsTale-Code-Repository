using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeHandArmor : RuntimeArmor
    {
        [Header("Hand Meshes Refs.")]
        [ReadOnlyInspector] public Mesh[] handMeshes;

        [ReadOnlyInspector]
        public HandArmorItem _referedArmorItem;

        #region Vanilla Init.
        public void InitRuntimeHand(HandArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitRuntimeItem();

            InitHandDefaultStatus();
            InitModifiableStats();
            InitHandMesh();
        }

        void InitHandDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Hand;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedArmorItem.itemName;
        }

        void InitHandMesh()
        {
            Transform _childTransform;
            switch (_referedArmorItem.handArmorType)
            {
                case HandArmorTypeEnum.No_Attachment:
                    for (int i = 0; i < 6; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        handMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;

                case HandArmorTypeEnum.All_Attachment:
                    for (int i = 0; i < 10; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        handMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;

                case HandArmorTypeEnum.Shoulder_Attachment_Only:
                    for (int i = 0; i < 8; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        handMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;

                case HandArmorTypeEnum.Elbow_Attachment_Only:
                    for (int i = 0; i < 8; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        handMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;
            }
        }
        #endregion

        #region Deprived Init.
        public void InitDeprivedHand(HandArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;
            
            InitDeprivedHandDefaultStatus();
            InitModifiableStats();
            InitHandMesh();
        }

        void InitDeprivedHandDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Hand;
            isDeprivedArmor = true;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeHandFromSave(SavableHandState _savableHandState, HandArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitHandDefaultStatus();
            InitModifiableStats();
            InitHandMesh();
            LoadStatsFromSavable(_savableHandState);
        }
        
        public SavableHandState SaveHandStateToSave()
        {
            SavableHandState _savableHandState = new SavableHandState();
            _savableHandState.savableHandName = _referedArmorItem.itemName;
            _savableHandState.savableHandUniqueId = uniqueId;
            _savableHandState.savableHandSlotSide = (int)currentSlotSideType;
            return _savableHandState;
        }

        void LoadStatsFromSavable(SavableHandState _savableHandState)
        {
            uniqueId = _savableHandState.savableHandUniqueId;
            currentSlotSideType = (ArmorSlotSideTypeEnum)_savableHandState.savableHandSlotSide;
        }
        #endregion

        public override RuntimeHandArmor GetHandArmor()
        {
            return this;
        }

        public override ArmorItem GetReferedArmorItem()
        {
            return _referedArmorItem;
        }

        public override Item GetReferedItem()
        {
            return _referedArmorItem;
        }

        public enum HandArmorTypeEnum
        {
            No_Attachment,              // --- 0 - 5
            All_Attachment,             // --- 0 - 9
            Shoulder_Attachment_Only,   // --- 0 - 7
            Elbow_Attachment_Only       // --- 0 - 5 & 8, 9
        }
    }
}