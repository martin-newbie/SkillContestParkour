using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float radius;
    public Transform FeetPos;
    public Transform TopPos;
    public Transform ArmPos;
    public Transform HeadFrontPos;

    public bool JumpOverAble => !Physics2D.OverlapCircle(HeadFrontPos.position, radius, LayerMask.GetMask("Ground")) && IsHover;
    public bool WallGrabAble => Physics2D.OverlapCircle(ArmPos.position, radius, LayerMask.GetMask("Ground")) && IsHover;
    public bool JumpAble => !IsHover && Physics2D.OverlapCircle(TopPos.position, radius, LayerMask.GetMask("Ground"));
    public bool IsHover => !Physics2D.OverlapCircle(FeetPos.position, radius, LayerMask.GetMask("Ground"));

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
