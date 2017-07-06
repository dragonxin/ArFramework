package com.jimmy.dao.proxy;

import com.jimmy.dao.IUserDAO;
import com.jimmy.dao.UserDAOImpI;
import com.jimmy.dbc.DatabaseConnection;
import com.jimmy.vo.User;

public class UserDAOProxy implements IUserDAO{

	private DatabaseConnection dbc = null;
	private IUserDAO dao = null;
	
	public UserDAOProxy()
	{
		try
		{
			dbc = new DatabaseConnection();
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
		dao = new UserDAOImpI(dbc.getConnection());
	}
	
	public boolean findLogin(User user) throws Exception {
		boolean flag = false;
		
		try
		{
			flag = dao.findLogin(user);
		}
		catch(Exception e)
		{
			throw e;
		}
		finally
		{
			dbc.close();
		}
		return flag;
	}

}
