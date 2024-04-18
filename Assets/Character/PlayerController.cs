using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Rigidbody rb;
    public float jog_speed;
    public float sprint_speed;
    public float rotate_speed;
    [SerializeField] private Camera Third_person_camera;
   
    public double delta_angle;
    private float original_rotate_speed;
    public bool StateChange_able = true;
    public float kickforce = 280;
    public float angle_to_target;
    public float camera_rot;
    public float player_rot;
    public List<GameObject> Enemy = new List<GameObject>();

    public Collider Atk_Colli;
    double Timing = 0;
   
    void Start()
    {
        original_rotate_speed = rotate_speed;
        Original_drag = rb.drag;
    }

    // Update is called once per frame
    void Update()
    {
        Timing += Time.deltaTime;
       StateUpdate();
        if (Input.GetMouseButtonDown(0)) 
        {
            StateChange_able = false;
            Debug.Log("Click_start : "+Timing);
            ResetAllTrigger();
            animator.SetTrigger("Kick");
            rb.velocity = Vector3.zero;
            
            if (this.Target != null)
            {
                float Distance = Vector3.Distance(gameObject.transform.position, this.Target.transform.position);
                float MaxDistance = 5f;
                if(Distance < MaxDistance) 
                {
                    rb.AddForce(gameObject.transform.forward * kickforce * Distance);
                    Debug.Log("Addforec");
                    
                }
                else 
                {
                   rb.AddForce(gameObject.transform.forward * kickforce * MaxDistance);
                    Debug.Log("Addforec");
                }
               
            }           
            {
                gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * kickforce);
                Debug.Log("Addforec");
            }
            rb.drag = 0f;
           
        }
        else if (StateChange_able == true)
        {
            Debug.Log("Checkpress");
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) && (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick") == false))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y - 45);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y + 45);
                    }
                    else
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y);
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {

                    if (Input.GetKey(KeyCode.A))
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y + 45);
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y - 45);
                    }
                    else
                    {
                        Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y);
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y - 90f);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    Rotate_To_Y(Third_person_camera.transform.rotation.eulerAngles.y + 90f);
                }


                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rb.velocity = gameObject.transform.forward * sprint_speed * Time.deltaTime;
                    rotate_speed = original_rotate_speed * 0.6f;
                }
                else
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        rb.velocity = -(gameObject.transform.forward * jog_speed * Time.deltaTime);
                    }
                    else
                    {
                        rb.velocity = gameObject.transform.forward * jog_speed * Time.deltaTime;
                    }

                    rotate_speed = original_rotate_speed;
                }

            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick")) 
        {
            Debug.Log("KickStart : "+Timing);
        }
       
        freeflow_system();
        camera_rot = Third_person_camera.transform.rotation.eulerAngles.y;
        player_rot = gameObject.transform.rotation.eulerAngles.y;
    }
    private void FixedUpdate()
    {
        if (Input.anyKey == false)
        {
            ResetAllTrigger();
            animator.SetTrigger("idle");

        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                ResetAllTrigger();
                animator.SetTrigger("jog");
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ResetAllTrigger();
                    animator.SetTrigger("sprint");
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ResetAllTrigger();
                animator.SetTrigger("jogback");
            }
            else if (Input.GetKey(KeyCode.A))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ResetAllTrigger();
                    animator.SetTrigger("sprint");
                }
                else
                {
                    ResetAllTrigger();
                    animator.SetTrigger("jog");
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ResetAllTrigger();
                    animator.SetTrigger("sprint");
                }
                else
                {
                    ResetAllTrigger();
                    animator.SetTrigger("jog");
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (animator.GetBool("Dance") == false)
                {
                    ResetAllTrigger();
                    animator.SetBool("Dance", true);
                }
                else if (animator.GetBool("Dance") == true)
                {
                    ResetAllTrigger();
                    animator.SetBool("Dance", false);
                }
            }

        }
    }
    private void ResetAllTrigger()
    {
        animator.ResetTrigger("idle");
        animator.ResetTrigger("jog");
        animator.ResetTrigger("sprint");
        animator.ResetTrigger("jogback");

    }
    private void SetRotate_Y(float Y)
    {
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, Y, gameObject.transform.rotation.eulerAngles.z);
    }
    private void Rotate_To_Y(float euler_rotation)
    {
        //delta_angle = math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation);
        float DeadZone_Rotate = 2f;
        if (euler_rotation > 360) 
        {
            euler_rotation -= 360;
        }
        if(euler_rotation < 0) 
        {
            euler_rotation += 360;
        }
        if (math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation) <= 180)
        {
            if (gameObject.transform.rotation.eulerAngles.y > euler_rotation + DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, -rotate_speed * Time.deltaTime, 0);
                
            }
            else if (gameObject.transform.rotation.eulerAngles.y < euler_rotation- DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, rotate_speed * Time.deltaTime, 0);

            }
        }
        else if (math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation) > 180)
        {
            if (gameObject.transform.rotation.eulerAngles.y > euler_rotation+ DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, rotate_speed * Time.deltaTime, 0);

            }

            else if (gameObject.transform.rotation.eulerAngles.y < euler_rotation-DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, -rotate_speed * Time.deltaTime, 0);

            }
        }
    }
    private void Rotate_To_Y(float euler_rotation,float rotspeed_multiply)
    {
        //delta_angle = math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation);
        float DeadZone_Rotate = 2f;
        if (euler_rotation > 360)
        {
            euler_rotation -= 360;
        }
        if (euler_rotation < 0)
        {
            euler_rotation += 360;
        }
        if (math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation) <= 180)
        {
            if (gameObject.transform.rotation.eulerAngles.y > euler_rotation + DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, -rotate_speed*rotspeed_multiply * Time.deltaTime, 0);

            }
            else if (gameObject.transform.rotation.eulerAngles.y < euler_rotation - DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, rotate_speed * rotspeed_multiply * Time.deltaTime, 0);

            }
        }
        else if (math.abs(gameObject.transform.rotation.eulerAngles.y - euler_rotation) > 180)
        {
            if (gameObject.transform.rotation.eulerAngles.y > euler_rotation + DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, rotate_speed * rotspeed_multiply * Time.deltaTime, 0);

            }

            else if (gameObject.transform.rotation.eulerAngles.y < euler_rotation - DeadZone_Rotate)
            {
                gameObject.transform.Rotate(0, -rotate_speed * rotspeed_multiply * Time.deltaTime, 0);

            }
        }
    }
    private float Original_drag; 
    private void StateUpdate() 
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick"))
        {
            rb.drag = 0f;
            StateChange_able = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle")) 
        {
            rb.drag = Original_drag;
            StateChange_able = true;
        }
    }
    public LayerMask layerMask;
    public GameObject Target;
    
    private void freeflow_system() 
    {       
       foreach(GameObject enemy in Enemy) 
        {
           
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick")) 
            {
                
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.45f) 
                {
                    
                    if (Atk_Colli.bounds.Intersects(enemy.GetComponent<Enemy>().Colli.bounds)) 
                    {                       
                        enemy.GetComponent<Enemy>().Push(600, gameObject.transform.forward);
                    }
                }
            }
            
        }
        if (Physics.SphereCast(gameObject.transform.position,0.8f,gameObject.transform.forward*1000,out RaycastHit Target,1000,layerMask))
        {            
            if (Target.rigidbody.CompareTag("Target")) 
            {
                Debug.Log("Detect");
                this.Target = Target.rigidbody.gameObject;
                float this_angle_target;
                Vector3 vector3 = Target.transform.position - transform.position;
                this_angle_target = math.atan2(vector3.x, vector3.z) * Mathf.Rad2Deg;
                if (this_angle_target < 0)
                {
                    this_angle_target += 360;
                }
                angle_to_target = this_angle_target;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinKick") && this.Target != null) 
        {
           
            Rotate_To_Y(angle_to_target, 1.2f);
        }
    }
   
}
