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
		
		// �����̨����ļ������ݳ���
		System.out.println(request.getContentLength());
		// ���������
		if (request.getContentLength() > 297) {

			// ======s============��ʼ�����ļ�===================

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
			
		// ==================��ѹ�ļ�===================

		try {
			Tool.unZipFiles(fileFolder + "/StreamingAssets.rar", fileFolder + "/StreamingAssets/");

		} 
		catch (Exception e) {
			e.printStackTrace();
		}


			// ==================�����ļ�����===================

			// �����̨����ļ��ϴ��ɹ�
			System.out.println("File upload success!" + " Save Path: ");
		} 
		else 
		{
			// ������ʾʧ�ܣ�
			System.out.println("No file!");

			// ��Unity����һ��False�ַ���
			Writer out = response.getWriter();
			out.write("false");
			out.close();
		}
	}
	
	public void init() throws ServletException {
		//��ȡ��Ŀ����Ŀ¼
		String contentPath = getServletContext().getRealPath("/");
		this.fileFolder = contentPath+"/upLoad/";
	}
}
