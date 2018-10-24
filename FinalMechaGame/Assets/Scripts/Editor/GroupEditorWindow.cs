using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class GroupEditorWindow : EditorWindow {
    [SerializeField]
    public NodeGroup nodegroup;
    public Node nodo;
    private string newFolderName = "Bullet prefabs";
    bool hasFolder;
    private string folderPath;

    [MenuItem("IA/Node Group")]
    public static void OpenWindow() {       //Start 
        GetWindow(typeof(GroupEditorWindow));
        
    }
    private void OnEnable() {
        folderPath = "Assets/Node editor/Node Prefabs";
    }

    private void OnGUI() {
        Rect buttonRect = new Rect(position.height/3,position.width/5, 0, 0);
        EditorGUILayout.LabelField("Grupo de Nodos", EditorStyles.centeredGreyMiniLabel);

        EditorGUILayout.BeginHorizontal();
        ChooseNodeGroup();
        NewNodeGroup();
        DeleteGroup();
        EditorGUILayout.EndHorizontal();

        if ( nodegroup ) {

            //TODO Funciones+
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(nodegroup.name + ": Edicion", EditorStyles.boldLabel);
            if ( GUILayout.Button("Editar") ) {
                Debug.Log("editadah");
                var popup = new EditNamePopup {
                    ng = nodegroup
                };
                PopupWindow.Show(buttonRect, popup );
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GetNodePrefab();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CreateNode();
            AddFirst();
            AddLast();
            ClearButton();
            EditorGUILayout.EndHorizontal();


            if (AssetDatabase.IsValidFolder(folderPath)) {
                if (GUILayout.Button("Guardar grupo como archivo") ) {
                    PrefabUtility.CreatePrefab(folderPath + "/" + nodegroup.name + ".prefab", nodegroup.gameObject);
                }
            }
            else {
                EditorGUILayout.HelpBox("No tenes carpeta de Prefabs! Haz click para crear una.", MessageType.Warning);
                if ( GUILayout.Button("Crear Carpeta ") ) {
                    AssetDatabase.CreateFolder("Assets/Node editor", "Node Prefabs");
                }

            }
        }
    }

    void ChooseNodeGroup() {
        EditorGUILayout.LabelField("Grupo", GUILayout.Width(50));
        nodegroup = (NodeGroup) EditorGUILayout.ObjectField(nodegroup, typeof(NodeGroup), true);
    }

    public void NewNodeGroup() {
        if ( GUILayout.Button("+", GUILayout.Width(20)) ) {

            NodeGroup ng = new GameObject().AddComponent<NodeGroup>(); ;
            ng.nodeList = new List<Node>();
            nodegroup = ng;
            nodegroup.name = "NewGroup";
        }
    }

    public void DeleteGroup() {
        if ( GUILayout.Button("-", GUILayout.Width(20)) && nodegroup ) {
            DestroyImmediate(nodegroup.gameObject);
            Repaint();
        }
    }

    void GetNodePrefab() {
        EditorGUILayout.LabelField("Nodo", GUILayout.Width(50));
        nodo = (Node) EditorGUILayout.ObjectField(nodo, typeof(Node), true);
        nodegroup.nodePrefab = nodo;
    }

    private void CreateNode() {
        if ( GUILayout.Button("Crear Nodo") ) {
            Node n = nodegroup.NewNode();
            n.name = n.id.ToString();
            Selection.activeGameObject = n.gameObject;
        }
    }
    private void AddFirst() {
        if ( GUILayout.Button("Agregar primer") ) {
            Node n = nodegroup.NewNode();
            nodegroup.AddFirst(n);
            n.name = n.id.ToString();
        }
    }
    private void AddLast() {
        if ( GUILayout.Button("Agregar ultimo") ) {
            Node n = nodegroup.NewNode();
            nodegroup.AddLast(n);
            n.name = n.id.ToString();
        }

    }

    void ClearButton() {
        if ( GUILayout.Button("Limpiar nodos") ) {
            nodegroup.Clear();
        }

    }


}
