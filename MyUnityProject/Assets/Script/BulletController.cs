using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // �Ѿ� �ӵ�
    private float Speed;

    // �Ѿ��� �浹�� Ƚ��
    private float hp;

    // ����Ʈȿ�� ����
    public GameObject fxPrefab;

    // �Ѿ��� ������ ����
    public Vector3 Direction { get; set;} //�� ����
   
    private void Start()
    {
        // �ӵ� �ʱⰪ
        Speed = 10.0f;
        // �Ѿ��� �ִ� �浹 Ƚ�� ����
        hp = 2;
    }
    void Update()
    {
        // �������� �ӵ���ŭ ��ġ�� ����
        transform.position += Direction * Speed * Time.deltaTime;
    }

    // �浹ü�� ���������� ���Ե� ������Ʈ�� �ٸ� �浹ü�� �浹�Ѵٸ� ����Ǵ� �Լ�.

    // Enter : �浹�� ���� ������ 
    // Stay : �浹�� ���� �����Ӻ��� Exit������ ��������
    // Exit : �浹�� ���� �� �� ���� ������

    // Trigger : �浹 �������ϰ� �浹ü�� ����Ѵ�.
    // Collision : �浹 �������ϰ� �浹ü�� �浹�Ѵ�.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹 Ƚ�� ����.
        --hp;

        // ����Ʈȿ�� ����
        GameObject Obj = Instantiate(fxPrefab);

        // ����ȿ���� ����ϴ� ������ ����
        GameObject camera = new GameObject("Camera Test");

        // ����ȿ�� ����
        camera.AddComponent<CameraController>();

        // ����Ʈ ȿ���� ��ġ�� ����
        Obj.transform.position = transform.position;

        // �浹�� ����� ����
        if(collision.transform.tag != "wall" && collision.transform.tag != "BackGround")
            Destroy(collision.transform.gameObject);
        else
            Destroy(this.gameObject);
            
        if (hp == 0)
            Destroy(this.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        print("Stay");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Exit");
    }
}
