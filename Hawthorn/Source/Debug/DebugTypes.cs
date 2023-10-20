namespace Hawthorn;

#if DEBUG
public enum DebugLogLevel
{
	Message,
	Warning,
	Error
}

[Flags]
public enum DebugFlags
{
	None,
	Failures,
	Busy,
	Successess
}

public delegate void DebugLogEvent(DebugLogLevel level, string message);

#endif
