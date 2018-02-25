using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private AudioSource gimmickSource;
    private AudioSource playerMoveSource;
    public AudioSource PlayerMoveSource
    {
        get { return playerMoveSource; }
    }
    private AudioSource playerSource;
    private AudioSource systemSource;

    public AudioClip mushroomSe;
    public AudioClip growthTreeSe;
    public AudioClip jumpSe;
    public AudioClip walkSe;
    public AudioClip runSe;
    public AudioClip titlePushSe;

    public static SoundManager Instance { get; private set; }
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

    void Start ()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        gimmickSource = audioSources[0];
        playerMoveSource = audioSources[1];
        playerSource = audioSources[2];
        systemSource = audioSources[3];
    }
	
	void Update ()
    {
		
	}

    public void PlayGimmickMushroomSE()
    {
        gimmickSource.PlayOneShot(mushroomSe);
    }

    public void PlayGimmickGrowthTreeSE()
    {
        gimmickSource.PlayOneShot(growthTreeSe);
    }

    public void PlayJumpSE()
    {
        playerSource.PlayOneShot(jumpSe);
    }

    public void PlayWalkSE()
    {
        playerMoveSource.loop = true;
        playerMoveSource.clip = walkSe;
        playerMoveSource.Play();
    }

    public void PlayRunSE()
    {
        playerMoveSource.loop = true;
        playerMoveSource.clip = runSe;
        playerMoveSource.Play();
    }

    public void StopMoveSE()
    {
        playerMoveSource.loop = false;
        playerMoveSource.Stop();
    }

    public void PlayTitlePushSE()
    {
        systemSource.PlayOneShot(titlePushSe);
    }
}
