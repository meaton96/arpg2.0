using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollectionSingleton : MonoBehaviour {
    public static AbilityCollectionSingleton Instance;
    public bool UpdateCollection = false;
    [SerializeField] private AbilityCollection abilityCollection;
    // Start is called before the first frame update
    void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        if (UpdateCollection) {
            abilityCollection.LoadAllAssets();
        }
    }
    private Ability GetAbilityByID(int id) {
        Ability ability = null;
        try {
            ability = abilityCollection.GetAbilityByID(id);
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        
        return ability;
    }
    public Buff GetBuffByID(int id) {
        Buff buff = null;
        try {
            buff = abilityCollection.GetBuffByID(id);
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        
        return buff;
    }
    public Ability GetAbilityCopyByID(int id, GameCharacter caster) {
        Ability b = GetAbilityByID(id).CopyInstance();
        b.Init(caster);
        return b;
    }
    public Buff GetBuffCopyByID(int id, GameCharacter caster) {
        Buff b = GetBuffByID(id).CopyInstance();
     //   b.Init(caster);
        return b;
    }
    public Ability GetAbilityCopyByName(string name, GameCharacter caster) {
        var ab = abilityCollection.GetAbilityByName(name);
        if (ab == null) {
            throw new System.Exception("invalid ability name");
        }
        ab.Init(caster);
        return ab;
    }
    public Buff GetBuffByName(string name, GameCharacter caster) {
        var buff = abilityCollection.GetBuffByName(name);
        if (buff == null) {
            throw new System.Exception("invalid buff name");
        }
       // buff.Init(caster);
        return buff;
    }
}
