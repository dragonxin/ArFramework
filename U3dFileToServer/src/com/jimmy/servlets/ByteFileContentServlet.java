package com.jimmy.servlets;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.RandomAccessFile;
import java.io.UnsupportedEncodingException;
import java.io.Writer;
import java.util.Random;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.jimmy.tools.Tool;

public class ByteFileContentServlet extends HttpServlet
{
	private String fileFolder;
	
	public void doPost(HttpServletRequest request, HttpServletResponse response)	throws ServletException, IOException {
		
		// 向控制台输出文件的内容长度
		System.out.println(request.getContentLength());
		// 如果有内容
		if (request.getContentLength() > 297) {

			// ======s============开始处理文件===================

			InputStream in = request.getInputStream();
			File f = new File(fileFolder + "/StreamingAssets.rar");
			
			File folder = new File(fileFolder);
			if (!folder.exists())
			{
				folder.mkdirs();
			}
			
			FileOutputStream fout = new FileOutputStream(f);
			byte[] b = new byte[1024];
			int n = 0;
			while( (n=in.read(b)) != -1)
			{
				fout.write(b, 0, n);
			}
			fout.close();
			in.close();
			
		// ==================解压文件===================

		try {
			Tool.unZipFiles(fileFolder + "/StreamingAssets.rar", fileFolder + "/StreamingAssets/");

		} 
		catch (Exception e) {
			e.printStackTrace();
		}


			// ==================处理文件结束===================

			// 向控制台输出文件上传成功
			System.out.println("File upload success!" + " Save Path: ");
		} 
		else 
		{
			// 否则显示失败，
			System.out.println("No file!");

			// 向Unity返回一个False字符串
			Writer out = response.getWriter();
			out.write("false");
			out.close();
		}
	}
	
	public void init() throws ServletException {
		//获取项目所在目录
		String contentPath = getServletContext().getRealPath("/");
		this.fileFolder = contentPath+"/upLoad/";
	}
}
