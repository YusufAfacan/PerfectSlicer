using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerHullMovement : MonoBehaviour
{
    
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime * Vector3.down;

        if (transform.position.y <= -5)
        {
            Destroy(gameObject);
        }
    }

   
}
