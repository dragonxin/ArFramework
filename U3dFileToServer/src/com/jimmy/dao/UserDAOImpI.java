package com.jimmy.dao;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import com.jimmy.vo.User;

public class UserDAOImpI implements IUserDAO {

	private Connection conn = null;
	private PreparedStatement pstmt = null;
	
	public UserDAOImpI(Connection conn)
	{
		this.conn = conn;
	}
	
	public boolean findLogin(User user) throws Exception {
		
		boolean flag = false;
		
		try
		{
			String sql = "select Username from user where Username=? and Password=?";
			pstmt = conn.prepareStatement(sql);
			pstmt.setString(1, user.getUsername());
			pstmt.setString(2, user.getPassword());
			
			ResultSet rSet = pstmt.executeQuery();
			
			if(rSet.next())
			{
				user.setUsername(rSet.getString(1));
				flag = true;
			}
		}
		catch(Exception e)
		{
			throw e;
		}
		finally
		{
			if(pstmt != null)
			{
				try
				{
					pstmt.close();
				}
				catch(Exception e)
				{
					throw e;
				}
			}
		}
		
		return flag;
	}
}
