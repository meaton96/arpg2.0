using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class ExpandingIconBar : MonoBehaviour
{
    public List<BuffUIWrapper> ActiveBuffs;
    int numBuffs = 0;
    [SerializeField] private GameObject BuffUIWrapperPrefab;
    public float IconSize = 50;
    public float IconSpacing = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        ActiveBuffs = new();
    }
    //adds the buff the to buff bar
    public void AddBuff(Buff buff) {
        //create the buff wrapper
        var buffWrapper = Instantiate(BuffUIWrapperPrefab, transform).GetComponent<BuffUIWrapper>();
        //get how far to display it based on the number of buffs + size and spacing
        var distance = numBuffs * (IconSize + IconSpacing);
        //set buff wrapper location
        buffWrapper.transform.localPosition = new Vector3(distance, 0, 0);
        //set buff wrapper icon
        buffWrapper.DisplayBuffIcon(buff, this);

        numBuffs++;
        //add the buff wrapper to the storage
        ActiveBuffs.Add(buffWrapper);   
        
    }
    public void RemoveBuffFromList(BuffUIWrapper b) {
        ActiveBuffs.Remove(b);
        numBuffs--;
    }
    //removes the buff from the display and the list
    //use to terminate a buff early
    public void RemoveBuff(Buff buff) {
        try {
            //get the buff from the list and remove it
            var buffWrapper = ActiveBuffs.SingleOrDefault(b => b.Buff.id == buff.id);
            buffWrapper.RemoveBuff();
        }
        catch (System.Exception) {
            Debug.Log("no buff found to remove - shouldnt ever happen");
        }
        
    }
    
}
