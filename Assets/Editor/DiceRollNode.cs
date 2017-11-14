using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DiceRollNode : BaseInputNode
{

    public BaseInputNode input1;
    public Rect input1Rect;


    //Elijo que tipo de tirada tiene que hacer el personaje
    //Fuerza/Inteligencia/Destreza/etc
    //Seteo la dificultad de la tirada

    //Opcion 1: hacer que cada tirada sea un nodo por separado
    //          y que este nodo no tenga texto, solo sea un calculo
    //          y transicion al siguiente nodo
    //Opcion 2: hacer todas las tiradas en un solo nodo
    //          haciendo un hibrido entre Choices y DiceRoll

    public DialogueRollType.RollType rollType;
    public string challenge = "";
    public int difficulty;

   /* public enum RollType {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }*/

    public DiceRollNode()
    {
        windowTittle = "Dice Roll";
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

        rollType = (DialogueRollType.RollType)EditorGUILayout.EnumPopup("Roll Stat: ", rollType);

        EditorGUILayout.LabelField("Select Difficulty: " + challenge);
        difficulty = EditorGUILayout.IntSlider(difficulty,0,25);
        if (difficulty >= 0 && difficulty <= 5)
        {
            challenge = "Very Easy";
        }
        else if (difficulty > 5 && difficulty <= 10)
        {
            challenge = "Easy";
        }
        else if (difficulty > 10 && difficulty <= 15)
        {
            challenge = "Moderate";
        }
        else if (difficulty > 15 && difficulty <= 20)
        {
            challenge = "Hard";
        }
        else if (difficulty > 20 && difficulty <= 25)
        {
            challenge = "Very Hard";
        }
        //EditorGUILayout.LabelField("challenge: " + challenge);
        /* agregar:
         * selector de tirada (Stat)                            LISTO
         * selector de dificultad (valor)                       LISTO
         * calcular tirada (random 1d20 + playerStat VS DC)     ---
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

            NodeEditor.DrawNodeCurve(input1.windowRect, rect, Color.magenta);
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
