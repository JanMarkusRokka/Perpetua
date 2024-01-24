using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{

    private bool isTransparent = false;
    private float invisibilityEnds;

    // Still needs fixing (remember last rayed objects and compare?)
    void Update()
    {
        if (isTransparent && Time.time > invisibilityEnds)
        {
            isTransparent = false;

            FadeIn();
        }
    }

    public void MakeTransparent()
    {
        if (!isTransparent) FadeOut();

        isTransparent = true;

        invisibilityEnds = Time.time + 0.1f;
    }

    private void FadeOut()
    {
        //for (float alpha = 1f; alpha >= 0.4; alpha -= 0.1f)
        //{
            Renderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (Renderer r in childRenderers)
            {
                r.enabled = false;
                /**
                Color c = r.material.GetColor("_Color");
                c.a = alpha;
                r.material.SetColor("_Color", c);
                */
            }
            Renderer renderer = GetComponent<MeshRenderer>();

            if (renderer)
            {
                renderer.enabled = false;

                /**
                Color c = renderer.material.GetColor("_Color");
                c.a = alpha;
                renderer.material.SetColor("_Color", c);
                */
            }
            //yield return new WaitForSeconds(.001f);
        //}
    }

    private void FadeIn()
    {

        //for (float alpha = 0.4f; alpha <= 1.1f; alpha += 0.1f)
        //{
            Renderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (Renderer r in childRenderers)
            {
                r.enabled = true;
                /**
                Color c = r.material.GetColor("_Color");
                c.a = alpha;
                r.material.SetColor("_Color", c);
                */
            }
            Renderer renderer = GetComponent<MeshRenderer>();

            if (renderer)
            {
                renderer.enabled = true;
                /**
                Color c = renderer.material.GetColor("_Color");
                c.a = alpha;
                renderer.material.SetColor("_Color", c);
                */
            }
        //yield return new WaitForSeconds(.001f);
        //}
        this.enabled = false;

    }
}
