using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer woodRenderer = null;
    float WoodHeight;

    public Vector2 GetShatterPosition()
    {
        return (Vector2)transform.position - new Vector2(0, WoodHeight - 1f);
    }

    public void SetSize(float _height)
    {
        woodRenderer.size = new Vector2(woodRenderer.size.x, _height);
        WoodHeight = _height;
    }
}
