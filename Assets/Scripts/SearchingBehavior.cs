using UnityEngine;
using System.Collections.Generic;

public class SearchingBehavior : MonoBehaviour
{
	public event System.Action<GameObject> onFound = (obj) => { };
	public event System.Action<GameObject> onLost = (obj) => { };
	public Color NearSenosrColor = Color.red;
	public Color SectorSenosrColor = Color.red;

	[SerializeField, Range(0.0f, 360.0f)]
	private float mSectorSenosrAngle = 0.0f;

	[SerializeField, Range(0.0f,15.0f)]
	private float mSectorLength = 0.0f;

	[SerializeField, Range(0.0f, 5.0f)]
	private float mNearLength = 0.0f;

	public float SectorSenosrAngle { get { return mSectorSenosrAngle; } }
	public float SectorLength { get { return mSectorLength; } }
	public float NearLength { get { return mNearLength; } }


	private SphereCollider mSphereCollider = null;
	private List<FoundData> mFoundList = new List<FoundData>();



	public float SearchAngle
	{
		get { return mSectorSenosrAngle; }
	}

	public float SearchRadius
	{
		get
		{
			if (mSphereCollider == null)
			{
				mSphereCollider = GetComponent<SphereCollider>();
			}
			return mSphereCollider != null ? mSphereCollider.radius : 0.0f;
		}
	}


	private void Awake()
	{
		mSphereCollider = GetComponent<SphereCollider>();
		ApplySearchAngle();
	}

	private void OnDisable()
	{
		mFoundList.Clear();
	}

	// シリアライズされた値がインスペクター上で変更されたら呼ばれます。
	private void OnValidate()
	{
		ApplySearchAngle();
	}

	private void ApplySearchAngle()
	{
		float searchRad = mSectorSenosrAngle * 0.5f * Mathf.Deg2Rad;
		//mLength = Mathf.Cos(searchRad);
	}

	private void Update()
	{
		UpdateFoundObject();
	}

	private void UpdateFoundObject()
	{
		foreach (var foundData in mFoundList)
		{
			GameObject targetObject = foundData.Obj;
			if (targetObject == null)
			{
				continue;
			}

			bool isFound = CheckFoundObject(targetObject);
			foundData.Update(isFound);

			if (foundData.IsFound())
			{
				onFound(targetObject);
			}
			else if (foundData.IsLost())
			{
				onLost(targetObject);
			}
		}
	}

	private bool CheckFoundObject(GameObject i_target)
	{
		Vector3 targetPosition = i_target.transform.position;
		Vector3 myPosition = transform.position;

		Vector3 myPositionXZ = Vector3.Scale(myPosition, new Vector3(1.0f, 0.0f, 1.0f));
		Vector3 targetPositionXZ = Vector3.Scale(targetPosition, new Vector3(1.0f, 0.0f, 1.0f));

		Vector3 toTargetFlatDir = (targetPositionXZ - myPositionXZ).normalized;
		Vector3 myForward = transform.forward;
		if (!IsWithinRangeAngle(myForward, toTargetFlatDir, mSectorLength))
		{
			return false;
		}

		Vector3 toTargetDir = (targetPosition - myPosition).normalized;

		if (!IsHitRay(myPosition, toTargetDir, i_target))
		{
			return false;
		}

		return true;
	}

	private bool IsWithinRangeAngle(Vector3 i_forwardDir, Vector3 i_toTargetDir, float i_cosTheta)
	{
		// 方向ベクトルが無い場合は、同位置にあるものだと判断する。
		if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
		{
			return true;
		}

		float dot = Vector3.Dot(i_forwardDir, i_toTargetDir);
		return dot >= i_cosTheta;
	}

	private bool IsHitRay(Vector3 i_fromPosition, Vector3 i_toTargetDir, GameObject i_target)
	{
		if (i_toTargetDir.sqrMagnitude <= Mathf.Epsilon)
		{
			return true;
		}

		RaycastHit onHitRay;
		if (!Physics.Raycast(i_fromPosition, i_toTargetDir, out onHitRay, SearchRadius))
		{
			return false;
		}

		if (onHitRay.transform.gameObject != i_target)
		{
			return false;
		}

		return true;
	}

	private void OnTriggerEnter(Collider iCollider)
	{
		GameObject enterObject = iCollider.gameObject;

		if (mFoundList.Find(value => value.Obj == enterObject) == null)
		{
			mFoundList.Add(new FoundData(enterObject));
		}
	}

	private void OnTriggerExit(Collider i_other)
	{
		GameObject exitObject = i_other.gameObject;

		var foundData = mFoundList.Find(value => value.Obj == exitObject);
		if (foundData == null)
		{
			return;
		}

		if (foundData.IsCurrentFound())
		{
			onLost(foundData.Obj);
		}

		mFoundList.Remove(foundData);
	}


	private class FoundData
	{
		public FoundData(GameObject i_object)
		{
			m_obj = i_object;
		}

		private GameObject m_obj = null;
		private bool m_isCurrentFound = false;
		private bool m_isPrevFound = false;

		public GameObject Obj
		{
			get { return m_obj; }
		}

		public Vector3 Position
		{
			get { return Obj != null ? Obj.transform.position : Vector3.zero; }
		}

		public void Update(bool i_isFound)
		{
			m_isPrevFound = m_isCurrentFound;
			m_isCurrentFound = i_isFound;
		}

		public bool IsFound()
		{
			return m_isCurrentFound && !m_isPrevFound;
		}

		public bool IsLost()
		{
			return !m_isCurrentFound && m_isPrevFound;
		}

		public bool IsCurrentFound()
		{
			return m_isCurrentFound;
		}
	}

}