using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryBuffWrapper : MonoBehaviour {
    Buff buff;
    GameCharacter caster;
    GameCharacter attachedCharacter;
    float range;
    float timer;
    float pollTime;
        

    private void Update() {
        if (timer >= pollTime) {
            var casterPos = caster.transform.position;
            var myPos = transform.position;
            var dist = Mathf.Pow(casterPos.x - myPos.x, 2) + Mathf.Pow(casterPos.y - myPos.y, 2);
            if (dist > range) {
                attachedCharacter.RemoveBuff(buff);
                Destroy(gameObject);
            }
            timer = 0;
        }else {
            timer += Time.deltaTime;
        }

    }

    public void Attach(Buff buff, GameCharacter caster, GameCharacter attachedCharacter, float range, float pollTimer) {
        this.buff = buff;
        this.caster = caster;
        this.attachedCharacter = attachedCharacter;
        this.range = range;
        transform.parent = attachedCharacter.transform;
        this.pollTime = pollTimer;
        attachedCharacter.ApplyBuff(buff);
    }
}
