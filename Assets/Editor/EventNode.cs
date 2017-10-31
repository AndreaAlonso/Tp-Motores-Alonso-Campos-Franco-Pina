using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventNode : BaseNode{
    public override void DrawCurves()
    {
    }

    /* Este nodo va contener un texto explicando el resultado de una elección previa
     * además va a ser el encargado de modificar algun que otro valor en el player
     * ya sea Oro,Exp,Vida,Quests,etc
     */
    public string text = "";

    private EventReward reward;

    private int gold;

    private int healthPoints;

    private string itemId;

    public enum EventReward {
        gold,
        item,
        life
    }
    public EventNode()
    {
        windowTittle = "Event";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        text = EditorGUILayout.TextField("dialogue", text);
        reward = (EventReward)EditorGUILayout.EnumPopup("Reward Type: ", reward);

        if (reward == EventReward.gold)
        {
            EditorGUILayout.LabelField("Gold Amount:");
            gold = EditorGUILayout.IntField(gold);
        }
        else if (reward == EventReward.item)
        {
            EditorGUILayout.LabelField("Item ID:");
            itemId = EditorGUILayout.TextField(itemId);
        }
        else if (reward == EventReward.life)
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
}
