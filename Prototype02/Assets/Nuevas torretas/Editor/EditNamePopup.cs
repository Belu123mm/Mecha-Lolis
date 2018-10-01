using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditNamePopup : PopupWindowContent {
    public NodeGroup ng;
    public override Vector2 GetWindowSize() {
        return new Vector2(200, 75);
    }

    public override void OnGUI( Rect rect) {
        EditorGUI.DrawRect(rect, new Color32(149, 180, 209, 255));
        GUILayout.Label("Change Group Name", EditorStyles.boldLabel);
        ng.name = EditorGUILayout.TextField(ng.name);
        if ( GUILayout.Button("Save") ) {
            editorWindow.Close();
            editorWindow.Repaint();
       }
    }

}