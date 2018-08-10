using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    SpriteRenderer background;

    void Start() {
        background = GetComponent<SpriteRenderer>();
        ConstrainSprite();
    }

    void Update() {

    }

    void ConstrainSprite() {        
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = background.sprite.bounds.size;
        
        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y) { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        } else { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }
        
        // transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
}
