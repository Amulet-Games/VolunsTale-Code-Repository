using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(EgilAIManager))]
    [CanEditMultipleObjects]
    public class EgilAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty canPlayIndicator;

        // Change To 2P Anim Velocity.
        SerializedProperty changeToVelocity;
        SerializedProperty _2P_fallbackVelocity;
        SerializedProperty showChangeTo2PAnimVelocity;
        bool openChangeTo2PAnimVelocity;

        // Private ref.
        EgilAIManager ai;

        private void OnEnable()
        {
            ai = (EgilAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Egil_OnInspectorGUI_Init();

            // Mods

            EgilStaminaMod_Init(serializedObject);

            RollIntervalMod_Init(serializedObject);

            EgilExecutionMod_Init(serializedObject);

            PerilousAttackMod_Init(serializedObject);

            KnockDownPlayerMod_Init(serializedObject);

            Egil_KMA_Mod_Init(serializedObject);

            EnemyCentralPhaseMod_Init(serializedObject);

            Enemy2ndPhase_EP_Mod_Init(serializedObject);

            Enemy3rdPhase_EP_Mod_Init(serializedObject);

            RightLegDamageColliderMod_Init(serializedObject);

            LeftShoulderDamageColliderMod_Init(serializedObject);

            LimitEnemyTurning_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            // General

            OnInspectorGUI_Tick();

            // Class

            Egil_OnInspectorGUI_Tick();

            // Mods

            Egil_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Egil_OnInspectorGUI_Init()
        {
            #region Indicators.
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
            #endregion

            #region Change To 2P Anim Velocity.
            changeToVelocity = serializedObject.FindProperty("changeToVelocity");
            _2P_fallbackVelocity = serializedObject.FindProperty("_2P_fallbackVelocity");

            showChangeTo2PAnimVelocity = serializedObject.FindProperty("showChangeTo2PAnimVelocity");
            openChangeTo2PAnimVelocity = showChangeTo2PAnimVelocity.boolValue;
            #endregion
        }

        void Egil_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            #region Indicators.
            openIndicators = EditorGUILayout.BeginFoldoutHeaderGroup(openIndicators, "AI Indicators", classFoldoutHeaderGUIStyle);
            if (openIndicators)
            {
                showIndicators.boolValue = true;
                EditorGUILayout.PropertyField(canPlayIndicator);

            }
            else
            {
                showIndicators.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Change To 2P Anim Velocity.
            openChangeTo2PAnimVelocity = EditorGUILayout.BeginFoldoutHeaderGroup(openChangeTo2PAnimVelocity, "2nd Phase Change Anim Velocity", classFoldoutHeaderGUIStyle);
            if (openChangeTo2PAnimVelocity)
            {
                showChangeTo2PAnimVelocity.boolValue = true;
                EditorGUILayout.PropertyField(changeToVelocity);
                EditorGUILayout.PropertyField(_2P_fallbackVelocity);

            }
            else
            {
                showChangeTo2PAnimVelocity.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            #endregion

            DrawUILine1();

            EditorGUILayout.EndVertical();
        }

        void Egil_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            EgilStaminaMod_Tick();

            RollIntervalMod_Tick();

            EgilExecutionMod_Tick();

            PerilousAttackMod_Tick();

            KnockDownPlayerMod_Tick();

            Egil_KMA_Mod_Tick();

            EnemyCentralPhaseMod_Tick();

            Enemy2ndPhase_EP_Mod_Tick();

            Enemy3rdPhase_EP_Mod_Tick();

            RightLegDamageColliderMod_Tick();

            LeftShoulderDamageColliderMod_Tick();

            LimitEnemyTurning_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}