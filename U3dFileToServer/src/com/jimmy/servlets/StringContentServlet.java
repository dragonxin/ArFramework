package com.jimmy.servlets;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.Writer;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.jimmy.dao.proxy.UserDAOProxy;
import com.jimmy.vo.User;

public class StringContentServlet extends HttpServlet {

	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		String username = request.getParameter("Username");
		String password = request.getParameter("Password");
		System.out.println(username + "+:+" + password);
		
		//连接数据库 查询
		UserDAOProxy userDAOProxy = new UserDAOProxy();
		User user = new User();
		user.setUsername(username);
		user.setPassword(password);

		boolean result =false;
		
		try
		{
			result = userDAOProxy.findLogin(user);
		}
		catch (Exception e) {
			e.printStackTrace();
		}
		
		Writer out = response.getWriter();
		if(result)
		{
			out.write("true"); 
		}
		else
		{
			out.write("false"); 
		}
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

	}
}
