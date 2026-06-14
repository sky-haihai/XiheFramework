namespace XiheFramework.Runtime.FSM {
    public  static class FsmEvents {
        public const string OnStateEnterEventName = "OnStateEnter";
        public const string OnStateExitEventName = "OnStateExit";
        
        public struct OnStateEnteredEventArgs {
            public string fsmName;
            public string enteredStateName;

            public OnStateEnteredEventArgs(string fsmName, string enteredStateName) {
                this.fsmName = fsmName;
                this.enteredStateName = enteredStateName;
            }
        }
        
        public struct OnStateExitedEventArgs {
            public string fsmName;
            public string exitedStateName;

            public OnStateExitedEventArgs(string fsmName, string exitedStateName) {
                this.fsmName = fsmName;
                this.exitedStateName = exitedStateName;
            }
        }
    }
}