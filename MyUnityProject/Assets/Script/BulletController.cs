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
        hp = 1;
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
        

        if (collision.tag == ("wall"))
        {
            Destroy(this.gameObject);
            return;
        }
        else if (hp == 0) // �Ѿ��� �浹 Ƚ���� 0 �Ǹ� �Ѿ� ����.
           Destroy(this.gameObject);
           

        // ����Ʈȿ�� ����
        GameObject Obj = Instantiate(fxPrefab);

        // ����ȿ���� ����ϴ� ������ ����
        GameObject camera = new GameObject("Camera Test");

        // ����ȿ�� ����
        camera.AddComponent<CameraController>();

        // ����Ʈ ȿ���� ��ġ�� ����
        Obj.transform.position = transform.position;

        // �浹�� ����� ����
        Destroy(collision.transform.gameObject);     
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
