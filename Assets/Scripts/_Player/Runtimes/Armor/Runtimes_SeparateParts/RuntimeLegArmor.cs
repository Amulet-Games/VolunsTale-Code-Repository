using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeLegArmor : RuntimeArmor
    {
        [Header("Leg Meshes Refs.")]
        [ReadOnlyInspector] public Mesh[] legMeshes;

        [ReadOnlyInspector]
        public LegArmorItem _referedArmorItem;

        #region Vanilla Init.
        public void InitRuntimeLegVanilla(LegArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitRuntimeItem();

            InitLegDefaultStatus();
            InitModifiableStats();
            InitLegMesh();
        }

        void InitLegDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Leg;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedArmorItem.itemName;
        }

        void InitLegMesh()
        {
            Transform _childTransform;
            switch (_referedArmorItem.legArmorTypeEnum)
            {
                case LegArmorTypeEnum.No_Attachment:

                    for (int i = 0; i < 2; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        legMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;

                case LegArmorTypeEnum.Kneel_Attachment:

                    for (int i = 0; i < 4; i++)
                    {
                        _childTransform = transform.GetChild(i);
                        legMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                    break;
            }
        }
        #endregion

        #region Deprived Init.
        public void InitDeprivedLeg(LegArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;
            
            InitDeprivedLegDefaultStatus();
            InitModifiableStats();
            InitLegMesh();
        }

        void InitDeprivedLegDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Leg;
            isDeprivedArmor = true;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeLegFromSave(SavableLegState _savableLegState, LegArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitLegDefaultStatus();
            InitModifiableStats();
            InitLegMesh();
            LoadStatsFromSavable(_savableLegState);
        }
        
        public SavableLegState SaveLegStateToSave()
        {
            SavableLegState _savableLegState = new SavableLegState();
            _savableLegState.savableLegName = _referedArmorItem.itemName;
            _savableLegState.savableLegUniqueId = uniqueId;
            _savableLegState.savableLegSlotSide = (int)currentSlotSideType;
            return _savableLegState;
        }

        void LoadStatsFromSavable(SavableLegState _savableLegState)
        {
            uniqueId = _savableLegState.savableLegUniqueId;
            currentSlotSideType = (ArmorSlotSideTypeEnum)_savableLegState.savableLegSlotSide;
        }
        #endregion

        public override RuntimeLegArmor GetLegArmor()
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

        public enum LegArmorTypeEnum
        {
            No_Attachment,
            Kneel_Attachment
        }
    }
}