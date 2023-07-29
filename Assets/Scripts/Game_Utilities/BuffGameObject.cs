using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffGameObject : MonoBehaviour
{
    public string uniqueID;
    public float duration;
    protected GameCharacter character;
    protected float timer;
    protected Buff buff;

    private void Start() {
        character = GetComponent<GameCharacter>();
        timer = duration;
    }

    private void Update() {
        if (duration < 0) return; // If duration is -1 or less, this is an infinite duration buff

        timer -= Time.deltaTime;
        if (timer <= 0) {
            RemoveBuff();
        }
    }

    public void SetDuration(float newDuration) {
        duration = newDuration;
        timer = newDuration;
    }
    public abstract void ApplyBuff();

    public void RemoveBuff() {
        // Undo the buff effect
        UndoBuff();
        // Then remove this buff component
        Destroy(this);
    }

    public abstract void UndoBuff();
}
