using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour {

    //[SerializeField] private GameObject[] spellBarImages;
    //[SerializeField] private GameObject[] auraBarImages;
    //[SerializeField] private Sprite noAbilityEquippedImage;
    [SerializeField] private Player player;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject manaBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI characterDataText;
    [SerializeField] private GameObject charPanel;
    [SerializeField] private ExpandingIconBar BuffBar;
    private bool charPanelDisplayed = false;
    ResourceManager playerResourceManager;

    public bool showHealthAndManaNumbers = true;
    // Start is called before the first frame update
    void Start() {

        playerResourceManager = player.resourceManager;

    }

    // Update is called once per frame
    void FixedUpdate() {
        //scale health and mana bars
        float healthPercent = playerResourceManager.currentHealth / playerResourceManager.maxHealth;
        float manaPercent = playerResourceManager.currentMana / playerResourceManager.maxMana;

        healthBar.transform.localScale = new Vector3(1, healthPercent, 1);
        manaBar.transform.localScale = new Vector3(1, manaPercent, 1);

        //update health and mana text
        healthText.text = playerResourceManager.currentHealth.ToString("0.#") + "/" + playerResourceManager.maxHealth;
        manaText.text = playerResourceManager.currentMana.ToString("0.#") + "/" + playerResourceManager.maxMana;

    }
    private void Update() {
        if (Input.GetKeyDown(Player.KEY_CODE_CHAR_PANEL)) {
            ToggleCharacterPanel();
        }
        UpdateCharacterPanelInfo();
    }

    void UpdateCharacterPanelInfo() {
        characterDataText.text = player.ToString();
    }
    //enable or disable the character panel on key press
    public void ToggleCharacterPanel() {
        charPanelDisplayed = !charPanelDisplayed;
        charPanel.SetActive(charPanelDisplayed);
        

    }
    //pass a new buff to the buff bar to display
    public void DisplayNewBuff(Buff buff) {
        BuffBar.AddBuff(buff);
    }
    //remove the buff from the buff bar
    public void ForceRemoveBuff(Buff buff) {
        BuffBar.RemoveBuff(buff);
    }
}
