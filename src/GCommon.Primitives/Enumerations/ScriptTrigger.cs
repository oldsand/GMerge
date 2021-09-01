using Ardalis.SmartEnum;

namespace GCommon.Primitives.Enumerations
{
    public class ScriptTrigger : SmartEnum<ScriptTrigger>
    {
        private ScriptTrigger(string name, int value) : base(name, value)
        {
        }
        
        public static readonly ScriptTrigger WhileTrue = new ScriptTrigger("WhileTrue", 0);
        public static readonly ScriptTrigger WhileFalse = new ScriptTrigger("WhileFalse", 1);
        public static readonly ScriptTrigger OnTrue = new ScriptTrigger("OnTrue", 2);
        public static readonly ScriptTrigger OnFalse = new ScriptTrigger("OnFalse", 3);
        public static readonly ScriptTrigger ValueChange = new ScriptTrigger("ValueChange", 4);
    }
}