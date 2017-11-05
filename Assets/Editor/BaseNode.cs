using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseNode : ScriptableObject {

    public Rect windowRect;

    public bool hasInput;

    public string windowTittle = "";

    public virtual void DrawWindow()
    {
        windowTittle = EditorGUILayout.TextField("Tittle",windowTittle);
    }
    public abstract void DrawCurves();

    public virtual void SetInput(BaseInputNode input, Vector2 clickpos)
    {

    }
    public virtual void NodeDeleted(BaseNode node)
    {

    }
    public virtual BaseInputNode ClickedOnInput(Vector2 pos)
    {
        return null;
    }
    
}
