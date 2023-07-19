using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeadResourceManager : MonoBehaviour
{
   
    [SerializeField] GameObject healthBar;
    //[SerializeField] GameCharacter character;
    [SerializeField] private ResourceManager resourceManager;
    // Start is called before the first frame update
    void Start()
    {
        //resourceManager = character.resourceManager;
    }
    public void SetResourceManager(ResourceManager resourceManager) {
        this.resourceManager = resourceManager;
    }

    // Update is called once per frame
    void Update()
    {

        var scale = healthBar.transform.localScale; 
        scale.x = Mathf.Clamp01(resourceManager.GetPercentHealth());
        healthBar.transform.localScale = scale;
    }
}
