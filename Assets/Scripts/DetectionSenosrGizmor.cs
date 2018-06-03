using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class DetectionSenosrGizmor {

	private static Mesh CreatFunMesh(float i_angle, int i_triangleCount)
	{
		Mesh mesh = new Mesh();

		mesh.vertices = null;
		mesh.triangles = null;

		mesh.RecalculateNormals();

		return mesh;
	}

	private static Vector3[] CreateFanVertices(float iAngle, int iTriangleCount)
	{
		if (iAngle <= 0.0f)
		{
			throw new System.ArgumentException(string.Format("角度がおかしい！ i_angle={0}", iAngle));
		}

		if (iTriangleCount <= 0)
		{
			throw new System.ArgumentException(string.Format("数がおかしい！ i_triangleCount={0}", iTriangleCount));
		}

		iAngle = Mathf.Min(iAngle, 360.0f);

		List<Vector3> aVertices = new List<Vector3>(iTriangleCount + 2);

		aVertices.Add(Vector3.zero);

		float radian = iAngle * Mathf.Deg2Rad;
		float startRad = -radian / 2;
		float incRad = radian / iTriangleCount;

		for (int i = 0; i < iTriangleCount + 1; ++i)
		{
			float aCurrentRad = startRad + (incRad * i);

			Vector3 vertex = new Vector3(Mathf.Sin(aCurrentRad), 0.0f, Mathf.Cos(aCurrentRad));
			aVertices.Add(vertex);
		}

		return aVertices.ToArray();
	}

	private static Mesh CreateFanMesh(float i_angle, int i_triangleCount)
	{
		Mesh aMesh = new Mesh();

		Vector3[] aVertices = CreateFanVertices(i_angle, i_triangleCount);

		List<int> aTriangleIndexes = new List<int>(i_triangleCount * 3);

		for (int i = 0; i < i_triangleCount; ++i)
		{
			aTriangleIndexes.Add(0);
			aTriangleIndexes.Add(i + 1);
			aTriangleIndexes.Add(i + 2);
		}

		aMesh.vertices = aVertices;
		aMesh.triangles = aTriangleIndexes.ToArray();

		aMesh.RecalculateNormals();

		return aMesh;
	}


	[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
	private static void DrawPointGizmos(SearchingBehavior iSensor, GizmoType iGizmoType)
	{
		if (iSensor.SectorLength > 0.0f)
		{
			Gizmos.color                    = iSensor.SectorSenosrColor;
			Transform   aTransform = iSensor.transform;
			Vector3        aPosition    = aTransform.position + Vector3.up * 0.1f;
			Quaternion aRotation   = aTransform.rotation;
			Vector3        aScale         = Vector3.one * iSensor.SectorLength;
			Mesh            aFanMesh  = CreateFanMesh(CIRCLE_ANGLE, TRIANGLE_COUNT);

			if (iSensor.SectorSenosrAngle > 0.0f)
			{
				Mesh fanMesh = CreateFanMesh(iSensor.SectorSenosrAngle, TRIANGLE_COUNT);

				Gizmos.DrawMesh(fanMesh, aPosition, aRotation, aScale);
				Gizmos.DrawMesh(fanMesh, aPosition, aRotation * Quaternion.AngleAxis(180.0f, Vector3.forward), aScale);
			}
		}

		if (iSensor.NearLength > 0.0f)
		{
			Gizmos.color                    = iSensor.NearSenosrColor;
			Transform   aTransform = iSensor.transform;
			Vector3        aPosition    = aTransform.position + Vector3.up * 0.1f;
			Quaternion aRotation   = aTransform.rotation;
			Vector3        aScale         = Vector3.one * iSensor.NearLength;
			Mesh            aFanMesh  = CreateFanMesh(CIRCLE_ANGLE, TRIANGLE_COUNT);

			Gizmos.DrawMesh(aFanMesh, aPosition, aRotation, aScale);
			Gizmos.DrawMesh(aFanMesh, aPosition, aRotation * Quaternion.AngleAxis(180.0f, Vector3.forward), aScale);
		}
	}

	private static readonly int CIRCLE_ANGLE = 360;
	private static readonly int TRIANGLE_COUNT = 36;
	private static readonly Color MESH_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.7f);
}
