using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public float zoomOutMin = 1;
    public float zooomOutMax = 15;
     
    private Vector3 touchStart;


    public GameObject cameraParen;
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;
    void Update()
    {
        // click and hold mouse left button to pan, mouse middle wheel to zoomin/zoomout & right button to rotate

        // drag
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        // rotation     
        if (Input.GetMouseButton(1))
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            cameraParen.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
        
        // zoom
        ZoomOut(Input.GetAxis("Mouse ScrollWheel") * 2f);
    }
    private void ZoomOut(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zooomOutMax);
    }

    public void Restore()
    {
        Camera.main.transform.localPosition = new Vector3(0, 1f, -10f);
       
        cameraParen.transform.position = Vector3.zero;
        cameraParen.transform.rotation = Quaternion.Euler(Vector3.zero) ;

        Camera.main.orthographicSize = 5;
       
    }
}