package com.jimmy.vo;

public class User {
	
	public int getUserid() {
		return userid;
	}
	public void setUserid(int userid) {
		this.userid = userid;
	}
	public String getUsername() {
		return Username;
	}
	public void setUsername(String name) {
		this.Username = name;
	}
	public String getPassword() {
		return Password;
	}
	public void setPassword(String password) {
		this.Password = password;
	}
	private int userid;
	private String Username;
	private String Password;
}
