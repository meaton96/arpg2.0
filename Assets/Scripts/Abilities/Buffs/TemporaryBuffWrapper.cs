using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryBuffWrapper : MonoBehaviour
{
    Buff buff;
    GameCharacter caster;
    GameCharacter attachedCharacter;

    private void Update() {
        
    }

    public void Attach(Buff buff, GameCharacter caster, GameCharacter attachedCharacter) {
        this.buff = buff;
        this.caster = caster;
        this.attachedCharacter = attachedCharacter;
        caster.ApplyBuff(buff);
    } 
}
