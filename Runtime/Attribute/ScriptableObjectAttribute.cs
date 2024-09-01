using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectAttribute : PropertyAttribute
{
    public string label;

    public ScriptableObjectAttribute(string label = "")
    {
        this.label = label;
    }
}
