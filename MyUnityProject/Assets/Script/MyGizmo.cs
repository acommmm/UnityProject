using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // Gizmos 색상 설정
        Gizmos.color = Color.green;
        // Gizmos 그린다.
        Gizmos.DrawSphere(this.transform.position, 0.2f);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
