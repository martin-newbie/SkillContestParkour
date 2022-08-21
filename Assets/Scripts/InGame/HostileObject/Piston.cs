using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{

    float originY = -1;

    public float speed;
    public SpriteRenderer ropeSprite;
    public Transform pistonTransform;
    public Transform endPos;
    public BoxCollider2D col;

    private void Start()
    {
        StartCoroutine(MainLogicCoroutine());
    }

    private void Update()
    {
        float dis = originY - pistonTransform.localPosition.y;
        float fill_h = dis + 0.7f;
        ropeSprite.size = new Vector2(0.7f, fill_h);
    }

    IEnumerator MainLogicCoroutine()
    {
        while (true)
        {
            yield return StartCoroutine(MoveDown());
            col.enabled = false;
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(MoveUp());
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator MoveUp()
    {
        do
        {
            pistonTransform.Translate(Vector3.up * Time.deltaTime);
            yield return null;
        } while (pistonTransform.localPosition.y < -1);

        yield break;
    }

    IEnumerator MoveDown()
    {
        col.enabled = true;
        do
        {
            pistonTransform.Translate(Vector3.down * Time.deltaTime * speed);
            yield return null;
        } while (Physics2D.OverlapCircle(endPos.position, 0.05f, LayerMask.GetMask("Ground")) == null);

        yield break;
    }
}
