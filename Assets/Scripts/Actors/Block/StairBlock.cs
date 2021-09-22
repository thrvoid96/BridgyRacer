using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockBehaviour;

public class StairBlock : BlockBehaviours
{
    private MeshRenderer meshRenderer;
    private Image[] images;

    public Material blockMaterial
    {
        get { return meshRenderer.material; }
        set { meshRenderer.material = value; }
    }

    // Start is called before the first frame update
   private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        images = GetComponentsInChildren<Image>();

        foreach (Image img in images)
        {
            img.color = new Color(1, 1, 1, 0);
        }
    }

    public void doStairEffects()
    {
        StopAllCoroutines();
        StartCoroutine(doBlinkingEffect());
    }


    private IEnumerator doBlinkingEffect()
    {
        foreach(Image img in images)
        {
            img.color = new Color(1, 1, 1, 1);
        }

        while (images[0].color.a >= 0)
        {
            foreach (Image img in images)
            {
                img.color -= new Color(0, 0, 0, 0.1f);
                yield return null;
            }
        }
    }

}
