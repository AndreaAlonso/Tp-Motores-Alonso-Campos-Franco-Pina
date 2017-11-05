using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : ScriptableObject {

    public string dialogueText;

    public List<string> avaliableChoices;

    public List<BaseScriptableObject> nextOptions;
}
