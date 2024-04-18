using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motionstopeffect : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    float frame_interval;
    float FPS = 60;
    public float animation_frame = 7;
    public float Timing_Update = 0;
    public bool stop_motion;
    void Start()
    {
        Application.targetFrameRate = (int)FPS;        
        Time.fixedDeltaTime = 1f/FPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(stop_motion == true) 
        {
            frame_interval = (float)(1f / animation_frame);
            animator.speed = 0;
            Timing_Update += Time.fixedDeltaTime;
            if (Timing_Update >= frame_interval)
            {
                Timing_Update = 0;
                animator.speed =(float)(FPS / animation_frame);
            }
        }
        else 
        {
            animator.speed = 1;
        }
        
    }
}
