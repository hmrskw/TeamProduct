using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

/// <summary>
/// Json形式でセーブできるクラスを提供します。
/// </summary>
/// <remarks>
/// 最初に値を設定、取得するタイミングでファイル読み出します。
/// </remarks>
public class SaveData
{
    /// <summary>
    /// SingletonなSaveDatabaseクラス
    /// </summary>
    private static SaveDataBase savedatabase = null;

    private static SaveDataBase Savedatabase
    {
        get
        {
            if (savedatabase == null)
            {
                string path = Application.persistentDataPath + "/";
                string fileName = Application.companyName + "." + Application.productName + ".savedata.json";
                Debug.Log("path" + path + "\nfileName" + fileName);
                savedatabase = new SaveDataBase(path, fileName);
            }
            return savedatabase;
        }
    }

    private SaveData()
    {
    }

    #region Public Static Methods

    /// <summary>
    /// 指定したキーとT型のクラスコレクションをセーブデータに追加します。
    /// </summary>
    /// <typeparam name="T">ジェネリッククラス</typeparam>
    /// <param name="key">キー</param>
    /// <param name="list">T型のList</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <remarks>指定したキーとT型のクラスコレクションをセーブデータに追加します。</remarks>
    public static void SetList<T>(string key, List<T> list) where T : class, new()
    {
        Savedatabase.SetList<T>(key, list);
    }

    /// <summary>
    ///  指定したキーとT型のクラスコレクションをセーブデータから取得します。
    /// </summary>
    /// <typeparam name="T">ジェネリッククラス</typeparam>
    /// <param name="key">キー</param>
    /// <param name="_default">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static List<T> GetList<T>(string key, List<T> _default) where T : class, new()
    {
        return Savedatabase.GetList<T>(key, _default);
    }

    /// <summary>
    ///  指定したキーとT型のクラスをセーブデータに追加します。
    /// </summary>
    /// <typeparam name="T">ジェネリッククラス</typeparam>
    /// <param name="key">キー</param>
    /// <param name="_default">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static T GetClass<T>(string key, T _default) where T : class, new()
    {
        return Savedatabase.GetClass(key, _default);

    }

    /// <summary>
    ///  指定したキーとT型のクラスコレクションをセーブデータから取得します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void SetClass<T>(string key, T obj) where T : class, new()
    {
        Savedatabase.SetClass<T>(key, obj);
    }

    /// <summary>
    /// 指定されたキーに関連付けられている値を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="value">値</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void SetString(string key, string value)
    {
        Savedatabase.SetString(key, value);
    }

    /// <summary>
    /// 指定されたキーに関連付けられているString型の値を取得します。
    /// 値がない場合、_defaultの値を返します。省略した場合、空の文字列を返します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="_default">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static string GetString(string key, string _default = "")
    {
        return Savedatabase.GetString(key, _default);
    }

    /// <summary>
    /// 指定されたキーに関連付けられているInt型の値を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="value">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void SetInt(string key, int value)
    {
        Savedatabase.SetInt(key, value);
    }

    /// <summary>
    /// 指定されたキーに関連付けられているInt型の値を取得します。
    /// 値がない場合、_defaultの値を返します。省略した場合、0を返します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="_default">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static int GetInt(string key, int _default = 0)
    {
        return Savedatabase.GetInt(key, _default);
    }

    /// <summary>
    /// 指定されたキーに関連付けられているfloat型の値を取得します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="value">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void SetFloat(string key, float value)
    {
        Savedatabase.SetFloat(key, value);
    }

    /// <summary>
    /// 指定されたキーに関連付けられているfloat型の値を取得します。
    /// 値がない場合、_defaultの値を返します。省略した場合、0.0fを返します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <param name="_default">デフォルトの値</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static float GetFloat(string key, float _default = 0.0f)
    {
        return Savedatabase.GetFloat(key, _default);
    }


