using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SA
{
    [CustomPropertyDrawer(typeof(AnimStateVariables))]
    public class AnimStateVariablesDrawer : PropertyDrawer
    {
        ReorderableList enemyAnimStates_ReorderableList;
        ReorderableList playerAnimStates_ReorderableList;

        SerializedProperty enemyTargetList;
        SerializedProperty playerTargetList;
        SerializedProperty showFoldout;

        int enemyHeightScaler;
        int playerHeightScaler;
        int heightScaler;

        int singleLineHeight = 18;
        int buffer;

        bool init = false;
        bool unfold = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            // REODERABLE LIST
            if (!init)
            {
                // Enemy Anim States
                enemyTargetList = property.FindPropertyRelative("enemyAnimStates");
                if (enemyTargetList == null)
                {
                    Debug.Log("Enemy target list is null");
                }

                // Player Anim States
                playerTargetList = property.FindPropertyRelative("playerAnimStates");
                if (playerTargetList == null)
                {
                    Debug.Log("Player target list is null");
                }

                showFoldout = property.FindPropertyRelative("showFoldout");
                unfold = showFoldout.boolValue;

                enemyAnimStates_ReorderableList = new ReorderableList(property.serializedObject, enemyTargetList, true, true, true, true);

                enemyAnimStates_ReorderableList.drawHeaderCallback = DrawEnemyHeaderCallback;
                enemyAnimStates_ReorderableList.drawElementCallback = DrawEnemyElementCallback;

                playerAnimStates_ReorderableList = new ReorderableList(property.serializedObject, playerTargetList, true, true, true, true);

                playerAnimStates_ReorderableList.drawHeaderCallback = DrawPlayerHeaderCallback;
                playerAnimStates_ReorderableList.drawElementCallback = DrawPlayerElementCallback;

                init = true;
            }

            enemyHeightScaler = enemyTargetList.arraySize;
            playerHeightScaler = playerTargetList.arraySize;
            heightScaler = enemyHeightScaler + playerHeightScaler;
            if (heightScaler == 0)
                heightScaler = 2;

            // RECT POSITIONS
            Rect top = new Rect(position.x, position.y, position.width, singleLineHeight);
            Rect middleTop = new Rect(position.x, position.y + singleLineHeight + 10, position.width, position.height);
            Rect middleDown = new Rect(position.x, position.y + singleLineHeight * enemyHeightScaler + 110, position.width, position.height);

            unfold = EditorGUI.Foldout(top, unfold, "Anim State Variables");
            if(unfold)
            {
                enemyAnimStates_ReorderableList.DoList(middleTop);
                playerAnimStates_ReorderableList.DoList(middleDown);

                buffer = 300;
                showFoldout.boolValue = true;
            }
            else
            {
                heightScaler = 1;
                buffer = 0;
                showFoldout.boolValue = false;
            }

            // APPLY SAVES
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (singleLineHeight + 2) * heightScaler + buffer;
        }

        void DrawEnemyHeaderCallback(Rect rect)
        {
            GUIStyle boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;
            boldStyle.normal.textColor = Color.white;
            Rect mainFontRect = new Rect(rect.x, rect.y + 2, rect.width, rect.height);
            GUI.Label(mainFontRect, "Enemy Anim State Variables", boldStyle);
        }

        void DrawPlayerHeaderCallback(Rect rect)
        {
            GUIStyle boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;
            boldStyle.normal.textColor = Color.white;
            Rect mainFontRect = new Rect(rect.x, rect.y + 2, rect.width, rect.height);
            GUI.Label(mainFontRect, "Player Anim State Variables", boldStyle);
        }

        void DrawEnemyElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = enemyAnimStates_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

            Rect mainObjectFieldRect = new Rect(rect.x += 6, rect.y, rect.width, singleLineHeight);
            EditorGUI.ObjectField(mainObjectFieldRect, element, GUIContent.none);

            //if (element.objectReferenceValue != null)
            //{
            //    SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);

            //    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.82f, rect.y, 60, singleLineHeight), "Index :");

            //    SerializedProperty returnScoreProperty = elementObj.FindProperty("animStateHash");

            //    returnScoreProperty.intValue = EditorGUI.IntField(new Rect(rect.x + rect.width * 0.92f, rect.y, 30, singleLineHeight), returnScoreProperty.intValue);
            //}
        }

        void DrawPlayerElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = playerAnimStates_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

            Rect mainObjectFieldRect = new Rect(rect.x += 6, rect.y, rect.width, singleLineHeight);
            EditorGUI.ObjectField(mainObjectFieldRect, element, GUIContent.none);

            //if (element.objectReferenceValue != null)
            //{
            //    SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);

            //    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.82f, rect.y, 60, singleLineHeight), "Index :");

            //    SerializedProperty returnScoreProperty = elementObj.FindProperty("animStateHash");

            //    returnScoreProperty.intValue = EditorGUI.IntField(new Rect(rect.x + rect.width * 0.92f, rect.y, 30, singleLineHeight), returnScoreProperty.intValue);
            //}
        }
    }
}