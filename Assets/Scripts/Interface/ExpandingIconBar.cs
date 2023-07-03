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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddBuff(Buff buff) {
        var buffWrapper = Instantiate(BuffUIWrapperPrefab,
            new Vector3((IconSize + IconSpacing) * numBuffs, 0, 0) + transform.position,
            Quaternion.identity).GetComponent<BuffUIWrapper>();
        numBuffs++;
        ActiveBuffs.Add(buffWrapper);   
        buffWrapper.DisplayBuffIcon(buff, this);
    }
    public void RemoveBuff() {
        numBuffs--;
    }
    public void RemoveBuff(Buff buff) {
        var buffWrapper = ActiveBuffs.Single(b => b.Buff._name == buff._name);
        buffWrapper.RemoveBuff();
        numBuffs--;
    }
    
}
