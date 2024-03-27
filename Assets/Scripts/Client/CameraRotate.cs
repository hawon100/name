using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotate : MonoBehaviour
{
    #region Variable
    private bool isRotate = false;

    [SerializeField] public CameraDirection cameraDirection { get; private set; }

    [Header("Direction Transform")]
    [SerializeField] private Transform south;
    [SerializeField] private Transform west;
    [SerializeField] private Transform north;
    [SerializeField] private Transform east;
    #endregion

    #region Function
    private void _ChangeDirection()
    {
        if (!isRotate)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int result = (int)cameraDirection - 1;
                if (result < 0) result = 3;
                cameraDirection = (CameraDirection)result;

                isRotate = true;
                _Rotate();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                int result = (int)cameraDirection + 1;
                if (result > 3) result = 0;
                cameraDirection = (CameraDirection)result;

                isRotate = true;
                _Rotate();
            }
        }
    }

    private void _Rotate()
    {
        switch (cameraDirection)
        {
            case CameraDirection.S:
                transform.DOMoveX(south.position.x, 1).SetEase(Ease.OutQuad);
                transform.DOMoveZ(south.position.z, 1).SetEase(Ease.OutQuad);
                transform.DORotate(south.eulerAngles, 1).OnComplete(() => isRotate = false);
                break;
            case CameraDirection.W:
                transform.DOMoveX(west.position.x, 1).SetEase(Ease.OutQuad);
                transform.DOMoveZ(west.position.z, 1).SetEase(Ease.OutQuad);
                transform.DORotate(west.eulerAngles, 1).OnComplete(() => isRotate = false);
                break;
            case CameraDirection.E:
                transform.DOMoveX(east.position.x, 1).SetEase(Ease.OutQuad);
                transform.DOMoveZ(east.position.z, 1).SetEase(Ease.OutQuad);
                transform.DORotate(east.eulerAngles, 1).OnComplete(() => isRotate = false);
                break;
            case CameraDirection.N:
                transform.DOMoveX(north.position.x, 1).SetEase(Ease.OutQuad);
                transform.DOMoveZ(north.position.z, 1).SetEase(Ease.OutQuad);
                transform.DORotate(north.eulerAngles, 1).OnComplete(() => isRotate = false);
                break;
        }
    }
    #endregion

    #region Unity_Function
    private void Start()
    {
        cameraDirection = CameraDirection.S;
    }

    private void Update()
    {
        _ChangeDirection();
        float y = GameManager.instance.playerObject.transform.position.y;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    #endregion
}
