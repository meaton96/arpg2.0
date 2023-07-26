using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSettingsHelper : MonoBehaviour
{
    public const string SETTINGS_PATH = "/Resources/playerSettings.ini";
    public const string DEFAULT_SETTINGS =
        "[Input]\r\n" +
        "FORCE_MOVEMENT_BUTTON=2\r\n" +
        "KEY_CODE_DODGE=32\r\n" +
        "KEY_CODE_ABILITY_0=113\r\n" +
        "KEY_CODE_ABILITY_1=119\r\n" +
        "KEY_CODE_ABILITY_2=101\r\n" +
        "KEY_CODE_ABILITY_3=114\r\n" +
        "KEY_CODE_CHAR_PANEL=99\r\n" +
        "[Other]\r\n" +
        "DISPLAY_ENEMY_HEALTH_ALWAYS=1\r\n" +
        "DISPLAY_FLOATING_COMBAT_TEXT=1";
    public static string GetSettingsSection(string section) {
        var aPath = Application.dataPath + SETTINGS_PATH;
        if (File.Exists(aPath)) {

            //grab all the text, split it into pieces by the deliminator character - !
            using StreamReader sr = File.OpenText(aPath);

            string settingsText = "";
            string line;
            while (!sr.EndOfStream) {
                line = sr.ReadLine();
                if (line.Contains(section)) {
                    line = sr.ReadLine();
                    while (!sr.EndOfStream && !line.Contains("[")) {
                        line = sr.ReadLine();
                        settingsText += line + "\n";
                    }
                    sr.Close();
                    return settingsText;
                }
            }

        }
        else {
            //if settings is missing create the default settings file
            using StreamWriter sw = File.CreateText(aPath);
            sw.Write(DEFAULT_SETTINGS);
            sw.Close();
            return GetSettingsSection(section); 
        }
        throw new System.Exception("This should not be possible - error parsing settings file");
    }

    //iterates through the setting text
    //sets the fields from the player object to the values from the settings file
    
    public static void InitObjectSettings(Object ob, string section) {
        var settingsText = GetSettingsSection(section);
        var lines = settingsText.Split("\n");
        var playerFields = ob.GetType().GetFields();
        foreach (var line in lines) {
            var splitLine = line.Split('=');
            foreach (var field in playerFields) {
                if (field.Name == splitLine[0]) {
                    if (int.TryParse(splitLine[1], out int num)) {
                        if (field.FieldType == typeof(int)) {
                            
                            field.SetValue(ob, num);
                            
                        }
                        else {
                            field.SetValue(ob, (KeyCode)num);
                        }
                    }
                }
            }
        }
    }

}
