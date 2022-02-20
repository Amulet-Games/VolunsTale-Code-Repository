using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AIManagerInterfaces
    {
    }

    interface IEnemyStamina
    {
        bool GetIsEnemyTiredBool();

        void RefillEnemyStamina();

        void DepleteEnemyStamina(float staminaUsage);
    }

    interface IEnemyRollInterval
    {
        void SetEnemyRolledBoolToTrue();

        bool GetEnemyRolledBool();
    }

    interface IEnemyBlocking
    {
        bool GetEnemyBlockedBool();
    }

    interface IEnemyMoveInFixDirection
    {
        void SetIsMovingFixDirectionToTrue();

        void SetCurrentFixDirectionEnum(FixDirectionMoveMod.FixDirectionTypeEnum currentMoveDirection);

        bool GetIsFixDirectionInCooldownBool();
    }

    interface IEnemyHitCountable
    {
        bool GetIsHitCountEventTriggeredBool();

        void ResetHitCountingStates();
    }

    interface IEnemyAiming
    {
    }

    interface IEnemyTwoStanceCombat
    {
        void SetIsRightStanceBool(bool isRightStance);

        bool GetIsRightStanceBool();

        bool GetCheckCombatStanceBool();
    }

    interface IEnemyTaunt
    {
        void SetTauntedPlayerToTrue();
    }

    interface IEnemyParry
    {
        void SetIsWaitingToParryBool(bool isWaitingToParry);

        bool GetIsWaitingToParryBool();

        bool GetTriedParryPlayerBool();

    }

    interface IEnemyPerilousAttack
    {
        void SetUsedPerilousAttackToTrue();
    }

    interface IEnemyObservePlayerStatus
    {
        bool GetHasSpammedBlockingBool();

        bool GetHasSpammedAttackingBool();

        void ResetSpammedBlockingStatus();

        void ResetSpammedAttackingStatus();
    }

    interface IEnemyDualWeapon
    {
        bool GetIsUsingSecondWeaponBool();
    }
}