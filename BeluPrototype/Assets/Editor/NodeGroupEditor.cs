using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class NodeGroupEditor : EditorWindow {

    public bool isGroup;
    public NodeGroup nodegroup;

    [MenuItem("IA/Node Group")]
    public static void OpenWindow() {
        GetWindow(typeof(NodeGroupEditor)); 
    }
    private void OnGUI() {
        EditorGUI.BeginChangeCheck();
        ChooseNodeGroup();
        if ( isGroup ) {
            //TODO Funciones+
            EditorGUILayout.LabelField(nodegroup.name + ": Edicion", EditorStyles.boldLabel);
            CreateNode();
            ClearButton();

        if (GUILayout.Button("Guardar Cambios >:3") ) {
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
        }
        EditorGUI.EndChangeCheck();
    }

    private void CreateNode() {
        if ( GUILayout.Button("Crear Nodo") ) {
            Node n = nodegroup.NewNode();
            Selection.activeGameObject = n.gameObject;
        }
    }

    void ChooseNodeGroup() {
        EditorGUILayout.LabelField("Grupo de Nodos", EditorStyles.centeredGreyMiniLabel);
        nodegroup = (NodeGroup)EditorGUILayout.ObjectField("Grupo", nodegroup, typeof(NodeGroup), true);
        isGroup = nodegroup;
    }
    void ClearButton() {
        if (GUILayout.Button("Limpiar nodos") ) {
            nodegroup.Clear();
        }
    }
}
