using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SA
{
    [CustomEditor(typeof(FictionalCharacter))]
    public class AISessionVisualizer : Editor
    {
        FictionalCharacter _fictChar;
        
        private void OnEnable()
        {
            _fictChar = (FictionalCharacter)target;
        }
        
        void OnSceneGUI()
        {
            Handles.color = Color.red;

            GUIStyle _groupfontStyle = new GUIStyle();
            ModifyGUIStyleFont(_groupfontStyle, Color.red, 21);

            GUIStyle _disfontStyle = new GUIStyle();
            ModifyGUIStyleFont(_disfontStyle, Color.black, 20);

            float _closetDistance = 20000;
            AI_Group _closetGroup = null;

            for (int i = 0; i < _fictChar._aiGroupManagable.groupsInSession.Length; i++)
            {
                if (_fictChar._aiGroupManagable.groupsInSession != null)
                {
                    AI_Group _group = _fictChar._aiGroupManagable.groupsInSession[i];
                    Handles.DrawLine(_fictChar.transform.position, _group.transform.position);

                    float _curDistance = GetDistance(_fictChar.transform.position, _group.transform.position);
                    if (_curDistance < _closetDistance)
                    {
                        _closetGroup = _group;
                        _closetDistance = _curDistance;
                    }
                }
            }

            Handles.Label(_fictChar.transform.position + new Vector3(0, 3.5f, -0.75f), _closetGroup.gameObject.name, _groupfontStyle);
            Handles.Label(_fictChar.transform.position + new Vector3(0, 3f, -2.15f), _closetDistance.ToString(), _disfontStyle);

            Handles.color = Color.blue;
            for (int i = 0; i < _fictChar._aiGroupManagable.groupsInSession.Length; i++)
            {
                if (_fictChar._aiGroupManagable.groupsInSession != null)
                {
                    AI_Group _group = _fictChar._aiGroupManagable.groupsInSession[i];
                    AI_Group _nextGroup = i == _fictChar._aiGroupManagable.groupsInSession.Length - 1 ? _fictChar._aiGroupManagable.groupsInSession[0] : _fictChar._aiGroupManagable.groupsInSession[i + 1];

                    Handles.DrawLine(_group.transform.position, _nextGroup.transform.position);
                }
            }
        }

        float GetDistance(Vector3 _v1, Vector3 _v2)
        {
            _v1.y = 0;
            _v2.y = 0;
            return Vector3.Distance(_v1, _v2);
        }

        void ModifyGUIStyleFont(GUIStyle guiStyle, Color color, int size)
        {
            guiStyle.fontStyle = FontStyle.Bold;
            guiStyle.fontSize = size;
            guiStyle.normal.textColor = color;
        }
    }
}