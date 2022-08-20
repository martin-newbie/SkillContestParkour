using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float duration;
    public Transform laserPos;
    public LineRenderer line;
    public BoxCollider2D col;

    private void Start()
    {
        StartCoroutine(LaserLogic());
    }

    IEnumerator LaserLogic()
    {
        while (true)
        {
            col.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(laserPos.position, laserPos.right, 10f, LayerMask.GetMask("Ground"));
            line.SetPosition(0, laserPos.position);
            line.SetPosition(1, hit.point);

            float x = Vector3.Distance(hit.point, laserPos.position);
            col.size = new Vector2(x, 1);
            col.offset = new Vector2(x / 2, 0);

            yield return new WaitForSeconds(duration);
            line.SetPosition(0, laserPos.position);
            line.SetPosition(1, laserPos.position);
            col.enabled = false;
            yield return new WaitForSeconds(duration);

        }
    }
}
