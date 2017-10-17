using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyNodeEditor : EditorWindow {

    [MenuItem("La/Ruta")]
    static void OpenMyWindow()
    {
        var m = (MyNodeEditor)GetWindow<MyNodeEditor>();
        m.Init();
    }

    private List<Node> _allNodes;

    private Node _nodo;
    private void Init()
    {
        _allNodes = new List<Node>();
        
    }
    
    private string _currentName;
    private bool _isDragging;

    private void OnGUI()
    {
        _isDragging =EditorGUILayout.Toggle("Is Dragging",_isDragging);
        minSize = maxSize = new Vector2(800, 600);
        EditorGUILayout.BeginHorizontal();
        _currentName = EditorGUILayout.TextField(_currentName);
        if(GUILayout.Button("Create Node"))
        {
            _allNodes.Add(new Node(_currentName));
        }
        EditorGUILayout.EndHorizontal();

        if (_allNodes == null) _allNodes = new List<Node>();

        BeginWindows();
        for (int i = _allNodes.Count - 1; i >= 0; i--)
        {
            _allNodes[i].window = GUI.Window(i, _allNodes[i].window, DrawNode, _allNodes[i].windowName);
            for (int j = _allNodes[i].allConnections.Count - 1; j >= 0; j--)
            {
                CreateLine(_allNodes[i].window,
                    _allNodes[_allNodes[i].allConnections[j]].window);
            }
        }
        EndWindows();
    }

    private void CreateLine(Rect a, Rect b)
    {
        Handles.DrawLine(a.position + new Vector2(400, 50), b.position+ new Vector2(0,50));      
    }

    private void DrawNode(int id)
    {
        if (_isDragging)
            GUI.DragWindow();
        EditorGUILayout.LabelField("Mi id es "+id);
        _allNodes[id].currentNodeToConnect = EditorGUILayout.IntField(_allNodes[id].currentNodeToConnect);
        if(GUILayout.Button("Conectar") && !_allNodes[id].allConnections.Contains(_allNodes[id].currentNodeToConnect))
        {
            _allNodes[id].allConnections.Add(_allNodes[id].currentNodeToConnect);
        }
        if(GUILayout.Button("Sacar") && _allNodes[id].allConnections.Contains(_allNodes[id].currentNodeToConnect))
        {
            _allNodes[id].allConnections.Remove(_allNodes[id].currentNodeToConnect);
        }
    }
}
