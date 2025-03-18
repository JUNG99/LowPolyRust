using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderTrans : MonoBehaviour
{
    public void SetTransparent(Material targetMaterial)
    {
        if (targetMaterial == null) return;

        targetMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        targetMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        targetMaterial.SetInt("_ZWrite", 0);
        targetMaterial.SetFloat("_Mode", 3);
        targetMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        targetMaterial.EnableKeyword("_ALPHABLEND_ON");
        targetMaterial.DisableKeyword("_ALPHATEST_ON");
        targetMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }
}
