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
    private Buff GetBuffByID(int id) {
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
        b.Init(caster);
        return b;
    }
    //public Buff GetBuffCopy(Buff buff) {
    //    if (buff == null) throw new System.ArgumentNullException("buff is null");
    //    Buff b = buff.CopyInstance();
        
    //    return b;
    //}
    //public Ability GetAbilityCopy(Ability ability) {
    //    if (ability == null) throw new System.ArgumentNullException("ability is null");
    //    Ability a = ability.CopyInstance();
        
    //    return a;
    //}
}
