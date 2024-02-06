using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator playerAnim;
    public Rigidbody playerRigid;
	public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
	public bool walking;

    public float speed = 6f;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }


        if(Input.GetKeyDown(KeyCode.W)){
			playerAnim.SetTrigger("walk");
			//steps1.SetActive(true);
		}
    

        if(Input.GetKeyDown(KeyCode.W)){
			playerAnim.SetTrigger("walk");
			playerAnim.ResetTrigger("idle");
			walking = true;
			//steps1.SetActive(true);
		}
		if(Input.GetKeyUp(KeyCode.W)){
			playerAnim.ResetTrigger("walk");
			playerAnim.SetTrigger("idle");
			walking = false;
			//steps1.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.S)){
			playerAnim.SetTrigger("walk-back");
			playerAnim.ResetTrigger("idle");
			//steps1.SetActive(true);
		}
		if(Input.GetKeyUp(KeyCode.S)){
			playerAnim.ResetTrigger("walk-back");
			playerAnim.SetTrigger("idle");
			//steps1.SetActive(false);
		}
        if (walking)
{
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
        w_speed = w_speed + rn_speed;
        playerAnim.SetTrigger("run");
        speed = 12;
        playerAnim.ResetTrigger("walk");
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
        w_speed = olw_speed;
        playerAnim.ResetTrigger("run");
        speed = 6;

        playerAnim.SetTrigger("walk");
    }
}
		if (Input.GetKey(KeyCode.A))
{
    cam.Rotate(0, -ro_speed * Time.deltaTime, 0);
    transform.Rotate(Vector3.down * ro_speed * Time.deltaTime);
    if (!walking) // Check if not walking to avoid interference with walking animation
    {
        playerAnim.SetTrigger("walk"); // Assuming you have a trigger named "turnLeft"
    }
}
else if (Input.GetKeyUp(KeyCode.A)) // Added else if to handle key release
{
    if (!walking) // Check if not walking to avoid interference with walking animation
    {
        playerAnim.ResetTrigger("walk");
        playerAnim.SetTrigger("idle"); // Set idle trigger when A key is released
    }
}

if (Input.GetKey(KeyCode.D))
{
    cam.Rotate(0, ro_speed * Time.deltaTime, 0);
    transform.Rotate(Vector3.up * ro_speed * Time.deltaTime);
    if (!walking) // Check if not walking to avoid interference with walking animation
    {
        playerAnim.SetTrigger("walk"); // Assuming you have a trigger named "turnRight"
    }
}
else if (Input.GetKeyUp(KeyCode.D)) // Added else if to handle key release
{
    if (!walking) // Check if not walking to avoid interference with walking animation
    {
        playerAnim.ResetTrigger("walk");
        playerAnim.SetTrigger("idle"); // Set idle trigger when D key is released
    }
}

	}
    
}
