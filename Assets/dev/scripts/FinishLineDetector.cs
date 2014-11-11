using UnityEngine;
using System.Collections;

public class FinishLineDetector : MonoBehaviour 
{
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("You've Won!!!");
		}
	}
}
