using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyRuntimeSidearm : MonoBehaviour
    {
        [Header("Sidearm. Drag and Drops")]
        public StandardEnemyWeaponHook e_weaponHook;

        [ReadOnlyInspector] public EnemySidearm referedEnemySidearm;

        public void Init(AIManager ai)
        {
            InitSidearmToPosition();
            InitSidearmWeaponHook();

            void InitSidearmToPosition()
            {
                AISheathTransform sidearmSheathTransform = referedEnemySidearm.weaponSheathTransform;

                transform.parent = ai.anim.GetBoneTransform(HumanBodyBones.Spine);
                transform.localPosition = sidearmSheathTransform.pos;
                transform.localEulerAngles = sidearmSheathTransform.eulers;
                transform.localScale = sidearmSheathTransform.scale;
            }

            void InitSidearmWeaponHook()
            {
                e_weaponHook = GetComponent<StandardEnemyWeaponHook>();
                e_weaponHook.Setup(ai);
            }
        }

        public void SheathSidearmToPosition(AIManager ai)
        {
            AISheathTransform sidearmSheathTransform = referedEnemySidearm.weaponSheathTransform;

            transform.parent = ai.anim.GetBoneTransform(HumanBodyBones.Spine);
            transform.localPosition = sidearmSheathTransform.pos;
            transform.localEulerAngles = sidearmSheathTransform.eulers;
            transform.localScale = sidearmSheathTransform.scale;
        }

        public void ParentEnemySidearmUnderHand(AIManager ai)
        {
            transform.parent = ai.anim.GetBoneTransform(HumanBodyBones.LeftHand);
            transform.localPosition = ai.vector3Zero;
            transform.localEulerAngles = ai.vector3Zero;
            transform.localScale = ai.vector3One;
        }
    }
}