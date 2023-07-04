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
    public void AddBuff(Buff buff) {
        var buffWrapper = Instantiate(BuffUIWrapperPrefab, transform).GetComponent<BuffUIWrapper>();
        //buffs not displaying on UI 
        numBuffs++;
        ActiveBuffs.Add(buffWrapper);   
        buffWrapper.DisplayBuffIcon(buff, this);
    }
    public void RemoveBuff() {
        numBuffs--;
    }
    public void RemoveBuff(Buff buff) {
        Debug.Log(ActiveBuffs.Count);
        try {
            var buffWrapper = ActiveBuffs.SingleOrDefault(b => b.Buff._name == buff._name);
            buffWrapper.RemoveBuff();
            numBuffs--;
        }
        catch (System.Exception) {
            Debug.Log("no buff found to remove - shouldnt ever happen");
        }
        
    }
    
}
