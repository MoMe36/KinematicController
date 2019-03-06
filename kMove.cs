using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinematic; 

namespace Kinematic
{

[System.Serializable]
public class MoveParams
{
    public float MaxSpeed = 1f; 
    public float Acceleration = 1f; 
    public float Decceleration = 1f; 
    public float JumpSpeed =1f; 
    public float RotationSpeed = 1f; 
}

[System.Serializable]
public class GravityParams
{
    public float Gravity = 1f; 
    public float GroundedGravity = 1f; 
    public float MaxFallSpeed = 1f; 
}

public class kUtils
{
    public static float Remap(float value_, float min_before, float max_before, float min_current, float max_current)
    {
        float t= (value_ - min_before)/(max_before - min_before); 

        return Mathf.LerpUnclamped(min_current, max_current, t); 
    }

}
    


[RequireComponent(typeof(CharacterController))]
public class kMove : MonoBehaviour {

    [Header("Move")]

    public MoveParams move_params; 
    float target_speed; 
    float actual_speed; 
    float acceleration; 
    float vertical_speed; 
    Vector3 last_horizontal_direction; 

    [Header("Fall")]

    public GravityParams gravity_params; 
    public bool Grounded; 

    [HideInInspector] public CharacterController controller; 
    Camera cam; 
    Animator anim; 


    // Use this for initialization
    void Start () {

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();  
        cam = Camera.main; 
        
    }
    
    // Update is called once per frame
    void Update () {

        UpdateGrounded(); 
        UpdateAnim(); 
    }

    void UpdateAnim()
    {
        anim.SetBool("IsGrounded", Grounded); 

        float remaped_horizontal = kUtils.Remap(actual_speed, 0f, move_params.MaxSpeed, 0f, 1f); 
        float remaped_vertical = kUtils.Remap(vertical_speed, -gravity_params.MaxFallSpeed, move_params.JumpSpeed, -1f, 1f); 

        anim.SetFloat("HorizontalSpeed",remaped_horizontal); 
        anim.SetFloat("VerticalSpeed", remaped_horizontal); 
    }


    void UpdateGrounded()
    {
        Ray ray = new Ray(transform.position + controller.center, Vector3.down); 
        RaycastHit hit; 

        // Color ray_color = Grounded ? Color.red : Color.blue; 
        // Debug.DrawRay(transform.position + controller.center + Vector3.right, Vector3.down*controller.height*0.55f, ray_color, 0.1f); 

        if(Physics.Raycast(ray, out hit, 0.55f*controller.height))
        {
            Grounded = true; 
        }
        else
        {
            Grounded = false; 
        }
    }

    public void Move(Vector3 direction, bool jump)
    {
        // ==================== HORIZONTAL SPEED ==================== 

        if(direction.magnitude > 0.2f)
        {
            target_speed = direction.magnitude*move_params.MaxSpeed; 
            acceleration = move_params.Acceleration; 
            Vector3 ideal_direction = ProjectInputDirection(direction); 
            float angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(transform.forward, Vector3.up), ideal_direction, Vector3.up); 
            transform.forward = Vector3.Lerp(transform.forward, Quaternion.AngleAxis(angle, Vector3.up)*transform.forward, move_params.RotationSpeed*Time.deltaTime); 
            last_horizontal_direction = transform.forward;
        }
        else
        {
            target_speed = 0f;
            acceleration = move_params.Decceleration;  
        }
        actual_speed = Mathf.MoveTowards(actual_speed, target_speed, Time.deltaTime*acceleration); 

        // ==================== VERTICAL SPEED ====================

        if(Grounded)
        {
            vertical_speed = -gravity_params.GroundedGravity; 
            if(jump)
                vertical_speed = move_params.JumpSpeed; 
        }
        else
        {
            vertical_speed = Mathf.MoveTowards(vertical_speed, -gravity_params.MaxFallSpeed, gravity_params.Gravity*Time.deltaTime); 
        }
      

        // ==================== TOTAL SPEED ====================

        Vector3 move_dir = last_horizontal_direction*actual_speed + vertical_speed*Vector3.up; 
        controller.Move(move_dir*Time.deltaTime); 
    }

    Vector3 ProjectInputDirection(Vector3 player_input)
    {
        Vector3 cam_forward = Vector3.ProjectOnPlane(transform.position - cam.transform.position, Vector3.up); 
        Vector3 cam_right = Quaternion.AngleAxis(90, Vector3.up)*cam_forward; 

        return (cam_forward*player_input.z + cam_right*player_input.x).normalized; 
    }
}
}
