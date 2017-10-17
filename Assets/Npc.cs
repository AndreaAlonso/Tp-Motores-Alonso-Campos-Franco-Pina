using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour {

    public Dialogue[] npcDialogues;
    public Dialogue[] playerAnswers;
    public Dialogue[] npcAnswers;

    public Dialogue GetDialogue(int dialogueNum)
    {
        return npcDialogues[dialogueNum];
    }

    public Dialogue GetPlayerAnswer(int dialogueNum)
    {
        return playerAnswers[dialogueNum];
    }

    public Dialogue GetNpcAnswer(int dialogueNum)
    {
        return npcAnswers[dialogueNum];
    }
}
