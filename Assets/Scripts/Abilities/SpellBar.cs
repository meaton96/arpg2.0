using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBar : MonoBehaviour {

    //ability bar
    [SerializeField] private AbilityWrapper abilityWrapperPrefab;
    public AbilityWrapper[] spellWrappers = new AbilityWrapper[6];
    [SerializeField] private GameObject[] spellBarImages = new GameObject[6];
    [SerializeField] private TextMeshProUGUI[] spellTimers = new TextMeshProUGUI[6];

    //aura bar
    [SerializeField] private AuraWrapper auraWrapperPrefab;
    public AuraWrapper[] auraWrappers = new AuraWrapper[3];
    [SerializeField] private GameObject[] auraBarImages = new GameObject[3];


    [SerializeField] private Sprite noAbilityEquippedImage;
    [SerializeField] private Player player;

    private void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    #region Abilties Bar
    //equip the ability in the slot slot
    //ability is equipped via ID number and pulled from the game controller singleton
    public void EquipAbility(int slot, int id) {
        var ability = AbilityCollectionSingleton.Instance.GetAbilityByID(id);
        UpdateIcons();
        spellWrappers[slot] = Instantiate(abilityWrapperPrefab, transform);
        spellWrappers[slot].Init(ability, spellTimers[slot]);
        spellBarImages[slot].GetComponent<Image>().sprite = ability.iconImage;
    }
    public void EquipAbility(int slot, Ability ability) {
        UpdateIcons();
        spellWrappers[slot] = Instantiate(abilityWrapperPrefab, transform);
        spellWrappers[slot].Init(ability, spellTimers[slot]);
        spellBarImages[slot].GetComponent<Image>().sprite = ability.iconImage;
    }
    //unequip the ability in the slot slot
    public void UnEquipAbility(int slot) {
        Destroy(spellWrappers[slot]);
        spellWrappers[slot] = null;
        spellBarImages[slot].GetComponent<Image>().sprite = noAbilityEquippedImage;
    }
    //cast the ability in the current slot
    public void Cast(int slot) {
        //if the cast was successful play the cast animation
        //dont play cast animation for movement skills
        if (spellWrappers[slot].Cast(player)) {
            if (!spellWrappers[slot].ability.tags.Contains("Movement")) {
                Debug.Log("casting animation");
                player.PlayCastAnimation();
            }
        }
    }
    //update the icons on the ability and aura bars
    //checks if there is an ability or aura equipped in the slot and sets the sprite
    //to the default image if there isnt one equipped
    public void UpdateIcons() {
        for (int x = 0; x < spellWrappers.Length; x++) {
            if (spellWrappers[x] == null)
                spellBarImages[x].GetComponent<Image>().sprite = noAbilityEquippedImage;
        }
        for (int x = 0; x < auraWrappers.Length; x++) {
            if (auraWrappers[x] == null)
                auraBarImages[x].GetComponent<Image>().sprite = noAbilityEquippedImage;
        }
    }
    #endregion

    #region Aura Bar
    public void EquipAura(int slot, Aura aura) {
        UpdateIcons();
        if (slot < 0 || slot >= auraWrappers.Length) { return; }
        if (auraWrappers[slot] != null) {
            UnEquipAura(slot);
        }
        auraWrappers[slot] = Instantiate(auraWrapperPrefab, transform);
        auraWrappers[slot].Init(aura, auraBarImages[slot]);

        auraWrappers[slot].EquipAura(player);
    }


    public void UnEquipAura(int slot) {
        if (slot < 0 || slot >= auraWrappers.Length) { return; }
        auraWrappers[slot].UnEquipAura(player);
        auraWrappers[slot] = null;
        auraWrappers[slot].GetComponent<Image>().sprite = noAbilityEquippedImage;
    }
    #endregion

}
