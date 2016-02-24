using UnityEngine;
using System.Collections;
using EpPathFinding.cs;
using System;

public class CustomerController : MonoBehaviour
{

    public Customer customer;
    public Customer Customer { get { return customer; } }

    public Transform bodyTransform;
    public float baseBodyScaleX;
    public float baseBodyScaleZ;

    public void SetCustomer(Customer customer)
    {
        this.customer = customer;
    }

	// Use this for initialization
	void Start () {
        baseBodyScaleX = bodyTransform.localScale.x;
        baseBodyScaleZ = bodyTransform.localScale.z;
        bodyTransform.localScale = GetBodyScale(customer.hunger);
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x != customer.destinationX || transform.position.z != customer.destinationZ) // Has to use transform.position.x so the algorithm stops running when its exactly on destination. Cant use HasArrived() which is an rounded position.
        {
            if (transform.position.x == customer.movingToX && transform.position.z == customer.movingToZ)
                customer.SetNextMovingToPosition();

            float step = customer.moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(customer.movingToX, transform.position.y, customer.movingToZ), step);

            customer.SetPosition((int)Math.Round(transform.position.x, 0), (int)Math.Round(transform.position.z, 0));
        }
        else // Has arrived at exact destination
        {
            customer.TriggerArrived();

            WorldController worldController = WorldController.Instance();

            if (customer.DestinationBuilding is KebabBuilding)
            {
                worldController.player.ChangeCash(10);
                bodyTransform.localScale = GetBodyScale(customer.hunger);
                customer.DecideDestinationAndPath();
            }
            else
            {
                worldController.world.RemoveCustomer(customer);
                Destroy(gameObject);
            }

        }
    }

    private Vector3 GetBodyScale(int hunger)
    {
        float scaleX = baseBodyScaleX - (0.4f * hunger / 100.0f);
        float scaleZ = baseBodyScaleZ - (0.4f * hunger / 100.0f);
        return new Vector3(scaleX, bodyTransform.localScale.y, scaleZ);
    }

}
