using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InitialRecoil))]
public class InitialRecoilEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InitialRecoil recoilScript = (InitialRecoil)target;

        // Отображаем стандартные поля инспектора
        DrawDefaultInspector();

        // Добавляем пространство перед sightEnum и sight
        GUILayout.Space(20); // Расстояние в пикселях (например, 20)

        // Отображаем поле для sightEnum
        recoilScript.sightEnum = (Sight)EditorGUILayout.EnumPopup("Sight Mode", recoilScript.sightEnum);

        // Проверяем текущее значение Enum
        if (recoilScript.sightEnum == Sight.yes)
        {
            // Создаем стиль для поля
            GUIStyle style = new GUIStyle(EditorStyles.objectField);

            // Задаем цвет в зависимости от того, указано ли значение sight
            if (recoilScript.sight == null)
            {
                style.normal.textColor = Color.red;
                EditorGUILayout.HelpBox("Поле 'Sight' не указано! Укажите объект прицела.", MessageType.Warning);
            }
            else
            {
                style.normal.textColor = Color.green;
            }

            // Отображаем поле sight с созданным стилем
            Rect position = GUILayoutUtility.GetRect(0, 20); // Рект для размещения объекта
            recoilScript.sight = (GameObject)EditorGUI.ObjectField(position, "Sight", recoilScript.sight, typeof(GameObject), true);
        }
    }
}
