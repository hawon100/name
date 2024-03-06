using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnPos
{
    public Transform pos_1;
    public Transform pos_2;
    public Transform pos_3;
    public Transform pos_4;
}

public class Parabola : MonoBehaviour
{
    public Transform cube;
    public Transform front, back;
    public TurnPos turnPos;

    [Range(-1, 4)] public int t;

    void Update()
    {
        //switch(t)
        //{
        //    case -1: t = 4; break;
        //    case 0: front.position = turnPos.pos_1.position; back.position = turnPos.pos_2.position; break;
        //    case 1: front.position = turnPos.pos_2.position; back.position = turnPos.pos_3.position; break;
        //    case 2: front.position = turnPos.pos_3.position; back.position = turnPos.pos_4.position; break;
        //    case 3: front.position = turnPos.pos_4.position; back.position = turnPos.pos_1.position; break;
        //    case 4: t = 0; break;
        //}
        if(t == 0)
        {
            front = turnPos.pos_1;
            back = turnPos.pos_2;
        }
        if (t == 1)
        {
            front = turnPos.pos_2;
            back = turnPos.pos_3;
        }

        Vector3 dir = cube.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);
        transform.position = Vector3.Slerp(front.position, back.position, t);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.x = 0;
        currentRotation.z = 0;
        transform.rotation = Quaternion.Euler(currentRotation);
    }
}
