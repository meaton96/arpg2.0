using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuraWrapper : MonoBehaviour {

    public Aura aura;
    public bool active;
    public GameObject uiImage;

    public void TurnOffAura(Player player) {
        active = false;
        aura.DeactivateAura(player);
       // uiImage.GetComponent<Image>().color = Color.white; 
    }

    public void EquipAura(Player player) {
        active = true;
        //uiImage.GetComponent<Image>().color = Color.green;
        aura.ActivateAura(player);  
    }
    public void UnEquipAura(Player player) {
        aura.DeactivateAura(player);
    }
    public void Init(Aura aura, GameObject uiImage) {
        this.aura = aura;
        uiImage.GetComponent<Image>().sprite = aura.iconImage;
        this.uiImage = uiImage;
    }
}
