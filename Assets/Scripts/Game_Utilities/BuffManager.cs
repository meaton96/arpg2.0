using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour {
    private GameCharacter character;
   // [SerializeField] private GameObject buffPrefab;
    private void Start() {
        character = GetComponent<GameCharacter>();
    }
    public void AddBuffOne(Buff buff) {
        
    }
    public void AddBuff(BuffGameObject newBuff) {
        // Check if there is already a buff of this type
        var existingBuff = character.GetComponents<BuffGameObject>().FirstOrDefault(b => b.uniqueID == newBuff.uniqueID);
        if (existingBuff != null) {
            // If the buff already exists, just refresh its duration
            existingBuff.SetDuration(newBuff.duration);
        }
        else {
            // If the buff doesn't exist yet, add it as a component
            BuffGameObject buff = gameObject.AddComponent(newBuff.GetType()) as BuffGameObject;
            buff.uniqueID = newBuff.uniqueID;
            buff.duration = newBuff.duration;
            buff.ApplyBuff();
        }
    }
}
