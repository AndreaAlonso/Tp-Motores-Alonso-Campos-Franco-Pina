using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class InputNode : BaseInputNode {

    private InputType inputType;

    public enum InputType
    {
        Number,
        Randomization
    }

    private string randomFrom = "";
    private string randomTo = "";

    private string inputValue = "";

    public InputNode()
    {
        windowTittle = "Input Node";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();

        inputType = (InputType)EditorGUILayout.EnumPopup("Input Type : ", inputType);

        if (inputType == InputType.Number)
            inputValue = EditorGUILayout.TextField("Value", inputValue);
        else if (inputType == InputType.Randomization)
        {
            randomFrom = EditorGUILayout.TextField("From", randomFrom);
            randomTo = EditorGUILayout.TextField("To", randomTo);

            if (GUILayout.Button("Calculate Random"))
                CalculateRandom();
        }
    }
    public override void DrawCurves()
    { } 

    private void CalculateRandom()
    {
        float rFrom = 0;
        float rTo = 0;

        float.TryParse(randomFrom, out rFrom);
        float.TryParse(randomTo, out rTo);

        int randFrom = (int)(rFrom * 10);
        int randTo = (int)(rTo * 10);

        int selected = Random.Range(randFrom, randTo + 1);

        float selectedValue = selected / 10;

        inputValue = selectedValue.ToString();
    }
    public override string GetResult()
    {
        return inputValue.ToString();
    }  
}
