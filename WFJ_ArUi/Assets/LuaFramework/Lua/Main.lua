--主入口函数。从这里开始lua逻辑
function Main()					
	LuaHelper = LuaFramework.LuaHelper;
	resMgr = LuaHelper.GetResManager();
	resMgr:LoadPrefab('UI',{'UI_LoadingPanel','UI_MainPanel','UI_ContentPanel'},OnLoadFinish)	 		
end

--加载之后的回调
function OnLoadFinish(objs)
	local sceneName = LuaFramework.Util.GetActiveScene();

	if(sceneName == "loading") then
		go = UnityEngine.GameObject.Instantiate(objs[0]);
	elseif(sceneName == "main") then
		go = UnityEngine.GameObject.Instantiate(objs[1]);
	elseif(sceneName == "content") then
		go = UnityEngine.GameObject.Instantiate(objs[2]);
	end

	local parent = UnityEngine.GameObject.Find("Canvas");
	go.transform:SetParent(parent.transform);
	go.transform.localScale = Vector3.one;
	go.transform.localPosition = Vector3.zero;
end


--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end