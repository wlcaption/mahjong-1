using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

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
    private int _version;
    private int _resversion;
    private int _step;
    private WWW _request;

    void Awake() {
        if (current == null) {
            current = this;
        }
    }

    void Start() { }

    void Update() {
        //_request.progress;
    }

    public void FetchVersion(Action cb) {
        _step = 0;
        StartCoroutine(FetchVersionFile(cb));
    }

    IEnumerator FetchVersionFile(Action cb) {
        JSONObject sjson = null;
        string url = DataConfig.Instance.GetItem(1).value;
        WWW srequest = new WWW(url + "/version.json");
        _request = srequest;
        yield return srequest;
        if (srequest.text != null) {
            sjson = new JSONObject(srequest.text);
        }

        JSONObject ljson = null;
        WWW lrequest = new WWW(Application.persistentDataPath + "/version.json");
        yield return lrequest;
        if (lrequest.text != null && lrequest.text != string.Empty) {
            ljson = new JSONObject(lrequest.text);
        } else {
            ResourceRequest lrrequest = Resources.LoadAsync<TextAsset>("version");
            yield return lrrequest;
            ljson = new JSONObject(((TextAsset)lrrequest.asset).text);
        }

        // 创建version
        CreateDirOrFile(Application.persistentDataPath, new string[] { "version.json" }, 0, ASCIIEncoding.ASCII.GetBytes(srequest.text));

        int lversion = (int)ljson.GetField("version").i;
        int sversion = (int)sjson.GetField("version").i;
        _resversion = sversion;

        if (sversion > lversion) {
            // 下载资源
            JSONObject sabs = sjson.GetField("abs");
            JSONObject labs = ljson.GetField("abs");
            for (int i = 0; i < sabs.keys.Count; i++) {
                JSONObject shash = sabs.GetField(sabs.keys[i]);
                JSONObject lhash = labs.GetField(sabs.keys[i]);
                if (lhash == null || (shash.i != lhash.i)) {
                    // 没有下载存储

                    WWW frequest = new WWW(url + "/" + sabs.keys[i]);
                    yield return frequest;
                    string[] sp = sabs.keys[i].Split(new char[] { '/' });
                    if (sp.Length > 1) {
                        CreateDirOrFile(Application.persistentDataPath, sp, 0, frequest.bytes);
                    }

                    _path[sabs.keys[i]] = PathType.PER;
                } else {
                    _path[sabs.keys[i]] = PathType.STR;
                }
            }
        }
        cb();
    }

    private void CreateDirOrFile(string parent, string[] sp, int i, byte[] bytes) {
        if (i <= (sp.Length - 2)) {
            string name = sp[i];
            string path = parent + "/" + name;

            if (Directory.Exists(path)) {
            } else {
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

    public T LoadAB<T>(string path, string name) where T : UnityEngine.Object {
        if (_dic.ContainsKey(path)) {
            AssetBundle ab = _dic[path];
            return ab.LoadAsset<T>(name);
        } else {
            if (_manifest == null) {
                if (_path.ContainsKey(ABConfig.ABMANIFEST) && _path[ABConfig.ABMANIFEST] == PathType.PER) {
                    AssetBundle manifest = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + ABConfig.ABMANIFEST);
                    _manifest = manifest.LoadAsset<AssetBundleManifest>(ABConfig.ABMANIFEST);
                } else {
                    AssetBundle manifest = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + ABConfig.ABMANIFEST);
                    _manifest = manifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }
            if (_manifest != null) {
                string[] depends = _manifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    LoadAB<UnityEngine.Object>(path, depends[i]);
                }
            }
            if (_path.ContainsKey(path) && _path[path] == PathType.PER) {
                AssetBundle ab = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + path);
                if (ab.Contains(name)) {
                    return ab.LoadAsset<T>(name);
                } else {
                    Debug.LogError("no exits");
                    return null;
                }
            } else {
                AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + path);
                if (ab != null && ab.Contains(name)) {
                    return ab.LoadAsset<T>(name);
                } else {
                    Debug.LogError("no exits");
                    return null;
                }
            }
        }
        return null;
    }

    public void LoadABAsync<T>(string path, string name, Action<T> cb) where T : UnityEngine.Object {
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
        if (_manifest == null) {
            if (_path.ContainsKey(ABConfig.ABMANIFEST) && _path[ABConfig.ABMANIFEST] == PathType.PER) {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + ABConfig.ABMANIFEST);
                yield return request;
                _manifest = request.assetBundle.LoadAsset<AssetBundleManifest>(ABConfig.ABMANIFEST);
            } else {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + ABConfig.ABMANIFEST);
                yield return request;
                _manifest = request.assetBundle.LoadAsset<AssetBundleManifest>(ABConfig.ABMANIFEST);
            }
        }
        if (_manifest != null) {
            string[] depends = _manifest.GetAllDependencies(path);
            for (int i = 0; i < depends.Length; i++) {
                if (_path.ContainsKey(depends[i]) && _path[depends[i]] == PathType.PER) {
                    AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/" + depends[i]);
                    yield return depend_request;
                } else {
                    AssetBundleCreateRequest depend_request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + depends[i]);
                    yield return depend_request;
                }
            }
            if (_path.ContainsKey(path) && _path[path] == PathType.PER) {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.persistentDataPath + "/" + path);
                yield return request;
                AssetBundle ab = request.assetBundle;
                T asset = ab.LoadAsset<T>(name);
                cb(asset);
            } else {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + path);
                yield return request;
                AssetBundle ab = request.assetBundle;
                T asset = ab.LoadAsset<T>(name);
                cb(asset);
            }
        }
    }

    public void LoadResAsync<T>(string path, Action<T> cb) where T : UnityEngine.Object {
        if (_res.ContainsKey(path)) {
            cb(_res.ContainsKey(path) as T);
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

    public T LoadRes<T>(string path) where T : UnityEngine.Object {
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

