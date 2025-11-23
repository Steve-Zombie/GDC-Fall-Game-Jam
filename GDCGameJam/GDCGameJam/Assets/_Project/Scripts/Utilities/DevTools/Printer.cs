using UnityEngine;

[CreateAssetMenu(fileName = "Printer", menuName = "Util/Printer")]
// [HideInStackTrace(true)]
public class Printer : ScriptableObject
{
    public enum PrintType
    {
        Log,
        Warning,
        Error
    }

    [Header("Global Printer Settings")]
    public bool enablePrinting = true;

    [Header("Prefix Settings")]
    public string printPrefix = "[PRINTER]";
    public Color prefixColor = Color.blue;

    private static Printer debug;
    public static Printer Debug => debug ??= Resources.Load<Printer>("Printers/Debug");
    // private static bool loggerInitialized = false;

    // private static void EnsureLoggerInitialized()
    // {
    //     if (loggerInitialized) return;

    //     Log.Logger = new Logger(new LoggerConfig()
    //         .MinimumLevel.Debug()
    //         .CaptureStacktrace(true)
    //         .OutputTemplate("{Message}")
    //         .WriteTo.File("Printer.log", minLevel: LogLevel.Verbose, formatter: LogFormatterJson.Formatter)
    //         .WriteTo.StdOut(/*outputTemplate: "{Message}"*/)
    //         .WriteTo.UnityDebugLog(/*outputTemplate: "{Message}"*/)); 

    //     loggerInitialized = true;
    // }
    [HideInCallstack]
    private string GetFormattedPrefix()
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(prefixColor)}>{printPrefix}</color>";
    }

    [HideInCallstack]
    public void Print(string message, PrintType type)
    {
        if (!enablePrinting) return;
        var fullMessage = $"{GetFormattedPrefix()} {message}";
        // EnsureLoggerInitialized();
        switch (type)
        {
            case PrintType.Warning:
                // Log.Warning(fullMessage);
                UnityEngine.Debug.LogWarning(fullMessage);
                break;
            case PrintType.Error:
                // Log.Error(fullMessage);
                UnityEngine.Debug.LogError(fullMessage);
                break;
            default:
                // Log.Info(fullMessage);
                UnityEngine.Debug.Log(fullMessage);
                break;
        }
    }
    [HideInCallstack]
    public void print(string message)
    {
        Print(message, PrintType.Log);
    }
    [HideInCallstack]
    public static void PrintLog(string loggerName, string message, PrintType type = PrintType.Log)
    {
        var loadedLogger = Resources.Load<Printer>($"Printers/{loggerName}");
        if (loadedLogger == null)
        {
            UnityEngine.Debug.LogWarning($"Could not find logger named '{loggerName}'. Message: {message}");
            // Log.Warning($"Could not find logger named '{loggerName}'. Message: {message}");
        }
        else
        {
            loadedLogger.Print(message, type);
        }
    }
    [HideInCallstack]
    public static void PrintLog(string loggerName, string message) =>
        PrintLog(loggerName, message, PrintType.Log);
    [HideInCallstack]
    public static void PrintWarning(string loggerName, string message) =>
        PrintLog(loggerName, message, PrintType.Warning);
    [HideInCallstack]
    public static void PrintError(string loggerName, string message) =>
        PrintLog(loggerName, message, PrintType.Error);
}