    /// <summary>
    /// 攻略情報のロード
    /// </summary>
    public static void LoadMasteringData()
    {
        if (ContainsKey("masteringData"))
        {
            DataManager.Instance.masteringData = GetClass<DataManager.MasteringData>("masteringData", new DataManager.MasteringData());
            //Debug.Log(DataManager.Instance.masteringData.masteringCharacterID);
        }
        else
        {
            Debug.Log("攻略データが見つかりませんでした。");
        }
        if (DataManager.Instance.masteringData.masteringCharacterID != -1)
        {
            DataManager.Instance.nowReadChapterID = DataManager.Instance.masteringData.readChapterID;
            DataManager.Instance.nowReadStoryID = DataManager.Instance.masteringData.readStoryID;
        }

    }

    /// <summary>
    /// 攻略情報のセーブ
    /// </summary>
    public static void SaveMasteringData()
    {
        DataManager.Instance.masteringData.readChapterID = DataManager.Instance.nowReadChapterID;
        DataManager.Instance.masteringData.readStoryID = DataManager.Instance.nowReadStoryID;
        SaveData.SetClass<DataManager.MasteringData>("masteringData", DataManager.Instance.masteringData);
        SaveData.Save();
    }

    /// <summary>
    /// 攻略情報の削除
    /// </summary>
    public static void ResetMasteringData()
    {
        if (SaveData.ContainsKey("masteringData"))
        {
            SaveData.Remove("masteringData");
            Debug.Log("セーブデータを削除しました");
        }
        else
        {
            Debug.Log("セーブデータが見つかりませんでした。");
        }
    }

    /// <summary>
    /// 各キャラクターの進捗情報のロード
    /// </summary>
    public static void LoadFinishedStoryData()
    {
        for (int i = 0; i < 4; i++)
        {
            if (SaveData.ContainsKey("FinishedStoryData" + i.ToString()))
            {
                DataManager.Instance.finishedStoryData[i] = SaveData.GetClass<DataManager.FinishedStoryData>("FinishedStoryData" + i.ToString(), new DataManager.FinishedStoryData());
            }
            else
            {
                Debug.Log("各キャラクターの進捗データが見つかりませんでした。");
                SaveFinishedStoryData(i);
            }
        }/*
        if (DataManager.Instance.masteringData.masteringCharacterID != -1)
        {
            DataManager.Instance.nowReadChapterID = DataManager.Instance.finishedStoryData[DataManager.Instance.masteringData.masteringCharacterID].finishedReadChapterID;
            DataManager.Instance.nowReadStoryID = DataManager.Instance.finishedStoryData[DataManager.Instance.masteringData.masteringCharacterID].finishedReadStoryID;
        }*/
    }

    /// <summary>
    /// 全キャラクターの進捗情報のセーブ
    /// </summary>
    public static void SaveFinishedStoryData()
    {
        DataManager.Instance.finishedStoryData[DataManager.Instance.masteringData.masteringCharacterID].finishedReadStoryID = Mathf.Max(DataManager.Instance.nowReadStoryID,0);
        DataManager.Instance.finishedStoryData[DataManager.Instance.masteringData.masteringCharacterLastChapterID].finishedReadChapterID = Mathf.Max(DataManager.Instance.nowReadChapterID, 0);
        for (int i = 0; i < 4; i++)
        {
            SaveData.SetClass<DataManager.FinishedStoryData>("FinishedStoryData" + i.ToString(), DataManager.Instance.finishedStoryData[i]);
        }
        SaveData.Save();
    }

    /// <summary>
    /// 特定キャラクターの進捗情報のセーブ
    /// </summary>
    /// <param name="characterID"></param>
    public static void SaveFinishedStoryData(int characterID)
    {
        if (DataManager.Instance.finishedStoryData[characterID].finishedReadStoryID < DataManager.Instance.nowReadStoryID)
        {
            DataManager.Instance.finishedStoryData[characterID].finishedReadStoryID = Mathf.Max(DataManager.Instance.nowReadStoryID, 0);
        }
        if (DataManager.Instance.finishedStoryData[characterID].finishedReadChapterID < DataManager.Instance.nowReadChapterID){
            DataManager.Instance.finishedStoryData[characterID].finishedReadChapterID = Mathf.Max(DataManager.Instance.nowReadChapterID, 0);
        }
        SaveData.SetClass<DataManager.FinishedStoryData>("FinishedStoryData" + characterID.ToString(), DataManager.Instance.finishedStoryData[characterID]);
        SaveData.Save();
    }

