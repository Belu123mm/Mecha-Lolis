using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {
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
}