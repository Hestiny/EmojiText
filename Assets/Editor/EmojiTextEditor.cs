using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EmojiText), true), CanEditMultipleObjects]
public class EmojiTextEditor : UnityEditor.UI.TextEditor
{
    private readonly List<SerializedProperty> _serializedPropertyS = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        InitSerializeFieldProperty();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawCustomSerializedProperty();
        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawCustomSerializedProperty()
    {
        foreach (var property in _serializedPropertyS)
        {
            EditorGUILayout.PropertyField(property);
        }
    }

    private void InitSerializeFieldProperty()
    {
        _serializedPropertyS.Clear();
        foreach (var propertyName in CustomExtensions.GetSerializeFieldProperty(typeof(EmojiText)))
        {
            _serializedPropertyS.Add(serializedObject.FindProperty(propertyName));
        }
    }
}

public static class CustomExtensions
{
    public static List<string> GetSerializeFieldProperty(Type type)
    {
        List<string> propertyNames = new List<string>();
        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        foreach (var item in fields)
        {
            if (item.IsPrivate && item.IsDefined(typeof(SerializeField), false))
            {
                propertyNames.Add(item.Name);
            }
            else if (item.IsPublic)
            {
                propertyNames.Add(item.Name);
            }
        }

        return propertyNames;
    }
}