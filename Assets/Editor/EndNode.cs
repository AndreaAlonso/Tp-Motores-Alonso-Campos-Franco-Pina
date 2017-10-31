using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndNode : BaseNode{
    public override void DrawCurves()
    {
    }
    /*Este nodo solo se encarga de finalizar el dialogo
     *puede llevar texto, pero no da opciones, solo "continue" o "fin"
     *aunque puede que ni se visualize
     */
    public EndNode()
    {
        windowTittle = "End";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
    }
}
