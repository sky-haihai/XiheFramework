namespace XiheFramework.Runtime.Blackboard {
    public static class BlackboardModuleEvents {
        public const string OnBlackboardCreatedEventName = "Blackboard.OnCreated";
        public const string OnBlackboardDataChangeEventName = "Blackboard.OnDataChange";
        public const string OnBlackboardReleasedEventName = "Blackboard.OnReleased";

        public struct OnBlackboardCreatedEventArgs {
            public string blackboardName;
        }

        public struct OnBlackboardDataChangeEventArgs {
            public string blackboardName;
            public string dataName;
            public object oldValue;
            public object newValue;
        }
        
        public struct OnBlackboardReleasedEventArgs {
            public string blackboardName;
        }
    }
}