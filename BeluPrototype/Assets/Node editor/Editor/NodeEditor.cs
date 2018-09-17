using UnityEditor;
[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {
    private Node _node;

    private void OnEnable() {
        _node = (Node) target;

    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }
}