using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : ScriptableObject {

    public string dialogueText;
    public bool finishesConversation;
    public Dialogue nextQuestion;
}
