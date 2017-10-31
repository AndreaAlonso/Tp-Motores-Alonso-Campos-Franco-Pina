using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInputNode : BaseNode {

    public virtual string GetResult()
    {
        return "none";
    }
    public override void DrawCurves()
    {
        
    }
}
