using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueNode : BaseInputNode
{

    private BaseInputNode input1;
    private Rect input1Rect;


    /* Este es el nodo comun, solo lleva texto y boton de "continue"
     */

    public string text = "";

    public DialogueNode()
    {
        windowTittle = "Text";
    }

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
        EditorGUILayout.LabelField("Dialogue:");
        text = EditorGUILayout.TextField(text,GUILayout.Height(50));
        //inputValue = EditorGUILayout.TextField(inputValue,GUILayout.MaxWidth(windowRect.width),GUILayout.MaxHeight(50f));
        /*agregar:
         * texto                    LISTO
         * output                   ---
         * boton continue           ---
         */
    }
    public override void DrawCurves()
    {
        if (input1)
        {

        Rect rect = windowRect;
        rect.x += input1Rect.x;
        rect.y += input1Rect.y;
        rect.width = 1;
        rect.height = 1;

        NodeEditor.DrawNodeCurve(input1.windowRect, rect, Color.blue);
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
