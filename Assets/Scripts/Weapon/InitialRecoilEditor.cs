using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InitialRecoil))]
public class InitialRecoilEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InitialRecoil recoilScript = (InitialRecoil)target;

        // ���������� ����������� ���� ����������
        DrawDefaultInspector();

        // ��������� ������������ ����� sightEnum � sight
        GUILayout.Space(20); // ���������� � �������� (��������, 20)

        // ���������� ���� ��� sightEnum
        recoilScript.sightEnum = (Sight)EditorGUILayout.EnumPopup("Sight Mode", recoilScript.sightEnum);

        // ��������� ������� �������� Enum
        if (recoilScript.sightEnum == Sight.yes)
        {
            // ������� ����� ��� ����
            GUIStyle style = new GUIStyle(EditorStyles.objectField);

            // ������ ���� � ����������� �� ����, ������� �� �������� sight
            if (recoilScript.sight == null)
            {
                style.normal.textColor = Color.red;
                EditorGUILayout.HelpBox("���� 'Sight' �� �������! ������� ������ �������.", MessageType.Warning);
            }
            else
            {
                style.normal.textColor = Color.green;
            }

            // ���������� ���� sight � ��������� ������
            Rect position = GUILayoutUtility.GetRect(0, 20); // ���� ��� ���������� �������
            recoilScript.sight = (GameObject)EditorGUI.ObjectField(position, "Sight", recoilScript.sight, typeof(GameObject), true);
        }
    }
}
