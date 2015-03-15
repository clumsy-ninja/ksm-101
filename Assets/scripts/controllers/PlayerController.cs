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
	/// Upper bound of the speed the player can move
	/// </summary>
	public float runMaxSpeed = 5.0f;

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

	/// <summary> Reference to sibling Rigidbody2D </summary>
	private Rigidbody2D physics;

	/// <summary> Reference to child component that detects if the player can jump </summary>
	private CircleDetector jumpDetector;

	/// <summary> Start using the component </summary>
	void Start()
	{
		// load references to commonly-used components
		this.physics = GetComponent<Rigidbody2D>();
		this.jumpDetector = GetComponentInChildren<CircleDetector>();
	}

	/// <summary> Runs after every physics system update </summary>
	void FixedUpdate()
	{
		float move = Input.GetAxis(this.runInputAxis);
		this.physics.velocity = new Vector2(move*runMaxSpeed, this.physics.velocity.y);

		float jump = Input.GetAxis(this.jumpInputAxis);
		if(jump != 0 && this.canJump())
			this.physics.AddForce(new Vector2(0, jumpScale*jump));
	}

	/// <summary>
	/// Determine if the player can jump right now
	/// </summary>
	/// <returns>
	/// <c>true</c> if the player can jump, <c>false</c> otherwise.
	/// </returns>
	private bool canJump()
	{
		if(Mathf.Abs(this.physics.velocity.y) > jumpGroundThreshold)
			return false;

		return this.jumpDetector.Detect();
	}
}