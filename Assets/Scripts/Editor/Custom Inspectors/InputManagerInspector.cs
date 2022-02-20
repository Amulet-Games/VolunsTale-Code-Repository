using System;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(InputManager))]
    public class InputManagerInspector : Editor
    {
        GUIStyle reorderableListHeaderGUIStyle;
        GUIStyle contentHeaderGUIStyle;
        GUIStyle contentSubHeaderGUIStyle;

        bool openFixedTickList;
        bool openTickList;
        bool openLateTickList;
        bool openSkyboxFoldout;
        bool openCurrentInputType;
        bool openGeneralInputs;
        bool openMenuInputs;
        bool openBools;
        bool openFloats;
        bool openRefs;
        bool openDragAndDropRefs;

        // Mono Actions
        ReorderableList fixedTick_Actions_ReorderableList;
        ReorderableList tick_Actions_ReorderableList;
        ReorderableList lateTick_Actions_ReorderableList;
        SerializedProperty fixedTickActionsList;
        SerializedProperty tickActionsList;
        SerializedProperty lateTickActionsList;
        
        // Current Input Type
        SerializedProperty currentInputType;

        // General Inputs
        SerializedProperty gen_mouse_scroll;

        // Menu Inputs
        SerializedProperty menu_up_input;
        SerializedProperty menu_down_input;
        SerializedProperty menu_right_input;
        SerializedProperty menu_left_input;
        SerializedProperty menu_mouse_position;

        SerializedProperty menu_select_mouse;
        SerializedProperty menu_select_input;
        SerializedProperty menu_remove_input;
        SerializedProperty menu_quit_input;
        SerializedProperty menu_switch_input;
        SerializedProperty menu_hide_input;
        
        SerializedProperty isNeglectSelectionMenu;
        SerializedProperty isInMenuManager;
        
        // Floats
        SerializedProperty delta;
        SerializedProperty fixedDelta;

        // Skybox
        //SerializedProperty _skyboxRotateSpeed;
        //SerializedProperty _currentSkyBox;

        // Refs
        SerializedProperty _states;
        SerializedProperty _camHandler;
        SerializedProperty _mainHudManager;
        SerializedProperty _charRegistWindow;
        SerializedProperty _selectionMenuManager;
        SerializedProperty _equipmentMenuManager;
        SerializedProperty _instructionMenuManager;
        SerializedProperty _checkpointMenuManager;
        SerializedProperty _levelingMenuManager;
        
        // Show Foldout
        SerializedProperty showFixedTickFoldout;
        SerializedProperty showTickFoldout;
        SerializedProperty showLateTickFoldout;
        SerializedProperty showSkyboxFoldout;
        SerializedProperty showCurrentInputTypeFoldout;
        SerializedProperty showGeneralInputFoldout;
        SerializedProperty showMenuInputFoldout;
        SerializedProperty showBoolsFoldout;
        SerializedProperty showFloatsFoldout;
        SerializedProperty showRefsFoldout;
        SerializedProperty showDragAndDropRefsFoldout;

        int singleLineHeight = 18;

        private void OnEnable()
        {
            if (target == null)
                return;

            InitReorderableListHeaderStyle();

            #region Fixed Tick Reorderable List.
            if (fixedTickActionsList == null)
                fixedTickActionsList = serializedObject.FindProperty("fixedTick_Actions");

            fixedTick_Actions_ReorderableList = new ReorderableList(serializedObject, fixedTickActionsList, true, true, true, true);
            fixedTick_Actions_ReorderableList.drawHeaderCallback = DrawFixedTickHeaderCallback;
            fixedTick_Actions_ReorderableList.drawElementCallback = DrawFixedTickElementCallback;
            #endregion

            #region Tick Reorderable List.
            if (tickActionsList == null)
                tickActionsList = serializedObject.FindProperty("tick_Actions");

            tick_Actions_ReorderableList = new ReorderableList(serializedObject, tickActionsList, true, true, true, true);
            tick_Actions_ReorderableList.drawHeaderCallback = DrawTickHeaderCallback;
            tick_Actions_ReorderableList.drawElementCallback = DrawTickElementCallback;
            #endregion

            #region Late Tick Reorderable List.
            if (lateTickActionsList == null)
                lateTickActionsList = serializedObject.FindProperty("lateTick_Actions");

            lateTick_Actions_ReorderableList = new ReorderableList(serializedObject, lateTickActionsList, true, true, true, true);
            lateTick_Actions_ReorderableList.drawHeaderCallback = DrawLateTickHeaderCallback;
            lateTick_Actions_ReorderableList.drawElementCallback = DrawLateTickElementCallback;
            #endregion

            // Current Input Type.
            currentInputType = serializedObject.FindProperty("currentInputType");

            // General Input
            gen_mouse_scroll = serializedObject.FindProperty("gen_mouse_scroll");

            // Menu Inputs
            menu_up_input = serializedObject.FindProperty("menu_up_input");
            menu_down_input = serializedObject.FindProperty("menu_down_input");
            menu_right_input = serializedObject.FindProperty("menu_right_input");
            menu_left_input = serializedObject.FindProperty("menu_left_input");
            menu_mouse_position = serializedObject.FindProperty("menu_mouse_position");

            menu_select_mouse = serializedObject.FindProperty("menu_select_mouse");
            menu_select_input = serializedObject.FindProperty("menu_select_input");
            menu_remove_input = serializedObject.FindProperty("menu_remove_input");
            menu_quit_input = serializedObject.FindProperty("menu_quit_input");
            menu_switch_input = serializedObject.FindProperty("menu_switch_input");
            menu_hide_input = serializedObject.FindProperty("menu_hide_input");
            
            isNeglectSelectionMenu = serializedObject.FindProperty("isNeglectSelectionMenu");
            isInMenuManager = serializedObject.FindProperty("isInMenuManager");
            
            // Floats.
            delta = serializedObject.FindProperty("delta");
            fixedDelta = serializedObject.FindProperty("fixedDelta");
            
            // Skybox.
            //_skyboxRotateSpeed = serializedObject.FindProperty("_skyboxRotateSpeed");
            //_currentSkyBox = serializedObject.FindProperty("_currentSkyBox");
            
            // Refs.
            _states = serializedObject.FindProperty("_states");
            _camHandler = serializedObject.FindProperty("_camHandler");
            _mainHudManager = serializedObject.FindProperty("_mainHudManager");
            _charRegistWindow = serializedObject.FindProperty("_charRegistWindow");
            _selectionMenuManager = serializedObject.FindProperty("_selectionMenuManager");
            _equipmentMenuManager = serializedObject.FindProperty("_equipmentMenuManager");
            _instructionMenuManager = serializedObject.FindProperty("_instructionMenuManager");
            _checkpointMenuManager = serializedObject.FindProperty("_checkpointMenuManager");
            _levelingMenuManager = serializedObject.FindProperty("_levelingMenuManager");
            
            #region Open Show Foldouts.
            showFixedTickFoldout = serializedObject.FindProperty("showFixedTickFoldout");
            openFixedTickList = showFixedTickFoldout.boolValue;

            showTickFoldout = serializedObject.FindProperty("showTickFoldout");
            openTickList = showTickFoldout.boolValue;

            showLateTickFoldout = serializedObject.FindProperty("showLateTickFoldout");
            openLateTickList = showLateTickFoldout.boolValue;

            showCurrentInputTypeFoldout = serializedObject.FindProperty("showCurrentInputTypeFoldout");
            openCurrentInputType = showCurrentInputTypeFoldout.boolValue;

            showGeneralInputFoldout = serializedObject.FindProperty("showGeneralInputFoldout");
            openGeneralInputs = showGeneralInputFoldout.boolValue;

            showMenuInputFoldout = serializedObject.FindProperty("showMenuInputFoldout");
            openMenuInputs = showMenuInputFoldout.boolValue;

            showBoolsFoldout = serializedObject.FindProperty("showBoolsFoldout");
            openBools = showBoolsFoldout.boolValue;

            showFloatsFoldout = serializedObject.FindProperty("showFloatsFoldout");
            openFloats = showFloatsFoldout.boolValue;

            showSkyboxFoldout = serializedObject.FindProperty("showSkyboxFoldout");
            openSkyboxFoldout = showSkyboxFoldout.boolValue;

            showRefsFoldout = serializedObject.FindProperty("showRefsFoldout");
            openRefs = showRefsFoldout.boolValue;

            showDragAndDropRefsFoldout = serializedObject.FindProperty("showDragAndDropRefsFoldout");
            openDragAndDropRefs = showDragAndDropRefsFoldout.boolValue;
            #endregion

            InitContentHeaderGUIStyle();
            InitContentSubHeaderGUIStyle();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();

            #region Fixed Tick Actions.
            openFixedTickList = EditorGUILayout.BeginFoldoutHeaderGroup(openFixedTickList, "Fixed Tick Actions.");
            if (openFixedTickList)
            {
                EditorGUILayout.Space();
                showFixedTickFoldout.boolValue = true;
                fixedTick_Actions_ReorderableList.DoLayoutList();
            }
            else
            {
                showFixedTickFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Tick Actions.
            openTickList = EditorGUILayout.BeginFoldoutHeaderGroup(openTickList, "Tick Actions.");
            if (openTickList)
            {
                EditorGUILayout.Space();
                showTickFoldout.boolValue = true;
                tick_Actions_ReorderableList.DoLayoutList();
            }
            else
            {
                showTickFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Late Ticks.
            openLateTickList = EditorGUILayout.BeginFoldoutHeaderGroup(openLateTickList, "Late Tick Actions.");
            if (openLateTickList)
            {
                EditorGUILayout.Space();
                showLateTickFoldout.boolValue = true;
                lateTick_Actions_ReorderableList.DoLayoutList();
            }
            else
            {
                showLateTickFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Current Input Type.
            openCurrentInputType = EditorGUILayout.BeginFoldoutHeaderGroup(openCurrentInputType, "Current Input Type.");
            if (openCurrentInputType)
            {
                showCurrentInputTypeFoldout.boolValue = true;

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(currentInputType);
            }
            else
            {
                showCurrentInputTypeFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region General Input.
            openGeneralInputs = EditorGUILayout.BeginFoldoutHeaderGroup(openGeneralInputs, "General Inputs.");
            if (openGeneralInputs)
            {
                showGeneralInputFoldout.boolValue = true;

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(gen_mouse_scroll);
            }
            else
            {
                showGeneralInputFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Menu Inputs.
            openMenuInputs = EditorGUILayout.BeginFoldoutHeaderGroup(openMenuInputs, "Menu Inputs.");
            if (openMenuInputs)
            {
                showMenuInputFoldout.boolValue = true;

                DrawContentHeader("Menu Inputs", contentHeaderGUIStyle);
                DrawContentSubHeader("Movement", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(menu_up_input);
                EditorGUILayout.PropertyField(menu_down_input);
                EditorGUILayout.PropertyField(menu_right_input);
                EditorGUILayout.PropertyField(menu_left_input);
                EditorGUILayout.PropertyField(menu_mouse_position);
                EditorGUI.indentLevel -= 1;

                DrawContentSubHeader("Options", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(menu_select_mouse);
                EditorGUILayout.PropertyField(menu_select_input);
                EditorGUILayout.PropertyField(menu_remove_input);
                EditorGUILayout.PropertyField(menu_quit_input);
                EditorGUILayout.PropertyField(menu_switch_input);
                EditorGUILayout.PropertyField(menu_hide_input);
                EditorGUI.indentLevel -= 1;
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showMenuInputFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Bools.
            openBools = EditorGUILayout.BeginFoldoutHeaderGroup(openBools, "Bools.");
            if (openBools)
            {
                showBoolsFoldout.boolValue = true;
                DrawContentHeader("Selections", contentHeaderGUIStyle);
                EditorGUILayout.PropertyField(isNeglectSelectionMenu);
                EditorGUILayout.PropertyField(isInMenuManager);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showBoolsFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Floats.
            openFloats = EditorGUILayout.BeginFoldoutHeaderGroup(openFloats, "Floats.");
            if (openFloats)
            {
                showFloatsFoldout.boolValue = true;

                DrawContentHeader("Floats", contentHeaderGUIStyle);
                EditorGUILayout.PropertyField(delta);
                EditorGUILayout.PropertyField(fixedDelta);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showFloatsFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Skybox.
            openSkyboxFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(openSkyboxFoldout, "Skybox.");
            if (openSkyboxFoldout)
            {
                showSkyboxFoldout.boolValue = true;

                //EditorGUILayout.Space();
                //EditorGUILayout.PropertyField(_skyboxRotateSpeed);
                //EditorGUILayout.PropertyField(_currentSkyBox);
            }
            else
            {
                showSkyboxFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion

            #region Refs.
            openRefs = EditorGUILayout.BeginFoldoutHeaderGroup(openRefs, "Refs.");
            if (openRefs)
            {
                showRefsFoldout.boolValue = true;
                DrawContentSubHeader("Cores", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_states);
                EditorGUILayout.PropertyField(_camHandler);
                EditorGUI.indentLevel -= 1;

                DrawContentSubHeader("Hud / Windows", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_mainHudManager);
                EditorGUILayout.PropertyField(_charRegistWindow);
                EditorGUI.indentLevel -= 1;

                DrawContentSubHeader("Selection Menus", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_selectionMenuManager);
                EditorGUILayout.PropertyField(_equipmentMenuManager);
                EditorGUILayout.PropertyField(_instructionMenuManager);
                EditorGUI.indentLevel -= 1;

                DrawContentSubHeader("Checkpoint Menus", contentSubHeaderGUIStyle);
                EditorGUILayout.PropertyField(_checkpointMenuManager);
                EditorGUILayout.PropertyField(_levelingMenuManager);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                showRefsFoldout.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            #endregion
            
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        void InitReorderableListHeaderStyle()
        {
            if (reorderableListHeaderGUIStyle == null)
            {
                reorderableListHeaderGUIStyle = new GUIStyle();
                reorderableListHeaderGUIStyle.fontStyle = FontStyle.Bold;
                reorderableListHeaderGUIStyle.normal.textColor = Color.white;
            }
        }

        void InitContentHeaderGUIStyle()
        {
            if (contentHeaderGUIStyle == null)
            {
                contentHeaderGUIStyle = new GUIStyle();
                contentHeaderGUIStyle.fontStyle = FontStyle.Bold;
                contentHeaderGUIStyle.fontSize = 12;
                Color32 headerColor = new Color32(235, 89, 110, 255);
                contentHeaderGUIStyle.normal.textColor = headerColor;
            }
        }

        void InitContentSubHeaderGUIStyle()
        {
            if (contentSubHeaderGUIStyle == null)
            {
                contentSubHeaderGUIStyle = new GUIStyle();
                contentSubHeaderGUIStyle.fontStyle = FontStyle.Bold;
                contentSubHeaderGUIStyle.fontSize = 12;
                Color32 subHeaderColor = new Color32(245, 172, 183, 255);
                contentSubHeaderGUIStyle.normal.textColor = subHeaderColor;
            }
        }

        void DrawFixedTickHeaderCallback(Rect rect)
        {
            Rect headerFontRect = new Rect(rect.x + 15, rect.y + 3, rect.width, rect.height);
            EditorGUI.LabelField(headerFontRect, "Fixed Tick Actions", reorderableListHeaderGUIStyle);
        }

        void DrawTickHeaderCallback(Rect rect)
        {
            Rect headerFontRect = new Rect(rect.x + 15, rect.y + 3, rect.width, rect.height);
            EditorGUI.LabelField(headerFontRect, "Tick Actions", reorderableListHeaderGUIStyle);
        }

        void DrawLateTickHeaderCallback(Rect rect)
        {
            Rect headerFontRect = new Rect(rect.x + 15, rect.y + 3, rect.width, rect.height);
            EditorGUI.LabelField(headerFontRect, "Late Tick Actions", reorderableListHeaderGUIStyle);
        }

        void DrawFixedTickElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty fixedTickActionsProperty = fixedTick_Actions_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, singleLineHeight), fixedTickActionsProperty, GUIContent.none);
        }

        void DrawTickElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty tickActionsProperty = tick_Actions_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, singleLineHeight), tickActionsProperty, GUIContent.none);
        }

        void DrawLateTickElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty lateTickActionsProperty = lateTick_Actions_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, singleLineHeight), lateTickActionsProperty, GUIContent.none);
        }

        void DrawContentHeader(string headerName, GUIStyle headerFontStyle)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField(headerName, headerFontStyle);
            EditorGUI.indentLevel += 1;
        }

        void DrawContentSubHeader(string subHeaderName, GUIStyle subHeaderFontStyle)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField(subHeaderName, subHeaderFontStyle);
            EditorGUI.indentLevel += 1;
        }
    }
}