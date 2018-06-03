using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<AISystem> : MonoBehaviour where AISystem : MonoBehaviour
{
	private static AISystem mInstance;

	private static object mLock = new object();

	public static AISystem Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.LogWarning("[Singleton] Instance '" + typeof(AISystem) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock (mLock)
			{
				if (mInstance == null)
				{
					mInstance = (AISystem)FindObjectOfType(typeof(AISystem));

					if (FindObjectsOfType(typeof(AISystem)).Length > 1)
					{
						return mInstance;
					}

					if (mInstance == null)
					{
						GameObject singleton = new GameObject();
						mInstance = singleton.AddComponent<AISystem>();
						singleton.name = "(singleton) " + typeof(AISystem).ToString();

						DontDestroyOnLoad(singleton);
					}
					else {
						Debug.Log("[Singleton] Using instance already created: " +
							mInstance.gameObject.name);
					}
				}

				return mInstance;
			}
		}
	}

	private static bool applicationIsQuitting = false;

	public void OnDestroy()
	{
		applicationIsQuitting = true;
	}
}
