using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffManager : MonoBehaviour {
    public const int AURA_COLLISION_LAYER = 14;
    private Dictionary<int, Buff> currentBuffs = new();            //keeps track of current active buffs
    [SerializeField] private StatManager statManager;   //pointer to character stat manager script
    [SerializeField] private GameCharacter character;
    private Dictionary<int, OnDeathEffect> onDeathEffects = new();
    private Dictionary<int, DamageOverTimeEffect> dotEffects = new();

    private void Update() {
        CheckForExpiringEffects();
        //check dot effects and see for dot ticks
        foreach (var DoT in dotEffects.Values) {
            DoT.tickTimer -= Time.deltaTime;
            if (DoT.tickTimer <= 0) {
                DoT.tickTimer = DoT.tickTime;
                character.DamageHealth(DoT.damagePerTick);
            }
        }
        

    }
    public void CheckForExpiringEffects() {
        foreach (var onDeathEffect in onDeathEffects) {
            if (onDeathEffect.Value.duration != -1) {
                onDeathEffect.Value.duration -= Time.deltaTime;
                if (onDeathEffect.Value.duration < 0) {
                    RemoveBuff(onDeathEffect.Value);
                    return;
                }
            }
        }//check each buff and reduce the timer if it has a duration
        //remove it if it has a negative duration
        foreach (var buff in currentBuffs) {
            if (buff.Value.duration != -1) {
                buff.Value.duration -= Time.deltaTime;
                if (buff.Value.duration < 0) {
                    RemoveBuff(buff.Value);
                    return;
                }
            }
        }

        foreach (var DoT in dotEffects) {
            if (DoT.Value.duration != -1) {
                DoT.Value.duration -= Time.deltaTime;
                if (DoT.Value.duration < 0) {
                    RemoveBuff(DoT.Value);
                    return;
                }
            }
        }
    }
    //attempt to apply the buff to the character
    //if the buff already exists then refresh its duration
    public virtual void ApplyBuff(Buff buff) {
        Debug.Log($"trying to applying {buff._name} to {name}");
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
            AddEffect(ability.onHitDebuff);
        }
       // if (ability.caster.gameObject.layer == GameController.PLAYER_LAYER) {
      //      ability.caster.globalOnHitEffects.ForEach(buff => AddEffect(buff));
        //}
    }
    public void AddEffect(Buff effect) {
        
        switch (effect.etype) {
            case Buff.EffectType.DamageOverTime:
                AddDamageOverTimeEffect(effect as DamageOverTimeEffect);
                break;
            case Buff.EffectType.OnDeath:
                AddOnDeathEffect(effect as OnDeathEffect);
                break;
            default:
                ApplyBuff(effect);
                break;
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
    public bool RemoveOnDeathEffect(OnDeathEffect effect) {
        return onDeathEffects.Remove(effect.id);    
    }
    public void ProcessOnDeathEffects() {
        foreach (var effect in onDeathEffects.Values) {
            effect.ApplyOnDeathEffect(statManager, transform.position, GetComponent<Collider2D>());
        }
    }
    public void AddDamageOverTimeEffect(DamageOverTimeEffect effect) {
        if (dotEffects.ContainsKey(effect.id)) {
            var dot = dotEffects[effect.id];
            dot.duration = effect.duration;
        }
        else {
            dotEffects.Add(effect.id, effect);
        }
    }
}
