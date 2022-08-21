using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawObject : MonoBehaviour
{

    public Transform[] movePos;
    public Vector3[] poses = new Vector3[2];
    public Transform sawBlade;
    public LineRenderer line;
    public float speed;
    public float t;
    Vector2 offset;

    private void Start()
    {
        StartCoroutine(TMove());
    }

    IEnumerator TMove()
    {
        int dir = 1;
        while (true)
        {
            if(dir == 1 && t < 1f)
            {
                t += Time.deltaTime * dir * speed;
                if (t >= 1f) dir = -1;
            }
            else if(dir == -1 && t > 0f)
            {
                t += Time.deltaTime * dir * speed;
                if (t <= 0f) dir = 1;
            }

            yield return null;
        }
    }

    void Update()
    {
        for (int i = 0; i < movePos.Length; i++)
        {
            poses[i] = movePos[i].position;
        }
        line.SetPositions(poses);

        sawBlade.Rotate(Vector3.back * 1000f);
        sawBlade.position = Vector3.Lerp(movePos[0].position, movePos[1].position, t);

        offset.x += Time.deltaTime * speed; 
        line.material.SetTextureOffset("_MainTex", offset);
    }
}
