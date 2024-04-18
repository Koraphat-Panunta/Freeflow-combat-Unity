using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider Colli;
    void Start()
    {
        
    }
    float push_able_duration = 0.8f;

    // Update is called once per frame
    void Update()
    {
        if(push_able == false) 
        {
            push_able_duration -= Time.deltaTime;   
            if(push_able_duration <= 0) 
            {
                push_able_duration = 0.8f;
                push_able = true;
            }
        }
    }
    bool push_able = true;
    public void Push(float force,Vector3 Direction) 
    {
        if (push_able == true)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Direction * force);
            Debug.Log("Kick");
            push_able = false;
        }
        
    }
}
