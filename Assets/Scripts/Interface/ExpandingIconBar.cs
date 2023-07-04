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
        var distance = numBuffs * (IconSize + IconSpacing);
        buffWrapper.transform.localPosition = new Vector3(distance, 0, 0);
        buffWrapper.DisplayBuffIcon(buff, this);
        numBuffs++;
        ActiveBuffs.Add(buffWrapper);   
        
    }
    public void RemoveBuff() {
        numBuffs--;
    }
    public void RemoveBuff(Buff buff) {
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
