using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OutPutNode : BaseNode {

    private string result = "";

    private BaseInputNode inputNode;
    private Rect inputNodeRect;

    public OutPutNode()
    {
        windowTittle = "OutPut Node";
        hasInput = true;
    }
    public override void DrawWindow()
    {
        base.DrawWindow();

        Event e = Event.current;

        string inputTitle = "None";
        if (inputNode)
        {
            inputTitle = inputNode.GetResult();
        }

        GUILayout.Label("Input l:" + inputTitle);

        if (e.type == EventType.Repaint)
        {
            inputNodeRect = GUILayoutUtility.GetLastRect();
        }

        GUILayout.Label("Result" + result);
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

            NodeEditor.DrawNodeCurve(inputNode.windowRect, rect);
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
