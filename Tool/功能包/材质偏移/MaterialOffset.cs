using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffset : MonoBehaviour
{
    private Material m_Material;
    public bool isOffsetX;
    public float xSpeed;
    private float xOffset;
    public bool isOffsetY;
    public float ySpeed;
    private float yOffset;

    private float offsetXLimit = 10f;
    private float offsetYLimit = 10f;

    void Start()
    {
        m_Material = GetComponent<Renderer>().material;
    }

    void FixedUpdate()
    {
        if (isOffsetX && isOffsetY)
        {
            xOffset += xSpeed * Time.deltaTime;
            yOffset += ySpeed * Time.deltaTime;
            m_Material.mainTextureOffset = new Vector2(xOffset, yOffset);
            if (Mathf.Abs(Mathf.Abs(xOffset) - offsetXLimit) < 0)
            {
                xOffset = 0;
            }
            if (Mathf.Abs(Mathf.Abs(yOffset) - offsetYLimit) < 0)
            {
                yOffset = 0;
            }
        }
        else if (isOffsetX)
        {
            xOffset += xSpeed * Time.deltaTime;
            m_Material.mainTextureOffset = new Vector2(xOffset, 0);
            if (Mathf.Abs(Mathf.Abs(xOffset) - offsetXLimit) < 0)
            {
                xOffset = 0;
            }
        }
        else if (isOffsetY)
        {
            yOffset += ySpeed * Time.deltaTime;
            m_Material.mainTextureOffset = new Vector2(0, yOffset);
            if (Mathf.Abs(Mathf.Abs(yOffset) - offsetYLimit) < 0)
            {
                yOffset = 0;
            }
        }
    }
}
