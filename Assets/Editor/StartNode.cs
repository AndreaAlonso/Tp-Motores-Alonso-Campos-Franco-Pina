using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StartNode : BaseNode {
    public override void DrawCurves()
    {
    }
    /*Este nodo esta para indicar el inicio del dialogo, puede que ni se visualize
     * pero podria tener texto de introducción o descriptivo
     */
    public StartNode() {
        windowTittle = "Start";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
    }
}
