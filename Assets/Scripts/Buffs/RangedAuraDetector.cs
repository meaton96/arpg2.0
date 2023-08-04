using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAuraDetector : MonoBehaviour
{
    [HideInInspector] public Buff Buff;
    [HideInInspector] public bool middleEnabled = true;
    [SerializeField] private GameObject MiddlePiece, OutsidePiece;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init(bool middleEnabled, Buff buff) {
        Buff = buff;
        this.middleEnabled = middleEnabled;
        if (!middleEnabled) {
            MiddlePiece.SetActive(false);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
