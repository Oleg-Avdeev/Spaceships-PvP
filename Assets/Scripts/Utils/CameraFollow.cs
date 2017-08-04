using System;
using UnityEngine;

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;
        }

		public void setTarget (Transform _target)
		{target = _target;}

	public void SetBattle(bool battle)
	{
		if (battle)
		{
			damping = 0.05f;
			lookAheadFactor = 0.1f;
			lookAheadReturnSpeed = 0.0f;
			lookAheadMoveThreshold = 0.0f;
		}
		else
		{
			damping = 3;
			lookAheadFactor = 3;
			lookAheadReturnSpeed = 0.5f;
			lookAheadMoveThreshold = 0.1f;
		}
	}
	void SetRest()
	{

	}

	bool stop = false;

	void Stop()
	{
		target = GameObject.Find("Home").transform;
	}
        // Update is called once per frame
        private void FixedUpdate()
        {
			Vector3 MoveDelta = (target.position - m_LastTargetPosition);

			bool updateLookAheadTarget = Mathf.Abs (MoveDelta.magnitude) > lookAheadMoveThreshold;

			if (updateLookAheadTarget) {
			m_LookAheadPos = lookAheadFactor * MoveDelta;
			} else {
				m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
			}

			Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
			Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

			transform.position = newPos;

			m_LastTargetPosition = target.position;
		}

    }

