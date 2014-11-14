using UnityEngine;
using System.Collections;

/// <summary>
/// Controller script that defines how the camera moves in response
/// to the player's movement.
/// </summary>
public class CameraController : MonoBehaviour 
{
	/// <summary>
	/// GameObject that the camera must stay within the bounds of.
	/// </summary>
	public GameObject world;

	/// <summary>
	/// GameObject that the camera should follow (typically the player)
	/// </summary>
	public GameObject target;

	/// <summary>
	/// 	Performs a check after each render to force the camera to only 
	/// 	render what's currently in the scene.
	/// </summary>
	/// <remarks>
	/// 	This can be optimized for cases when we know that the player's
	///		position has changed. It's unnecessary otherwise.
	/// </remarks>
	void Update()
	{
		Bounds camBounds = getTargetedCameraBounds();
		Bounds worldBounds = getWorldBounds();
		Vector3 adjusted = new Vector3();

		// force the camera's x coord to fit into the current scene
		if(camBounds.min.x < worldBounds.min.x)
			adjusted.x = worldBounds.min.x + camBounds.extents.x;
		else if(camBounds.max.x > worldBounds.max.x)
			adjusted.x = worldBounds.max.x - camBounds.extents.x;
		else
			adjusted.x = target.transform.position.x;

		// force the camera's y coord to fit into the current scene
		if(camBounds.min.y < worldBounds.min.y)
			adjusted.y = worldBounds.min.y + camBounds.extents.y;
		else if(camBounds.max.y > worldBounds.max.y)
			adjusted.y = worldBounds.max.y - camBounds.extents.y;
		else
			adjusted.y = target.transform.position.y;

		adjusted.z = transform.position.z;
		transform.position = adjusted;
	}

	/// <summary>
	/// Retrieve the axis-aligned bounding box for what the camera can 
	/// currently see.
	/// </summary>
	/// <remarks>
	/// 	For in-game calls, the current transform's position is considered
	///  	to be the center point for the bounds.
	/// </remarks>
	protected Bounds getCameraBounds()
	{
		return new Bounds(transform.position, getCameraExtents());
	}

	/// <summary>
	/// 	Calculate the worldspace extents (width, height) of the sibling camera.
	/// </summary>
	/// <returns>
	/// 	The width and height of the camera's viewing area, as a Vector2
	/// </returns>
	protected Vector2 getCameraExtents()
	{
		float size = 2*this.camera.orthographicSize;
		float ratio = (float)Screen.width/Screen.height;
		return new Vector2(size*ratio, size);
	}

	/// <summary>
	/// Retrieve the axis-aligned bounding box for what the camera would see if it
	/// were centered on it's target object.
	/// </summary>
	/// <remarks>
	/// 	For in-game calls, the current transform's position is considered
	///  	to be the center point for the bounds.
	/// </remarks>
	protected Bounds getTargetedCameraBounds()
	{
		return new Bounds(target.transform.position, getCameraExtents());
	}

	/// <summary>
	/// Return the full world-space bounds for the current level
	/// </summary>
	/// <returns>The world bounds.</returns>
	protected Bounds getWorldBounds()
	{
		return this.world.GetComponent<SpriteRenderer>().sprite.bounds;
	}
}
