using System;
using System.Collections;
using UnityEngine;

namespace SWS
{
	public class PathIndicator : MonoBehaviour
	{
		public float modRotation;

		private ParticleSystem pSys;

		private void Start()
		{
			pSys = GetComponentInChildren<ParticleSystem>();
			StartCoroutine("EmitParticles");
		}

		private IEnumerator EmitParticles()
		{
			yield return new WaitForEndOfFrame();
			while (true)
			{
				float rot = (base.transform.eulerAngles.y + modRotation) * ((float)Math.PI / 180f);
				pSys.startRotation = rot;
				pSys.Emit(1);
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
}
