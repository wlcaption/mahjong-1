using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Bacon.DataSet;

[XLua.LuaCallCSharp]
public class ABLoader : MonoBehaviour {

    enum PathType {
        RES,
        STR,
        PER,
    }

    public static ABLoader current = null;

    private AssetBundleManifest _manifest = null;
    private Dictionary<string, AssetBundle> _dic = new Dictionary<string, AssetBundle>();
    private Dictionary<string, UnityEngine.Object> _res = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, PathType> _path = new Dictionary<string, PathType>();
    private string _abversion = ".normal";
    private int _version;
    private int _resversion;
    private int _step;
    private WWW _request;

    void Awake() {
        if (current == null) {
            current = this;
        }
    }

    void Start() {
    }

    void Update() {
        //_request.progress;
    }

    public void FetchVersion(Action cb) {
        _step = 0;
        LoadPath();
        StartCoroutine(FetchVersionFile(cb));
    }

    public void LoadPath() {
        TextAsset asset = LoadRes<TextAsset>("version");
        JSONObject json = new JSONObject(asset.text);
        JSONObject abs = json.GetField("abs");
        for (int i = 0; i < abs.keys.Count; i++) {
            _path[abs.keys[i]] = PathType.STR;
        }
    }

    IEnumerator FetchVersionFile(Action cb) {
        JSONObject sjson = null;
        string url = SayDataSet.Instance.GetDataItem(1).value;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        url = "http://127.0.0.1:80/mahjong/Win64";
#elif UNITY_IOS
        url = "http://127.0.0.1:80/mahjong/iOS";
#elif UNITY_ANDROID
        url = "http://127.0.0.1:80/mahjong/Android";
#endif
        WWW srequest = new WWW(url + "/version.json");
        _request = srequest;
        yield return srequest;
        if (srequest.text != null || srequest.text.Length > 0) {
            sjson = new JSONObject(srequest.text);
        } else {
            cb();
            yield break;
        }

        JSONObject ljson = null;
        WWW lrequest = new WWW(Application.persistentDataPath + "/version.json");
        yield return lrequest;
        if (lrequest.text != null && lrequest.text != string.Empty) {
            ljson = new JSONObject(lrequest.text);

            // 比较服务器与当地，不同就下载更新
            long lversion = ljson.GetField("version").i;
            long sversion = sjson.GetField("version").i;

            if (sversion > lversion) {
                // 下载资源
                JSONObject sabs = sjson.GetField("abs");
                JSONObject labs = ljson.GetField("abs");
                for (int i = 0; i < sabs.keys.Count; i++) {
                    JSONObject shash = sabs.GetField(sabs.keys[i]);
                    JSONObject lhash = labs.GetField(labs.keys[i]);
                    if (lhash == null || (shash.i != lhash.i)) {
                        // 没有下载存储

                        WWW frequest = new WWW(url + "/" + sabs.keys[i]);
                        yield return frequest;
                        string[] sp = sabs.keys[i].Split(new char[] { '/' });
                        if (sp.Length > 1) {
                            CreateDirOrFile(Application.persistentDataPath, sp, 0, frequest.bytes);
                        }

                        _path[sabs.keys[i]] = PathType.PER;

                        // manifest
                        WWW mrequest = new WWW(url + "/" + sabs.keys[i] + ".manifest");
                        yield return mrequest;
                        string[] msp = sabs.keys[i].Split(new char[] { '/' });
                        if (msp.Length > 1) {
                            CreateDirOrFile(Application.persistentDataPath, msp, 0, mrequest.bytes);
                        }
                        _path[sabs.keys[i] + ".manifest"] = PathType.PER;

                    } else {
                        _path[sabs.keys[i]] = PathType.STR;
                    }
                }
            }
        } else {
            // 本地不存在直接更新写入
            ResourceRequest lrrequest = Resources.LoadAsync<TextAsset>("version");
            yield return lrrequest;
            ljson = new JSONObject(((TextAsset)lrrequest.asset).text);

            long sversion = sjson.GetField("version").i;
            long lversion = ljson.GetField("version").i;
            
            if (sversion > lversion) {
                // 创建version
                CreateDirOrFile(Application.persistentDataPath, new string[] { "version.json" }, 0, ASCIIEncoding.ASCII.GetBytes(srequest.text));
                JSONObject sabs = sjson.GetField("abs");
                for (int i = 0; i < sabs.keys.Count; i++) {
                    WWW frequest = new WWW(url + "/" + sabs.keys[i]);
                    yield return frequest;
                    string[] sp = sabs.keys[i].Split(new char[] { '/' });
                    if (sp.Length > 1) {
                        CreateDirOrFile(Application.persistentDataPath, sp, 0, frequest.bytes);
                    }
                    _path[sabs.keys[i]] = PathType.PER;

                    // manifest
                    WWW mrequest = new WWW(url + "/" + sabs.keys[i] + ".manifest");
                    yield return mrequest;
                    string[] msp = sabs.keys[i].Split(new char[] { '/' });
                    if (msp.Length > 1) {
                        CreateDirOrFile(Application.persistentDataPath, msp, 0, mrequest.bytes);
                    }
                    _path[sabs.keys[i] + ".manifest"] = PathType.PER;
                }
            }
        }
        cb();
    }

