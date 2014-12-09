using UnityEngine;
using System.Collections;

/// <summary>
/// Detect 2D colliders within a radius of the object's Transform component
/// </summary>
public class CircleDetector : MonoBehaviour
{
	/// <summary>
	/// Radius of the circle (centered at the sibling Transform) about which the
	/// detection will occur
	/// </summary>
	public float radius = 1.0f;

	/// <summary>
	/// Layers that will be detected
	/// </summary>
	public LayerMask layers;

	/// <summary>
	/// Subdivision count for the circle renderer
	/// </summary>
	private const int SUBD_COUNT = 100;

	/// <summary>
	/// The collider that was detected the last time a detection was run
	/// </summary>
	private Collider2D detected;

	/// <summary>
	/// Run the detection
	/// </summary>
	/// <returns>
	/// The collider object that was detected in the circle defined by the
	/// sibling transform and the instance variable 'radius', or null if
	/// no such collider was detected.
	/// </returns>
	public Collider2D Detect()
	{
		return Physics2D.OverlapCircle(this.transform.position, this.radius, this.layers);
	}

	/// <summary>
	/// gizmo renderer, draws a simple sphere in the editor window for the
	/// sphere around which detection will occur
	/// </summary>
	void OnDrawGizmosSelected()
	{
		float inc = 2*Mathf.PI/(float)CircleDetector.SUBD_COUNT;
		float rads;

		Vector2 last = new Vector2(this.radius*Mathf.Cos(inc)+this.transform.position.x,
		                           this.radius*Mathf.Sin(inc)+this.transform.position.y);
		Vector2 next = new Vector2();

		Gizmos.color = new Color(0.6f, 0.7f, 0.8f);
		for(int i=1; i<SUBD_COUNT; i++)
		{
			rads = i*inc;
			next.x = this.radius*Mathf.Cos(rads) + this.transform.position.x;
			next.y = this.radius*Mathf.Sin(rads) + this.transform.position.y;

			Gizmos.DrawLine(last, next);
			last = next;
		}
	}
}
