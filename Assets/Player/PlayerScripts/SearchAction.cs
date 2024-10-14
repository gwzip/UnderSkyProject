using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchAction : MonoBehaviour
{
    Rigidbody2D rigid;

    float h;
    float v;


    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2 (h, v);
    }
}
