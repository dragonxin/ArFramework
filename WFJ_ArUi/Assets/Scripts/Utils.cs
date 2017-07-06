using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System;
using ICSharpCode.SharpZipLib.Checksums;

public static class Utils
{

    public static void DecompressToDirectory(string targetPath, string zipFilePath)//targetPath是我们解压到哪里，zipFilePath是我们的zip压缩文件目录(包括文件名和后缀)
    {
        if (File.Exists(zipFilePath))
        {
            var compressed = File.OpenRead(zipFilePath);
            compressed.DecompressToDirectory(targetPath);
        }
        else
        {
            Debug.LogError("Zip不存在: " + zipFilePath);
        }
    }

    public static void DecompressToDirectory(this Stream source, string targetPath)//自己写stream的扩展方法
    {
        targetPath = Path.GetFullPath(targetPath);
        using (ZipInputStream decompressor = new ZipInputStream(source))
        {
            ZipEntry entry;

            while ((entry = decompressor.GetNextEntry()) != null)
            {
                string name = entry.Name;
                if (entry.IsDirectory && entry.Name.StartsWith("\\"))
                {
                    name = entry.Name.Replace("\\", "");
                    //name = entry.Name.ReplaceFirst("\\", "");
                }


                string filePath = Path.Combine(targetPath, name);
                string directoryPath = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                if (entry.IsDirectory)
                    continue;

                byte[] data = new byte[2048];
                using (FileStream streamWriter = File.Create(filePath))
                {
                    int bytesRead;
                    while ((bytesRead = decompressor.Read(data, 0, data.Length)) > 0)
                    {
                        streamWriter.Write(data, 0, bytesRead);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 压缩文件
    /// </summary>
    /// <param name="filePath">zip文件路径</param>
    /// <param name="zipPath">压缩到哪个文件路径</param>
    public static void ZipFile(string filePath, string zipPath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("需要压缩的文件不存在");
        }

        string zipFileName = zipPath + Path.GetFileNameWithoutExtension(filePath) + ".zip";
        Debug.Log(zipFileName);
        using (FileStream fs = File.Create(zipFileName))
        {
            using (ZipOutputStream zipStream = new ZipOutputStream(fs))
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    string fileName = Path.GetFileName(filePath);
                    ZipEntry zipEntry = new ZipEntry(fileName);
                    zipStream.PutNextEntry(zipEntry);
                    byte[] buffer = new byte[1024];
                    int sizeRead = 0;
                    try
                    {
                        do
                        {
                            sizeRead = stream.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sizeRead);
                        } while (sizeRead > 0);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                    stream.Close();
                }
                zipStream.Finish();
                zipStream.Close();
            }
            fs.Close();
        }
    }

    /// <summary>
    /// 压缩多层目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="zipedFile">The ziped file.</param>
    public static void ZipFileDirectory(string strDirectory, string zipedFile)
    {
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                ZipSetp(strDirectory, s, "");
            }
        }
    }

    /// <summary>
    /// 递归遍历目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="s">The ZipOutputStream Object.</param>
    /// <param name="parentPath">The parent path.</param>
    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
    {
        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        {
            strDirectory += Path.DirectorySeparatorChar;
        }
        Crc32 crc = new Crc32();

        string[] filenames = Directory.GetFileSystemEntries(strDirectory);

        foreach (string file in filenames)// 遍历所有的文件和目录
        {

            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string pPath = parentPath;
                pPath += file.Substring(file.LastIndexOf("\\") + 1);
                pPath += "\\";
                ZipSetp(file, s, pPath);
            }

            else // 否则直接压缩文件
            {
                //打开压缩文件
                using (FileStream fs = File.OpenRead(file))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
