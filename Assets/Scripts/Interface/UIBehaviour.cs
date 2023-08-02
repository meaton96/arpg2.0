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
    [SerializeField] private ExpandingIconBar DebuffBar;
    private bool charPanelDisplayed = false;
    // ResourceManager playerResourceManager;
    public StatManager StatManager;
    [Range(1f, 3f)]
    public float uiScale = 1;

    private float previousScale;

    public bool showHealthAndManaNumbers = true;
    // Start is called before the first frame update
    void Start() {

        StatManager = player.StatManager;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void FixedUpdate() {
        //scale health and mana bars
     //   float healthPercent = ;
     //   float manaPercent = playerResourceManager.currentMana / playerResourceManager.maxMana;

        healthBar.transform.localScale = new Vector3(1, StatManager.GetPercentHealth(), 1);
        manaBar.transform.localScale = new Vector3(1, StatManager.GetPercentMana(), 1);

        //update health and mana text
        healthText.text = StatManager.GetCurrentHealth().ToString("0.#") + "/" + StatManager.GetMaxHealth();
        manaText.text = StatManager.GetCurrentMana().ToString("0.#") + "/" + StatManager.GetMaxMana();  

    }
    private void Update() {
        if (Input.GetKeyDown(Player.KEY_CODE_CHAR_PANEL)) {
            ToggleCharacterPanel();
        }
        UpdateCharacterPanelInfo();
        ScaleInterface();
        
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
    private void ScaleInterface() {
        transform.localScale = new Vector3(uiScale, uiScale, 1f);
        previousScale = uiScale;
    }

}
