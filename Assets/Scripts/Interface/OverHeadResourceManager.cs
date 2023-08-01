using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeadResourceManager : MonoBehaviour
{
   
    [SerializeField] GameObject healthBar;
    [SerializeField] private StatManager StatManager;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        var scale = healthBar.transform.localScale; 
        scale.x = Mathf.Clamp01(StatManager.GetPercentHealth());
        healthBar.transform.localScale = scale;
    }
}
