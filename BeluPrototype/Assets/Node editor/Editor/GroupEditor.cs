using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGroup))]

public class GroupEditor : Editor {
    private NodeGroup _group;
    private GUIStyle centeredstyle;

    private void OnEnable() {
        _group = (NodeGroup) target;
        centeredstyle = new GUIStyle();
        centeredstyle.alignment = TextAnchor.MiddleCenter;
        centeredstyle.fontSize = 15;
        centeredstyle.fontStyle = FontStyle.Bold;
    }
    public override void OnInspectorGUI() {
        EditorGUILayout.LabelField(_group.name, centeredstyle);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("1°", GUILayout.Width(20));
        EditorGUI.BeginDisabledGroup(true);
        _group._first = (Node)EditorGUILayout.ObjectField(_group._first, typeof(Node), true);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.LabelField("N°", GUILayout.Width(20));
        EditorGUI.BeginDisabledGroup(true);
        _group._last = (Node) EditorGUILayout.ObjectField(_group._last, typeof(Node), true);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Cantidad: " + _group._count);

        if (! _group.nodePrefab ) {
            EditorGUILayout.LabelField("Node Prefab");
            EditorGUILayout.HelpBox("No tenes ningun prefab de nodo para usar! Abre el editor para seleccionar uno.", MessageType.Warning);
            if (GUILayout.Button("Abrir Editor") ) {
                GroupEditorWindow.OpenWindow();
            }
        }


    }
}

/*

private Node _node;

private void OnEnable() {
    _node = (Node) target;

}
public override void OnInspectorGUI() {
    GUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Id " + _node.id, GUILayout.Width(50));
    _node.color = EditorGUILayout.ColorField(_node.color);
    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal();
    GoToPrev();
    GoToNext();
    GUILayout.EndHorizontal();

}
void GoToNext() {
    if ( GUILayout.Button("Siguiente") ) {
        Selection.activeGameObject = _node.next.gameObject;
    }

}
void GoToPrev() {
    if ( GUILayout.Button("Anterior") ) {
        Selection.activeGameObject = _node.previous.gameObject;

    }

}

    */