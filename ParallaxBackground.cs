using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //public GameObject [] bgpieces;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField]private Vector2 parallaxeffectmod;
    private float textureUnitSizeX;
    // Start is called before the first frame update
    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        textureUnitSizeX = texture.width/sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxeffectmod.x,deltaMovement.y * parallaxeffectmod.y,0f);
        lastCameraPosition = cameraTransform.position;

        if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(cameraTransform.position.x + offsetPositionX,transform.position.y);
        }
        
    }
}
