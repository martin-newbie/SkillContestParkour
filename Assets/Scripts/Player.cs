using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    StandBy, // 처음 시작때 가만히 있기
    Moving,
    Jumping,
    Holding, // 벽 잡고 있는 상태
    GameOver
}

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector3 startPos;
    public int startCount;
    public PlayerState state;
    public int moveCount;
    public int gravityDir = 1;

    [Header("Movement")]
    public float radius;
    public Transform FeetPos;
    public Transform TopPos;
    public Transform ArmPos;
    public Transform HeadFrontPos;

    public bool JumpOverAble => !Physics2D.OverlapCircle(HeadFrontPos.position, radius, LayerMask.GetMask("Ground")) && Physics2D.OverlapCircle(ArmPos.position, radius, LayerMask.GetMask("Ground"));
    public bool WallGrabAble => Physics2D.OverlapCircle(ArmPos.position, radius, LayerMask.GetMask("Ground")) && IsHover;
    public bool JumpAble => !IsHover && Physics2D.OverlapCircle(TopPos.position, radius, LayerMask.GetMask("Ground"));
    public bool IsHover => !Physics2D.OverlapCircle(FeetPos.position, radius, LayerMask.GetMask("Ground"));


    public bool LeftMove
    {
        get
        {
            if (moveCount > 0 && Input.GetKeyDown(KeyCode.LeftArrow) && dir == 1)
            {
                moveCount--;
                return true;
            }
            else return false;
        }
    }
    public bool RightMove
    {
        get
        {
            if (moveCount > 0 && Input.GetKeyDown(KeyCode.RightArrow) && dir == -1)
            {
                moveCount--;
                return true;
            }
            else return false;
        }
    }
    public bool Jump
    {
        get
        {
            if (moveCount > 0 && Input.GetKeyDown(KeyCode.UpArrow) && (!IsHover || state == PlayerState.Holding))
            {
                moveCount--;
                return true;
            }
            else return false;
        }
    }

    [Header("Value")]
    public float moveSpeed;
    public float jumpHeight;
    public int dir;
    public bool active;
    public bool gameEnd;
    public int life;
    public float lifeTime;
    public float score;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startCount = InGameManager.Instance.startCounts[InGameManager.Instance.mapIdx];
        startPos = transform.position;
        moveCount = startCount;
    }

    void Replay()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            life--;
            if(life < 0)
            {
                InGameManager.Instance.GameOver();
                return;
            }

            transform.position = startPos;
            moveCount = startCount;
            state = PlayerState.StandBy;
            dir = 1;
            active = false;
        }
    }

    void SetUI()
    {
        InGameUIManager.Instance.SetMoveCount(moveCount);
        InGameUIManager.Instance.SetScore(0f);
        InGameUIManager.Instance.SetTime(lifeTime);
    }

    void Update()
    {
        InGameUIManager.Instance.SetLife(life);
        if (gameEnd) return;
        if (!active)
        {
            GameStartFunc();
            return;
        }

        SetUI();
        Replay();
        lifeTime += Time.deltaTime;
        switch (state)
        {
            case PlayerState.StandBy:
                break;
            case PlayerState.Moving:
                MoveInput();
                break;
            case PlayerState.Jumping:
                JumpInput();
                break;
            case PlayerState.Holding:
                HoldInput();
                break;
            case PlayerState.GameOver:
                break;
        }
    }

    void RotateFunc()
    {

        float x = gravityDir == 1 ? 0 : 180;
        float y = dir == -1 ? 0 : 180;

        transform.rotation = Quaternion.Euler(x, y, 0);
    }

    private void FixedUpdate()
    {
        if (!active) return;

        switch (state)
        {
            case PlayerState.StandBy:
                break;
            case PlayerState.Moving:
                MovingFunc();
                break;
            case PlayerState.Jumping:
                MovingFunc();
                break;
            case PlayerState.Holding:
                HoldFunc();
                break;
            case PlayerState.GameOver:
                break;
        }
    }

    void JumpInput()
    {
        if (rb.velocity.y <= 0 && !IsHover)
        {
            state = PlayerState.Moving;
        }
    }

    void MoveInput()
    {
        if (LeftMove)
        {
            dir = -1;
        }

        if (RightMove)
        {
            dir = 1;
        }

        if (Jump)
        {
            rb.AddForce(new Vector2(0, jumpHeight * gravityDir), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
    }

    void MovingFunc()
    {
        transform.Translate(transform.right * Time.deltaTime * moveSpeed * dir);
        rb.gravityScale = 3f * gravityDir;
        RotateFunc();
    }

    void GameStartFunc()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            state = PlayerState.Moving;
            dir = -1;
            active = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            state = PlayerState.Moving;
            dir = 1;
            active = true;
        }
        rb.gravityScale = 3f * gravityDir;
    }

    void HoldInput()
    {
        if (LeftMove)
        {
            dir = -1;
            rb.AddForce(new Vector2(5f * dir, jumpHeight * gravityDir), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
        else if (RightMove)
        {
            dir = 1;
            rb.AddForce(new Vector2(5f * dir, jumpHeight * gravityDir), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
    }

    void HoldFunc()
    {
        rb.gravityScale = 0f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (JumpOverAble && state != PlayerState.StandBy)
        {
            state = PlayerState.StandBy;
            StartCoroutine(JumpOverCoroutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (WallGrabAble)
            {
                rb.velocity = Vector2.zero;
                state = PlayerState.Holding;

                if (JumpOverAble)
                {
                    state = PlayerState.StandBy;
                    StartCoroutine(JumpOverCoroutine());
                }
            }
        }

        if (collision.gameObject.CompareTag("Hostile"))
        {
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            score += 10;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Hostile"))
        {
            Hit();
        }

        if (collision.CompareTag("Gravity"))
        {
            gravityDir = collision.GetComponent<GravityUpsideDown>().gravityDir;
        }
    }

    void Hit()
    {
        life--;

        if (life < 0 && !gameEnd)
        {
            InGameManager.Instance.GameOver();
        }
    }

    IEnumerator JumpOverCoroutine()
    {
        float timer = 0.5f;
        do
        {
            rb.gravityScale = 0f;
            transform.Translate(Vector3.up * Time.deltaTime * 2f);

            timer -= Time.deltaTime;
            yield return null;
        } while (timer > 0f);

        state = PlayerState.Moving;
        yield break;
    }
}
