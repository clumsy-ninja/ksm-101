using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public GameObject world;
    public GameObject follow;

    private Camera _camera;
    private Transform _transform;
    private SpriteRenderer _followSprite;
    private SpriteRenderer _worldSprite;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _transform = GetComponent<Transform>();

        _followSprite = follow.GetComponent<SpriteRenderer>();
        _worldSprite = world.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Bounds camBounds = GetFollowBounds();
        Bounds worldBounds = _worldSprite.bounds;
        Vector3 adjusted = camBounds.center;

        // force the _camera's x coord to fit into the current scene
        if(camBounds.min.x < worldBounds.min.x)
            adjusted.x += (worldBounds.min.x - camBounds.min.x);
        else if(camBounds.max.x > worldBounds.max.x)
            adjusted.x -= (camBounds.max.x - worldBounds.max.x);

        // force the _camera's y coord to fit into the current scene
        if(camBounds.min.y < worldBounds.min.y)
            adjusted.y += (worldBounds.min.y - camBounds.min.y);
        else if(camBounds.max.y > worldBounds.max.y)
            adjusted.y -= (camBounds.max.y - worldBounds.max.y);

        adjusted.z = _transform.position.z;
        transform.position = adjusted;
    }

    private Bounds GetFollowBounds()
    {
        float size1D = 2 * _camera.orthographicSize;
        float ratio = (float) Screen.width / Screen.height;
        Vector2 size = new Vector2(size1D*ratio, size1D);
        return new Bounds(_followSprite.bounds.center, size);
    }
}
