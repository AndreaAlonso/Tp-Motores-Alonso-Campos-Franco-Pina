using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator : MonoBehaviour {

    [MenuItem("Personalizado/ScriptableObjects/Preguntas")]
    public static void CreateQuestion()
    {
        ScriptableObjectUtility.CreateAsset<Dialogue>();
    }

    [MenuItem("Personalizado/ScriptableObjects/Opciones")]
    public static void CreateAnswer()
    {
        ScriptableObjectUtility.CreateAsset<Choice>();
    }

    [MenuItem("Personalizado/ScriptableObjects/Evento")]
    public static void CreateEventDialogue()
    {
        ScriptableObjectUtility.CreateAsset<EventDialogue>();
    }

    [MenuItem("Personalizado/ScriptableObjects/Roll")]
    public static void CreateRoll()
    {
        ScriptableObjectUtility.CreateAsset<DiceDialogue>();
    }
}
