using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Variable : MonoBehaviour
{
   public static Camera Camera;
    private void Start()
    {
        Camera = GetComponent<Camera>();
    }


}
