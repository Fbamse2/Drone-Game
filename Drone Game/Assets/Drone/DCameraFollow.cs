using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCameraFollow : MonoBehaviour
{
    private Transform Drone;
    void Awake()
    {
        Drone = GameObject.FindGameObjectWithTag("Player").transform;
    }

    Vector3 velocityCameraFollow;
    public Vector3 begindPosition = new Vector3(0, 2, -4);
    public float angle;
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Drone.transform.TransformPoint(begindPosition) + Vector3.up * Input.GetAxis("Vertical"), ref velocityCameraFollow, 0.1f);
        transform.rotation = Quaternion.Euler(new Vector3(angle, Drone.GetComponent<DroneMovement>().currentYRotation, 0));
    }


}
