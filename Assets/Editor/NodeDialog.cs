using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeDialog : EditorWindow {

    [MenuItem("Personalizado/Dialogos")]
    static void OpenMyWindow()
    {
        var m = (NodeDialog)GetWindow<NodeDialog>();
        m.Init();
    }

    private List<Node> _allDialog;

    
    private void Init()
    {
        _allDialog = new List<Node>();
        
    }
    
    private string _name;
    private bool _drag;

    private void OnGUI()
    {
        _drag =EditorGUILayout.Toggle("esta draggeando",_drag);

        minSize = new Vector2(800, 600);
        maxSize = new Vector2(1200, 1000);

        EditorGUILayout.BeginHorizontal();
        _name = EditorGUILayout.TextField(_name);
        if(GUILayout.Button("Crear Dialogo"))
        {
            _allDialog.Add(new Node(_name));
        }
        EditorGUILayout.EndHorizontal();

        if (_allDialog == null) _allDialog = new List<Node>();

        BeginWindows();
        for (int i = _allDialog.Count - 1; i >= 0; i--)
        {
            _allDialog[i].window = GUI.Window(i, _allDialog[i].window, DrawDialog, _allDialog[i].windowName);
            for (int j = _allDialog[i].allConnections.Count - 1; j >= 0; j--)
            {
                PrintLine(_allDialog[i].window,
                    _allDialog[_allDialog[i].allConnections[j]].window);
            }
        }
        EndWindows();
    }

    private void PrintLine(Rect a, Rect b)
    {
        Handles.DrawLine(a.position + new Vector2(400, 50), b.position+ new Vector2(0,50));      
    }

    private void DrawDialog(int id)
    {
        if (_drag)
            GUI.DragWindow();
        EditorGUILayout.LabelField("La id es "+id);
        _allDialog[id].currentNodeToConnect = EditorGUILayout.IntField(_allDialog[id].currentNodeToConnect);
        if(GUILayout.Button("Conectar") && !_allDialog[id].allConnections.Contains(_allDialog[id].currentNodeToConnect))
        {
            _allDialog[id].allConnections.Add(_allDialog[id].currentNodeToConnect);
        }
        if(GUILayout.Button("Desconectar") && _allDialog[id].allConnections.Contains(_allDialog[id].currentNodeToConnect))
        {
            _allDialog[id].allConnections.Remove(_allDialog[id].currentNodeToConnect);
        }
    }
}
