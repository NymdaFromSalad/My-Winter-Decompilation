using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class ExplosionFireAndDebris : MonoBehaviour
	{
		public Transform[] debrisPrefabs;

		public Transform firePrefab;

		public int numDebrisPieces;

		public int numFires;

		private IEnumerator Start()
		{
			float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;
			for (int n = 0; (float)n < (float)numDebrisPieces * multiplier; n++)
			{
				Transform prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];
				Vector3 pos = base.transform.position + Random.insideUnitSphere * 3f * multiplier;
				Quaternion rot = Random.rotation;
				Object.Instantiate(prefab, pos, rot);
			}
			yield return null;
			float r = 10f * multiplier;
			Collider[] cols = Physics.OverlapSphere(base.transform.position, r);
			Collider[] array = cols;
			foreach (Collider col in array)
			{
				if (numFires > 0)
				{
					Ray fireRay = new Ray(base.transform.position, col.transform.position - base.transform.position);
					RaycastHit fireHit;
					if (col.Raycast(fireRay, out fireHit, r))
					{
						AddFire(col.transform, fireHit.point, fireHit.normal);
						numFires--;
					}
				}
			}
			float testR = 0f;
			while (numFires > 0 && testR < r)
			{
				Ray fireRay2 = new Ray(base.transform.position + Vector3.up, Random.onUnitSphere);
				RaycastHit fireHit2;
				if (Physics.Raycast(fireRay2, out fireHit2, testR))
				{
					AddFire(null, fireHit2.point, fireHit2.normal);
					numFires--;
				}
				testR += r * 0.1f;
			}
		}

		private void AddFire(Transform t, Vector3 pos, Vector3 normal)
		{
			pos += normal * 0.5f;
			Transform transform = (Transform)Object.Instantiate(firePrefab, pos, Quaternion.identity);
			transform.parent = t;
		}
	}
}
