﻿#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter((luaenv, translator) => {
			    
				translator.DelayWrapLoader(typeof(Bacon.InitService), BaconInitServiceWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Bacon.MainController), BaconMainControllerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Maria.Actor), MariaActorWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Maria.Application), MariaApplicationWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Maria.Command), MariaCommandWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Maria.Context), MariaContextWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Maria.EventCmd), MariaEventCmdWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.BaseClass), TutorialBaseClassWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.TestEnum), TutorialTestEnumWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.DrivenClass), TutorialDrivenClassWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.DrivenClass.TestEnumInner), TutorialDrivenClassTestEnumInnerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.Calc), TutorialCalcWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tutorial.DrivenClassExtensions), TutorialDrivenClassExtensionsWrap.__Register);
				
				
				translator.AddInterfaceBridgeCreator(typeof(CSCallLua.ItfD), CSCallLuaItfDBridge.__Create);
				
			});
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	public static partial class Utils
    {
	    
	    static Utils()
		{
		    extension_method_map = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
		}
	}
}
