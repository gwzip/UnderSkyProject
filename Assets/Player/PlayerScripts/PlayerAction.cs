using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float Speed;

    Rigidbody2D rigid;
    Animator anim;
    Vector3 dirVec;
    GameObject scanObject;
    float h;
    float v;
    bool isHorizonMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //이동 값 설정
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        //이동 버튼 확인
        bool hDown = Input.GetButtonDown("Horizontal");
        bool vDown = Input.GetButtonDown("Vertical");
    
        //레이 방향 판단하기
        if (vDown && v == 1)    //위쪽 방향
            dirVec = Vector3.up;
        else if (vDown && v == -1)    //아래쪽 방향
            dirVec = Vector3.down;
        else if (hDown && h == -1)    //왼쪽 방향
            dirVec = Vector3.left;
        else if (hDown && h == 1)    //오른쪽 방향
            dirVec = Vector3.right;

        //오브젝트 스캔 출력
        if (Input.GetButtonDown("Jump") && scanObject != null)
            Debug.Log("this is : " + scanObject.name);
    }

    void FixedUpdate()
    {
        //레이 사용하기
        Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
    }
}