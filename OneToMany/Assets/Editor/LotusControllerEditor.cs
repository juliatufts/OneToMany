using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LotusController))]
public class LotusControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LotusController lotusController = (LotusController) target;

        if (GUILayout.Button("Create Submeshes"))
        {
            lotusController.CollidersToSubmeshes();
        }

        if (GUILayout.Button("Update Materials"))
        {
            lotusController.UpdateMaterials();
        }

        if (GUILayout.Button("Reset"))
        {
            lotusController.Reset();
        }
    }
}
