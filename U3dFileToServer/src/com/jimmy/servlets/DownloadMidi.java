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
		
		//如果访问参数符合条件
		if(request.getParameter("Download").equals("Midi"))
		{
			//获取输出流
			OutputStream out=response.getOutputStream();
			//把文件变成byte字节流传入输出流
			out.write(Tool.getBytes(this.getServletContext().getRealPath("/")+"/upLoad/midi.mid"));
			//刷新流
			out.flush();
			//关闭流
			out.close();
			//向控制台提示成功
			System.out.println("Download success!");
		}
	}
}