    private void CreateDirOrFile(string parent, string[] sp, int i, byte[] bytes) {
        Debug.Assert(parent.Length > 0);
        Debug.Assert(sp != null && sp.Length > 0);
        if (i <= (sp.Length - 2)) {
            string name = sp[i];
            string path = parent + "/" + name;

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            i++;
            CreateDirOrFile(path, sp, i, bytes);
        } else {
            string name = sp[i];
            string path = parent + "/" + name;
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(bytes);
            sw.Close();
            fs.Close();
        }
    }

    public T LoadAsset<T>(string path, string name) where T : UnityEngine.Object {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string res_path = "WIN64/" + path.ToLower();
#elif UNITY_IOS
        string res_path = "iOS/" + path.ToLower();
#elif UNITY_ANDROID
        string res_path = "Android/" + path.ToLower();
#endif
        T res = LoadAB<T>(res_path, name);
        if (res == null) {
            string xpath = path + "/" + name;
            res = LoadRes<T>(xpath);
        }
        return res;
    }

    public TextAsset LoadTextAsset(string path, string name) {
        return LoadAsset<TextAsset>(path, name);
    }

    public void LoadAssetAsync<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string res_path = "WIN64/" + path.ToLower();
#elif UNITY_IOS
        string res_path = "iOS/" + path.ToLower();
#elif UNITY_ANDROID
        string res_path = "Android/" + path.ToLower();
#endif
        LoadABAsync<T>(res_path, name, (T asset) => {
            if (asset == null) {
                LoadResAsync<T>(Path.Combine(path, name), cb);
            } else {
                cb(asset);
            }
        });
    }

    private T LoadAB<T>(string xpath, string name) where T : UnityEngine.Object {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string per_prefix = Application.persistentDataPath + "/Win64/";
        string str_prefix = Application.streamingAssetsPath + "/Win64/";
#elif UNITY_IOS
        string per_prefix = Application.persistentDataPath + "/iOS/";
        string str_prefix = Application.streamingAssetsPath + "/iOS/";
#elif UNITY_ANDROID
        string per_prefix = Application.persistentDataPath + "/Android/";
        string str_prefix = Application.streamingAssetsPath + "/Android/";
#endif

        string path = xpath + _abversion;
        if (_dic.ContainsKey(path)) {
            AssetBundle ab = _dic[path];
            return ab.LoadAsset<T>(name);
        } else {
            if (_manifest == null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                string manifest = "Win64";
#elif UNITY_IOS 
                string manifest = "iOS";
#elif UNITY_ANDROID
                string manifest = "Android";
#endif
                if (_path.ContainsKey(manifest) && _path[manifest] == PathType.PER) {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(per_prefix + manifest);
                    _manifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                } else {
                    AssetBundle manifestab = AssetBundle.LoadFromFile(str_prefix + manifest);
                    _manifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }
            if (_manifest != null) {
                string[] depends = _manifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    LoadAB<UnityEngine.Object>(path, depends[i]);
                }
            }
            if (_path.ContainsKey(path)) {
                if (_path[path] == PathType.PER) {
                    AssetBundle ab = AssetBundle.LoadFromFile(per_prefix + path);
                    if (ab.Contains(name)) {
                        return ab.LoadAsset<T>(name);
                    } else {
                        Debug.LogError("no exits");
                        return null;
                    }
                } else {
                    AssetBundle ab = AssetBundle.LoadFromFile(str_prefix + path);
                    if (ab != null && ab.Contains(name)) {
                        return ab.LoadAsset<T>(name);
                    } else {
                        Debug.LogError("no exits");
                        return null;
                    }
                }
            } else {
                return null;
            }
        }
        return null;
    }

    private void LoadABAsync<T>(string xpath, string name, Action<T> cb) where T : UnityEngine.Object {
        string path = xpath + _abversion;
        if (_dic.ContainsKey(path)) {
            AssetBundle ab = _dic[path];
            if (ab.Contains(name)) {
                T asset = ab.LoadAsset<T>(name);
                cb(asset);
            }
        } else {
            StartCoroutine(LoadABAsyncImp(path, name, cb));
        }
    }

    IEnumerator LoadABAsyncImp<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string per_prefix = Application.persistentDataPath + "/Win64/";
        string str_prefix = Application.streamingAssetsPath + "/Win64/";
