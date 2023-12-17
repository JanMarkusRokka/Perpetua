using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{

    private bool isTransparent = false;
    private float invisibilityEnds;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isTransparent && Time.time > invisibilityEnds)
        {
            isTransparent = false;

            StartCoroutine(FadeIn());
        }
    }

    public void MakeTransparent()
    {
        if (!isTransparent) StartCoroutine(FadeOut());

        isTransparent = true;

        invisibilityEnds = Time.time + 0.1f;
    }

    IEnumerator FadeOut()
    {
        for (float alpha = 1f; alpha >= 0.4; alpha -= 0.1f)
        {
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer r in childRenderers)
            {
                Color c = r.material.color;
                c.a = alpha;
                r.material.color = c;
            }
            Renderer renderer = GetComponent<Renderer>();

            if (renderer)
            {
                Color c = renderer.material.color;
                c.a = alpha;
                renderer.material.color = c;
            }
            yield return new WaitForSeconds(.001f);
        }
        this.enabled = false;
    }

    IEnumerator FadeIn()
    {

        for (float alpha = 0.4f; alpha <= 1.1f; alpha += 0.1f)
        {
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer r in childRenderers)
            {
                Color c = r.material.color;
                c.a = alpha;
                r.material.color = c;
            }
            Renderer renderer = GetComponent<Renderer>();

            if (renderer)
            {
                Color c = renderer.material.color;
                c.a = alpha;
                renderer.material.color = c;
            }
            yield return new WaitForSeconds(.001f);
        }
    }
}
