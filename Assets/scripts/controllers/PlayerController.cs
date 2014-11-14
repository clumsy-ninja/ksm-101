using UnityEngine;
using System.Collections;

/// <summary>
/// Set of bit flags indicating the actions that either can be
/// performed by the player or are being performed.
/// </summary>
public enum PlayerActions
{
	None = 0x00000000,
	Jump = 0x00000001,
	Run  = 0x00000002
}

/// <summary>
/// Main controller script that defines the behavior of the player object.
/// </summary>
public class PlayerController : MonoBehaviour 
{
	/// <summary>
	/// 2D Box storing the distances (from the player) where the
	/// environment was detected.
	/// </summary>
	private class DetectionBox
	{
		public RaycastHit2D? left = null;
		public RaycastHit2D? right = null;
		public RaycastHit2D? up = null;
		public RaycastHit2D? down = null;
	}

	/// <summary>
	/// Name of the axis to use for left-right movement
	/// </summary>
	public string runInputAxis = "Horizontal";

	/// <summary>
	/// Scaling factor to apply to the player movement speed
	/// </summary>
	public float runSpeedScale = 2.0f;

	/// <summary>
	/// Upper bound of the speed the player can move
	/// </summary>
	public float runMaxSpeed = 3.0f;

	/// <summary>
	/// Name of the axis to use for left-right movement
	/// </summary>
	public string jumpInputAxis = "Jump";

	/// <summary>
	/// Scaling factor to apply to the jump force
	/// </summary>
	public float jumpScale = 25.0f;

	/// <summary>
	/// Distance above the nearest ground layer point that determines
	/// whether a jump is performed or not.
	/// </summary>
	public float jumpGroundThreshold = 0.01f;

	void Update()
	{
	}

	void FixedUpdate()
	{
		Vector3 force = Vector3.zero;
		float horz = Input.GetAxis("Horizontal");
		if(horz != 0 && rigidbody2D.velocity.x < runMaxSpeed)
			force.x += runSpeedScale*horz;

		float jump = Input.GetAxis("Jump");
		DetectionBox box = this.detect();
		if(jump != 0 && box.down != null)
		{
			Vector2 ray = (Vector2)transform.position - box.down.Value.point;
			if(box.down != null && ray.magnitude < jumpGroundThreshold)
				force.y += jumpScale*jump;
		}

		rigidbody2D.AddForce(force);

#if false
		// draw the detection results
		if(box.left != null)
			Debug.DrawLine(transform.position, box.left.Value.point, Color.red);
		if(box.right != null)
			Debug.DrawLine(transform.position, box.right.Value.point, Color.red);
		if(box.up != null)
			Debug.DrawLine(transform.position, box.up.Value.point, Color.green);
		if(box.down != null)
			Debug.DrawLine(transform.position, box.down.Value.point, Color.green);
#endif
	}

	/// <summary>
	/// Detects the distances in each primary direction to the nearest 
	/// point in the environment
	/// </summary>
	/// <returns>
	/// The distance from the player to the nearest point in 
	/// the environment in each direction
	/// </returns>
	/// <remarks>
	/// TODO: allow the caller to specify different layers
	/// TODO: allow the user to specify maximum distance on a per-action basis
	/// </remarks>
	private DetectionBox detect()
	{
		DetectionBox rval = new DetectionBox();
		RaycastHit2D result;

		result = Physics2D.Raycast(transform.position, Vector2.right, 3);
		if(result.collider)
			rval.right = result;

		result = Physics2D.Raycast(transform.position, -Vector2.right, 3);
		if(result.collider)
			rval.left = result;

		result = Physics2D.Raycast(transform.position, Vector2.up, 3);
		if(result.collider)
			rval.up = result;

		result = Physics2D.Raycast(transform.position, -Vector2.up, 3);
		if(result.collider)
			rval.down = result;

		return rval;
	}
}