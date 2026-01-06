using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction PCMac")]
public class ColorCorrectionEffectPCMac_26 : ImageEffectBase
{
	public Texture textureRampPC;

	public Texture textureRampMac;

	public float rampOffsetR;

	public float rampOffsetG;

	public float rampOffsetB;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		bool flag = false;
		switch (Application.platform)
		{
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.OSXWebPlayer:
		case RuntimePlatform.OSXDashboardPlayer:
			flag = true;
			break;
		}
		if (flag)
		{
			base.material.SetTexture("_RampTex", textureRampMac);
		}
		else
		{
			base.material.SetTexture("_RampTex", textureRampPC);
		}
		base.material.SetVector("_RampOffset", new Vector4(rampOffsetR, rampOffsetG, rampOffsetB, 0f));
		Graphics.Blit(source, destination, base.material);
	}
}
