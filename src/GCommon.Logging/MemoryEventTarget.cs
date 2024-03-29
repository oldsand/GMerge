﻿using System;
using NLog;
using NLog.Targets;

namespace GCommon.Logging
{
    public class MemoryEventTarget : Target
    {
        public MemoryEventTarget(string name)
        {
            Name = name;
        }
        
        public event Action<LogEventInfo> EventReceived;

        protected override void Write(LogEventInfo logEvent)
        {
            EventReceived?.Invoke(logEvent);
        }
    }
}