using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NodeSystem;
public class AIRig : MonoBehaviour {

	public CreatNodeObject AIData;
	private  NavMeshAgent mNavMeshAgent;
	[SerializeField]
	public List<Transform> PathNodes;
	private int mPathIndex = 0;
	void Start ()
	{
		mNavMeshAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mNavMeshAgent != null && mNavMeshAgent.isOnNavMesh)
		{
			mNavMeshAgent.destination = PathNodes[mPathIndex].position;
		}
		if(Vector3.Distance(transform.position, PathNodes[mPathIndex].position)<=0.1f)
		{
			Debug.Log("mPathIndex  " + mPathIndex);
			mPathIndex = mPathIndex >= PathNodes.Count-1 ? 0 : mPathIndex + 1;
		}
	}
	public void OnDrawGizmosSelected()
	{
		if (PathNodes.Count > 0)
		{
			Gizmos.color = Color.red;
			Vector3 aHeight = Vector3.up * 0.5f;
			for (int i = 0; i < PathNodes.Count; i++)
			{
				Gizmos.DrawSphere(PathNodes[i].position + aHeight, 0.25f);
				if (i == 0)
				{
					Gizmos.DrawLine(transform.position + aHeight, PathNodes[i].position + aHeight);
				}
				else
				{
					Gizmos.DrawLine(PathNodes[i - 1].position + aHeight, PathNodes[i].position + aHeight);
				}
			}
		}
	}
	private List<Vector3> PathPositions;
}
