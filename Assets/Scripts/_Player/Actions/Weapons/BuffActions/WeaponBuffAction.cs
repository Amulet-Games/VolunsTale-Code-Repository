using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class WeaponBuffAction : WeaponAction
    {
        [Header("Base Config.")]
        public float effectAmount;
        public int weaponBuffId;

        [Header("Base Drag and Drop Refs.")]
        public Sprite weaponBuffIcon;
    }
}