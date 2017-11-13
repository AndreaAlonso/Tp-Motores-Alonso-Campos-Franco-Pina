using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TutorialWindow : EditorWindow
{
    private Vector2 _scrollPosition;

    [MenuItem("Personalizado/Node Tutorial")]
    static void CreateWindow()
    {
        ((TutorialWindow)GetWindow(typeof(TutorialWindow))).Show();
    }

    void OnGUI()
    {
        minSize = new Vector2(430, 500);
        maxSize = new Vector2(430, 800);

        EditorGUILayout.BeginVertical();
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition,false, false);
        
        
        GUILayout.Label("Bienvenido al tutorial del editor de nodos del editor de dialogo.", EditorStyles.label);
        GUILayout.Label("Para abrir el editor, entrar a Personalizado/Node Tutorial.", EditorStyles.label);
        GUI.DrawTexture(GUILayoutUtility.GetRect(380, 214), (Texture2D)Resources.Load("Directorio"),ScaleMode.ScaleToFit);
        GUILayout.Label("Una vez abierto, para crear los nodos hacer click en los botones", EditorStyles.label);
        GUILayout.Label("superiores o hacer click derecho en la grilla de nodos", EditorStyles.label);
        GUI.DrawTexture(GUILayoutUtility.GetRect(380, 233), (Texture2D)Resources.Load("Nodos"), ScaleMode.ScaleToFit);
        GUILayout.Label("El nodo start es el que inicia el dialogo, el nodo Dialogue", EditorStyles.label);
        GUILayout.Label("tendra la opcion Tittle, donde pondras el nombre del nodo", EditorStyles.label);
        GUILayout.Label("y Dialogue, donde podras escribir lo que quieras dentro", EditorStyles.label);
        GUILayout.Label("del cuadro de texto y luego se vera en el juego. En el nodo Choice", EditorStyles.label);
        GUILayout.Label("tendras un titulo, un slider con el numero de cantidad de deciciones", EditorStyles.label);
        GUILayout.Label("y finalmente los cuadros de texto de todas las decisiones. El nodo", EditorStyles.label);
        GUILayout.Label("roll lo podras usar si quieres hacer una tirada de estadistica", EditorStyles.label);
        GUILayout.Label("para poder seguir o no con el dialogo. El nodo event sirve para", EditorStyles.label);
        GUILayout.Label("Dar una recompenza y finalmente el nodo end finaliza el dialogo.", EditorStyles.label);
        GUILayout.Label("Para unir dos nodos hacer click derecho en el nodo que quieras unir", EditorStyles.label);
        GUILayout.Label("luego hace click en el nodo con el que lo quieras unir.", EditorStyles.label);
        GUILayout.Label("Una vez que hayas terminado todos los nodos haz click en generate", EditorStyles.label);
        GUILayout.Label("conversation y se creata un scriptable object con el dialogo adentro", EditorStyles.label);
        GUI.DrawTexture(GUILayoutUtility.GetRect(380, 200), (Texture2D)Resources.Load("Final"), ScaleMode.ScaleToFit);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        
    }
}
