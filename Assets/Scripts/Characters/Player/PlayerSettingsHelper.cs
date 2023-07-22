using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSettingsHelper : MonoBehaviour
{
    public const string SETTINGS_PATH = "/Resources/playerSettings.ini";

    public static string GetSettingsTextFromPath() {
        var aPath = Application.dataPath + SETTINGS_PATH;
        if (File.Exists(aPath)) {

            //grab all the text, split it into pieces by the deliminator character - !
            using StreamReader sr = File.OpenText(aPath);

            string settingsText = "";
            string line;
            while (!sr.EndOfStream) {
                line = sr.ReadLine();


                if (line.Contains("Input")) {
                    line = sr.ReadLine();
                    while (!line.Contains("[")) {
                        line = sr.ReadLine();
                        settingsText += line + "\n";
                    }
                    return settingsText;
                }
            }

        }
        //replace with creating a default file
        throw new FileNotFoundException("missing settings.ini file");
    }

    public static void InitPlayerControls(Player player) {
        var settingsText = GetSettingsTextFromPath();
        var lines = settingsText.Split("\n");
        foreach (var line in lines) {
            var splitLine = line.Split('=');
            var playerFields = player.GetType().GetFields();
            foreach (var field in playerFields) {
                if (field.Name == splitLine[0]) {
                    if (int.TryParse(splitLine[1], out int num)) {
                        if (field.FieldType == typeof(int))
                            field.SetValue(player, num);
                        else {
                            field.SetValue(player, (KeyCode)num);
                        }
                    }
                }
            }
        }
    }

}
