using UnityEngine;
using System.Collections;

namespace UnityBuildingsTest
{
    public class CameraManager : MonoBehaviour
    {

        Camera mainCamera;

        void Start()
        {
            mainCamera = Camera.main;
        }

        void Update()
        {
            MoveCameraVertifalHorizontal();
            MoveCameraUpDown();
        }

        private void MoveCameraVertifalHorizontal()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical") * Mathf.Sin(transform.rotation.eulerAngles.x * (Mathf.PI / 180));
            float z = Input.GetAxis("Vertical") * Mathf.Cos(transform.rotation.eulerAngles.x * (Mathf.PI / 180));

            transform.Translate(x, y, z, Space.Self);
        }

        private void MoveCameraUpDown()
        {
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                mainCamera.orthographicSize *= 0.80f;
            }
            else if (d < 0f)
            {
                mainCamera.orthographicSize *= 1.20f;
            }
        }

    }
}