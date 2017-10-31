using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DiceRollNode : BaseNode{
    public override void DrawCurves()
    {
    }

    //Elijo que tipo de tirada tiene que hacer el personaje
    //Fuerza/Inteligencia/Destreza/etc
    //Seteo la dificultad de la tirada

    //Opcion 1: hacer que cada tirada sea un nodo por separado
    //          y que este nodo no tenga texto, solo sea un calculo
    //          y transicion al siguiente nodo
    //Opcion 2: hacer todas las tiradas en un solo nodo
    //          haciendo un hibrido entre Choices y DiceRoll

    private RollType rollType;
    private string challenge = "";
    private int difficulty;

    public enum RollType {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Charisma
    }

    public DiceRollNode()
    {
        windowTittle = "Dice Roll";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        rollType = (RollType)EditorGUILayout.EnumPopup("Roll Stat: ", rollType);

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
}
