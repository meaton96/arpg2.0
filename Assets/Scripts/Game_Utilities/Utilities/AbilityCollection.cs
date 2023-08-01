using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    const string BUFF_FOLDER_PREFIX = "Ability Assets/";
    const string ABILITY_FOLDER_PREFIX = "Ability Assets/Abilities/";
    [SerializeField] List<string> BuffFolderNames;
    [SerializeField] List<string> AbilityFolderNames;



    public Ability GetAbilityByID(int id) {
        Ability ability = null;
        try {
            ability = allAbilitiesNew[id];
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return ability;
    }
    public Buff GetBuffByID(int id) {
        Buff buff = null;
        try {
            buff = allBuffsAndDebuffsNew[id];
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return buff;
    }
    public void LoadAllAssets() {
        _allAbilities = new();
        _allBuffsAndDebuffs = new();
        

        foreach (var folderName in BuffFolderNames) {
            
            Buff[] buffs = Resources.LoadAll<Buff>(BUFF_FOLDER_PREFIX + folderName);
          //  Debug.Log(BUFF_FOLDER_PREFIX + folderName);
           // Debug.Log(buffs.Length);
            foreach (Buff buff in buffs) {
               // Debug.Log(buff.name);
                _allBuffsAndDebuffs[buff.id] = buff;
            }
        }
        foreach (var folderName in AbilityFolderNames) {
            Ability[] abilities = Resources.LoadAll<Ability>(ABILITY_FOLDER_PREFIX + folderName);
            foreach (Ability abilitiy in abilities) {
                _allAbilities[abilitiy.id] = abilitiy;
            }
        }
        allAbilitiesNew = new(_allAbilities);
        allBuffsAndDebuffsNew = new(_allBuffsAndDebuffs);

    }

    
}








