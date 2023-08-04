using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "AbilityCollection", menuName = "Custom Assets/Utilities/Ability Collection")]
public class AbilityCollection : ScriptableObject {
    

    Dictionary<int, Ability> _allAbilities;
    Dictionary<int, Buff> _allBuffsAndDebuffs;

    [SerializeField]
    SerializableDictionary<int, Ability> allAbilitiesNew;
    [SerializeField]
    SerializableDictionary<int, Buff> allBuffsAndDebuffsNew;

    const string BUFF_FOLDER_PATH = "Ability Assets/Buffs";
    const string ABILITY_FOLDER_PATH = "Ability Assets/Abilities/";



    public Ability GetAbilityByID(int id) {
        return _allAbilities[id];
    }
    public Buff GetBuffByID(int id) {
        return _allBuffsAndDebuffs[id];
    }
    public Ability GetAbilityByName(string name) {
        foreach (var ability in _allAbilities) {
            if (ability.Value._name == name) {
                return _allAbilities[ability.Key];
            }
        }
        return null;
    }
    public Buff GetBuffByName(string name) {
        foreach (var buff in _allBuffsAndDebuffs) {
            if (buff.Value._name == name) {
                return _allBuffsAndDebuffs[buff.Key];
            }
        }
        return null;
    }

    /// <summary>
    /// reads in all assets from the folder name lists
    /// folder name lists are serializable member variables meant to be set in inpector
    /// </summary>
    public void LoadAllAssets() {
        //itialize new serializeable dictionaries
        //these are just to allow the dictionaries to be viewable in inspector to check for errors
        _allAbilities = new();
        _allBuffsAndDebuffs = new();
        //get everything in folders
        Resources.LoadAll<Buff>(BUFF_FOLDER_PATH).ToList().ForEach(buff => _allBuffsAndDebuffs[buff.id] = buff);
        Resources.LoadAll<Ability>(ABILITY_FOLDER_PATH).ToList().ForEach(ab => _allAbilities[ab.id] = ab);

        

        allAbilitiesNew = new(_allAbilities);
        allBuffsAndDebuffsNew = new(_allBuffsAndDebuffs);

    }

    
}








