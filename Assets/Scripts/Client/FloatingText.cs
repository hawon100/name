using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro tmpro;
    public string text = "";
    float a = 1;
    float y;
    private void Start()
    {
        tmpro = GetComponent<TextMeshPro>();
        y = transform.position.y;
    }

    private void Update()
    {
        a -= Time.deltaTime;
        y += Time.deltaTime * 0.5f;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        tmpro.color = new Color(1, 1, 1, a);
        if (a <= 0) Destroy(gameObject);
    }
    
    public void Init(string text, Vector3 pos, Color color)
    {

    }
}
