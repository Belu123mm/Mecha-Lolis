using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class NodeGroupEditor : EditorWindow {
    //Con lo del undo guarda first y last, pero no la lista 
    [SerializeField]
    public NodeGroup nodegroup;
    public Node bulletprefab;

    [MenuItem("IA/Node Group")]
    public static void OpenWindow() {
        GetWindow(typeof(NodeGroupEditor)); 
    }
    private void OnGUI() {
        ChooseNodeGroup();
        NewNodeGroup();
        DeleteGroup();
        if ( nodegroup ) {
            //TODO Funciones+
            EditorGUILayout.LabelField(nodegroup.name + ": Edicion", EditorStyles.boldLabel);
            //TODO Assetdatabase
            bulletprefab = (Node)EditorGUILayout.ObjectField("Prefab de nodo", bulletprefab, typeof(Node), true);
            CreateNode();
            ClearButton();
            AddFirst();
        }
    }

    public void NewNodeGroup()
    {
        if (GUILayout.Button("Nuevo Grupo"))
        {

            NodeGroup ng = new GameObject().AddComponent<NodeGroup>(); ;
            nodegroup = (NodeGroup)EditorGUILayout.ObjectField("Grupo", ng, typeof(NodeGroup), true);
            nodegroup.name = "NewGroup";
        }
    }
    public void DeleteGroup()
    {
        if (GUILayout.Button("Borrar Grupo")&& nodegroup)
        {
            DestroyImmediate(nodegroup.gameObject);
            Repaint();
        }
    }

    private void CreateNode() {
        if ( GUILayout.Button("Crear Nodo") ) {
            Node n = nodegroup.NewNode();
            n.name = n.id.ToString();
            Selection.activeGameObject = n.gameObject;
        }
        Undo.RecordObject(nodegroup, "Nodo creado");
    }
    private void AddFirst()
    {
        if (GUILayout.Button("Agregar primer nodo"))
        {
            Node n = nodegroup.NewNode();
                nodegroup.AddFirst(n);
            n.name = n.id.ToString();
        }
    }

    void ChooseNodeGroup() {
        EditorGUILayout.LabelField("Grupo de Nodos", EditorStyles.centeredGreyMiniLabel);
        nodegroup = (NodeGroup)EditorGUILayout.ObjectField("Grupo", nodegroup, typeof(NodeGroup), true);
    }
    void ClearButton() {
        if (GUILayout.Button("Limpiar nodos") ) {
            nodegroup.Clear();
        }
        Undo.RecordObject(nodegroup, "Nodos Limpiados");

    }



}
