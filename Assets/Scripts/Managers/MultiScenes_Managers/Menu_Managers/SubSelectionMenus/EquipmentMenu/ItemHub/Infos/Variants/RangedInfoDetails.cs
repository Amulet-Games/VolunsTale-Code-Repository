using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class RangedInfoDetails : MeleeInfoDetails
    {
        [Header("RangedWeapon Info Text.")]
        [SerializeField] Text range_Text;

        protected override void UpdateWeaponInfoDetails()
        {
        }
    }
}