using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffUIWrapper : MonoBehaviour {
    public Buff Buff;
    float timer;
    private bool flagInfDuration = false;
    private Image displayImage;
    [SerializeField] private TextMeshProUGUI durationText;
    private ExpandingIconBar bar;
    // Start is called before the first frame update
    void Start() {
        displayImage = GetComponent<Image>();

    }
    public void DisplayBuffIcon(Buff buff, ExpandingIconBar bar) {
        this.bar = bar;
        this.Buff = buff;
        Debug.Log("buff created");
        timer = Buff.duration;
        flagInfDuration = timer < 0;
        displayImage.sprite = Buff.iconImage;
        if (flagInfDuration)
            durationText.text = "";
        else
            durationText.text = timer.ToString("#.##");

    }

    // Update is called once per frame
    void Update() {
        if (flagInfDuration) { }
        else {
            if (timer >= 0) {
                timer -= Time.deltaTime;
                durationText.text = timer.ToString("#.##");
            }
            else {
                RemoveBuff();
            }
        }

    }
    public void RemoveBuff() {
        Buff.RemoveEffect(GameController.Instance.player);
        bar.RemoveBuff();
        Destroy(gameObject);
    }
}
