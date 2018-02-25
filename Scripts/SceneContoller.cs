using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneContoller : MonoBehaviour {

    [SerializeField]
    private string sceneName;

    public Slider loadSlider;
    public Text loadText;
    public GameObject loadUis;

    public static SceneContoller Instance { get; private set; }
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

    public IEnumerator LoadScene()
    {
        // ロード用のUIを表示
        loadUis.SetActive(true);

        AsyncOperation nextScene = Application.LoadLevelAsync(sceneName);
        nextScene.allowSceneActivation = false;
        while (nextScene.progress < 0.9f)
        {
            // ロードの現状を％で表示
            loadText.text = (nextScene.progress * 100).ToString("F0") + "%";
            // ロードの現状をバーで表示
            loadSlider.value = nextScene.progress;
            yield return new WaitForEndOfFrame();
        }
        loadSlider.value = 1;
        loadText.text = "100%";
        yield return new WaitForSeconds(0.5f);
        nextScene.allowSceneActivation = true;
        yield return null;

    }
}
