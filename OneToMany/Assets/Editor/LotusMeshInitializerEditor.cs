using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LotusMeshInitializer))]
public class LotusMeshInitializerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LotusMeshInitializer lotusInitializer = (LotusMeshInitializer) target;

        if (GUILayout.Button("Create Submeshes"))
        {
            lotusInitializer.CollidersToSubmeshes();
        }

        if (GUILayout.Button("Update Materials"))
        {
            lotusInitializer.UpdateMaterials();
        }

        if (GUILayout.Button("Reset"))
        {
            lotusInitializer.Reset();
        }
    }
}
