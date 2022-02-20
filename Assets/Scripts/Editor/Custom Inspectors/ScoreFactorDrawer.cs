using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SA
{
    [CustomPropertyDrawer(typeof(AIScoreFactors))]
    public class ScoreFactorDrawer : PropertyDrawer
    {
        int singleLineHeight = 18;

        Rect middleTop;

        ReorderableList scoreFactors_ReorderableList;

        SerializedProperty targetList;

        bool init = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            middleTop = new Rect(position.x, position.y + singleLineHeight, position.width, position.height);

            scoreFactors_ReorderableList.DoList(middleTop);

            // APPLY SAVES
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!init)
            {
                //REODERABLE LIST
                targetList = property.FindPropertyRelative("value");
                if (targetList == null)
                {
                    Debug.Log("list is null");
                }

                scoreFactors_ReorderableList = new ReorderableList(property.serializedObject, targetList, true, true, true, true);

                scoreFactors_ReorderableList.drawHeaderCallback = DrawHeaderCallback;
                scoreFactors_ReorderableList.drawElementCallback = DrawElementCallback;

                init = true;
            }


            return singleLineHeight * targetList.arraySize + 80;
        }

        void DrawHeaderCallback(Rect rect)
        {
            GUIStyle boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;
            boldStyle.normal.textColor = Color.white;
            Rect mainFontRect = new Rect(rect.x, rect.y + 2, rect.width, rect.height);
            GUI.Label(mainFontRect, "Score Factors", boldStyle);
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = scoreFactors_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

            Rect mainObjectFieldRect = new Rect(rect.x += 6, rect.y, rect.width * 0.7f, singleLineHeight);
            EditorGUI.ObjectField(mainObjectFieldRect, element, GUIContent.none);

            if (element.objectReferenceValue != null)
            {
                EditorGUI.BeginChangeCheck();

                SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);

                EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.78f, rect.y, 60, singleLineHeight), "Scores :");

                SerializedProperty returnScoreProperty = elementObj.FindProperty("m_ReturnScore");

                returnScoreProperty.intValue = EditorGUI.IntField(new Rect(rect.x + rect.width * 0.92f, rect.y, 30, singleLineHeight), returnScoreProperty.intValue);

                if (EditorGUI.EndChangeCheck())
                {
                    elementObj.ApplyModifiedProperties();
                }
            }
        }
    }
}