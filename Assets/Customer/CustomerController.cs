using UnityEngine;
using System.Collections;
using EpPathFinding.cs;
using System;

public class CustomerController : MonoBehaviour
{

    WorldController worldController;

    public Customer customer;
    public Customer Customer { get { return customer; } }

    public GameObject stateGameObject;

    public Transform bodyTransform;
    public float baseBodyScaleX;
    public float baseBodyScaleZ;

    public void SetCustomer(Customer customer)
    {
        this.customer = customer;
    }

	// Use this for initialization
	private void Start () {
        worldController = WorldController.Instance();

        baseBodyScaleX = bodyTransform.localScale.x;
        baseBodyScaleZ = bodyTransform.localScale.z;
        bodyTransform.localScale = GetBodyScale(customer.hunger);
    }
	
	// Update is called once per frame
	private void Update () {
        if (transform.position.x != customer.destinationX || transform.position.z != customer.destinationZ) // Has to use transform.position.x so the algorithm stops running when its exactly on destination. Cant use HasArrived() which is an rounded position.
            Move();
        else  // This only triggers when arrived
        {
            StateHandler();
            MoodHandler();
        }
    }

    private void StateHandler()
    {
        if (customer.State == CustomerState.MovingToEat)
        {
            customer.TriggerArrivedAtKebabBuilding();
            worldController.world.player.ChangeCash(10);
        }
        else if (customer.State == CustomerState.MovingToOrigin || customer.State == CustomerState.MovingToMapEnd) // This only triggers when arrived
        {
            worldController.world.RemoveCustomer(customer);
            Destroy(gameObject);
        }
        else if (customer.State == CustomerState.Eating && Time.time >= customer.eatingUntil)
        {
            customer.StopEating();
            bodyTransform.localScale = GetBodyScale(customer.hunger);
        }
    }

    private void MoodHandler()
    {
        if (customer.Mood == CustomerMood.AngryNoCapacity)
            stateGameObject.SetActive(true);
        else
            stateGameObject.SetActive(false);
    }

    private void Move()
    {
        if (transform.position.x == customer.movingToX && transform.position.z == customer.movingToZ)
            customer.SetNextMovingToPosition();

        float step = customer.moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(customer.movingToX, transform.position.y, customer.movingToZ), step);

        customer.SetPosition((int)Math.Round(transform.position.x, 0), (int)Math.Round(transform.position.z, 0));
    }

    private Vector3 GetBodyScale(int hunger)
    {
        float scaleX = baseBodyScaleX - (0.4f * hunger / 100.0f);
        float scaleZ = baseBodyScaleZ - (0.4f * hunger / 100.0f);
        return new Vector3(scaleX, bodyTransform.localScale.y, scaleZ);
    }

}
