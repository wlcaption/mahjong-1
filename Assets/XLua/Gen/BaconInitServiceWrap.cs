#if USE_UNI_LUA
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
    public class BaconInitServiceWrap
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			Utils.BeginObjectRegister(typeof(Bacon.InitService), L, translator, 0, 4, 8, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHandshake", _m_SendHandshake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Handshake", _m_Handshake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRadio", _m_OnRadio);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SMActor", _g_get_SMActor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Ping", _g_get_Ping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataTime", _g_get_DataTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "User", _g_get_User);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Board", _g_get_Board);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Adver", _g_get_Adver);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SysInBox", _g_get_SysInBox);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RecordMgr", _g_get_RecordMgr);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Board", _s_set_Board);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Adver", _s_set_Adver);
            
			Utils.EndObjectRegister(typeof(Bacon.InitService), L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(typeof(Bacon.InitService), L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Name", Bacon.InitService.Name);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnderlyingSystemType", typeof(Bacon.InitService));
			
			
			Utils.EndClassRegister(typeof(Bacon.InitService), L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			try {
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<Maria.Context>(L, 2))
				{
					Maria.Context ctx = (Maria.Context)translator.GetObject(L, 2, typeof(Maria.Context));
					
					Bacon.InitService __cl_gen_ret = new Bacon.InitService(ctx);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Bacon.InitService constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
            
            
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
            
            
            Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
            
            
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
        static int _m_Handshake(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Sproto.SprotoTypeBase responseObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                    __cl_gen_to_be_invoked.Handshake( responseObj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRadio(RealStatePtr L)
        {
            
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
            
            
            try {
                
                {
                    Sproto.SprotoTypeBase requestObj = (Sproto.SprotoTypeBase)translator.GetObject(L, 2, typeof(Sproto.SprotoTypeBase));
                    
                        Sproto.SprotoTypeBase __cl_gen_ret = __cl_gen_to_be_invoked.OnRadio( requestObj );
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
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SMActor);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Ping(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.Ping);
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
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, __cl_gen_to_be_invoked.DataTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_User(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.User);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Board(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Board);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Adver(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.Adver);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SysInBox(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.SysInBox);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RecordMgr(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.RecordMgr);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Board(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Board = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Adver(RealStatePtr L)
        {
            ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            try {
			
                Bacon.InitService __cl_gen_to_be_invoked = (Bacon.InitService)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.Adver = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
