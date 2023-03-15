using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private EnemyManager() { }

    public static EnemyManager instance = null;

    public static EnemyManager GetInstance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    // ** �����Ǵ� Enemy�� ��Ƶ� ���� ��ü
    private GameObject Parent;
    // ** Enemy�� ����� ���� ��ü
    private GameObject Prefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // ** ���� ����Ǿ ��� �����ɼ� �ְ� ���ش�.
            DontDestroyOnLoad(this.gameObject);

            // ** �����Ǵ� Enemy�� ��Ƶ� ���� ��ü
            Parent = new GameObject("EnemyList");

            // ** Enemy�� ����� ���� ��ü
            Prefab = Resources.Load("Prefabs/Enemy/Enemy") as GameObject;
        }    
    }

    // ** �������ڸ��� Start�Լ��� �ڷ�ƾ �Լ��� ����
    private IEnumerator Start()
    {     
        while(true)
        {
            // ** Enemy ������ü�� �����Ѵ�.
            GameObject Obj = Instantiate(Prefab);

            // ** Enemy �۵� ��ũ��Ʈ ����
            //Obj.AddComponent<EnemyController>();

            // ** Clone�� ��ġ�� �ʱ�ȭ
            Obj.transform.position = new Vector3(
                18.0f, Random.Range(-8.2f, -5.2f), 0.0f);
            // ** Clone�� �̸� �ʱ�ȭ
            Obj.transform.name = "Enemy";
            // ** Clone�� �������� ����
            Obj.transform.parent = Parent.transform;

            // ** 1.5�� �޽�
            yield return new WaitForSeconds(1.5f);
        }
        
    }

    public void CreateTest()
    {
        
    }
}
