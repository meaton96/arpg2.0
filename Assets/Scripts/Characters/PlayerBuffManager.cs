using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffManager : BuffManager
{
    public UIBehaviour HUD;

    private void Start() {
        HUD = GameObject.FindWithTag("HUD").GetComponent<UIBehaviour>();
    }

    public override void ApplyBuff(Buff buff) {
        base.ApplyBuff(buff);
        HUD.DisplayNewBuff(buff);
        
    }

    public override void RemoveBuff(Buff buff) {
        //HUD.ForceRemoveBuff(buff);
        base.RemoveBuff(buff);
    }
    protected override void OnTriggerEnter2D(Collider2D collision) { }
    protected override void OnTriggerExit2D(Collider2D collision) { }
}
