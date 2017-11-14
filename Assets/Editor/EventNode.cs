using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventNode : BaseInputNode
{
    public BaseInputNode input1;
    public Rect input1Rect;


    /* Este nodo va contener un texto explicando el resultado de una elección previa
     * además va a ser el encargado de modificar algun que otro valor en el player
     * ya sea Oro,Exp,Vida,Quests,etc
     */
    public string text = "";

    public DialogueEventType.EventReward reward;

    public int gold;

    public int healthPoints;

    public string itemId;

  /*  public enum EventReward {
        gold,
        item,
        life
    }*/
    public EventNode()
    {
        windowTittle = "Event";
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

        text = EditorGUILayout.TextField("dialogue", text);
        reward = (DialogueEventType.EventReward)EditorGUILayout.EnumPopup("Reward Type: ", reward);

        if (reward == DialogueEventType.EventReward.gold)
        {
            EditorGUILayout.LabelField("Gold Amount:");
            gold = EditorGUILayout.IntField(gold);
        }
        else if (reward == DialogueEventType.EventReward.item)
        {
            EditorGUILayout.LabelField("Item ID:");
            itemId = EditorGUILayout.TextField(itemId);
        }
        else if (reward == DialogueEventType.EventReward.life)
        {
            EditorGUILayout.LabelField("HP Amount:");
            healthPoints = EditorGUILayout.IntField(healthPoints);
        }

        /*agregar:
         * Texto                                LISTO
         * Boton Continue                       ---
         * Lista de Recompensas/Castigos        LISTO
         * Input/Output                         ---
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

            NodeEditor.DrawNodeCurve(input1.windowRect, rect, Color.yellow);
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
