using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinematic; 

namespace Kinematic
{


public class kInputs : MonoBehaviour {

    public Vector2 Direction; 
    public Vector2 CamDirection; 
    public bool Jump; 

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {

        Direction.x = Input.GetAxis("Horizontal"); 
        Direction.y = Input.GetAxis("Vertical"); 

        CamDirection.x = Input.GetAxis("HorCam"); 
        CamDirection.y = Input.GetAxis("VerCam"); 

        Jump = Input.GetButton("AButton"); 
        
    }
}


}
