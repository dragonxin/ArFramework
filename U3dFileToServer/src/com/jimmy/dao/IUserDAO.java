package com.jimmy.dao;
import com.jimmy.vo.User;

public interface IUserDAO 
{
	public boolean findLogin(User user) throws Exception;
}
