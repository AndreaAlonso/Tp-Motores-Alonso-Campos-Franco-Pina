using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class NodeEditor : EditorWindow
{

    private List<BaseNode> windows = new List<BaseNode>();

    private Vector2 mousePos;

    private BaseNode selectedNode;
    private StartNode startNode;

    private bool makeTransitionMode = false;

    private Vector2 offset;
    private Vector2 drag;

    private bool _startEnd;

    //[SerializeField]
    public List<BaseScriptableObject> ScriptToDelete = new List<BaseScriptableObject>();

    [MenuItem("Personalizado/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    void OnGUI()
    {
        DrawButtons();
        DrawGrid(20, 0.2f, Color.black);
        DrawGrid(100, 0.6f, Color.black);

        NodeStartEnd();

        MouseActions();

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

        DropAreaGUI();
    }


    private void SaveConversation()
    {
        BaseScriptableObject lastNode = null;

        for (int i = 0; i < windows.Count; i++)

        {
            if (windows[i] is StartNode)
            {
                StartNode nodeNew = (StartNode)windows[i];
                Dialogue start = ScriptableObjectUtility.CreateAsset<Dialogue>();
                //start.name = "Start";
                start.start = true;
                start.windowPos = nodeNew.windowRect;
                lastNode = start;
                ScriptToDelete.Add((BaseScriptableObject)start);

            }
            if (windows[i] is DialogueNode)
            {
                DialogueNode nodeNew = (DialogueNode)windows[i];
                Dialogue npcDialogue = ScriptableObjectUtility.CreateAsset<Dialogue>();
                npcDialogue.dialogue = nodeNew.text;
                npcDialogue.windowPos = nodeNew.windowRect;
                if (lastNode != null)
                    lastNode.next = npcDialogue;
                lastNode = npcDialogue;
                ScriptToDelete.Add((BaseScriptableObject)npcDialogue);

            }
            if (windows[i] is EventNode)
            {
                EventNode nodeNew = (EventNode)windows[i];
                EventDialogue npcDialogue = ScriptableObjectUtility.CreateAsset<EventDialogue>();
                npcDialogue.dialogue = nodeNew.text;
                npcDialogue.rewardToRecieve = nodeNew.reward;
                if (nodeNew.healthPoints != 0)
                    npcDialogue.goldOrHp = nodeNew.healthPoints;
                else if (nodeNew.gold != 0)
                    npcDialogue.goldOrHp = nodeNew.gold;
                else npcDialogue.item = nodeNew.itemId;

                npcDialogue.windowPos = nodeNew.windowRect;

                if (lastNode != null)
                    lastNode.next = npcDialogue;
                lastNode = npcDialogue;

                ScriptToDelete.Add((BaseScriptableObject)npcDialogue);

            }
            if (windows[i] is EndNode)
            {
                EndNode endNode = (EndNode)windows[i];
                BaseScriptableObject end = ScriptableObjectUtility.CreateAsset<BaseScriptableObject>();
                //end.name = "End";
                end.dialogue = "Finished";
                end.windowPos = endNode.windowRect;
                lastNode.next = end;
                //lastNode = end;
                ScriptToDelete.Add((BaseScriptableObject)end);
            }
            if (windows[i] is DiceRollNode)
            {
                DiceRollNode nodeNew = (DiceRollNode)windows[i];
                DiceDialogue npcDialogue = ScriptableObjectUtility.CreateAsset<DiceDialogue>();
                npcDialogue.roll = nodeNew.rollType;
                npcDialogue.difficulty = nodeNew.difficulty;
                npcDialogue.windowPos = nodeNew.windowRect;
                ScriptToDelete.Add((BaseScriptableObject)npcDialogue);
                if (lastNode != null)
                    lastNode.next = npcDialogue;
                i++;
                EndNode endNode = (EndNode)windows[i];
                BaseScriptableObject end = ScriptableObjectUtility.CreateAsset<BaseScriptableObject>();
                //end.name = "End";
                end.dialogue = endNode.finished;
                end.windowPos = endNode.windowRect;
                npcDialogue.fail = end;
                lastNode = npcDialogue;
                ScriptToDelete.Add((BaseScriptableObject)end);
            }
        }
    }

    public void ReloadConversation(Object dragged)
    {
      /*  BaseNode selNode = windows[0];
        windows.RemoveAt(0);

        foreach (BaseNode n in windows)
        {
            n.NodeDeleted(selNode);
        }*/

        BaseScriptableObject dialogue = (BaseScriptableObject)dragged;
        BaseInputNode lastDiag;

        Dialogue start = (Dialogue)dialogue;
        StartNode startNode = (StartNode)windows[0];
        startNode.windowRect = start.windowPos;
        lastDiag = startNode;
        BaseScriptableObject lastDiagScriptObj=start;


        while (dialogue.next != null)
        {
            ScriptToDelete.Add(dialogue);

            if (dialogue is Dialogue)
            {
                Dialogue npcDialogue = (Dialogue)dialogue;

                if (!npcDialogue.start)
                {
                    DialogueNode newNode = new DialogueNode();
                    newNode.windowRect = npcDialogue.windowPos;
                    newNode.text = npcDialogue.dialogue;
                    newNode.hasInput = true;
                    newNode.input1 = lastDiag;
                    newNode.input1Rect = lastDiag.windowRect;
                    lastDiag = newNode;
                    windows.Add(newNode);
                }
            
               // dialogue = dialogue.next;
            }
            else if (dialogue is EventDialogue)
            {
                EventDialogue npcDialogue = (EventDialogue)dialogue;
                EventNode newNode = new EventNode();
                newNode.windowRect = npcDialogue.windowPos;
                newNode.text = npcDialogue.dialogue;
                newNode.reward = npcDialogue.rewardToRecieve;
                if (newNode.reward == DialogueEventType.EventReward.gold)
                    newNode.gold = npcDialogue.goldOrHp;
                else if (newNode.reward == DialogueEventType.EventReward.life)
                    newNode.healthPoints = npcDialogue.goldOrHp;
                else
                    newNode.itemId = npcDialogue.item;
                
                newNode.hasInput = true;
                newNode.input1 = lastDiag;
                newNode.input1Rect = lastDiag.windowRect;
                lastDiag = newNode;
                windows.Add(newNode);
            }
            else if(dialogue is DiceDialogue)
            {
                DiceDialogue diceRoll = (DiceDialogue)dialogue;
                DiceRollNode newNode = new DiceRollNode();
                newNode.windowRect = diceRoll.windowPos;
                newNode.rollType = diceRoll.roll;
                newNode.difficulty = diceRoll.difficulty;
                newNode.hasInput = true;
                newNode.input1 = lastDiag;
                newNode.input1Rect = lastDiag.windowRect;
                //BaseScriptableObject endObj = diceRoll.fail;
                EndNode newEnd = new EndNode();
                newEnd.windowRect = diceRoll.fail.windowPos;
                newEnd.finished = diceRoll.fail.dialogue;
                newEnd.hasInput = true;
                newEnd.inputNode = newNode;
                newEnd.inputNodeRect = newNode.windowRect;
                ScriptToDelete.Add(diceRoll.fail);
                lastDiag = newNode;
                windows.Add(newNode);
                windows.Add(newEnd);
            }
            
            lastDiagScriptObj = dialogue;
            Debug.Log("asdasd");
            dialogue = dialogue.next;
        }
        EndNode lastEnd = new EndNode();
        lastEnd.windowRect = lastDiagScriptObj.next.windowPos;
        lastEnd.finished = lastDiagScriptObj.next.dialogue;
        lastEnd.hasInput = true;
        lastEnd.inputNode = lastDiag;
        lastEnd.inputNodeRect = lastDiag.windowRect;
        ScriptToDelete.Add(lastDiagScriptObj.next);
        windows.Add(lastEnd);
        Debug.Log("ultimo alcanzado");
        Repaint();
    }

    void EraseScriptObj()
    {
        foreach (var _target in ScriptToDelete)
        {
            Debug.Log(_target.name);
            //DestroyImmediate(_target, true);
            AssetDatabase.DeleteAsset("Assets/NewObjects/" + _target.name + ".asset");
            AssetDatabase.DeleteAsset("Assets/NewObjects/" + _target.ToString() + ".asset");
        }
        ScriptToDelete.Clear();
       
    }

    //Dibuja los botones propios de la ventana del editor
    private void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Start"))
        {
            foreach (var item in windows)
            {
                //TIRA ERROR
                //if (item.GetType().Equals(typeof(StartNode))) {
                //    return;
                //}
                if (item.windowTittle == "Start")
                {
                    return;
                }
                StartNode startNode = new StartNode();
                startNode.windowRect = new Rect(50, 100, 200, 50);
                windows.Add(startNode);
            }
        }
        if (GUILayout.Button("Add Dialogue"))
        {
            DialogueNode dialogueNode = new DialogueNode();
            dialogueNode.windowRect = new Rect(200, 100, 200, 150);
            windows.Add(dialogueNode);
        }
        if (GUILayout.Button("Add Choice"))
        {
            ChoiceNode choiceNode = new ChoiceNode();
            choiceNode.windowRect = new Rect(400, 100, 200, 150);
            windows.Add(choiceNode);
        }
        if (GUILayout.Button("Add Roll"))
        {
            DiceRollNode diceRollNode = new DiceRollNode();
            diceRollNode.windowRect = new Rect(600, 100, 200, 150);
            windows.Add(diceRollNode);
        }
        if (GUILayout.Button("Add Event"))
        {
            EventNode eventNode = new EventNode();
            eventNode.windowRect = new Rect(800, 100, 200, 150);
            windows.Add(eventNode);
        }
        if (GUILayout.Button("Add End"))
        {
            EndNode endNode = new EndNode();
            endNode.windowRect = new Rect(1000, 100, 200, 50);
            windows.Add(endNode);
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Generate Conversation"))
        {
            if (ScriptToDelete.Count > 0)
                EraseScriptObj();
            SaveConversation();
        }
    }

    

    //Dibuja la grilla de fondo
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

    //Chequea los clics del mouse
    void MouseActions()
    {
        Event e = Event.current;
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
                    ContextMenu();
                    e.Use();
                }
                else
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Make Transition"), false, ContextCallBack, "makeTransition");
                    menu.AddSeparator("");
                    if (!windows[0].windowRect.Contains(mousePos))
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
        else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode)
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

            DrawNodeCurve(selectedNode.windowRect, mouseRect, Color.black);
            Repaint();
        }
    }

    //
    void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }

    //Menu contextual con las opciones (al hacer clic derecho)
    void ContextMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Start Node"), false, ContextCallBack, "startNode");
        menu.AddItem(new GUIContent("Add Dialogue Node"), false, ContextCallBack, "dialogueNode");
        menu.AddItem(new GUIContent("Add Choice Node"), false, ContextCallBack, "choiceNode");
        menu.AddItem(new GUIContent("Add Event Node"), false, ContextCallBack, "eventNode");
        menu.AddItem(new GUIContent("Add DiceRoll Node"), false, ContextCallBack, "diceRollNode");
        menu.AddItem(new GUIContent("Add End Node"), false, ContextCallBack, "endNode");

        menu.ShowAsContext();
    }

    //Contiene las funciones para las opciones del menu contextual
    void ContextCallBack(object obj)
    {
        string clb = obj.ToString();
        if (clb.Equals("startNode"))
        {
            foreach (var item in windows)
            {
                //TIRA ERROR
                //if (item.GetType().Equals(typeof(StartNode))) {
                //    return;
                //}
                if (item.windowTittle == "Start")
                {
                    return;
                }
                StartNode startNode = new StartNode();
                startNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 50);
                windows.Add(startNode);
            }
        }
        if (clb.Equals("dialogueNode"))
        {
            DialogueNode dialogueNode = new DialogueNode();
            dialogueNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(dialogueNode);
        }
        if (clb.Equals("choiceNode"))
        {
            ChoiceNode choiceNode = new ChoiceNode();
            choiceNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(choiceNode);
        }
        if (clb.Equals("eventNode"))
        {
            EventNode eventNode = new EventNode();
            eventNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(eventNode);
        }
        if (clb.Equals("diceRollNode"))
        {
            DiceRollNode diceRollNode = new DiceRollNode();
            diceRollNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);
            windows.Add(diceRollNode);
        }
        if (clb.Equals("endNode"))
        {
            EndNode endNode = new EndNode();
            endNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 50);
            windows.Add(endNode);
        }
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

    //Dibuja los nodos Start y End al abrir el editor
    public void NodeStartEnd()
    {
        if (!_startEnd)
        {
            //windows.Clear();
            StartNode startNode = new StartNode();
            startNode.windowRect = new Rect(0, 50, 200, 50);
            windows.Add(startNode);

           /* EndNode endNode = new EndNode();
            endNode.windowRect = new Rect(0, 150, 200, 50);
            windows.Add(endNode);*/

            _startEnd = true;
        }
    }

    //Dibuja las conexiones entre nodos
    public static void DrawNodeCurve(Rect start, Rect end, Color _color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;



        for (int i = 0; i < 3; i++)
        {
            Handles.DrawBezier(startPos, endPos, startTan, endTan, _color, null, (i + 1) * 2);
        }
        Handles.DrawBezier(startPos, endPos, startTan, endTan, _color, null, 1);
    }

    public void DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 0.0f, GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
       // GUI.Box(drop_area, "Add Trigger");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    foreach (Object dragged_object in DragAndDrop.objectReferences)
                    {
                        if(dragged_object is Dialogue)
                        {
                            Dialogue newDiag = (Dialogue)dragged_object;
                            if (newDiag.start)
                                ReloadConversation(dragged_object);
                        }
                            
                    }
                }
                break;
        }
    }


    
}
