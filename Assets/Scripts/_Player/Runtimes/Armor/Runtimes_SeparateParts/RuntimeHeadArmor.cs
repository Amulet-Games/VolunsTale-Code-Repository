using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeHeadArmor : RuntimeArmor
    {
        [Header("Head Meshes Refs.")]
        [ReadOnlyInspector] public Mesh[] headMesh;

        [ReadOnlyInspector]
        public HeadArmorItem _referedArmorItem;

        #region Vanilla Init.
        public void InitRuntimeHead(HeadArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitRuntimeItem();

            InitHeadDefaultStatus();
            InitModifiableStats();
            InitHeadMesh();
        }

        void InitHeadDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Head;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedArmorItem.itemName;
        }

        void InitHeadMesh()
        {
            Transform _childTransform;
            switch (_referedArmorItem.headArmorType)
            {
                case HeadArmorTypeEnum.Cover:
                    _childTransform = transform.GetChild(0);
                    headMesh[0] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    break;

                case HeadArmorTypeEnum.Helmet:
                    _childTransform = transform.GetChild(0);
                    headMesh[0] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    break;

                case HeadArmorTypeEnum.Helmet_Attachment:
                    for (int i = 0; i < 2; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        headMesh[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;
            }
        }
        #endregion

        #region Deprived Init.
        public void InitDeprivedHead(HeadArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;
            
            InitDeprivedHeadDefaultStats();
            InitModifiableStats();
            InitHeadMesh();
        }

        void InitDeprivedHeadDefaultStats()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Head;
            isDeprivedArmor = true;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeHeadFromSave(SavableHeadState _savableHeadState, HeadArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitHeadDefaultStatus();
            InitModifiableStats();
            InitHeadMesh();
            LoadStatsFromSavable(_savableHeadState);
        }
        
        public SavableHeadState SaveHeadStateToSave()
        {
            SavableHeadState _savableHeadState = new SavableHeadState();
            _savableHeadState.savableHeadName = _referedArmorItem.itemName;
            _savableHeadState.savableHeadUniqueId = uniqueId;
            _savableHeadState.savableHeadSlotSide = (int)currentSlotSideType;
            return _savableHeadState;
        }

        void LoadStatsFromSavable(SavableHeadState _savableHeadState)
        {
            uniqueId = _savableHeadState.savableHeadUniqueId;
            currentSlotSideType = (ArmorSlotSideTypeEnum)_savableHeadState.savableHeadSlotSide;
        }
        #endregion

        public override RuntimeHeadArmor GetHeadArmor()
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

        public enum HeadArmorTypeEnum
        {
            Cover,
            Helmet,
            Helmet_Attachment
        }
    }
}