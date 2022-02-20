using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(WarriorAIManager))]
    [CanEditMultipleObjects]
    public class WarriorAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        WarriorAIManager ai;

        private void OnEnable()
        {
            ai = (WarriorAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Warrior_OnInspectorGUI_Init();

            // Mods

            PerilousAttackMod_Init(serializedObject);

            EnemyTauntMod_Init(serializedObject);

            ThrowReturnalProjectileMod_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Warrior_OnInspectorGUI_Tick();

            // Mods

            Warrior_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Warrior_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Warrior_OnInspectorGUI_Tick()
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

        void Warrior_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            PerilousAttackMod_Tick();

            EnemyTauntMod_Tick();

            ThrowReturnalProjectileMod_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}