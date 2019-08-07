using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{

    public float Speed;
    public bool IsRotating = false;

    public bool RotateLeft;

    private int direction;
    private float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRotating)
            return;
        direction = RotateLeft ? 1 : -1;
        angle += Speed * direction * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
    }

    public void StartRotation() {
        IsRotating = true;
    }
    public void StopRotation() {
        IsRotating = false;
    }
}
