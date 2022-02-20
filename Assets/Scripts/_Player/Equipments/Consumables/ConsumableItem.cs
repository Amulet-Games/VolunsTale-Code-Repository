using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ConsumableItem : Item
    {
        [Header("General Consumable Item Stats.")]
        public Sprite _noBaseItemIcon;
        public int maxCarryingAmount;
        public int maxStoredAmount;
        [TextArea(1, 30)]
        public string consumableEffectText;

        [Header("Config.")]
        public bool canMoveWhileUsing;
        public ConsumableTypeEnum consumableType;

        [Header("Anim States.")]
        public AnimStateVariable consumableUsedAnim;

        [Header("Effect.")]
        public RuntimeConsumableEffect effectPrefab;

        [Header("IK.")]
        public bool _useLookAtIKJob;
        public bool _isLookAtIKHeadOnly;
        [Range(0f, 1f)] public float _lookAtWeight;

        /// ENUM
        public enum ConsumableTypeEnum
        {
            StatsEffectConsumable,
            ThrowableConsumable
        }

        public virtual Sprite GetEmptyConsumableSprite()
        {
            return null;
        }

        public virtual Sprite GetEmptyNoBaseConsumableSprite()
        {
            return null;
        }

        public virtual bool GetIsVessel()
        {
            return false;
        }

        public virtual bool GetIsVolun()
        {
            return false;
        }
    }
}