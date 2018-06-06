using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{

	public List<Transform> PathNodes;
	public List<Vector3> PathPositions;
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
			/*LineRenderer line = this.GetComponent<LineRenderer>();
			if (line == null)
			{
				line = this.gameObject.AddComponent<LineRenderer>();
				line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
				line.SetWidth(0.5f, 0.5f);
				line.SetColors(Color.yellow, Color.yellow);
			}

			line.SetVertexCount(PathPositions.Count);*/
			Gizmos.color = Color.red;
			
			for (int i = 0; i < PathNodes.Count; i++)
			{
				if(i==0)
				{
					Gizmos.DrawLine(transform.position, PathNodes[i].position);
				}
				else
				{
					Gizmos.DrawLine(PathNodes[i-1].position, PathNodes[i].position);
				}
			}				
		}
	}
}