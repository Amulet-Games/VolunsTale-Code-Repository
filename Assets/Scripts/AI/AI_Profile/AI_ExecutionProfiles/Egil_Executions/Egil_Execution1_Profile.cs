using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Profile/Execution Profile/Egil_Execution1_Profile")]
    public class Egil_Execution1_Profile : AI_BaseExecution_Profile
    {
        [Header("Knockback Velocity.")]
        public float _exe1_fallbackVelocity_backward = 7.5f;
        public float _exe1_fallbackVelocity_downward = 3f;

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
                _rb.AddForce((_exe1_fallbackVelocity_downward * -_rb.transform.up) + (_exe1_fallbackVelocity_backward * -_rb.transform.forward), ForceMode.Impulse);
            }
        }
    }
}