using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject test;
    public GameObject Player;

    void Start()
    {
        //Vector3 Direction = (Player.transform.position - test.transform.position).normalized;
        //transform.position += Direction * Time.deltaTime * 2.0f;
    }

    void Update()
    {
        /* ���� ���ϴ� ����.1
        Vector3 Direction = new Vector3(
        Player.transform.position.x - test.transform.position.x,
        Player.transform.position.y - test.transform.position.y,
        0.0f);
        Direction.Normalize();
        */

        /* ���� ���ϴ� ����.2
        Vector3 Direction = Player.transform.position - test.transform.position;
        Direction.Normalize();
        */

        // ** ���� ���ϴ� ����.3
        Vector3 Direction = (Player.transform.position - test.transform.position).normalized;



        test.transform.position += Direction * Time.deltaTime * 2.0f;



    }
}
