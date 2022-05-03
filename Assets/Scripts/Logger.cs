using System;
using System.IO;

//https://stackoverflow.com/questions/50216028/how-to-debug-why-unity-freezes
public class Logger
{
    public static Logger instance;

    public string LogFilePath { get; private set; }
    public int Indent = 0;
    private const int INDENT_SIZE = 2;

    public Logger(string logFilePath)
    {
        if (!logFilePath.EndsWith(".log"))
            logFilePath += ".log";
        LogFilePath = logFilePath;
        if (!File.Exists(LogFilePath))
            File.Create(LogFilePath).Close();
        WriteLine("New Session Started");
    }

    public void WriteLine(object message)
    {
        using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            writer.WriteLine(DateTime.Now.ToString() + ": " + new string(' ', Indent * INDENT_SIZE) + message.ToString());
    }

}