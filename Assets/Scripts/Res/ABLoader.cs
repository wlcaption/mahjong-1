using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ABLoader : MonoBehaviour {

    public static ABLoader current = null;

    private AssetBundleManifest _manifest = null;
    private Dictionary<string, AssetBundle> _dic = new Dictionary<string, AssetBundle>();
    private Dictionary<string, UnityEngine.Object> _res = new Dictionary<string, UnityEngine.Object>();

    public delegate void Completed<T>(T res);

    void Awake() {
        if (current == null) {
            current = this;
        }
    }

    public T LoadAB<T>(string path, string name) where T : UnityEngine.Object {
        if (_dic.ContainsKey(path)) {
            AssetBundle ab = _dic[path];
            return ab.LoadAsset<T>(name);
        } else {
            if (_manifest == null) {
                AssetBundle manifest = AssetBundle.LoadFromFile(ABConfig.ABPATH + ABConfig.ABMANIFEST);
                _manifest = manifest.LoadAsset<AssetBundleManifest>(ABConfig.ABMANIFEST);
            }
            if (_manifest != null) {
                string[] depends = _manifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    LoadAB<UnityEngine.Object>(path, depends[i]);
                }
            }
            AssetBundle ab = AssetBundle.LoadFromFile(ABConfig.ABPATH + path);
            if (ab.Contains(name)) {
                return ab.LoadAsset<T>(name);
            } else {
                Debug.LogError("no exits");
                return null;
            }
        }
    }

    public void LoadABAsync<T>(string path, string name, Completed<T> cb) where T : UnityEngine.Object {
        if (_dic.ContainsKey(path)) {
            AssetBundle ab = _dic[path];
            if (ab.Contains(name)) {
                T asset = ab.LoadAsset<T>(name);
                cb(asset);
            }
        } else {
            StartCoroutine(LoadABImp(path, name, cb));
        }
    }

    IEnumerator LoadABImp<T>(string path, string name, Completed<T> cb) where T : UnityEngine.Object {
        if (_dic.ContainsKey(path)) {
            yield return _dic[path];
        } else {
            if (_manifest == null) {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(ABConfig.ABPATH + ABConfig.ABMANIFEST);
                yield return request;
                _manifest = request.assetBundle.LoadAsset<AssetBundleManifest>(ABConfig.ABMANIFEST);
            }
            if (_manifest != null) {
                string[] depends = _manifest.GetAllDependencies(path);
                for (int i = 0; i < depends.Length; i++) {
                    LoadABImp(depends[i], name, cb);
                }
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(ABConfig.ABPATH + "/" + path);
                yield return request;
                AssetBundle ab = request.assetBundle;
                T asset = ab.LoadAsset<T>(name);
                cb(asset);
            }
        }
    }

    public void LoadResAsync<T>(string path, Completed<T> cb) where T : UnityEngine.Object {
        if (_res.ContainsKey(path)) {
            cb(_res.ContainsKey(path) as T);
        } else {
            StartCoroutine(LoadResImp<T>(path, cb));
        }
    }

    IEnumerator LoadResImp<T>(string path, Completed<T> cb) where T : UnityEngine.Object {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;
        T asset = request.asset as T;
        _res[path] = asset;
        cb(asset);
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
}

