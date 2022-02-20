using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeChestArmor : RuntimeArmor
    {
        [Header("Chest Meshes Refs.")]
        [ReadOnlyInspector] public Mesh[] chestMeshes = new Mesh[2];

        [ReadOnlyInspector]
        public ChestArmorItem _referedArmorItem;

        #region Vanilla Init.
        public void InitRuntimeChest(ChestArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitRuntimeItem();

            InitChestDefaultStatus();
            InitModifiableStats();
            InitChestMesh();
        }

        void InitChestDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Chest;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedArmorItem.itemName;
        }

        void InitChestMesh()
        {
            for (int i = 0; i < 2; i++)
            {
                Transform _childTransform = transform.GetChild(i);
                chestMeshes[i] = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
        }
        #endregion

        #region Deprived Init.
        public void InitDeprivedChest(ChestArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;
            
            InitDeprivedChestDefaultStatus();
            InitModifiableStats();
            InitChestMesh();
        }

        void InitDeprivedChestDefaultStatus()
        {
            slotNumber = 0;
            armorType = ArmorTypeEnum.Chest;
            isDeprivedArmor = true;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeChestFromSave(SavableChestState _savableChestState, ChestArmorItem _referedArmorItem)
        {
            this._referedArmorItem = _referedArmorItem;

            InitChestDefaultStatus();
            InitModifiableStats();
            InitChestMesh();
            LoadStatsFromSavable(_savableChestState);
        }
        
        public SavableChestState SaveChestStateToSave()
        {
            SavableChestState _savableChestState = new SavableChestState();
            _savableChestState.savableChestName = _referedArmorItem.itemName;
            _savableChestState.savableChestUniqueId = uniqueId;
            _savableChestState.savableChestSlotSide = (int)currentSlotSideType;
            return _savableChestState;
        }

        void LoadStatsFromSavable(SavableChestState _savableChestState)
        {
            uniqueId = _savableChestState.savableChestUniqueId;
            currentSlotSideType = (ArmorSlotSideTypeEnum)_savableChestState.savableChestSlotSide;
        }
        #endregion

        public override RuntimeChestArmor GetChestArmor()
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
    }
}