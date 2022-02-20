using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class VolunVesselConsumable : StatsEffectConsumable
    {
        [Header("Mat (Drops).")]
        [SerializeField] Material _emptyVesselMaterial;
        [SerializeField] Material _normalVesselMaterial;

        [Header("Ref.")]
        [ReadOnlyInspector, SerializeField] MeshRenderer _meshRenderer;

        public override void ExecuteStatsEffect(StatsAttributeHandler statsHandler)
        {
            statsHandler.IncrementPlayerHealth(_referedStatsEffectItem.effectAmount);
        }

        public override void ChangeVesselToEmpty()
        {
            _meshRenderer.sharedMaterial = _emptyVesselMaterial;
            _inventory._mainHudManager.ChangeConsumQSlotIconToEmpty();
        }
        
        public void ChangeVesselBackToNormal()
        {
            _meshRenderer.sharedMaterial = _normalVesselMaterial;
            _inventory._mainHudManager.ChangeConsumQSlotIconToNormal();
        }

        public void RefillCarryingVolunAmount(int _amount)
        {
            curCarryingAmount = _amount;
            isCarryingEmpty = false;

            ChangeVesselBackToNormal();
        }

        #region Info / Alter Details.
        public override void UpdateStatsEffectInfoDetails(StatsEffectInfoDetails _statsEffectInfoDetails)
        {
            /// General Info
            _statsEffectInfoDetails.itemTitle_Text.text = runtimeName;

            if (isCarryingEmpty)
            {
                _statsEffectInfoDetails.itemIcon_Image.sprite = _referedStatsEffectItem.GetEmptyConsumableSprite();
            }
            else
            {
                _statsEffectInfoDetails.itemIcon_Image.sprite = _referedStatsEffectItem.itemIcon;
            }

            /// Top Info Text
            _statsEffectInfoDetails.maxCarryingAmount_Text.text = curStoredAmount.ToString();
            _statsEffectInfoDetails.curCarryingAmount_Text.text = curCarryingAmount.ToString();

            _statsEffectInfoDetails.maxStoredAmount_Text.font = _statsEffectInfoDetails.arbutusSlab_normal_asset;
            _statsEffectInfoDetails.curStoredAmount_Text.font = _statsEffectInfoDetails.arbutusSlab_normal_asset;

            _statsEffectInfoDetails.maxStoredAmount_Text.text = "-";
            _statsEffectInfoDetails.curStoredAmount_Text.text = "-";

            /// Bottom Desc Text
            _statsEffectInfoDetails.consumableEffect_Text.text = _referedStatsEffectItem.consumableEffectText.ToString();
        }

        public override void UpdateStatsEffectAlterDetails(StatsEffectAlterDetails _statsEffectAlterDetails)
        {
            /// General Info
            _statsEffectAlterDetails.itemTitle_Text.text = runtimeName;
            
            if (isCarryingEmpty)
            {
                _statsEffectAlterDetails.itemIcon_Image.sprite = _referedStatsEffectItem.GetEmptyConsumableSprite();
            }
            else
            {
                _statsEffectAlterDetails.itemIcon_Image.sprite = _referedStatsEffectItem.itemIcon;
            }

            /// Top Desc Text
            _statsEffectAlterDetails.consumableEffect_Text.text = _referedStatsEffectItem.consumableEffectText.ToString();

            /// Bottom Info Text
            _statsEffectAlterDetails.maxCarryingAmount_Text.text = curStoredAmount.ToString();
            _statsEffectAlterDetails.curCarryingAmount_Text.text = curCarryingAmount.ToString();
        }
        #endregion

        #region Init.
        protected override void SetupVessels_NewInstance()
        {
            _inventory.SetupVolunVesselsDefault(this);

            SetupEmptyMaterial_NewInstance();

            void SetupEmptyMaterial_NewInstance()
            {
                _meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                ChangeVesselBackToNormal();
            }
        }

        protected override void SetupVessel_LoadedSave()
        {
            _inventory.runtimeVolunVessel = this;

            SetupEmptyMaterial_LoadSave();

            void SetupEmptyMaterial_LoadSave()
            {
                _meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

                if (curCarryingAmount > 1)
                {
                    _meshRenderer.sharedMaterial = _normalVesselMaterial;
                }
                else
                {
                    _meshRenderer.sharedMaterial = _emptyVesselMaterial;
                }
            }
        }
        #endregion
    }
}