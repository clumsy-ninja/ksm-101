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

	/// <summary> Runs after every physics system update </summary>
	void FixedUpdate()
	{
		Vector2 force = Vector2.zero;

		float horz = Input.GetAxis("Horizontal");
		if(horz != 0 && rigidbody2D.velocity.x < runMaxSpeed)
			force.x += runSpeedScale*horz;

		float jump = Input.GetAxis ("Jump");
		if(jump != 0 && this.canJump())
			force.y += jumpScale*jump;

		rigidbody2D.AddForce(force);
	}

	/// <summary>
	/// Determine if the player can jump right now
	/// </summary>
	/// <returns>
	/// <c>true</c> if the player can jump, <c>false</c> otherwise.
	/// </returns>
	private bool canJump()
	{
		if(Mathf.Abs(this.rigidbody2D.velocity.y) > jumpGroundThreshold)
			return false;

		return this.GetComponentInChildren<CircleDetector>().Detect();
	}
}