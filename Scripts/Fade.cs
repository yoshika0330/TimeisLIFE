using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

    public GameObject fadePanel;

    private float alpha;
    public float Alpha
    {
        get { return alpha; }
    }
    private float speed = 0.025f;

    public static Fade Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            enabled = false;
            DestroyImmediate(this);
            return;
        }
        Instance = this;
    }

    public IEnumerator FadeOut()
    {
        for(alpha = 0; alpha <= 1.0f; alpha += speed)
        {
            fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

	public IEnumerator FadeIn()
	{
		for(alpha = 1; alpha >= 0f; alpha -= speed)
		{
			fadePanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
			yield return null;
		}
	}
}
