using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfo : MonoBehaviour {

	public class DefaultInfo {
		public Vector2 velocity = Vector2.zero;
		public float gravityScale = 1;
	}

	public class DeadInfo {
		public Vector2 velocity = Vector2.zero;
		public float gravityScale = 0;
	}

	public class CaughtInfo {
		public Vector2 velocity = Vector2.zero;
		public float gravityScale = 0;
	}

	public DeadInfo deadInfo = new DeadInfo();
	public DefaultInfo defaultInfo = new DefaultInfo();
	public CaughtInfo caughtInfo = new CaughtInfo();

	[System.NonSerialized]
	public Vector2 position, velocity, acceleration;

	[System.NonSerialized]
	public Vector2 lastPosition, lastVelocity;
}
