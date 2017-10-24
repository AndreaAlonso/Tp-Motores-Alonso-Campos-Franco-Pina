using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour {

    public Dialogue[] npcDialogues;
    public string[] playerAnswers= new string[] { "Si", "No", "Tal Vez", "(Fin de la conversación.)"};
    private int _dialogueCount;
    public Text dialogueBox;
    private bool _beingQuestioned;
    private bool _conversationEnded;


    private void Start()
    {
        ShowDialogue();
    }

    private void Update()
    {
        if (_conversationEnded)
            return;

        if (_beingQuestioned)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ShowNpcAnswer(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ShowNpcAnswer(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ShowNpcAnswer(2);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
            ShowDialogue();

    }

    public void ShowDialogue()
    {
        _beingQuestioned = true;
        dialogueBox.text = npcDialogues[_dialogueCount].dialogueText+ "\n";
        ShowPlayerAnswer();
    }

    public void ShowPlayerAnswer(int end =0)
    {
        if(end==0)
            for (int i = 0; i < npcDialogues[_dialogueCount].numerOfPosibleAnswers; i++)
            {
                dialogueBox.text += "\n"+ playerAnswers[i];
            }
        else dialogueBox.text+= "\n" + "\n" + playerAnswers[3];
    }

    public void ShowNpcAnswer(int answerNumber)
    {
        _beingQuestioned = false;
        dialogueBox.text = npcDialogues[_dialogueCount].answers[answerNumber].dialogueText;
        if (npcDialogues[_dialogueCount].answers[answerNumber].finishesConversation)
        {
            _conversationEnded = true;
            ShowPlayerAnswer(-1);
        }
        _dialogueCount++;
    }

 
}
