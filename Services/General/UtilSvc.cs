using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public static class UtilSvc {
    public static void SetFlowCanvasBlackBoardValue<T>(Blackboard blackboard, string name, T value) {
        blackboard.SetVariableValue(name, value);
    }

    public static T GetFlowCanvasBlackBoardValue<T>(Blackboard blackboard, string name) {
        return blackboard.GetVariableValue<T>(name);
    }

    public static string JoinString(string a, string b) {
        return a + b;
    }
}