using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileUpload : MonoBehaviour
{
    //private string Url = "http://54.193.6.32:8080/U3dFileToServer/ByteFileContentServlet.do?";    //web
    private string Url = "http://192.168.1.199:8080/U3dFileToServer/ByteFileContentServlet.do?";  //local
    //点击上传按钮
    public void OnUpFileButtonClick()
    {
        //上传本地文件
        StartCoroutine(UpFileToJSP(Url, Application.dataPath + "/StreamingAssets.zip"));
    }

    //访问JSP服务器
    private IEnumerator UpFileToJSP(string url, string filePath)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", FileContent(filePath), "StreamingAssets.zip");

        WWW upLoad = new WWW(url, form);
        yield return upLoad;
        //如果失败
        if (!string.IsNullOrEmpty(upLoad.error) || upLoad.text.Equals("false"))
        {
            //在控制台输出错误信息
            print(upLoad.error);
        }
        else
        {
            //如果成功
            print("Finished Uploading Screenshot");
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
}
