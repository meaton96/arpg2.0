using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedBuff : MonoBehaviour
{
    [HideInInspector]
    public Buff buff;
    float timer;
    float time;
    GameCharacter character;
    

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0) {
            character.RemoveBuff(buff);
            Destroy(gameObject);
        }
        else {
            timer -= Time.deltaTime;
        }

    }
    public void ResetTimer() { timer = buff.duration; }
    public void SetTimer(float time) { timer = time;  }
    public void Init(Buff buff, GameCharacter gameCharacter) {
        this.buff = buff;
        character = gameCharacter;
        time = buff.duration;
        timer = time;
        
    }
}
