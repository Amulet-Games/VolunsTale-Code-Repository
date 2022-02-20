using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(AI_ActionHolder))]
    [CanEditMultipleObjects]
    public class AI_ActionHolderInspector : Editor
    {
        ReorderableList aiActions_ReorderableList;

        SerializedProperty actionsList;

        float singleLineHeight = 18;

        public void OnEnable()
        {
            if(target == null)
                return;

            if (actionsList == null)
                actionsList = serializedObject.FindProperty("actionsList");

            aiActions_ReorderableList = new ReorderableList(serializedObject, actionsList, true, true, true, true);

            aiActions_ReorderableList.drawHeaderCallback = DrawHeaderCallback;
            aiActions_ReorderableList.drawElementCallback = DrawElementCallback;
        }

        void DrawHeaderCallback(Rect rect)
        {
            GUIStyle reorderableListHeaderFontSytle = new GUIStyle();
            reorderableListHeaderFontSytle.fontStyle = FontStyle.Bold;
            reorderableListHeaderFontSytle.normal.textColor = Color.white;

            Rect reorderableListHeaderFontRect = new Rect(rect.x + 15, rect.y + 3, rect.width, rect.height);
            EditorGUI.LabelField(reorderableListHeaderFontRect, "AI Actions", reorderableListHeaderFontSytle);
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            // Create SerializedProperty for every Action inside the Action Holder.
            SerializedProperty aiActionsProperty = aiActions_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

            // Draw Object Field.
            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width * 0.7f, singleLineHeight), aiActionsProperty, GUIContent.none);

            if (aiActionsProperty.objectReferenceValue != null)
            {
                EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.78f, rect.y, 60, singleLineHeight), "Scores :");

                // Convert Action SerializedProperty into SerializedObject.
                SerializedObject aiActionsObject = new SerializedObject(aiActionsProperty.objectReferenceValue);

                // Create SerializedProperty for Score Factor Array.
                SerializedProperty valueProperty = aiActionsObject.FindProperty("scoreFactors").FindPropertyRelative("value");

                int scoreFactorTotalScore = 0;
                for (int i = 0; i < valueProperty.arraySize; i++)
                {
                    // Create SerializedObject for every Score Factor inside the Score Factor Array.
                    SerializedObject scoreObject = new SerializedObject(valueProperty.GetArrayElementAtIndex(i).objectReferenceValue);

                    // Create SerializedProperty for Return Score
                    SerializedProperty returnScoreProperty = scoreObject.FindProperty("m_ReturnScore");
                    scoreFactorTotalScore += returnScoreProperty.intValue;
                }

                EditorGUI.IntField(new Rect(rect.x + rect.width * 0.92f, rect.y, 30, singleLineHeight), scoreFactorTotalScore);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            aiActions_ReorderableList.DoLayoutList();

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}