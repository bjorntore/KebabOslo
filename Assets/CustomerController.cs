using UnityEngine;
using System.Collections;
using EpPathFinding.cs;
using System;

public class CustomerController : MonoBehaviour
{

    public Customer customer;

    int moveSpeed = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x != customer.destinationX || transform.position.z != customer.destinationZ)
        {
            if (transform.position.x == customer.movingToX && transform.position.z == customer.movingToZ)
                customer.MoveToNextPos();

            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(customer.movingToX, transform.position.y, customer.movingToZ), step);
            customer.x = (int)Math.Round(transform.position.x, 0);
            customer.z = (int)Math.Round(transform.position.z, 0);
        }
    }

}
