using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;

public class JsonHelper {

    public const int TYPE_LINE_NUMBER = 1;
    public const string _PREFAB_PATH_ = "Prefabs/Spells/";

    //iterates the json file at the location path
    public static Dictionary<int, Ability> ParseAllAbilities(string path) {
        var abilities = new Dictionary<int, Ability>();


        //grab the absoulte path instead of relative path
        string absolutePath = Application.dataPath + "/Resources" + path;

        //make sure its real
        if (File.Exists(absolutePath)) {

            //grab all the text, split it into pieces by the deliminator character - !
            using StreamReader sr = File.OpenText(absolutePath);
            string text = sr.ReadToEnd();
            var lines = text.Split("!");
            //iterate through each piece - should be about 1 spell
            for (int x = 1; x < lines.Length; x++) {
                //trim the ends 
                var spellIdString = lines[x][..lines[x].IndexOf('"')];
                lines[x] = lines[x][lines[x].IndexOf('{')..lines[x].IndexOf('}')];

                //parse the spell id string 
                if (int.TryParse(spellIdString, out int spellID)) {

                    //spell ID string parsed correctly so proceed:
                    //split each spell into seperate lines
                    var t = lines[x].Split('\n');
                    //grab the type string from the first line
                    string typeString = GetFieldFromJsonLine(t[TYPE_LINE_NUMBER]);

                    //if the type string parse correctly:

                    Ability ability = null;
                    switch (typeString) {
                        case Ability._ID_PROJECTILE:
                            ability = ScriptableObject.CreateInstance<ProjectileAbility>();

                            break;
                        case Ability._ID_SUMMON:
                            break;
                        case Ability._ID_AURA:
                            ability = ScriptableObject.CreateInstance<Aura>();
                            break;
                        case Ability._ID_ATTACK_PROJECTILE:
                            ability = ScriptableObject.CreateInstance<ProjectileAttack>();
                            break;
                        case Ability._ID_GROUND_TARGETED_AOE:
                            ability = ScriptableObject.CreateInstance<GroundTargetedAOEAbility>();
                            break;
                        case Ability._ID_TELEPORT:
                            ability = ScriptableObject.CreateInstance<Teleport>();
                            break;
                        case Ability._ID_BUFF:
                            ability = ScriptableObject.CreateInstance<SelfBuffAbility>();
                            break;
                        case Ability._ID_ATTACK_PROJ_MULTI:
                            ability = ScriptableObject.CreateInstance<MulticastProjectileAbility>();
                            break;
                        default:
                            break;
                    }


                    var ab = ParseAbility(ability, spellID, lines[x]);
                    //add it to the abilities dictionary - but parse the ability first
                    abilities.Add(spellID, ab);
                }
                else {
                    throw new Exception("Spell ID string - int parse error");
                }
            }
        }
        else {
            throw new FileNotFoundException("missing json file at " + Application.dataPath + "/Resources" + path);
        }
        return abilities;
    }

