using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMovement : MonoBehaviour
{
    public float startingSpeed;
    private float speed;
    public int tolerance;
    

    // Start is called before the first frame update
    void Start()
    {
        
        speed = startingSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (transform.position.x <= 0 )
        {
            speed = 0;
            gameObject.layer = 3;
            ReduceTolerance();
        }

        transform.position = transform.position + speed * Time.deltaTime * Vector3.left;
    }

    IEnumerator ReduceTolerance()
    {
        yield return new WaitForSeconds(1);
        tolerance--;
        ReduceTolerance();
    }

}
