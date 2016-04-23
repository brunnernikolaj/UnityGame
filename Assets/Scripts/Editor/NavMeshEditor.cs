using UnityEditor;
using UnityEngine;

namespace Assets
{
    [CustomEditor(typeof(NavMeshController))]
    class NavMeshEditor : Editor
    {
        private bool drawConnections;

        public override void OnInspectorGUI()
        {
            NavMeshController ctrl = (NavMeshController)target;           
           
            ctrl.WalkableLayer = EditorGUILayout.LayerField("Walkable Layer:", ctrl.WalkableLayer);
            ctrl.ObstacleLayer = EditorGUILayout.LayerField("Obstacle Layer:", ctrl.ObstacleLayer);

            ctrl.ActorSize = EditorGUILayout.IntField("Actor Diameter", ctrl.ActorSize);

            if (GUILayout.Button("Build NavMesh"))
            {
                ctrl.BuildMesh();
            }

            ctrl.ShouldDrawConnections = GUILayout.Toggle(ctrl.ShouldDrawConnections, "Show Connections");

            if (GUILayout.Button("Test path"))
            {
                ctrl.FindPathDebug();
            }

            DrawDefaultInspector();
            EditorUtility.SetDirty(ctrl);
        }
    }
}
