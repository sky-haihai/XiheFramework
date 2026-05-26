using System.Linq;
using TMPro;
using UnityEngine.UI;
using XiheFramework.Runtime.UI.UIEntity;

namespace XiheFramework.Runtime.Console.UI {
    public class ConsoleUIOverlay : UIOverlayEntity {
        public Button helpButton;
        public Button clearButton;
        public Button submitButton;
        public TMP_InputField inputField;
        public TMP_Text outputField;

        protected override void OnInitCallback() {
            base.OnInitCallback();

            helpButton.onClick.AddListener(() => { Game.Console.ExecuteCommand("help", out var message); });
            clearButton.onClick.AddListener(() => { outputField.text = ""; });
            submitButton.onClick.AddListener(() => { Game.Console.ExecuteCommand(inputField.text, out var message); });
            Game.Event.Subscribe(ConsoleModuleEvents.OnLogMessageEventName, OnLogMessage);
        }

        private void OnLogMessage(object sender, object e) {
            outputField.text = Game.Console.Logs.Aggregate((a, b) => a + "\n" + b);
        }
    }
}