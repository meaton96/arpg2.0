using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBuffWrapper_Timer : MonoBehaviour {
    Buff buff;
    GameCharacter attachedCharacter;
    float duration;
    float timer;


    private void Update() {

        if (timer >= duration) {
            attachedCharacter.RemoveBuff(buff);
            Destroy(gameObject);
        }
        else {
            timer += Time.deltaTime;
        }
    }

    public void Attach(Buff buff, GameCharacter attachedCharacter, float duration) {
        this.buff = buff;
        this.duration = duration;
        this.attachedCharacter = attachedCharacter;
        transform.parent = attachedCharacter.transform;
        attachedCharacter.ApplyBuff(buff);
    }
}
