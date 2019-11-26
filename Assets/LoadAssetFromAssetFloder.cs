using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class LoadAssetFromAssetFloder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 此方法只能在编辑器下使用，当项目打包后，在游戏内无法运作。参数为包含Assets内的文件全路径，并且需要文件后缀
        //TextAsset ta = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/test.txt");
        //Debug.Log(ta.text.Split('!')[0]);

        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.dataPath);
        //Debug.Log(Application.persistentDataPath);


        // 各平台下StreamingAssets文件夹的等价路径
        string path = Application.dataPath + "/StreamingAssets"; //Windows OR MacOS 
        path = Application.dataPath + "/Raw";   //IOS 
        path = "jar:file://" + Application.dataPath + "!/assets/";  // Android

        // StreamingAssets文件夹下的文件在游戏中只能通过IO Stream或者WWW的方式读取（AssetBundle除外）
        string msg = "这是新加的内容";
        byte[] newBtyes = System.Text.Encoding.UTF8.GetBytes(msg);
        using (FileStream fsWrite = new FileStream(Application.streamingAssetsPath + "/test01.txt", FileMode.Create))
        {
            fsWrite.Write(newBtyes, 0, newBtyes.Length);
        }

        using(FileStream fs = File.Open(Application.streamingAssetsPath+ "/test01.txt", FileMode.Open))
        {
            int len = (int)fs.Length;
            byte[] bytes = new byte[len];
            int r = fs.Read(bytes, 0, bytes.Length);
            string srt = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log(srt);

        }

        //  AssetBundle特有的同步读取方式（注意安卓平台下的路径区别）
        string assetBundlePath =
#if UNITY_ANDROID
            Application.dataPath+"!assets";
#else
            Application.streamingAssetsPath;
#endif
        AssetBundle ab = AssetBundle.LoadFromFile(assetBundlePath + "/name.assetBundle");
        GameObject go = ab.LoadAsset<GameObject>("cube");
        Instantiate(go, new Vector3(0,0,0), Quaternion.identity);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
