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

    [MenuItem("IA/Node Group")]
    public static void OpenWindow() {       //Start 
        GetWindow(typeof(GroupEditorWindow));
    }

    private void OnGUI() {
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
            //TODO Agregar ventana de editar nombre :3
            GUILayout.Button("Editar");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GetNodePrefab();
            SelectNodePrefab();
            EditorGUILayout.EndHorizontal();
            //TODO Assetdatabase
            CreateNode();
            ClearButton();
            AddFirst();
        }

        var currentPath = AssetDatabase.GetAssetPath(nodo);
        EditorGUILayout.LabelField(currentPath);

    }

    void ChooseNodeGroup() {
        EditorGUILayout.LabelField("Grupo", GUILayout.Width(50));
        nodegroup = (NodeGroup) EditorGUILayout.ObjectField(nodegroup, typeof(NodeGroup), true);
    }

    public void NewNodeGroup() {
        if ( GUILayout.Button("+", GUILayout.Width(20)) ) {

            NodeGroup ng = new GameObject().AddComponent<NodeGroup>(); ;
            nodegroup = (NodeGroup) EditorGUILayout.ObjectField("Grupo", ng, typeof(NodeGroup), false);
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
    void SelectNodePrefab() {
        GUILayout.Button("Seleccionar");
        //TODO Abrir ventana y seleccionar con assets y cosas
    }





    private void CreateNode() {
        if ( GUILayout.Button("Crear Nodo") ) {
            Node n = nodegroup.NewNode();
            n.name = n.id.ToString();
            Selection.activeGameObject = n.gameObject;
        }
    }
    private void AddFirst() {
        if ( GUILayout.Button("Agregar primer nodo") ) {
            Node n = nodegroup.NewNode();
            nodegroup.AddFirst(n);
            n.name = n.id.ToString();
        }
    }


    void ClearButton() {
        if ( GUILayout.Button("Limpiar nodos") ) {
            nodegroup.Clear();
        }

    }
    public void CheckAssetFolder() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Folder name: ", GUILayout.Width(75));
        newFolderName = EditorGUILayout.TextField(newFolderName);
        if ( GUILayout.Button("Create Folder") ) {
            //con esto podemos revisar si un folder existe o no.
            //SOLO VALIDO PARA CARPETAS DENTRO DE ASSETS
            //Y solo valido, obviamente, para cuando estamos trabajando dentro del editor de Unity
            if ( !AssetDatabase.IsValidFolder("Assets/" + newFolderName) ) {
                /*crea un nuevo folder dentro de assets
                el primer parámetro es la ruta a la carpeta padre, desde assets
                o sea, "Assets" si la queremos crear directo dentro de assets,
                o "Assets/OtraCarpeta", si lo quisieramos crear dentro de la carpeta "OtraCarpeta"*/
                AssetDatabase.CreateFolder("Assets", newFolderName);
                //UpdateDatabase();
            }
        }
        EditorGUILayout.EndHorizontal();


    }



}