#elif UNITY_IOS
        string per_prefix = Application.persistentDataPath + "/iOS/";
        string str_prefix = Application.streamingAssetsPath + "/iOS/";
#elif UNITY_ANDROID
        string per_prefix = Application.persistentDataPath + "/Android/";
        string str_prefix = Application.streamingAssetsPath + "/Android/";
#endif

        if (_manifest == null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            string manifest = "Win64";
#elif UNITY_IOS
                string manifest = "iOS";
#elif UNITY_ANDROID
                string manifest = "Android";
#endif
            if (_path.ContainsKey(manifest) && _path[manifest] == PathType.PER) {
                AssetBundle manifestab = AssetBundle.LoadFromFile(per_prefix + manifest);
                _manifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            } else {
                AssetBundle manifestab = AssetBundle.LoadFromFile(str_prefix + manifest);
                _manifest = manifestab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
        }
        if (_manifest != null) {
            string[] depends = _manifest.GetAllDependencies(path);
            for (int i = 0; i < depends.Length; i++) {
                if (_path.ContainsKey(depends[i]) && _path[depends[i]] == PathType.PER) {
                    AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(per_prefix + depends[i]);
                    yield return depend_request;
                    _dic[depends[i]] = depend_request.assetBundle;
                } else {
                    AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(str_prefix + depends[i]);
                    yield return depend_request;
                    _dic[depends[i]] = depend_request.assetBundle;
                }
            }
            if (_path.ContainsKey(path)) {
                if (_path[path] == PathType.PER) {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/" + path);
                    yield return request;
                    _dic[path] = request.assetBundle;
                    AssetBundle ab = request.assetBundle;
                    T asset = ab.LoadAsset<T>(name);
                    cb(asset);
                } else {
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + path);
                    yield return request;
                    _dic[path] = request.assetBundle;
                    AssetBundle ab = request.assetBundle;
                    T asset = ab.LoadAsset<T>(name);
                    cb(asset);
                }
            } else {
                cb(null);
            }
        }
    }

    private void LoadResAsync<T>(string path, Action<T> cb) where T : UnityEngine.Object {
        if (_res.ContainsKey(path)) {
            UnityEngine.Object o = _res[path];
            if (o != null) {
                cb(o as T);
            } else {
                UnityEngine.Debug.LogErrorFormat("path : {0} has been loaded res is null.");
            }
        } else {
            StartCoroutine(LoadResAsyncImp<T>(path, cb));
        }
    }

    IEnumerator LoadResAsyncImp<T>(string path, Action<T> cb) where T : UnityEngine.Object {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        T asset = request.asset as T;
        if (asset == null) {
            Debug.LogErrorFormat("load res occor wrong, path is {0}", path);
            cb(asset);
        } else {
            _res[path] = asset;
            cb(asset);
        }
    }

    private T LoadRes<T>(string path) where T : UnityEngine.Object {
        if (_res.ContainsKey(path)) {
            return _res[path] as T;
        } else {
            Debug.LogFormat("load res path: {0}", path);
            T res = Resources.Load(path) as T;
            if (res != null) {
                _res[path] = res;
                return res;
            }
            return null;
        }
    }

    public void Unload() {
        foreach (var item in _dic) {
            item.Value.Unload(true);
        }
    }
}

