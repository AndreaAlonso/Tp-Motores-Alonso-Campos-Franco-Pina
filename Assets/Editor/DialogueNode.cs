using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseNode{
    public override void DrawCurves()
    {
    }
    /* Este es el nodo comun, solo lleva texto y boton de "continue"
     */

    private string text = "";

    public DialogueNode()
    {
        windowTittle = "Text";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        EditorGUILayout.LabelField("Dialogue:");
        text = EditorGUILayout.TextField(text,GUILayout.Height(50));
        //inputValue = EditorGUILayout.TextField(inputValue,GUILayout.MaxWidth(windowRect.width),GUILayout.MaxHeight(50f));
        /*agregar:
         * texto                    LISTO
         * output                   ---
         * boton continue           ---
         */
    }
}