    /// <summary>
    /// 各キャラクターの進捗情報の削除
    /// </summary>
    public static void ResetFinishedStoryData()
    {
        for (int i = 0; i < 4; i++)
        {
            if (SaveData.ContainsKey("FinishedStoryData" + i.ToString()))
            {
                SaveData.Remove("FinishedStoryData" + i.ToString());
                Debug.Log("セーブデータを削除しました");
            }
            else
            {
                Debug.Log("セーブデータが見つかりませんでした。");
            }
        }
    }

    /// <summary>
    /// 設定情報のロード
    /// </summary>
    public static void LoadConfigData()
    {
        if (ContainsKey("configData"))
        {
            DataManager.Instance.configData = GetClass<DataManager.ConfigData>("configData", new DataManager.ConfigData());
        }
        else
        {
            Debug.Log("設定データが見つかりませんでした。");
            DataManager.Instance.configData.bgm = SoundManager.Instance.GetBGMVolume();
            DataManager.Instance.configData.se = SoundManager.Instance.GetSEVolume();
            DataManager.Instance.configData.voice = SoundManager.Instance.GetVoiceVolume();
            DataManager.Instance.configData.textBox = 0.5f;
            DataManager.Instance.configData.textSpd = 0.5f;

            SaveData.SaveConfigData();
        }
    }

    /// <summary>
    /// 設定情報のセーブ
    /// </summary>
    public static void SaveConfigData()
    {
        SaveData.SetClass<DataManager.ConfigData>("configData", DataManager.Instance.configData);
        SaveData.Save();
    }

    /// <summary>
    /// 設定情報の削除
    /// </summary>
    public static void ResetConfigData()
    {
        if (SaveData.ContainsKey("configData"))
        {
            SaveData.Remove("configData");
            Debug.Log("セーブデータを削除しました");
        }
        else
        {
            Debug.Log("セーブデータが見つかりませんでした。");
        }
    }

    /// <summary>
    /// セーブデータからすべてのキーと値を削除します。
    /// </summary>
    public static void Clear()
    {
        Savedatabase.Clear();
    }

    /// <summary>
    /// 指定したキーを持つ値を セーブデータから削除します。
    /// </summary>
    /// <param name="key">キー</param>
    /// <exception cref="System.ArgumentException"></exception>
    public static void Remove(string key)
    {
        Savedatabase.Remove(key);
    }

    /// <summary>
    /// セーブデータ内にキーが存在するかを取得します。
    /// </summary>
    /// <param name="_key">キー</param>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static bool ContainsKey(string _key)
    {
        return Savedatabase.ContainsKey(_key);
    }

    public static bool ContainsFile()
    {
        return Savedatabase.ContainsFile(); 
    }

    /// <summary>
    /// セーブデータに格納されたキーの一覧を取得します。
    /// </summary>
    /// <exception cref="System.ArgumentException"></exception>
    /// <returns></returns>
    public static List<string> Keys()
    {
        return Savedatabase.Keys();
    }

    /// <summary>
    /// 明示的にファイルに書き込みます。
    /// </summary>
    public static void Save()
    {
        Savedatabase.Save();
    }

    #endregion

    #region SaveDatabase Class

    [Serializable]
    private class SaveDataBase
    {
        #region Fields

        private string path;
        //保存先
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string fileName;
        //ファイル名
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private Dictionary<string, string> saveDictionary;
        //keyとjson文字列を格納

        #endregion

        #region Constructor&Destructor

        public SaveDataBase(string _path, string _fileName)
        {
            path = _path;
            fileName = _fileName;
            saveDictionary = new Dictionary<string, string>();
            Load();
        }

