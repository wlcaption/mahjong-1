﻿#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class BaconServiceInitServiceWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Bacon.Service.InitService), L, translator, 0, 4, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHandshake", _m_SendHandshake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRspHandshake", _m_OnRspHandshake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnReqRadio", _m_OnReqRadio);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SMActor", _g_get_SMActor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataTime", _g_get_DataTime);
            
			
			Utils.EndObjectRegister(typeof(Bacon.Service.InitService), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Bacon.Service.InitService), L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Name", Bacon.Service.InitService.Name);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Bacon.Service.InitService));
			
			
			Utils.EndClassRegister(typeof(Bacon.Service.InitService), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Maria.Context>(L, 2))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					
					Bacon.Service.InitService __cl_gen_ret = new Bacon.Service.InitService(ctx);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Bacon.Service.InitService constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float delta = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.Update( delta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHandshake(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    float delta = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.SendHandshake( delta );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRspHandshake(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.OnRspHandshake( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnReqRadio(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Sproto.SprotoTypeBase requestObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                        Sproto.SprotoTypeBase __cl_gen_ret = __cl_gen_to_be_invoked.OnReqRadio( requestObj );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SMActor(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SMActor);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataTime(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.Service.InitService __cl_gen_to_be_invoked = (Bacon.Service.InitService)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, __cl_gen_to_be_invoked.DataTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
