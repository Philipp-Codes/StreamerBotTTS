using System;

public class CPHInline
{
    public bool Execute()
    {
        // Attempt to get the raw input string from the channel points redemption
        if (CPH.TryGetArg("rawInput", out string rawInput))
        {
            CPH.LogInfo($"Received raw input: {rawInput}");

            // Split the input based on spaces to process words
            var parts = rawInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string message = "";
            string voiceAlias = "Brian"; // Default voice alias

            for (int i = 0; i < parts.Length; i++)
            {
                // Check if the current part has a colon
                if (parts[i].EndsWith(":"))
                {
                    // If there's a message accumulated before this part, send it
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        CPH.LogInfo($"Speaking message with {voiceAlias}: {message.Trim()}");
                        CPH.TtsSpeak(voiceAlias, message.Trim());
                        message = ""; // Clear message for next accumulation
                    }

                    // Get the voice alias from the current part
                    voiceAlias = parts[i].TrimEnd(':');
                    CPH.LogInfo($"Set voice alias to: {voiceAlias}");
                }
                else
                {
                    // Accumulate the message
                    message += parts[i] + " ";
                }
            }

            // Send the last accumulated message with the most recent voice alias, if any
            if (!string.IsNullOrWhiteSpace(message))
            {
                CPH.LogInfo($"Speaking remaining message with {voiceAlias}: {message.Trim()}");
                CPH.TtsSpeak(voiceAlias, message.Trim());
            }

            return true;
        }
        else
        {
            CPH.LogInfo("Failed to get raw input.");
            return false;
        }
    }
}
