using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    public Transform centerOfRotation; 

    public float rotationSpeed = 90.0f; 

    public float accumulatedTime = 0.0f;

    void Update()
    {
        accumulatedTime += Time.deltaTime;

        if (accumulatedTime >= 1.0f)
        {
            //accumulatedTime = 0.0f;
            transform.position = GetRoundedPosition();
            return;
        }

        float input = Input.GetAxis("Horizontal");
        float rotationDirection = input < 0 ? -1.0f : 1.0f;
        transform.RotateAround(centerOfRotation.position, Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
    }

    public Vector3 GetRoundedPosition()
    {
        Vector3 roundedPosition = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(transform.position.z)
        );

        return roundedPosition;
    }
}
