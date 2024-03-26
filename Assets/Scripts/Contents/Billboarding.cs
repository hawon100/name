using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    public Camera m_MainCamera;
    Quaternion m_OriginalRotation;

    void Start()
    {
        m_MainCamera= Camera.main;
        m_OriginalRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation 
            = m_MainCamera.transform.rotation * m_OriginalRotation;
    }
}
