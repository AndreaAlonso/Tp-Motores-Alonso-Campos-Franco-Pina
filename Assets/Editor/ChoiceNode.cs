using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChoiceNode : BaseNode{
    public override void DrawCurves()
    {
    }
    /* Este nodo tiene texto, pero ademas da a elegir opciones de dialogo
     * cada opcion tiene un output a otro nodo
     */

    Vector2 scrollPos = Vector2.zero;

    private string dialogue = "";
    private int numOptions;
    private List<string> options = new List<string>();

    public ChoiceNode()
    {
        windowTittle = "Choice";
    }
    /*
    void OnGUI() {
        //EditorGUILayout.BeginVertical();
        //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(15), GUILayout.Height(15));
        //EditorGUILayout.EndScrollView();
        //EditorGUILayout.EndVertical();
        scrollPos = GUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.EndScrollView();
    }
    */
    public override void DrawWindow()
    {

        base.DrawWindow();

        //GUILayout.BeginArea(new Rect(5f,25f,windowRect.width,Mathf.Infinity));
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(windowRect.width - 8f), GUILayout.Height(windowRect.height - 40f));

        EditorGUILayout.LabelField("num of Choices:");
        numOptions = EditorGUILayout.IntSlider(numOptions,0,6);
        for (int i = 0; i < numOptions; i++)
        {
            //por ahora es lo mejor que se me ocurre, esta medio raro
            options.Add("...");
            EditorGUILayout.LabelField("Choice " + (i+1).ToString() + ":");
            options[i] = EditorGUILayout.TextField(options[i]);
        }

        GUILayout.EndScrollView();
        //GUILayout.EndArea();

        /*agregar:
         * cantidad de opciones         LISTO
         * texto para cada opcion       LISTO
         * output para cada opcion      ---
         */
    }
}
