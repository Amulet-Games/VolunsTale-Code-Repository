using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Profile/Execution Profile/Egil_Execution2_Profile")]
    public class Egil_Execution2_Profile : AI_BaseExecution_Profile
    {
        [Header("Knockback Velocity.")]
        public float _exe2_fallbackVelocity_backward = 3;
        public float _exe2_fallbackVelocity_upward = 7;

        public override void KnockBackFromExecution(Rigidbody _rb)
        {
            _rb.transform.parent = null;
            ApplyKnockBackRootMotion();

            void ApplyKnockBackRootMotion()
            {
                _rb.mass = 1;
                _rb.drag = 0;
                _rb.angularDrag = 0.05f;
                _rb.useGravity = true;
                _rb.constraints = RigidbodyConstraints.FreezeRotation;
                _rb.AddForce((_exe2_fallbackVelocity_upward * _rb.transform.up) + (_exe2_fallbackVelocity_backward * -_rb.transform.forward), ForceMode.Impulse);
            }
        }
    }
}