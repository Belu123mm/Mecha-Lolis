using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeGroupEditor : EditorWindow {

    public Object group;
    public bool isGroup;
    public NodeGroup nodegroup;

    [MenuItem("IA/Node Group")]
    public static void OpenWindow() {
        GetWindow(typeof(NodeGroupEditor)); 
    }
    private void OnGUI() {
        ChooseNodeGroup();
        if ( isGroup ) {
            //TODO Funciones+
            
        }
        else {
            nodegroup = null;        }
    }
    void ChooseNodeGroup() {
        EditorGUILayout.LabelField("Grupo de Nodos", EditorStyles.boldLabel);
        nodegroup = (NodeGroup)EditorGUILayout.ObjectField("Grupo", nodegroup, typeof(NodeGroup), true);
        isGroup = group;
    }
}
