using System;
using UnityEngine;
using XiheFramework.Runtime;

namespace XiheFramework.Utility.GlobalDebug {
    public class DebugInfo : MonoBehaviour {
#if UNITY_EDITOR
        public string gameLoopFsmName = "GameLoop";

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        private void OnGUI() {
            GUI.color = Color.white;
            //default font size
            GUI.skin.label.fontSize = 20;
            //debug text for fps
            var text = "";
            if (Time.unscaledDeltaTime == 0f) {
                text += "FPS:0.00";
            }
            else {
                text += $"FPS:{(1f / Time.unscaledDeltaTime):0.0}";
            }

            text += "|";
            //debug for current state
            string state = "Unknown";
            if (Game.Fsm) {
                state = Game.Fsm.GetCurrentState(gameLoopFsmName);
            }

            if (String.IsNullOrEmpty(state)) {
                state = "Unknown";
            }

            text += state;

            text += "|";
            text += $"{Screen.width}x{Screen.height}";
            GUI.Label(new Rect(10, 10, 1000, 40), text);
        }
#endif
    }
}