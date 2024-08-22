using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _coll = null;
    [SerializeField] private SpriteRenderer _renderer = null;

    float Size = 3f;
    bool hasMushroom = false;

    public void MushroomSpawned()
    {
        hasMushroom = true;
    }

    public bool GetHasMushroom()
    {
        return hasMushroom;
    }

    public void SetScale(float Xscale)
    {
        Size = Xscale;
        _coll.size = new Vector2(Xscale, _coll.size.y);
        _renderer.size = new Vector2(Xscale, _renderer.size.y);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public float GetSize()
    {
        return Size;
    }
}