        /// <summary>
        /// クラスが破棄される時点でファイルに書き込みます。
        /// </summary>
        ~SaveDataBase()
        {
            Save();
        }

        #endregion

        #region Public Methods

        public bool ContainsFile()
        {
            return File.Exists(path + fileName);
        }

        public void SetList<T>(string key, List<T> list) where T : class, new()
        {
            keyCheck(key);
            var serializableList = new Serialization<T>(list);
            string json = JsonUtility.ToJson(serializableList);
            saveDictionary[key] = json;
        }

        public List<T> GetList<T>(string key, List<T> _default) where T : class, new()
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
            {
                return _default;
            }
            string json = saveDictionary[key];
            Serialization<T> deserializeList = JsonUtility.FromJson<Serialization<T>>(json);

            return deserializeList.ToList();
        }

        public T GetClass<T>(string key, T _default) where T : class, new()
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;

            string json = saveDictionary[key];
            T obj = JsonUtility.FromJson<T>(json);
            return obj;

        }

        public void SetClass<T>(string key, T obj) where T : class, new()
        {
            keyCheck(key);
            string json = JsonUtility.ToJson(obj);
            saveDictionary[key] = json;
        }

        public void SetString(string key, string value)
        {
            keyCheck(key);
            saveDictionary[key] = value;
        }

        public string GetString(string key, string _default)
        {
            keyCheck(key);

            if (!saveDictionary.ContainsKey(key))
                return _default;
            return saveDictionary[key];
        }

        public void SetInt(string key, int value)
        {
            keyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public int GetInt(string key, int _default)
        {
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;
            int ret;
            if (!int.TryParse(saveDictionary[key], out ret))
            {
                ret = 0;
            }
            return ret;
        }

        public void SetFloat(string key, float value)
        {
            keyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public float GetFloat(string key, float _default)
        {
            float ret;
            keyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                ret = _default;

            if (!float.TryParse(saveDictionary[key], out ret))
            {
                ret = 0.0f;
            }
            return ret;
        }

        public void Clear()
        {
            saveDictionary.Clear();

        }

        public void Remove(string key)
        {
            keyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                saveDictionary.Remove(key);
            }

        }

        public bool ContainsKey(string _key)
        {

            return saveDictionary.ContainsKey(_key);
        }

        public List<string> Keys()
        {
            return saveDictionary.Keys.ToList<string>();
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(path + fileName, false, Encoding.GetEncoding("utf-8")))
            {
                var serialDict = new Serialization<string, string>(saveDictionary);
                serialDict.OnBeforeSerialize();
                string dictJsonString = JsonUtility.ToJson(serialDict);
                writer.WriteLine(dictJsonString);
            }
        }

        public void Load()
        {
            if (File.Exists(path + fileName))
            {
                using (StreamReader sr = new StreamReader(path + fileName, Encoding.GetEncoding("utf-8")))
                {
                    if (saveDictionary != null)
                    {
                        var sDict = JsonUtility.FromJson<Serialization<string, string>>(sr.ReadToEnd());
                        sDict.OnAfterDeserialize();
                        saveDictionary = sDict.ToDictionary();
                    }
                }
            }
            else { saveDictionary = new Dictionary<string, string>(); }
        }

        public string GetJsonString(string key)
        {
            keyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                return saveDictionary[key];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// キーに不正がないかチェックします。
        /// </summary>
        private void keyCheck(string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentException("invalid key!!");
            }
        }

        #endregion
    }

    #endregion

    #region Serialization Class

    // List<T>
    [Serializable]
    private class Serialization<T>
    {
        public List<T> target;

        public List<T> ToList()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }
    // Dictionary<TKey, TValue>
    [Serializable]
    private class Serialization<TKey, TValue>
    {
        public List<TKey> keys;
        public List<TValue> values;
        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            int count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            Enumerable.Range(0, count).ToList().ForEach(i => target.Add(keys[i], values[i]));
        }
    }

    #endregion
}