using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	private GameObject player;
	public GameObject warpPoint;

	private bool isWarp = false;
	private bool onceFadeOut = false;

	void Start () 
	{
		player = GameObject.FindWithTag ("Player");
	}
	

	void Update () 
	{
		if (isWarp) 
		{
			Debug.Log ("warp");
			PlayerController.Instance.isMove = false;
			if (!onceFadeOut) 
			{
				Debug.Log ("warp01");
				onceFadeOut = true;
				StartCoroutine (Fade.Instance.FadeOut ());
			}
			if (Fade.Instance.Alpha >= 1)
			{
				player.transform.position = warpPoint.transform.position;
				StartCoroutine (Fade.Instance.FadeIn ());
				isWarp = false;
				PlayerController.Instance.isMove = true;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			isWarp = true;
			if (onceFadeOut) 
			{
				onceFadeOut = false;
			}
		}
	}
}
