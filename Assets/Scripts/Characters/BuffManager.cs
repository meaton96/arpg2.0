using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffManager : MonoBehaviour {
    public const int AURA_COLLISION_LAYER = 14;
    private Dictionary<int, Buff> currentBuffs = new();            //keeps track of current active buffs
    [SerializeField] private StatManager statManager;   //pointer to character stat manager script

    private Dictionary<int, OnDeathEffect> onDeathEffects = new();


    private void Update() {
        //check each buff and reduce the timer if it has a duration
        //remove it if it has a negative duration
        currentBuffs.Values.ToList().ForEach(buff => {
            if (buff.duration != -1) {
                buff.duration -= Time.deltaTime;
                if (buff.duration < 0) {
                    RemoveBuff(buff);
                    return;
                }
            }

        });

    }
    //attempt to apply the buff to the character
    //if the buff already exists then refresh its duration
    public virtual void ApplyBuff(Buff buff) {
        //Debug.Log($"trying to applying {buff._name} to {name}");
        //   var existingBuff = currentBuffs.Find(b => b.id == buff.id);
        if (currentBuffs.ContainsKey(buff.id)) {
            currentBuffs[buff.id].duration = buff.duration;
        }
        else {
            currentBuffs.Add(buff.id, buff);
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
        if (currentBuffs.Remove(buff.id)) {
            buff.RemoveBuff(statManager);
        }
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
    public void AddOnDeathEffect(OnDeathEffect effect) {

        if (onDeathEffects.ContainsKey(effect.id)) {
            var onDeath = onDeathEffects[effect.id];
            onDeath.duration = effect.duration;
        }
        else {
            onDeathEffects.Add(effect.id, effect);
        }
    }
    public void ProcessOnDeathEffects() {
        foreach (var effect in onDeathEffects.Values) {
            effect.ApplyOnDeathEffect(statManager, transform.position, GetComponent<Collider2D>());
        }
    }
}
