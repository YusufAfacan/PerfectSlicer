using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperHullMovement : MonoBehaviour
{
    public FoodMovement foodMovement;
    public float startingSpeed;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        foodMovement = FindObjectOfType<FoodMovement>();
        speed = foodMovement.startingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime * Vector3.left;
        Destroy(gameObject, 2f);
    }

    
}
