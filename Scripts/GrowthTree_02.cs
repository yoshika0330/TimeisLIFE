using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthTree_02 : MonoBehaviour
{

    GameObjHighlight _objHighlight;
    public GameObject growthTree;
    private Renderer r;
    private bool changeOnce = false;
    public int useTime = 5;


    void Start()
    {
        _objHighlight = GetComponent<GameObjHighlight>();
        r = growthTree.GetComponent<Renderer>();
    }

    void Update()
    {
        if (_objHighlight.IsHighlighted)
        {
            if (PlayerController.Instance.R_StickRotationCount >= useTime && !changeOnce)
            {
                changeOnce = true;
                StartCoroutine(ViewTree());
                GetComponent<MeshCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    IEnumerator ViewTree()
    {
        growthTree.GetComponent<MeshRenderer>().enabled = true;

        SoundManager.Instance.PlayGimmickGrowthTreeSE();
        for (float f = 0; f <= 1; f += 0.01f)
        {
            r.materials[0].SetFloat("_T", f);
            r.materials[1].SetFloat("_T", f);
            yield return null;
        }
//        SoundManager.Instance.PlayGimmickGrowthTreeSE();
//        for (float f = 0; f <= 1; f += 0.02f)
//        {
//            r.materials[0].SetFloat("_T", f);
//            yield return null;
//        }
        growthTree.GetComponent<MeshCollider>().enabled = true;
    }
}
