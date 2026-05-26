namespace XiheFramework.Runtime.Console {
    public static class ConsoleModuleEvents {
        public const string OnLogMessageEventName = "ConsoleModule.OnLogMessage";

        public struct OnLogMessageArgs {
            public string message;
            public CommandExecutionResult resultType;

            public OnLogMessageArgs(string message, CommandExecutionResult resultType) {
                this.message = message;
                this.resultType = resultType;
            }
        }
    }
}