    private static Ability ParseAbility(Ability ability, int id, string spellJson) {

        ability.id = id;                                            //init with id
        var lines = spellJson.Split('\n');                          //split lines by new line character
        var classFields = ability.GetType().GetFields();            //grab all class fields to populate

        //iterate through json lines skipping the first (opening bracket)
        for (int x = 1; x < lines.Length; x++) {
            //SetValue() was not working with objects so hardcoding these values for now:

            //populate tags list
            if (lines[x].Contains("tags")) {
                var arrayString = lines[x].Split(":")[1];
                arrayString = arrayString[(arrayString.IndexOf('[') + 1)..(arrayString.IndexOf(']') - 1)];

                var arr = new List<string>(arrayString.Split(','));
                foreach (var item in arr) {
                    item.Trim();
                }

                ability.tags = new(arr);
                continue;
            }
            //Load the icon 
            if (lines[x].Contains("icon")) {
                ability.iconImage = Resources.Load<Sprite>(GetFieldFromJsonLine(lines[x]));
                continue;
            }
            //load the prefab
            if (lines[x].Contains("prefab")) {
                var s = _PREFAB_PATH_ + GetFieldFromJsonLine(lines[x]);
                ability.abilityPreFab = Resources.Load<GameObject>(s);
                continue;
            }

            //iterate class fields
            for (int y = 0; y < classFields.Length; y++) {

                //check if class the json line against class field names
                //find a json line that contains the name of a variable
                if (lines[x].Contains(classFields[y].Name)) {
                    //do something depending on which variable type
                    //either grab the string or parse to int or float
                    if (classFields[y].FieldType == typeof(string)) {
                        classFields[y].SetValue(ability, GetFieldFromJsonLine(lines[x]));
                    }
                    else if (classFields[y].FieldType == typeof(int)) {
                        classFields[y].SetValue(ability, int.Parse(GetFieldFromJsonLine(lines[x])));
                    }
                    else if (classFields[y].FieldType == typeof(float)) {
                        classFields[y].SetValue(ability, float.Parse(GetFieldFromJsonLine(lines[x])));
                    }
                }
            }
        }

        ability.Init();

        return ability;
    }
    public static Dictionary<int, Buff> ParseAllBuffsAndDebuffs(string path) {
        var buffsAndDebuffs = new Dictionary<int, Buff>();


        //grab the absoulte path instead of relative path
        string absolutePath = Application.dataPath + "/Resources" + path;

        //make sure its real
        if (File.Exists(absolutePath)) {

            //grab all the text, split it into pieces by the deliminator character - !
            using StreamReader sr = File.OpenText(absolutePath);
            string text = sr.ReadToEnd();
            var lines = text.Split("!");
            //iterate through each piece - should be about 1 spell
            for (int x = 1; x < lines.Length; x++) {
                //trim the ends 
                var spellIdString = lines[x][..lines[x].IndexOf('"')];
                lines[x] = lines[x][lines[x].IndexOf('{')..lines[x].IndexOf('}')];

                //parse the spell id string 
                if (int.TryParse(spellIdString, out int spellID)) {

                    //spell ID string parsed correctly so proceed:
                    //split each spell into seperate lines
                    var t = lines[x].Split('\n');
                    //grab the type string from the first line
                    var type = int.Parse(GetFieldFromJsonLine(t[TYPE_LINE_NUMBER]));
                    Buff buff = ScriptableObject.CreateInstance<Buff>();
                    ParseBuff(buff, spellID, lines[x]);
                    buffsAndDebuffs.Add(spellID, buff);

                }
                else {
                    throw new Exception("Spell ID string - int parse error");
                }
            }
        }
        else {
            throw new FileNotFoundException("missing json file at " + Application.dataPath + "/Resources" + path);
        }
        return buffsAndDebuffs;
    }
    public static void ParseBuff(Buff buff, int id, string buffJson) {
        var lines = buffJson.Split('\n');
        var classFields = buff.GetType().GetFields();

        buff.id = id;

        for (int x = 0; x < lines.Length; x++) {
            if (lines.Contains("type")) {
                var type = int.Parse(GetFieldFromJsonLine(lines[x]));
                buff.etype = type == 0 ? Buff.EffectType.Buff : Buff.EffectType.Debuff;
                continue;
            }

            //iterate class fields
            for (int y = 0; y < classFields.Length; y++) {
                if (classFields[y].Name == "id") continue;

                //check if class the json line against class field names
                //find a json line that contains the name of a variable
                if (lines[x].Contains(classFields[y].Name)) {

                    //do something depending on which variable type
                    //either grab the string or parse to int or float
                    if (classFields[y].FieldType == typeof(string)) {
                        classFields[y].SetValue(buff, GetFieldFromJsonLine(lines[x]));
                    }
                    else if (classFields[y].FieldType == typeof(int)) {
                        classFields[y].SetValue(buff, int.Parse(GetFieldFromJsonLine(lines[x])));
                    }
                    else if (classFields[y].FieldType == typeof(float)) {
                        classFields[y].SetValue(buff, float.Parse(GetFieldFromJsonLine(lines[x])));
                    }
                }
            }
        }
        buff.Init();
    }

    public static string GetFieldFromJsonLine(string line) {
        if (line.Contains(','))
            line = line[..line.IndexOf(',')];
        var pieces = line.Split(':');
        var data = pieces[1];
        data = data.
            Replace('"', ' ').
            Trim();
        return data;
    }
}