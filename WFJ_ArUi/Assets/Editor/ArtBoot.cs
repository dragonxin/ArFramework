using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using System;

enum ImageOptions
{
    Load_AppLogo, Load_Outside, Load_Inside, Load_BG,
	Main_ArCam, Main_PDF, Main_BG,
	Content_Return, Content_Capture, Content_PlaySound, Content_PlyaAnimation, Content_Version
}

public class ArtBoot : EditorWindow
{
    bool isLogin = false;         //使用前先登陆验证
    string userName;
    string password;

    bool isLoadMode = false;      // edit loading settings
    bool isMainMode = false;       // edit main settings
    bool isContentMode = false;      // edit content settings

    GameObject selected;      //active ui 

    string sourcepath;   //image to copy
	string destpath;     //image to replace

    static string assetPath;

    [MenuItem("AR工具/美术引导")]
    static void Init()
    {
        ArtBoot filter = (ArtBoot)EditorWindow.GetWindowWithRect(typeof(ArtBoot), new Rect(0, 0, 600, 248));
        filter.Show();
    }

    void OnGUI()
    {
    
        if (isLogin)
        {
	        #region login in
	        EditorGUILayout.LabelField("1.点击对应按钮，导入并更新对应资源。");
	        EditorGUILayout.LabelField("2.点击编辑进行排版。");
	        EditorGUILayout.LabelField("3.PS保存时，统一成Web所用格式png。");
	        EditorGUILayout.LabelField("4.更新服务器完成上传。");
	
	        EditorGUILayout.BeginHorizontal();
	        GUI.color = Color.cyan;
	        if(GUILayout.Button("加载界面设置"))
	        {
	            isLoadMode = true;
	            isMainMode = false;
	            isContentMode = false;
	
	            //打开编辑场景及预览试图
	            EditorApplication.isPaused = false;
	            EditorApplication.isPlaying = false;
	            //EditorApplication.SaveScene(tempScene);
	            EditorSceneManager.OpenScene("Assets/Scene/loading.unity", OpenSceneMode.Single);
	        }
	
	        GUI.color = Color.red;
	        if (GUILayout.Button("主界面设置"))
	        {
	            isLoadMode = false;
	            isMainMode = true;
	            isContentMode = false;
	
	            //打开编辑场景及预览试图
	            EditorApplication.isPaused = false;
	            EditorApplication.isPlaying = false;
	            //EditorApplication.SaveScene(tempScene);
	            EditorSceneManager.OpenScene("Assets/Scene/main.unity", OpenSceneMode.Single);
	        }
	
	        GUI.color = Color.yellow;
	        if (GUILayout.Button("内容界面设置"))
	        {
	            isLoadMode = false;
	            isMainMode = false;
	            isContentMode = true;
	
	            //打开编辑场景及预览试图
	            EditorApplication.isPaused = false;
	            EditorApplication.isPlaying = false;
	            //EditorApplication.SaveScene(tempScene);
	            EditorSceneManager.OpenScene("Assets/Scene/content.unity", OpenSceneMode.Single);
	        }
	        EditorGUILayout.EndHorizontal();
	
	        GUI.color = Color.white;
	
	        if(isLoadMode)
	        {
	            //界面组件
	            GUI.color = Color.cyan;
	            EditorGUILayout.BeginHorizontal();
	
	            if (GUILayout.Button("产品LOGO图标      参考尺寸180*120", GUILayout.Width(460), GUILayout.Height(18)))
	            {
	                imageLoad(ImageOptions.Load_AppLogo);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("product_logo");
	                Selection.activeObject = selected;
	            }
	
	            GUI.color = Color.cyan;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("加载外框图标       参考尺寸800*40", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad(ImageOptions.Load_Outside);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("laoding_process");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.cyan;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("加载进度图标       参考尺寸800*40", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad(ImageOptions.Load_Inside);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("laoding_process");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.cyan;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("加载界面背景图片   参考尺寸960*720", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad(ImageOptions.Load_BG);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("laoding_bg");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.cyan;
	            EditorGUILayout.EndHorizontal();
	
	            if (GUILayout.Button("保存设置"))
	            {
	                selected = GameObject.Find("UI_LoadingPanel");
	                prefabApply(selected);
	            }
	
	            GUI.color = Color.green;
	            if (GUILayout.Button("更新服务器"))
	            {
	                assetPath = Application.dataPath;
	
	                Utils.ZipFileDirectory( assetPath + "/StreamingAssets", assetPath + "/StreamingAssets.zip");
	
	                UpFileToJSP();
	            }
	        }
	
	        if (isMainMode)
	        {
	            GUI.color = Color.red;
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("AR相机按钮图标     参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Main_ArCam);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("main_camera");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.red;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("PDF帮助按钮图标   参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Main_PDF);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("main_pdf_help");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.red;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("主界面背景图片      参考尺寸960*720", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Main_BG);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("main_bg");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.red;
	            EditorGUILayout.EndHorizontal();
	
	            if (GUILayout.Button("保存设置"))
	            {
	                selected = GameObject.Find("UI_MainPanel");
	                prefabApply(selected);
	            }
	
	            GUI.color = Color.green;
	            if (GUILayout.Button("更新服务器"))
	            {
	                assetPath = Application.dataPath;
	
	                Utils.ZipFileDirectory(assetPath + "/StreamingAssets", assetPath + "/StreamingAssets.zip");
	
	                UpFileToJSP();
	            }
	        }
	
	        if (isContentMode)
	        {
	            GUI.color = Color.yellow;
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("返回按钮图标           参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Content_Return);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("content_return");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.yellow;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("截取单帧按钮图标     参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Content_Capture);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("content_capture");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.yellow;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("播放声音按钮图标     参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Content_PlaySound);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("content_sound");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.yellow;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("播放动画按钮图标     参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Content_PlyaAnimation);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("content_animation");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.yellow;
	            EditorGUILayout.EndHorizontal();
	
	
	            EditorGUILayout.BeginHorizontal();
	            if (GUILayout.Button("版本图标                参考尺寸128*128", GUILayout.Width(460), GUILayout.Height(18)))
	            {
					imageLoad (ImageOptions.Content_Version);
	            }
	            GUI.color = Color.white;
	            if (GUILayout.Button("编辑"))
	            {
	                selected = GameObject.Find("content_version");
	                Selection.activeObject = selected;
	            }
	            GUI.color = Color.yellow;
	            EditorGUILayout.EndHorizontal();
	
	            if (GUILayout.Button("保存设置"))
	            {
	                selected = GameObject.Find("UI_ContentPanel");
	                prefabApply(selected);
	            }
	
	            GUI.color = Color.green;
	            if (GUILayout.Button("更新服务器"))
	            {
	                assetPath = Application.dataPath;
	
	                Utils.ZipFileDirectory(assetPath + "/StreamingAssets", assetPath + "/StreamingAssets.zip");
	
	                UpFileToJSP();
	            }
	        }
	        #endregion login in
        }
        else
        {
            #region login out
            userName = EditorGUILayout.TextField("用户名:", userName);
            password = EditorGUILayout.TextField("密码:", password);
            if (GUILayout.Button("登陆"))
            {
                isLogin = Login(userName, password);
                if(!isLogin)
                {
                    this.ShowNotification(new GUIContent("登陆失败"));
                }

            }
            #endregion login out
        }
    }

    //<summary>		
    //Load new image and override the old one in project
    //</summary>
    private void imageLoad(ImageOptions ops)
    {
		sourcepath = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");

		switch (ops) 
		{
		case ImageOptions.Load_AppLogo:
			destpath = Application.dataPath + "/Sprite/ui_loading_product_logo.png";
			break;
		case ImageOptions.Load_Outside:
			destpath = Application.dataPath + "/Sprite/ui_laoding_frame.png";
			break;
		case ImageOptions.Load_Inside:
			destpath = Application.dataPath + "/Sprite/ui_laoding_process.png";
			break;
		case ImageOptions.Load_BG:
			destpath = Application.dataPath + "/Sprite/ui_laoding_frame.png";
			break;

		case ImageOptions.Main_ArCam:
			destpath = Application.dataPath + "/Sprite/ui_main_bg.png";
			break;
		case ImageOptions.Main_PDF:
			destpath = Application.dataPath + "/Sprite/ui_main_bg.png";
			break;
		case ImageOptions.Main_BG:
			destpath = Application.dataPath + "/Sprite/ui_main_bg.png";
			break;

		case ImageOptions.Content_Return:
			destpath = Application.dataPath + "/Sprite/ui_content_return.png";
			break;
		case ImageOptions.Content_Capture:
			destpath = Application.dataPath + "/Sprite/ui_content_return.png";
			break;
		case ImageOptions.Content_PlaySound:
			destpath = Application.dataPath + "/Sprite/ui_content_return.png";
			break;
		case ImageOptions.Content_PlyaAnimation:
			destpath = Application.dataPath + "/Sprite/ui_content_animation.png";
			break;
		case ImageOptions.Content_Version:
			destpath = Application.dataPath + "/Sprite/ui_content_version.png";
			break;
		}

		if (!sourcepath.Equals ("")) {
			FileUtil.ReplaceFile (sourcepath, destpath);
		} else {
			EditorUtility.DisplayDialog ("错误", "图片路径不能为空！", "OK");
		}
        
        AssetDatabase.Refresh();
    }

    //压缩文件上传到服务器 
    private void UpFileToJSP()
    {
        //string url = "http://192.168.1.199:8080/U3dFileToServer/ByteFileContentServlet.do?"; //local
        string url = "http://54.193.6.32:8080/U3dFileToServer/ByteFileContentServlet.do?";
        string filePath = Application.dataPath + "/StreamingAssets.zip";

        WWWForm form = new WWWForm();
        form.AddBinaryData("streamZip", FileContent(filePath), "StreamingAssets.zip");

        WWW upLoad = new WWW(url, form);
        while (!upLoad.isDone)
        {
        }
        //yield return upLoad;

        //如果失败
        if (!string.IsNullOrEmpty(upLoad.error) || upLoad.text.Equals("false"))
        {
            //在控制台输出错误信息
            Debug.Log(upLoad.error);
            this.ShowNotification(new GUIContent("上传失败"));
        }
        else
        {
            //如果成功
            Debug.Log("Finished Uploading Screenshot");
            this.ShowNotification(new GUIContent("上传成功"));
        }

    }

    //将文件转换为字节流
    private byte[] FileContent(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            byte[] buffur = new byte[fs.Length];
            fs.Read(buffur, 0, (int)fs.Length);

            return buffur;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
        finally
        {
            if (fs != null)
            {
                //关闭资源  
                fs.Close();
            }
        }
    }

    #region 实现工程预制体Prefab的Apply应用保存

    private static void prefabApply(GameObject prefabInScene)
    {
        GameObject selectGo = prefabInScene;
        if (selectGo == null)
        {
            Debug.LogError("请选中需要Apply的Prefab实例");
            return;
        }
        PrefabType pType = EditorUtility.GetPrefabType(selectGo);

        if (pType != PrefabType.PrefabInstance)
        {
            Debug.LogError("选中的实例不是Prefab实例");
            return;
        }
        //这里必须获取到prefab实例的根节点，否则ReplacePrefab保存不了
        GameObject prefabGo = GetPrefabInstanceParent(selectGo);
        UnityEngine.Object prefabAsset = null;
        if (prefabGo != null)
        {
            prefabAsset = PrefabUtility.GetPrefabParent(prefabGo);
            if (prefabAsset != null)
            {
                PrefabUtility.ReplacePrefab(prefabGo, prefabAsset, ReplacePrefabOptions.ConnectToPrefab);
            }
        }
        AssetDatabase.SaveAssets();
    }

    //遍历获取prefab节点所在的根prefab节点
    private static GameObject GetPrefabInstanceParent(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        PrefabType pType = EditorUtility.GetPrefabType(go);
        if (pType != PrefabType.PrefabInstance)
        {
            return null;
        }
        if (go.transform.parent == null)
        {
            return go;
        }
        pType = EditorUtility.GetPrefabType(go.transform.parent.gameObject);
        if (pType != PrefabType.PrefabInstance)
        {
            return go;
        }
        return GetPrefabInstanceParent(go.transform.parent.gameObject);
    }
    #endregion

    //工具用户登录
    private bool Login(string username, string password)
    {
        string url = "http://54.193.6.32:8080/U3dFileToServer/StringContentServlet.do?";
        string parameter = "Username=" + username + "&" + "Password=" + password;
        string path = url + parameter;
        WWW www = new WWW(path);
        while(!www.isDone)
        {
        }

        if (www.text.Equals("true"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Login for UGUI
    /*
    public void LoginButtonClick()
    {
        string parameter = "";
        parameter += "Username=" + Username.text + "&";
        parameter += "Password=" + Password.text;
        //开始传递
        StartCoroutine(login(Url + parameter));
    }

    IEnumerator login(string path)
    {
        WWW www = new WWW(path);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.text.Equals("true"))
            {
                print("Login Success!");
                Application.LoadLevel("UpLoadFile");
            }
            else
            {
                print("Login Fail..");
            }
        }
    }
    */
    #endregion Login for UGUI

    void showMessage(string content)
    {
        //var notification : String = content;
        //EditorGUILayout.TextField(content);
        this.ShowNotification(new GUIContent(content));
    }
}
