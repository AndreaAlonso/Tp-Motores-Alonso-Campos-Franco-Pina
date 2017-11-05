using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChoiceNode : BaseInputNode
{


    private BaseInputNode input1;
    private Rect input1Rect;

    /* Este nodo tiene texto, pero ademas da a elegir opciones de dialogo
     * cada opcion tiene un output a otro nodo
     */

    Vector2 scrollPos = Vector2.zero;

    public string dialogue = "";
    public int numOptions;
    public List<string> options = new List<string>();

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
        Event e = Event.current;
        string inputTitle = "None";
        if (input1)
        {
            inputTitle = input1.GetResult();
        }
        if (e.type == EventType.repaint)
        {
            input1Rect = GUILayoutUtility.GetLastRect();
        }
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
    public override void DrawCurves()
    {
        if(input1)
        {

        Rect rect = windowRect;
        rect.x += input1Rect.x;
        rect.y += input1Rect.y;
        rect.width = 1;
        rect.height = 1;

        NodeEditor.DrawNodeCurve(input1.windowRect, rect,Color.green);
        }
    }
    public override void SetInput(BaseInputNode input, Vector2 clickpos)
    {
        clickpos.x -= windowRect.x;
        clickpos.y -= windowRect.y;

        if (input1Rect.Contains(clickpos))
            input1 = input;

    }
    public override BaseInputNode ClickedOnInput(Vector2 pos)
    {
        BaseInputNode retVal = null;

        pos.x -= windowRect.x;
        pos.y -= windowRect.y;

        if (input1Rect.Contains(pos))
        {
            retVal = input1;
            input1 = null;
        }

        return retVal;
    }

    public override void NodeDeleted(BaseNode node)
    {
        if (node.Equals(input1))
            input1 = null;


    }
}
