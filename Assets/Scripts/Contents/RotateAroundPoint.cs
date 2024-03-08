using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    public Transform centerOfRotation;

    private float _rotationSpeed = 90.0f;

    public float accumulatedTime = 1.0f;

    private bool _isDir;

    void Update()
    {
        Direction();

        if (accumulatedTime >= 1.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, Mathf.RoundToInt(transform.rotation.eulerAngles.y), 0.0f);
            transform.position = GetRoundedPosition();
            return;
        }

        accumulatedTime += Time.deltaTime;

        Move();
    }

    private void Move()
    {
        if (_isDir == true)
        {
            transform.RotateAround(centerOfRotation.position, Vector3.up, 1 * _rotationSpeed * Time.deltaTime);
        }

        if (_isDir == false)
        {
            transform.RotateAround(centerOfRotation.position, Vector3.up, -1 * _rotationSpeed * Time.deltaTime);
        }
    }

    private void Direction()
    {
        bool isLeft = Input.GetKeyDown(KeyCode.RightArrow);
        bool isRight = Input.GetKeyDown(KeyCode.LeftArrow);

        if (isLeft)
        {
            accumulatedTime = 0.0f;
            _isDir = false;
        }

        if (isRight)
        {
            accumulatedTime = 0.0f;
            _isDir = true;
        }
    }

    private Vector3 GetRoundedPosition()
    {
        Vector3 roundedPosition = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            Mathf.RoundToInt(transform.position.z)
        );

        return roundedPosition;
    }
}
