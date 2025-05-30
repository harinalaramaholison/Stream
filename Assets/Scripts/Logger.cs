using System;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private static readonly string logFilePath = "UnityLog.txt";

    void Start()
    {
        // Check if the file exists
        if (!File.Exists(logFilePath))
        {
            // Create the file
            File.Create(logFilePath).Dispose();
        }

        // Clear the file contents
        File.WriteAllText(logFilePath, string.Empty);
    }

    public static void Log(string message)
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";
        File.AppendAllText(logFilePath, logMessage);
        
    }
}
