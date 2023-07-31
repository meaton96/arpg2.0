using System.Collections.Generic;
using UnityEngine;

//TODO:
//Create stat manager script
//buff manager script interacts with stat manager ect 
//buff proximity may be able to be handled using collision methods as shown below
//make another game object on the support that uses a trigger to apply and remove buff
//start by just moving the code around and making the basic haste on use buff work
public class BuffManager : MonoBehaviour {
    public const int AURA_COLLISION_LAYER = 14;
    //public Dictionary<GameCharacter.CharacterStat, float> CharacterStats = new Dictionary<GameCharacter.CharacterStat, float>();
  //  [SerializeField] protected GameObject attachedBuffPrefab;
    private List<Buff> currentBuffs = new();
    [SerializeField] private StatManager statManager;

    //#region UI
    //Vector3 buffBarStart = new(-900, 480, 0);
    //Vector3 debuffBarStart = new(-900, 372, 0);

    //#endregion

    private void Start() {
        // Initialize the character's stats
        
        // Add more as required
    }

    private void Update() {
        for (int i = currentBuffs.Count - 1; i >= 0; i--) {
            if (currentBuffs[i].duration != -1) {
                currentBuffs[i].duration -= Time.deltaTime;
                if (currentBuffs[i].duration <= 0) {
                    RemoveBuff(currentBuffs[i]);
                }
            }
        }
    }

    public virtual void ApplyBuff(Buff buff) {
        Debug.Log($"trying to applying {buff._name} to {name}");
        var existingBuff = currentBuffs.Find(b => b.id == buff.id);
        if (existingBuff != null) {
            existingBuff.duration = buff.duration; // Refresh duration
        }
        else {
            currentBuffs.Add(buff);
            buff.ApplyBuff(statManager);
          //  ApplyBuffEffects(buff);
        }
    }
    public void HandleOnHitSpellEffect(DamagingAbility ability) {
        if (ability.onHitDebuff != null) {
          //  Debug.Log($"applying on hit effect {ability.onHitDebuff._name} from {ability} to {name}");
            ApplyBuff(ability.onHitDebuff);
            //ApplyBuff(ability.onHitDebuff);
        }   
    }

    public virtual void RemoveBuff(Buff buff) {
        currentBuffs.Remove(buff);
        buff.RemoveBuff(statManager);
     //   RemoveBuffEffects(buff);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (TryGetComponent(out RangedAuraDetector friendlyAura)) {
            if (friendlyAura.Buff != null)
                ApplyBuff(friendlyAura.Buff);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (TryGetComponent(out RangedAuraDetector friendlyAura)) {
            if (friendlyAura.Buff != null)
                RemoveBuff(friendlyAura.Buff);
        }
    }

    //private void ApplyBuffEffects(Buff buff) {
    //    foreach (var modifier in buff.StatModifiers) {
    //        CharacterStats[modifier.Key] += modifier.Value;
    //    }
    //}

    //private void RemoveBuffEffects(Buff buff) {
    //    foreach (var modifier in buff.StatModifiers) {
    //        CharacterStats[modifier.Key] -= modifier.Value;
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D other) {
    //    BuffProvider buffProvider = other.GetComponent<BuffProvider>();
    //    if (buffProvider != null) {
    //        ApplyBuff(buffProvider.GetBuff());
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other) {
    //    BuffProvider buffProvider = other.GetComponent<BuffProvider>();
    //    if (buffProvider != null) {
    //        RemoveBuff(buffProvider.GetBuff());
    //    }
    //}
}
