using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            

            ctrl.ObstacleLayer = EditorGUILayout.LayerField("Obstacle Layer:", ctrl.ObstacleLayer);
            ctrl.WalkableLayer = EditorGUILayout.LayerField("Walkable Layer:", ctrl.WalkableLayer);


            if (GUILayout.Button("Build NavMesh"))
            {
                ctrl.Build();
            }

            ctrl.ShouldDrawConnections = GUILayout.Toggle(ctrl.ShouldDrawConnections, "Show Connections");

            if (GUILayout.Button("Test path"))
            {
                ctrl.FindPath();
            }

            DrawDefaultInspector();
            EditorUtility.SetDirty(ctrl);
        }
    }
}
