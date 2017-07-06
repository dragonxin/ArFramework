package com.jimmy.servlets;

import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.jimmy.tools.Tool;

public class DownloadMidi extends HttpServlet {

	public void doGet(HttpServletRequest request, HttpServletResponse response)throws ServletException, IOException {
		
		//������ʲ�����������
		if(request.getParameter("Download").equals("Midi"))
		{
			//��ȡ�����
			OutputStream out=response.getOutputStream();
			//���ļ����byte�ֽ������������
			out.write(Tool.getBytes(this.getServletContext().getRealPath("/")+"/upLoad/midi.mid"));
			//ˢ����
			out.flush();
			//�ر���
			out.close();
			//�����̨��ʾ�ɹ�
			System.out.println("Download success!");
		}
	}
}
