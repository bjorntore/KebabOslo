using UnityEngine;
using System.Collections;

namespace UnityBuildingsTest
{
    public class CameraMovement : MonoBehaviour
    {
        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical") * Mathf.Sin(transform.rotation.eulerAngles.x * (Mathf.PI / 180));
            float z = Input.GetAxis("Vertical") * Mathf.Cos(transform.rotation.eulerAngles.x * (Mathf.PI / 180));

            transform.Translate(x, y, z, Space.Self);
        }


    }
}