using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthTrees : MonoBehaviour {

	GameObjHighlight _objHighlight;
	private bool changeOnce = false;
	private Renderer r;

	// Use this for initialization
	void Start () 
	{
		_objHighlight = GetComponent<GameObjHighlight> ();	
		r = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if (_objHighlight.IsHighlighted) 
		//{
			if (PlayerController.Instance.R_StickRotationCount >= 10 && !changeOnce) 
			{
				changeOnce = true;
				StartCoroutine (ViewTree ());
			}
		//}	
	}

	IEnumerator ViewTree()
	{
		for(float f = 0; f <= 1; f += 0.02f)
		{
			r.material.SetFloat("_T",f);
			yield return null;
		}
	}
}
