using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraVisual : MonoBehaviour {
    [SerializeField] private GameObject outerVisual;
    [SerializeField] private float rotSpeed =  100f;

    // Update is called once per frame
    void Update() {
        outerVisual.transform.Rotate(new Vector3(0, 0, rotSpeed * Time.deltaTime));
    }
}
