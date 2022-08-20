using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityUpsideDown : MonoBehaviour
{
    public int gravityDir;
    public MeshRenderer mesh;
    Vector2 offset;

    void Start()
    {
        offset = mesh.material.mainTextureOffset;
    }

    void Update()
    {
        offset.y += Time.deltaTime * gravityDir;
        mesh.material.SetTextureOffset("_MainTex", offset);
    }
}
