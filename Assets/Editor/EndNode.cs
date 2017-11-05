using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndNode : BaseNode{

    private BaseInputNode inputNode;
    private Rect inputNodeRect;

   
    /*Este nodo solo se encarga de finalizar el dialogo
     *puede llevar texto, pero no da opciones, solo "continue" o "fin"
     *aunque puede que ni se visualize
     */
     public string finished = "ended";
    public EndNode()
    {
        windowTittle = "End";
      
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        Event e = Event.current;

        if (e.type == EventType.Repaint)
        {
            inputNodeRect = GUILayoutUtility.GetLastRect();
        }

        
    }

    public override void DrawCurves()
    {
        if (inputNode)
        {
            Rect rect = windowRect;
            rect.x += inputNodeRect.x;
            rect.y += inputNodeRect.y;

            rect.width = 1;
            rect.height = 1;

            NodeEditor.DrawNodeCurve(inputNode.windowRect, rect,Color.red);
        }
    }
    public override void NodeDeleted(BaseNode node)
    {
        if (node.Equals(inputNode))
            inputNode = null;
    }

    public override BaseInputNode ClickedOnInput(Vector2 pos)
    {
        BaseInputNode retVal = null;

        pos.x = windowRect.x;
        pos.y = windowRect.y;

        if (inputNodeRect.Contains(pos))
        {
            retVal = inputNode;
            inputNode = null;
        }
        return retVal;
    }
    public override void SetInput(BaseInputNode input, Vector2 clickpos)
    {
        clickpos.x -= windowRect.x;
        clickpos.y -= windowRect.y;

        if (inputNodeRect.Contains(clickpos))
        {
            inputNode = input;
        }
    }
}
