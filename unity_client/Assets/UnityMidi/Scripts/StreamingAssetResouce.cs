using UnityEngine;
using System.IO;
using AudioSynthesis;

namespace UnityMidi
{
    [System.Serializable]
    public class StreamingAssetResouce : IResource
    {
        [SerializeField] string streamingAssetPath;

        public bool ReadAllowed()
        {
            return true;
        }

        public bool WriteAllowed()
        {
            return false;
        }

        public bool DeleteAllowed()
        {
            return false;
        }

        public string GetName()
        {
            return Path.GetFileName(streamingAssetPath);
        }

        public void InitializeFileCopy()
        {
#if UNITY_EDITOR
#elif UNITY_ANDROID
            string path = Path.Combine(Application.streamingAssetsPath, streamingAssetPath);
            var www = new WWW(path);
            while(!www.isDone)
            {
                // NOP.
            }
            string copyToPath = Path.Combine(Application.persistentDataPath, streamingAssetPath);

            string copyToDir = Path.GetDirectoryName(copyToPath);
            if (!Directory.Exists(copyToDir))
            {
                Directory.CreateDirectory(copyToDir);
            }

            File.WriteAllBytes(copyToPath, www.bytes);
#endif
        }

        public string AssetPath()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.streamingAssetsPath, streamingAssetPath);
#elif UNITY_ANDROID
            return Path.Combine(Application.persistentDataPath, streamingAssetPath);
#else
            return Path.Combine(Application.streamingAssetsPath, streamingAssetPath);
#endif
        }


        public Stream OpenResourceForRead()
        {
            return File.OpenRead(AssetPath());
        }

        public Stream OpenResourceForWrite()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteResource()
        {
            throw new System.NotImplementedException();
        }
    }
}
