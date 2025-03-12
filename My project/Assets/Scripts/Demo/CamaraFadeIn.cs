using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CamaraFadeIn : MonoBehaviour
{
    void Awake()
    {
        PostProcessVolume ppv = GetComponent<PostProcessVolume>();
        DepthOfField dof = ppv.profile.GetSetting<DepthOfField>();

        StartCoroutine(CameraFadeInEffect(dof));
    }

    private IEnumerator CameraFadeInEffect(DepthOfField dof)
    {
        float timer = 0f;
        float duration = 8f;

        for(float kp = timer / duration; timer < duration; timer += Time.deltaTime)
        {
            kp = timer / duration;
            dof.focusDistance.value += kp;

            yield return null;
        }
    }
}
