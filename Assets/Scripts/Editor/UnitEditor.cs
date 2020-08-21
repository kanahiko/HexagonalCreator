using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        TileType value = (TileType)property.intValue;
        value = (TileType)EditorGUI.EnumPopup(pos, value);
        property.intValue = (int)value;
        //_property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}


[CustomPropertyDrawer(typeof(EnumUnitTypeFlagsAttribute))]
public class EnumUnitTypeFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var pos = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        UnitType value = (UnitType)property.intValue;
        value = (UnitType)EditorGUI.EnumPopup(pos, value);
        property.intValue = (int)value;
        //_property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}