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

    private GameObject Prefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // ** 씬이 변경되어도 계속 유지될수 있게 해준다.
            DontDestroyOnLoad(this.gameObject);

            Prefab = Resources.Load("Prefabs/Enemy/Enemy") as GameObject;
        }    
    }

    private IEnumerator Start()
    {     
        while(true)
        {
            yield return new WaitForSeconds(1.5f);

            GameObject Obj = Instantiate(Prefab);
            Obj.transform.position = new Vector3(
                18.0f, Random.Range(-8.2f, -5.5f), 0.0f);
            Obj.transform.name = "TEST";
        }
        
    }

    public void CreateTest()
    {
        
    }
}
