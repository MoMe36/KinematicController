using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinematic; 

namespace Kinematic{

public class kCam : MonoBehaviour {

    [HideInInspector] kBase k_base;

    [Header("Target Parameters")]
    public Transform Target; 
    public Vector3 LookAtOffset;
    public float LerpSpeed = 1f;
    Vector3 Offset;  
    
    [Header("Rotation Parameters")]
    public Vector2 RotationSpeed; 
    public Vector2 yLim; 
    public Vector2 Rotations; 

    Vector3 velocity; 

    // Use this for initialization
    void Start () {

        Offset = transform.position - Target.position; 
        k_base = GetComponent<kBase>(); 
        
    }
    
    // Update is called once per frame
    void Update () {

        UpdateTransform(); 
    }

    void UpdateTransform()
    {
        Quaternion rotation = Quaternion.Euler(Rotations.y, Rotations.x, 0f); 
        Vector3 ideal_position = Target.position + rotation*Offset; 
        transform.position = Vector3.SmoothDamp(transform.position, ideal_position, ref velocity, Time.deltaTime*LerpSpeed); 
        transform.LookAt(Target.position + transform.rotation*LookAtOffset); 
    }

    public void Rotate(Vector2 inputs)
    {
        Rotations.x = (Rotations.x + inputs.x*RotationSpeed.x)%360f;  
        Rotations.y = Mathf.Clamp(Rotations.y + inputs.y*RotationSpeed.y, yLim.x, yLim.y); 
    }
}
}
