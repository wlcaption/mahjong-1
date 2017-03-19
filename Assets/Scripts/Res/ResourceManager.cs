using Maria;

public class ResourceManager : Singleton<ResourceManager> {

    public T LoadAsset<T>(string path, string name) where T : UnityEngine.Object {
        string abpath = UnityEngine.Application.dataPath + "/StreamingAssets/" + path;
        string respath = UnityEngine.Application.dataPath + "/Resources/" + path;
        if (System.IO.File.Exists(abpath)) {
            string abpathx = "StreamingAssets/" + path;
            int idx = abpathx.IndexOf('.');
            if (idx != -1) {
                abpathx = abpathx.Remove(idx);
            }
            return ABLoader.current.LoadAB<T>(abpathx, name);
        } else if (System.IO.File.Exists(respath)) {
            int idx = path.IndexOf('.');
            if (idx != -1) {
                path = path.Remove(idx);
            }
            return ABLoader.current.LoadRes<T>(path);
        }
        return null;
    }

    public void LoadAssetAsync<T>(string path, string name, ABLoader.Completed<T> cb) where T : UnityEngine.Object {
        string abpath = UnityEngine.Application.dataPath + "/StreamingAssets/" + path;
        string respath = UnityEngine.Application.dataPath + "/Resources/" + path;
        if (System.IO.File.Exists(abpath)) {
            string abpathx = "StreamingAssets/" + path;
            int idx = abpathx.IndexOf('.');
            if (idx != -1) {
                abpathx = abpathx.Remove(idx);
            }
            ABLoader.current.LoadABAsync<T>(abpathx, name, cb);
        } else if (System.IO.File.Exists(respath)) {
            int idx = path.IndexOf('.');
            if (idx != -1) {
                path = path.Remove(idx);
            }
            ABLoader.current.LoadResAsync<T>(path, cb);
        }
    }

}

