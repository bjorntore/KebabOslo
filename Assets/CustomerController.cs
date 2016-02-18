using UnityEngine;
using System.Collections;
using EpPathFinding.cs;

public class CustomerController : MonoBehaviour
{

    public Customer customer;

    int moveSpeed = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!customer.HasArrived())
        {
            if (transform.position.x == customer.x && transform.position.z == customer.z)
                customer.MoveToNextPos();

            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(customer.x, transform.position.y, customer.z), step);
        }
    }

}
