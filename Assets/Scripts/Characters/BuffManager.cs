using System.Collections.Generic;
using UnityEngine;


public class BuffManager : MonoBehaviour {
    public const int AURA_COLLISION_LAYER = 14;     
    private List<Buff> currentBuffs = new();            //keeps track of current active buffs
    [SerializeField] private StatManager statManager;   //pointer to character stat manager script



    private void Update() {
        //check each buff and reduce the timer if it has a duration
        //remove it if it has a negative duration
        for (int i = currentBuffs.Count - 1; i >= 0; i--) {
            if (currentBuffs[i].duration != -1) {
                currentBuffs[i].duration -= Time.deltaTime;
                if (currentBuffs[i].duration <= 0) {
                    RemoveBuff(currentBuffs[i]);
                }
            }
        }
    }
    //attempt to apply the buff to the character
    //if the buff already exists then refresh its duration
    public virtual void ApplyBuff(Buff buff) {
        //Debug.Log($"trying to applying {buff._name} to {name}");
        var existingBuff = currentBuffs.Find(b => b.id == buff.id);
        if (existingBuff != null) {
            existingBuff.duration = buff.duration; // Refresh duration
        }
        else {
            currentBuffs.Add(buff);
            buff.ApplyBuff(statManager);
        }
    }
    //handle possible on hit debuff application
    public void HandleOnHitSpellEffect(DamagingAbility ability) {
        if (ability.onHitDebuff != null) {
            ApplyBuff(ability.onHitDebuff);
        }   
    }
    //remove buff from list and from character stat manager
    public virtual void RemoveBuff(Buff buff) {
        currentBuffs.Remove(buff);
        buff.RemoveBuff(statManager);
    }
    //handle possible collision with an aura collider
    //for handling ranged buff auras from support unitys
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (TryGetComponent(out RangedAuraDetector friendlyAura)) {
            if (friendlyAura.Buff != null)
                ApplyBuff(friendlyAura.Buff);
        }
        
    }
    //handle exit collision to remove ranged buffs
    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (TryGetComponent(out RangedAuraDetector friendlyAura)) {
            if (friendlyAura.Buff != null)
                RemoveBuff(friendlyAura.Buff);
        }
    }
}
