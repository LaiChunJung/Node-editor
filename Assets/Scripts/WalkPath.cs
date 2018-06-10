using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{

	public List<Transform> PathNodes;
	private List<Vector3> PathPositions;
	// Use this for initialization
	void Start()
	{
		for (int Index = 0; Index < PathNodes.Count; Index++)
		{
			PathPositions.Add(PathNodes[Index].transform.position);
		}
	}

	void OnDrawGizmosSelected()
	{
		if (PathNodes.Count > 0)
		{
			Gizmos.color = Color.red;
			Vector3 aHeight = Vector3.up * 0.5f;
			for (int i = 0; i < PathNodes.Count; i++)
			{
				Gizmos.DrawSphere(PathNodes[i].position+aHeight, 0.25f);
				if (i==0)
				{
					Gizmos.DrawLine(transform.position+ aHeight, PathNodes[i].position+ aHeight);
				}
				else
				{
					Gizmos.DrawLine(PathNodes[i-1].position+ aHeight, PathNodes[i].position+ aHeight);
				}
			}				
		}
	}
}