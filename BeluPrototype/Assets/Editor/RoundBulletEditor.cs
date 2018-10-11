using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoundBullets))]

public class RoundBulletEditor : Editor {
    private RoundBullets _round;
    public Transform canon;

    private void OnEnable() {
        _round = (RoundBullets) target;

    }

    public override void OnInspectorGUI() {
        Canon();
        Bullet();
        Angle();
        Radius();
        Speed();
        Test();
    }


    void Canon() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Canon", GUILayout.Width(40));
        _round.canon = (Transform) EditorGUILayout.ObjectField(_round.canon, typeof(Transform), true);
        _round.target = _round.transform;
        EditorGUILayout.EndHorizontal();
    }
    void Bullet() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bullet", GUILayout.Width(40));
        _round.circle = (Bullet) EditorGUILayout.ObjectField(_round.circle, typeof(Bullet), true);
        EditorGUILayout.LabelField("#", GUILayout.Width(12));
        _round.numberOfBullets = EditorGUILayout.IntField(_round.numberOfBullets,GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();
    }
    void Angle() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Angle", GUILayout.Width(40));
        _round.angle = EditorGUILayout.IntSlider(_round.angle, 0, 360);
        EditorGUILayout.EndHorizontal();
    }
    void Radius() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Radius X", GUILayout.Width(60));
        _round.radiusX = EditorGUILayout.FloatField(_round.radiusX);
        EditorGUILayout.LabelField("Radius Y", GUILayout.Width(60));
        _round.radiusY = EditorGUILayout.FloatField(_round.radiusY);
        EditorGUILayout.EndHorizontal();
    }
    void Speed() {
        EditorGUILayout.LabelField("Speed",EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Linear", GUILayout.Width(60));
        _round.speed = EditorGUILayout.FloatField(_round.speed);
        EditorGUILayout.LabelField("Oscilated", GUILayout.Width(60));
        _round.speedOscilation = EditorGUILayout.FloatField(_round.speedOscilation);
        EditorGUILayout.EndHorizontal();
    }
    void Test() {
        if ( GUILayout.Button("Test") ) {
            _round.Shoot();
        }
    }
}
