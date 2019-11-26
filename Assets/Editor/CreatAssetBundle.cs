using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatAssetBundle 
{
    [MenuItem("Build/Build AssetBundle")]
    static void BuildAllAssetBundle()
    {
#if UNITY_ANDROID
        Debug.Log("Android平台打包成功");
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, 
            BuildAssetBundleOptions.UncompressedAssetBundle, 
            BuildTarget.Android);
#elif UNITY_IPHONE
        Debug.Log("IOS平台打包成功");
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, 
            BuildAssetBundleOptions.UncompressedAssetBundle, 
            BuildTarget.IOS);
#elif UNITY_STANDALONE_WIN||UNITY_EDITOR
        Debug.Log("PC平台打包成功");
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, 
            BuildAssetBundleOptions.UncompressedAssetBundle,       
            BuildTarget.StandaloneWindows);

        //AssetImporter asset = AssetImporter.GetAtPath(Application.dataPath+"/StreamingAssets/");
        //Debug.Log(asset);
        //asset.assetBundleName = "myAB";
        //asset.assetBundleVariant = "lpf";
#endif
    }

    [MenuItem("Build/Build MyScene")]
    static void BuildMyScene()
    {
        string[] path = { "Assets/Scenes/TestScene.unity" };
        BuildPipeline.BuildPlayer(path, Application.dataPath + "/my_scene.unity3d", BuildTarget.StandaloneWindows64, BuildOptions.BuildAdditionalStreamedScenes);
        AssetDatabase.Refresh();
        Debug.Log("场景打包成功");
    }

    //[MenuItem("Build/Build MyScene")]
    //static void BuildMyScene2()
    //{
    //    Caching.ClearCache();       // 清空一下缓存
    //    string path = Application.dataPath + "my_scene.unity3d";
    //    string[] levels = { "Assets/Scenes/TestScene.unity" };
    //    BuildPipeline.BuildPlayer(levels, path, BuildTarget.StandaloneWindows64, BuildOptions.BuildAdditionalStreamedScenes);
    //    AssetDatabase.Refresh();
    //    Debug.Log("场景打包成功");

    //}

    [MenuItem("Tools/New Option %#q")]
    static void My_test()
    {
        Debug.Log("快捷键");
    }
}
