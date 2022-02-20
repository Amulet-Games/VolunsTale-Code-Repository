using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(ShieldmanAIManager))]
    [CanEditMultipleObjects]
    public class ShieldmanAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        ShieldmanAIManager ai;

        private void OnEnable()
        {
            ai = (ShieldmanAIManager)target;

            // General
            
            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Shieldman_OnInspectorGUI_Init();

            // Mods

            EnemyBlockingMod_Init(serializedObject);

            PerilousAttackMod_Init(serializedObject);

            PlayerSpamBlockingMod_Init(serializedObject);

            RightLegDamageColliderMod_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Shieldman_OnInspectorGUI_Tick();

            // Mods

            Shieldman_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Shieldman_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Shieldman_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            #region Indicators.
            openIndicators = EditorGUILayout.BeginFoldoutHeaderGroup(openIndicators, "AI Indicators", classFoldoutHeaderGUIStyle);
            if (openIndicators)
            {
                showIndicators.boolValue = true;
                EditorGUILayout.PropertyField(rh_ParryIndicator);
                EditorGUILayout.PropertyField(canPlayIndicator);
            }
            else
            {
                showIndicators.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            #endregion

            DrawUILine1();

            EditorGUILayout.EndVertical();
        }

        void Shieldman_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            EnemyBlockingMod_Tick();

            PerilousAttackMod_Tick();

            PlayerSpamBlockingMod_Tick();

            RightLegDamageColliderMod_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}