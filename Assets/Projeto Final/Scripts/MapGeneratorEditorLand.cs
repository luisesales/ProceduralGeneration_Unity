using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGeneratorLand))]
public class MapGeneratorEditorLand : Editor
{
    public override void OnInspectorGUI()
    {
        MapGeneratorLand mapGen = (MapGeneratorLand)target; 

        if(DrawDefaultInspector()) { 
            if(mapGen.autoUpdate)
            {
                mapGen.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.DrawMapInEditor();
        }
    }
}
