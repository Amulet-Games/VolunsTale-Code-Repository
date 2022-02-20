using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(BomberAIManager))]
    [CanEditMultipleObjects]
    public class BomberAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        BomberAIManager ai;

        private void OnEnable()
        {
            ai = (BomberAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Bomber_OnInspectorGUI_Init();

            // Mods

            RollIntervalMod_Init(serializedObject);

            MoveInFixDirectionMod_Init(serializedObject);

            DualWeaponMod_Init(serializedObject);

            PlayerSpamAttackMod_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Bomber_OnInspectorGUI_Tick();

            // Mods

            Bomber_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Bomber_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Bomber_OnInspectorGUI_Tick()
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

        void Bomber_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            RollIntervalMod_Tick();

            MoveInFixDirectionMod_Tick();

            DualWeaponMod_Tick();

            PlayerSpamAttackMod_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}