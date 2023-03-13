using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    // BackGround�� ���ִ� ���������� �ֻ��� ��ü(�θ�)
    private Transform parent;

    // Sprite�� �����ϰ� �ִ� Conponenet
    private SpriteRenderer spriteRenderer;
    
    // �̹���
    private Sprite sprite;
    
    // ��������
    private float endPoint;
    // ���� ����
    private float exitPoint;
    // �̹��� �̵� �ӵ�
    public float Speed;

    // �÷��̾� ����
    private GameObject player;
    private PlayerController playerController;
    // ������ ����
    private Vector3 movemane;
    // �̹����� �߾� ��ġ�� ���������� ����� �� �ֵ��� �ϱ� ���� ���� ����.
    private Vector3 offset = new Vector3(0.0f, 7.5f, 0.0f);

    private void Awake()
    {
        // �÷��̾��� �⺻������ �޾ƿ´�.
        player = GameObject.Find("Player").gameObject;
        // �θ�ü�� �޾ƿ´�.
        parent = GameObject.Find("BackGround").transform;
        // �÷��̾� �̹����� ����ִ� ������Ҹ� �޾ƿ´�.
        playerController = player.GetComponent<PlayerController>();
        // ���� �̹����� ����ִ� ������Ҹ� �޾ƿ´�.
        spriteRenderer = GetComponent<SpriteRenderer>();

       
    }

    void Start()
    {
        // ������ҿ� ���Ե� �̹����� �޾ƿ´�.
        sprite = spriteRenderer.sprite;
        // ���������� ����
        endPoint = sprite.bounds.size.x * 0.5f + transform.position.x;
        // ���������� ����
        exitPoint = -(sprite.bounds.size.x * 0.5f) + player.transform.position.x;
    }

    void Update()
    {
        // �̵����� ���� 
        movemane = new Vector3(
            Input.GetAxisRaw("Horizontal") * Time.deltaTime * Speed + offset.x,
            player.transform.position.y + offset.y,
            0.0f + offset.z);
        // singletone
        // �÷��̾ �ٶ󺸰��ִ� ���⿡ ���� �б�ȴ�.
        if (ControllerManager.GetInstance().DirLeft)
        {
            // �̵� ���� ����
            endPoint -= movemane.x;
        }
        if(ControllerManager.GetInstance().DirRight)
        {
            //if(transform.position.x <= 0)           
                transform.position -= movemane;
        }
        //movemane = Vector3.zero;
        //if (Input.GetAxisRaw("Horizontal") > 0 && player.transform.position.x >= 0)
        //{
        //    movemane = new Vector3(
        //        Input.GetAxisRaw("Horizontal") * Time.deltaTime * Speed + offset.x,
        //        player.transform.position.y + offset.y,
        //        0.0f + offset.z);
        //}

        

        
        

        // ���� �̹��� ����
        if (player.transform.position.x + sprite.bounds.size.x * 0.5f + 1 > endPoint)
        {
            // ���� �̹��� ����
            GameObject Obj = Instantiate(this.gameObject);

            // ������ �̹����� �θ� �����Ѵ�.
            Obj.transform.parent = parent.transform;
            // ������ �̹����� �̸��� �����Ѵ�.
            Obj.transform.name = transform.name;

            // ������ �̹����� ��ġ ����
            Obj.transform.position = new Vector3(
                endPoint + sprite.bounds.size.x * 0.5f,
                transform.position.y,
                0.0f);

            // ���������� �����Ѵ�.
            endPoint += endPoint + sprite.bounds.size.x * 0.5f;
        }
        // �������� ���� �� �̹��� ����.
        if (transform.position.x + (sprite.bounds.size.x * 0.5f) - 2 < exitPoint)
        {
            Destroy(this.gameObject);
        }
        
    }
}
