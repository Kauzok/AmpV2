using UnityEngine;
using System.Collections;

/// <summary>
/// Example script showing how you might use the Lightning script to create a lightning effect at run time.
///  Just set "Spawn" to true in the inspector to generate another set of lighting bolts.
/// </summary>
public class LightningGenerator : MonoBehaviour {

	public Lightning LightningPrefab;

	public GameObject EndPoint;
	public int NumToSpawn = 5;

	public bool Spawn = true;

	void Update()
	{
		if (Spawn)
		{
			Spawn = false;
			for (int i = 0; i < NumToSpawn; i++)
			{
				GameObject litGO = Instantiate(LightningPrefab.gameObject, transform.position, Quaternion.identity) as GameObject;
				if (EndPoint != null)
				{
					Lightning lit = litGO.GetComponent<Lightning>();
					lit.EndPoint = EndPoint.transform;
				}
			}
		}
	}
}