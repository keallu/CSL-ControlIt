using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ControlIt.Helpers
{
    public static class LogHelper
    {
        public static void WriteTelemetryEntryToFile(string eventName, List<KeyValuePair<string, string>> keyValuePairs)
        {
            try
            {
                if (!Directory.Exists("Telemetry"))
                {
                    Directory.CreateDirectory("Telemetry");
                }

                string fileName = "Telemetry" + DateTime.Now.ToString("yyyy-MM-dd");

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("{\"");
                stringBuilder.Append(eventName);
                stringBuilder.Append("\": {");
                bool first = true;
                foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }
                    stringBuilder.Append("\"");
                    stringBuilder.Append(keyValuePair.Key);
                    stringBuilder.Append("\": \"");
                    stringBuilder.Append(keyValuePair.Value);
                    stringBuilder.Append("\"");
                }
                stringBuilder.Append("}}");
                stringBuilder.Append("\r\n\r\n");

                File.AppendAllText($"Telemetry{Path.DirectorySeparatorChar}{fileName}.txt", stringBuilder.ToString());
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] LogHelper:WriteTelemetryEntryToFile -> Exception: " + e.Message);
            }
        }
    }
}
