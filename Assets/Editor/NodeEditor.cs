using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeEditor : EditorWindow {

    private List<BaseNode> windows = new List<BaseNode>();

    private Vector2 mousePos;

    private BaseNode selectedNode;

    private bool makeTransitionMode = false;

    private Vector2 offset;
    private Vector2 drag;

    [MenuItem("Personalizado/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

     void OnGUI()
    {
        Event e = Event.current;

        DrawGrid(20, 0.2f, Color.black);
        DrawGrid(100, 0.6f, Color.black);

        mousePos = e.mousePosition;
        if (e.button == 1 && !makeTransitionMode)
        {
            if (e.type == EventType.MouseDown)
            {
                bool clickedOnWindow = false;
                int selectIndex = -1;

                for (int i = 0; i < windows.Count; i++)
                {

                    if (windows[i].windowRect.Contains(mousePos))
                    {
                        selectIndex = i;
                        clickedOnWindow = true;
                        break;
                    }

                }
                if (!clickedOnWindow)
                {
                    GenericMenu menu = new GenericMenu();

                    //menu.AddItem(new GUIContent("Add Input Node"), false, ContextCallBack, "inputNode");
                    //menu.AddItem(new GUIContent("Add Output Node"), false, ContextCallBack, "outputNode");
                    //menu.AddItem(new GUIContent("Add Calculation Node"), false, ContextCallBack, "calcNode");
                    //menu.AddItem(new GUIContent("Add Comparison Node"), false, ContextCallBack, "compNode");

                    menu.AddItem(new GUIContent("Add Start Node"), false, ContextCallBack, "startNode");
                    menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallBack, "dialogueNode");
                    menu.AddItem(new GUIContent("Add Choice Node"), false, ContextCallBack, "choiceNode");
                    menu.AddItem(new GUIContent("Add Event Node"), false, ContextCallBack, "eventNode");
                    menu.AddItem(new GUIContent("Add DiceRoll Node"), false, ContextCallBack, "diceRollNode");
                    menu.AddItem(new GUIContent("Add End Node"), false, ContextCallBack, "endNode");

                    menu.ShowAsContext();
                    e.Use();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallBack, "makeTransition");
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete Node"), false, ContextCallBack, "deleteNode");

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode)
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {

                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }

            }
            if (clickedOnWindow && !windows[selectIndex].Equals(selectedNode))
            {
                windows[selectIndex].SetInput((BaseInputNode)selectedNode, mousePos);
                makeTransitionMode = false;
                selectedNode = null;
            }
            if (!clickedOnWindow)
            {
                makeTransitionMode = false;
                selectedNode = null;
            }

            e.Use();
        }
        else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode )
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {

                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }

            }
            if (clickedOnWindow)
            {
                BaseInputNode nodeToChange = windows[selectIndex].ClickedOnInput(mousePos);

                if (nodeToChange != null)
                {
                    selectedNode = nodeToChange;
                    makeTransitionMode = true;
                }
            }
        }
        if (makeTransitionMode && selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(selectedNode.windowRect, mouseRect);
            Repaint();
        }
        foreach (BaseNode n in windows)
        {
            n.DrawCurves();
        }

        BeginWindows();

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].windowRect = GUI.Window(i, windows[i].windowRect, DrawNodeWindow, windows[i].windowTittle);
        }
        EndWindows();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
    void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    void ContextCallBack(object obj)
    {
        string clb = obj.ToString();
        if (clb.Equals("startNode")) {
            StartNode startNode = new StartNode();
            startNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 50);
            windows.Add(startNode);
        }
        if (clb.Equals("dialogueNode")){
            DialogueNode dialogueNode = new DialogueNode();
            dialogueNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(dialogueNode);
        }
        if (clb.Equals("choiceNode")){
            ChoiceNode choiceNode = new ChoiceNode();
            choiceNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(choiceNode);
        }
        if (clb.Equals("eventNode")){
            EventNode eventNode = new EventNode();
            eventNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(eventNode);
        }
        if (clb.Equals("diceRollNode")){
            DiceRollNode diceRollNode = new DiceRollNode();
            diceRollNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(diceRollNode);
        }
        if (clb.Equals("endNode")){
            EndNode endNode = new EndNode();
            endNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 50);
            windows.Add(endNode);
        }
        /*
        if (clb.Equals("inputNode"))
        {
            InputNode inputNode = new InputNode();
            inputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            windows.Add(inputNode);
        }
        else if (clb.Equals("outputNode"))
        {
            OutPutNode outputNode = new OutPutNode();
            outputNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(outputNode);
        }
        else if (clb.Equals("calcNode"))
        {
            CalcNode calcNode = new CalcNode();
            calcNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(calcNode);
        }
        else if (clb.Equals("compNode"))
        {
            ComparisonNode compNode = new ComparisonNode();
            compNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 100);

            windows.Add(compNode);
        }
        */
        else if (clb.Equals("makeTransition"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {

                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }

            }
            if (clickedOnWindow)
            {
                selectedNode = windows[selectIndex];
                makeTransitionMode = true;
            }
        }
        else if (clb.Equals("deleteNode"))
        {
            bool clickedOnWindow = false;
            int selectIndex = -1;

            for (int i = 0; i < windows.Count; i++)
            {

                if (windows[i].windowRect.Contains(mousePos))
                {
                    selectIndex = i;
                    clickedOnWindow = true;
                    break;
                }

            }
            if (clickedOnWindow)
            {
                BaseNode selNode = windows[selectIndex];
                windows.RemoveAt(selectIndex);

                foreach (BaseNode n in windows)
                {
                    n.NodeDeleted(selNode);
                }
            }
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x +start.width/2, start.y +start.height/2,0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right*50;
        Vector3 endTan = endPos + Vector3.left * 50;

        Color shadowCol = new Color(0, 0, 0, 0.6f);

        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 2);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
