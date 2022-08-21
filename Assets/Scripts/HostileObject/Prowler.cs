using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prowler : MonoBehaviour
{
    public float speed;
    public float standbyTime;
    public Transform prowler;
    public Transform[] poses = new Transform[2];

    void Start()
    {
        StartCoroutine(ProwlerLogic());
    }

    IEnumerator ProwlerLogic()
    {
        int idx = 0;
        Transform target = poses[idx];

        while (true)
        {
            prowler.Translate(Vector3.right * Time.deltaTime * speed);

            var col = Physics2D.OverlapCircle(prowler.position, 1f, LayerMask.GetMask("Pos"));
            if (col != null && col.transform == target)
            {
                idx = idx == 0 ? 1 : 0;
                target = poses[idx];

                float y = idx == 0 ? 0 : 180;
                prowler.rotation = Quaternion.Euler(0, y, 0);

                yield return new WaitForSeconds(standbyTime);
            }
            yield return null;
        }
    }
}
