using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject {

	public float moveSpeed = 7.0f;
	public float jumpPow = 5.0f;
	public float gimmickDistance = 10.0f;
	public int skillTime = 72;
}
