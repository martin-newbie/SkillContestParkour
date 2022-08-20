using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : MonoBehaviour
{
    public float radius;
    public float speed;
    public Player target;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {
        
    }

    void Update()
    {
        TargetFollow();
    }

    void TargetFollow()
    {
        target = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"))?.GetComponent<Player>();
        if (target == null) return;

        if(transform.position.x > target.transform.position.x)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

    }
}
