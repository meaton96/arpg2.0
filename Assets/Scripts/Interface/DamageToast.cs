using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageToast : MonoBehaviour
{
    public const float TOAST_DURATION = 2f;
    private float toastTimer;
    public readonly Vector3 TOAST_MOVEMENT_DIR = new(0, .05f, 0);
    [SerializeField] private TextMeshProUGUI toastText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toastTimer > TOAST_DURATION) Destroy(gameObject);
        toastTimer += Time.deltaTime;
        transform.position += TOAST_MOVEMENT_DIR;
    }
    public void SetDamageAmount(float amount) {
        toastText.text = amount.ToString();
    }
}
