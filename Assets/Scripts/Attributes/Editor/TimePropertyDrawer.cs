using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EditorTimeAttribute))]
public class TimePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float valueIn = property.floatValue;
        float h = (int)(valueIn / 3600), m = (int)(valueIn / 60 % 60), s = valueIn % 60.0f;

        float suffixWidth = 15.0f;
        var workingRect = EditorGUI.PrefixLabel(position, label);

        workingRect.width /= 7.0f;
        workingRect.width -= suffixWidth;

        System.Func<float, string, float> drawComp = (vin, l) =>
            {
                float vout = EditorGUI.FloatField(workingRect, vin);
                workingRect.x += workingRect.width;
                EditorGUI.LabelField(new Rect(workingRect.x, workingRect.y, suffixWidth, workingRect.height), l);
                workingRect.x += suffixWidth;
                return vout;
            };


        h = drawComp(h, "h");
        m = drawComp(m, "m");
        s = drawComp(s, "s");

        property.floatValue = h * 3600 + m * 60 + s;

        workingRect.width += suffixWidth;
        workingRect.x += workingRect.width;
        workingRect.width *= 3.0f;
        property.floatValue = EditorGUI.FloatField(workingRect, property.floatValue);
    }
}
