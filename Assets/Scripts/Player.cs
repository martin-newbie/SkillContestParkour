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
    Rigidbody2D rb;
    public PlayerState state;

    [Header("Movement")]
    public float radius;
    public Transform FeetPos;
    public Transform TopPos;
    public Transform ArmPos;
    public Transform HeadFrontPos;

    public bool JumpOverAble => !Physics2D.OverlapCircle(HeadFrontPos.position, radius, LayerMask.GetMask("Ground")) && IsHover;
    public bool WallGrabAble => Physics2D.OverlapCircle(ArmPos.position, radius, LayerMask.GetMask("Ground")) && IsHover;
    public bool JumpAble => !IsHover && Physics2D.OverlapCircle(TopPos.position, radius, LayerMask.GetMask("Ground"));
    public bool IsHover => !Physics2D.OverlapCircle(FeetPos.position, radius, LayerMask.GetMask("Ground"));
    public bool LeftMove => Input.GetKeyDown(KeyCode.LeftArrow) && dir == 1;
    public bool RightMove => Input.GetKeyDown(KeyCode.RightArrow) && dir == -1;
    public bool Jump => Input.GetKeyDown(KeyCode.UpArrow) && (!IsHover || state == PlayerState.Holding);

    [Header("Value")]
    public float moveSpeed;
    public float jumpHeight;
    public int dir;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.StandBy:
                StandbyFunc();
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
        if(dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
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
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
    }

    void MovingFunc()
    {
        transform.Translate(transform.right * Time.deltaTime * moveSpeed * dir);
        rb.gravityScale = 3f;
        RotateFunc();
    }

    void StandbyFunc()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            state = PlayerState.Moving;
            dir = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            state = PlayerState.Moving;
            dir = 1;
        }
        rb.gravityScale = 3f;
    }

    void HoldInput()
    {
        if (LeftMove)
        {
            dir = -1;
            rb.AddForce(new Vector2(5f * dir, jumpHeight), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
        else if (RightMove)
        {
            dir = 1;
            rb.AddForce(new Vector2(5f * dir, jumpHeight), ForceMode2D.Impulse);
            state = PlayerState.Jumping;
        }
    }

    void HoldFunc()
    {
        rb.gravityScale = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (WallGrabAble)
            {
                rb.velocity = Vector2.zero;
                state = PlayerState.Holding;
            }
        }
    }
}
