using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageToast : MonoBehaviour {
    [SerializeField] private float movementSpeed = 0.7f;
    public const float TOAST_DURATION = 2f;
    public readonly Vector3 TOAST_MOVEMENT_DIR = new(.577f, 1.1547f, 0);
    public readonly List<Vector3> popUpOffsets = new() {
        new Vector3 (0,0,0),
        new Vector3 (.75f,0,0),
        new Vector3 (-.75f,0,0),
        new Vector3 (0,.75f,0),
        new Vector3 (0,-.75f,0),
        new Vector3 (.75f,.75f,0),
        new Vector3 (-.75f,.75f,0),
        new Vector3 (.75f,-.75f,0),
        new Vector3 (-.75f,-.75f,0)
    };
    public static int offSetIndex = 0;
    // Start is called before the first frame update
    void Start() {
        transform.position += popUpOffsets[offSetIndex++ % popUpOffsets.Count];
        if (offSetIndex > 10000) {
            offSetIndex = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        Destroy(gameObject, TOAST_DURATION);
        transform.Translate(movementSpeed * TOAST_MOVEMENT_DIR);
    }
    public void SetDamageAmount(float amount) {
        GetComponent<TextMeshPro>().text = amount.ToString();
    }
}
