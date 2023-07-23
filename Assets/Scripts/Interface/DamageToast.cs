using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageToast : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1;
    public const float TOAST_DURATION = 2f;
    public readonly Vector3 TOAST_MOVEMENT_DIR = new(.577f, 1.1547f, 0);
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, TOAST_DURATION);
        transform.Translate(movementSpeed * TOAST_MOVEMENT_DIR) ;
    }
    public void SetDamageAmount(float amount) {
        GetComponent<TextMeshPro>().text = amount.ToString();
    }
    public void SetInitialPosition(Vector3 pos) {
       transform.position = pos;
    }
}
