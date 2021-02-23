using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomPropertyDrawer(typeof(EnumNamedArrayAttribute))]
public class DrawerEnumNamedArray : PropertyDrawer
{
    //Called when the property is to be drawn
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Get the reverence to the enumNames property attribute
        EnumNamedArrayAttribute enumNames = attribute as EnumNamedArrayAttribute;

        //propertyPath returns something like component_hp_max.Array.data[4], so get the index from there
        int index = Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
        
        //if the enum has been resized, then resize the enumNames attribute
        if(index > enumNames.names.Length)
        {
            Array.Resize(ref enumNames.names, GameManager.numPlayerTypes);
        }

        //change the label
        label.text = enumNames.names[index];
        //draw field
        EditorGUI.PropertyField(position, property, label, true);
    }
}