using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinematic; 

namespace Kinematic{



[RequireComponent(typeof(kInputs), typeof(kMove))]
public class kBase : MonoBehaviour {


    public enum States {normal, jump, hit, shoot}; 
    public States current_state; 
    
    [HideInInspector] public kMove move; 
    [HideInInspector] public kInputs inputs;
    public kCam cam; 

    Animator anim; 

    // Use this for initialization
    void Start () {

        // controller = GetComponent<CharacterController>(); 
        move = GetComponent<kMove>(); 
        inputs = GetComponent<kInputs>(); 
        anim = GetComponent<Animator>(); 
        
    }
    
    // Update is called once per frame
    void Update () {


        Vector3 direction = new Vector3(inputs.Direction.x, 0f, inputs.Direction.y); 
        move.Move(direction, inputs.Jump); 
        cam.Rotate(inputs.CamDirection); 
        
    }
}
}
