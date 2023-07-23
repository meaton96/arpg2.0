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
    
    //displays the buff icon on the buff bar
    public void DisplayBuffIcon(Buff buff, ExpandingIconBar bar) {
        //get the icon and set the bar class variable
        displayImage = GetComponent<Image>();
        this.bar = bar;
        Buff = buff;
        timer = Buff.duration;
       
        //check if the buff doesnt expire
        flagInfDuration = timer < 0;
        //set image
        displayImage.sprite = Buff.iconImage;
        //set text if it has a timer
        if (flagInfDuration)
            durationText.text = "";
        else
            durationText.text = timer.ToString("#.##");

    }

    // Update is called once per frame
    void Update() {
        if (flagInfDuration) { }
        else {
            //update timer and text if applicable
            if (timer >= 0) {
                timer -= Time.deltaTime;
                durationText.text = timer.ToString("#.##");
            }
            else {
                //remove the buff at the end of the duration
                bar.RemoveBuffFromList(this);

                Destroy(gameObject);
            }
        }

    }
    public void RemoveBuff() {
        //remove effect from player, remove it from the buff bar, then destroy
        GameController.Instance.player.RemoveBuff(Buff);
        bar.RemoveBuffFromList(this);

        Destroy(gameObject);
    }
}
