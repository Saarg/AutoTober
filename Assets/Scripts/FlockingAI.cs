using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : MonoBehaviour {

	static private List<FlockingAI> _entities = new List<FlockingAI>();

	[SerializeField]
	private Transform _target;

	[SerializeField][Range(0.0f, 20.0f)] private float _speed = 6.0f;

	[SerializeField][Range(0.0f, 100.0f)] private float _visionDistance = 10.0f;
	[SerializeField][Range(10, 360)] private int _visionField = 170;
	[SerializeField][Range(0, 180)] private int _angleVariation = 30;

	[SerializeField][Range(0.0f, 2.0f)]	private float _separation = 1.0f;
	[SerializeField][Range(0.0f, 2.0f)]	private float _alignment = 1.0f;
	[SerializeField][Range(0.0f, 2.0f)]	private float _cohesion = 1.0f;
	[SerializeField][Range(0.0f, 2.0f)]	private float _focus = 1.0f;
	[SerializeField][Range(0.0f, 2.0f)]	private float _bravery = 1.0f;
	[SerializeField][Range(0.0f, 2.0f)]	private float _homeLove = 1.0f;

	private Vector3 _lastDir;
	[SerializeField] bool randomStartRot = true;
	[SerializeField] bool randomize = true;

	[SerializeField] LayerMask _avoidLayerMask;

	void Start () 
	{
		_entities.Add(this);

		if (randomStartRot) {
			transform.Rotate(0, Random.Range(-180, 180), 0);
		}

		_lastDir = transform.forward;

		if (randomize) {
			_speed += Random.Range(-_speed/10, _speed/10);

			_visionDistance += Random.Range(-_visionDistance/10, _visionDistance/10);
			_visionField += Random.Range(-_visionField/10, _visionField/10);

			_angleVariation += Random.Range(-_angleVariation/10, _angleVariation/10);

			_separation += Random.Range(-_separation/10, _separation/10);
			_alignment += Random.Range(-_alignment/10, _alignment/10);
			_cohesion += Random.Range(-_cohesion/10, _cohesion/10);
			
			_focus += Random.Range(-_focus/10, _focus/10);
			_bravery += Random.Range(-_bravery/10, _bravery/10);
			_homeLove += Random.Range(-_homeLove/10, _homeLove/10);
		}

		// Randomize start animation to look less rigid
		Animator anim = GetComponentInChildren<Animator>();
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
		anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
	}
	
	void OnDestroy()
	{
		_entities.Remove(this);
	}

	void Update () 
	{
		Vector3 myPos = transform.position;
		Vector3 dir = Vector3.zero;
		
		// Compute flocking forces
		{
			int detectionCount = 0;
			Vector3 separation = Vector3.zero;
			Vector3 alignment = Vector3.zero;
			Vector3 cohesion = Vector3.zero;
			foreach (FlockingAI e in _entities)
			{
				if (e == null || e == this)
					continue;

				Vector3 distance = e.transform.position - myPos;

				if (distance.sqrMagnitude < _visionDistance*_visionDistance)
				{
					separation += distance;

					alignment += e.transform.forward;

					cohesion += e.transform.position;

					detectionCount++;
				}	
			}

			if (detectionCount > 0)
			{
				separation /= -detectionCount;
				alignment /= detectionCount;
				cohesion = cohesion / detectionCount - myPos;

				separation.Normalize();
				alignment.Normalize();
				cohesion.Normalize();

				Vector3 flocking = separation * _separation + alignment * _alignment + cohesion * _cohesion;
				flocking.Normalize();

				dir += flocking;
			}
		}

		// Target force
		if (_target != null)
		{
			Vector3 targetDir = _target.position - myPos;
			targetDir.Normalize();

			dir += targetDir * _focus;
		}
		
		// Wander
		{
			Vector3 Wander = Quaternion.AngleAxis(Random.Range(-1, 1) * Mathf.PerlinNoise(myPos.x, myPos.z) * 180, transform.up) * transform.forward;
			Wander.Normalize();

			dir += Wander * _bravery;
		}

		// Home force
		{
			Vector3 toCenter = -myPos;
			toCenter.y = 0;
			toCenter.Normalize();
			
			dir += toCenter * _homeLove;
		}

		// Obstacle avoidance
		{
			RaycastHit hit;

			Color noHit = Color.yellow;
			noHit.a = 0.2f;
			Color hasHit = Color.yellow;

			if (Physics.Raycast(transform.position + transform.up, Quaternion.AngleAxis(30, transform.up) * transform.forward, out hit, _speed, ~_avoidLayerMask))
			{
				Debug.DrawRay(transform.position + transform.up, Quaternion.AngleAxis(30, transform.up) * transform.forward * _speed, hasHit);

				_lastDir = Quaternion.AngleAxis(-30, transform.up) * _lastDir;
			}
			else
			{
				Debug.DrawRay(transform.position + transform.up, Quaternion.AngleAxis(30, transform.up) * transform.forward * _speed, noHit);
			}

			if (Physics.Raycast(transform.position + transform.up, Quaternion.AngleAxis(-30, transform.up) * transform.forward, out hit, _speed, ~_avoidLayerMask))
			{
				Debug.DrawRay(transform.position + transform.up, Quaternion.AngleAxis(-30, transform.up) * transform.forward * _speed, hasHit);

				_lastDir = Quaternion.AngleAxis(30, transform.up) * _lastDir;
			}
			else
			{
				Debug.DrawRay(transform.position + transform.up, Quaternion.AngleAxis(-30, transform.up) * transform.forward * _speed, noHit);
			}
		}

		dir.y = 0;
		dir.Normalize();

		_lastDir = Vector3.Lerp(_lastDir, dir, Time.deltaTime);

		#if UNITY_EDITOR
			Debug.DrawLine(myPos, myPos + dir, Color.red);
			Debug.DrawLine(myPos, myPos + _lastDir, Color.green);
		#endif

		transform.Translate(_lastDir * _speed * Time.deltaTime, Space.World);

		Quaternion oldRot = transform.rotation;
		transform.LookAt(myPos + _lastDir);

		transform.rotation = Quaternion.Lerp(oldRot, transform.rotation, 2 * Time.deltaTime);
	}

	void OnDrawGizmos()
    {
		Gizmos.color = new Color(0.0f, 0.6f, 0.6f, 0.1f);
        Gizmos.DrawSphere(transform.position, _visionDistance);

		Vector3 leftFOV = (Quaternion.AngleAxis(-_visionField/2, transform.up) * transform.forward * _visionDistance);
		Vector3 rightFOV = (Quaternion.AngleAxis(_visionField/2, transform.up) * transform.forward * _visionDistance);

		Gizmos.color = new Color(0.6f, 0.0f, 0.0f, 0.4f);
		Gizmos.DrawLine(transform.position, transform.position + leftFOV);
		Gizmos.DrawLine(transform.position, transform.position + rightFOV);

		Gizmos.color = new Color(0.4f, 0.0f, 0.0f, 0.1f);
		for (float i = 0.0f; i < 1; i += 0.1f) 
		{
			int j;
			for (j = 0; j < _visionField/2 - 10; j += 10) {
				Gizmos.DrawLine(transform.position + i * (Quaternion.AngleAxis(j, transform.up) * transform.forward * _visionDistance), transform.position + i * (Quaternion.AngleAxis(j+10, transform.up) * transform.forward * _visionDistance));
				Gizmos.DrawLine(transform.position + i * (Quaternion.AngleAxis(-j, transform.up) * transform.forward * _visionDistance), transform.position + i * (Quaternion.AngleAxis(-j-10, transform.up) * transform.forward * _visionDistance));
			}

			Gizmos.DrawLine(transform.position + i * (Quaternion.AngleAxis(j, transform.up) * transform.forward * _visionDistance), transform.position + i * rightFOV);
			Gizmos.DrawLine(transform.position + i * (Quaternion.AngleAxis(-j, transform.up) * transform.forward * _visionDistance), transform.position + i * leftFOV);
		}

		Gizmos.color = new Color(0.6f, 0.0f, 0.0f, 0.4f);

		int k;
		for (k = 0; k < _visionField/2 - 10; k += 10) {
			Gizmos.DrawLine(transform.position + (Quaternion.AngleAxis(k, transform.up) * transform.forward * _visionDistance), transform.position + (Quaternion.AngleAxis(k+10, transform.up) * transform.forward * _visionDistance));
			Gizmos.DrawLine(transform.position + (Quaternion.AngleAxis(-k, transform.up) * transform.forward * _visionDistance), transform.position + (Quaternion.AngleAxis(-k-10, transform.up) * transform.forward * _visionDistance));
		}

		Gizmos.DrawLine(transform.position + (Quaternion.AngleAxis(k, transform.up) * transform.forward * _visionDistance), transform.position + rightFOV);
		Gizmos.DrawLine(transform.position + (Quaternion.AngleAxis(-k, transform.up) * transform.forward * _visionDistance), transform.position + leftFOV);
    }
}
