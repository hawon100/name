using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    public Transform cube;
    public Transform front, back;

    [Range(0, 1)] public float t;

    void Update()
    {
        Rotate();
        Move();
    }

    void Move()
    {
        Vector3 dir = cube.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
        transform.position = Vector3.Slerp(front.position, back.position, t);
    }

    void Rotate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.x = 0;
        currentRotation.z = 0;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
