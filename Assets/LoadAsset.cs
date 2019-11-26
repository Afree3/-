using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadAsset : MonoBehaviour
{
    private string bundlePath;
    [Header("包名")]
    public string bundleName;
    [Header("资源名")]
    public string resName;
    
    void Start()
    {
        Debug.Log($"Application.streamingAssetsPath:{Application.streamingAssetsPath}");
        Debug.Log($"Application.dataPath:{Application.dataPath + "/StreamingAssets/"}");

        bundlePath =
#if UNITY_ANDROID
            "jar:file://+Application.dataPath+"!/assets/";
#elif UNITY_IPHONE
            Applicaton.dataPath+"/Raw/";
#elif UNITY_STANDALONE_WIN || UNTITY_EDITOR
            "file://" + Application.dataPath + "/StreamingAssets/";
            //"file://" + Application.streamingAssetsPath;// 为什么这个路径加载不了？？？
#endif
    }

    public IEnumerator Load(string bundleName,string resName)
    {
        string url = bundlePath + bundleName;
        #region www弃用

        //WWW www = new WWW(url);
        //yield return www;
        //if (!string.IsNullOrEmpty(www.error))
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    Object obj = www.assetBundle.LoadAsset(resName);
        //    yield return Instantiate(obj);
        //    www.assetBundle.Unload(false);
        //}
        //www.Dispose();

        #endregion

        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();

        //AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        GameObject go = ab.LoadAsset<GameObject>(resName);
        Instantiate(go);

    }

    public IEnumerator LoadSceneBundle(string bundleName,string resName)
    {
        string url = Application.dataPath + "bundleScene.unity";
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        SceneManager.LoadScene("BundleScene");

    }

    /// <summary>
    /// 异步加载场景Bundle
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneBundleAsync()
    {
        // 先下载场景的assetbundle
        //WWW download = WWW.LoadFromCacheOrDownload(Application.dataPath + "/my_scene.unity3d", 1);
        //yield return download;
        //AssetBundle bundle = download.assetBundle;

        string pathBundle = Application.dataPath + "/my_scene.unity3d";     // bundle路径
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(pathBundle);
        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

        // 异步加载场景
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync("TestScene");
        while (!sceneAsync.isDone)
        {
            Debug.Log("loading scene,progress:" + sceneAsync.progress);
            yield return null;
        }

    }

    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUILayout.Button("LoadAsset"))
        {
            StartCoroutine(Load(bundleName,resName));
        }

        if (GUILayout.Button("加载场景"))
        {
            //StartCoroutine(LoadSceneBundle("bundlescene.assetbundle", "BundleScene.unity"));
            StartCoroutine(LoadSceneBundleAsync());
        }
    }
}
