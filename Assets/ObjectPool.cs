/*
 * Inspired by: https://blogs.msdn.microsoft.com/dave_crooks_dev_blog/2014/07/21/object-pooling-for-unity3d/
 */
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private Stack<GameObject> pooledObjects;

    private GameObject pooledObj; // Prefab

    private int initialPoolSize;

    private GameObject parent;

    public ObjectPool(GameObject obj, int initialPoolSize, GameObject parent)
    {
        pooledObjects = new Stack<GameObject>();
        this.pooledObj = obj;
        this.parent = parent;
        this.initialPoolSize = initialPoolSize;

        for (int i = 0; i < initialPoolSize; i++)
            CreateGameObject();
    }

    public GameObject GetObject()
    {
        if (pooledObjects.Count > 0)
            return pooledObjects.Pop();

        return CreateGameObject();
    }

    public void ReleaseObject(GameObject releasedObject)
    {
        releasedObject.SetActive(false);
        pooledObjects.Push(releasedObject);
    }

    private GameObject CreateGameObject()
    {
        Debug.Log("Creating new customer object");
        GameObject newObject = GameObject.Instantiate(pooledObj, Vector3.zero, Quaternion.identity) as GameObject;
        newObject.transform.parent = parent.transform;
        newObject.SetActive(false);
        pooledObjects.Push(newObject);
        return newObject;
    } 

}
