using UnityEngine;

public class ThirdPersonMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator playerAnim;
	public Rigidbody playerRigid;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
	public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
	public bool walking;
    
    public float gravity = 9.8f;
    private float verticalVelocity;
    private float turnSmoothVelocity;

    void Update()
    {
        HandleMovementInput();
        ApplyGravity();
        HandleAnimationTriggers();
        FixedUpdate();
    }

void FixedUpdate(){
		if(Input.GetKey(KeyCode.W)){
			playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S)){
			playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
		}
	}
    void HandleMovementInput()
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
    }

    void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    void HandleAnimationTriggers()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            walking = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            walking = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("walk");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("walk");
        }

        if (Input.GetKey(KeyCode.A))
        {
            RotateCharacter(-ro_speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            RotateCharacter(ro_speed);
        }

        if (walking)
        {
            HandleRunning();
        }
    }

    void RotateCharacter(float rotationSpeed)
    {   
        cam.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void HandleRunning()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            w_speed += rn_speed;
            playerAnim.SetTrigger("run");
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            w_speed = olw_speed;
            playerAnim.ResetTrigger("run");
        }
    }
}
