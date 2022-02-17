using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveLoadManager
{
    #region Variables
    public static string dataFolderName, imagesFolderName;
    #endregion

    #region Methods

    #region Save

    public static void SaveStringAsJsonFile(string _data, string _fileName, bool _overrideFile)
    {
        if (!_overrideFile)
            if (FileExists(_fileName))
                return;

        // Save as file locally
        SaveJsonData(_data, _fileName, Ext.JSON);
    }

    public static void SaveImageAsFile(byte[] _bytes, string _fileName, bool _overrideFile)
    {
        if (!_overrideFile)
            if (FileExists(_fileName))
                return;

        // Save as file locally
        SaveData(_bytes, _fileName, Ext.JSON);
    }

    public static void SaveJsonData(string _dataToSave, string _dataFileName, Ext _fileExtension)
    {
        string tempPath = GetPathForData();

        tempPath = Path.Combine(tempPath, _dataFileName + GetExtension(_fileExtension));

        Debug.Log(tempPath);

        byte[] bytes = Encoding.UTF8.GetBytes(_dataToSave);

        // Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));

        try
        {
            File.WriteAllBytes(tempPath, bytes);
            if (Application.isEditor)
                Debug.Log("Saved " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));

        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To save " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }
    }

    public static void SaveData(byte[] _bytes, string _dataFileName, Ext _fileExtension)
    {
        string tempPath = GetPathForData();

        tempPath = Path.Combine(tempPath, _dataFileName + GetExtension(_fileExtension));

        // Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));

        try
        {
            File.WriteAllBytes(tempPath, _bytes);
            if (Application.isEditor)
                Debug.Log("Saved " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To save " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }
    }

    public static void SaveImageData(byte[] _bytes, string _dataFileName, Ext _fileExtension)
    {
        string tempPath = GetPathForImages();

        tempPath = Path.Combine(tempPath, _dataFileName + GetExtension(_fileExtension));

        // Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));

        try
        {
            File.WriteAllBytes(tempPath, _bytes);
            if (Application.isEditor)
                Debug.Log("Saved " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To save " + _dataFileName + " to: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }
    }

    #endregion

    #region Load

    // Load file from application persistentDataPath
    public static bool LoadData(string _dataFileName, Ext _fileExtension, out byte[] newBytes)
    {
        newBytes = null;
        string tempPath = GetPathForData();
        tempPath = Path.Combine(tempPath, _dataFileName + GetExtension(_fileExtension));

        // Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            if (Application.isEditor)
                Debug.Log(_dataFileName + " Directory does not exist at " + tempPath);
            return false;
        }

        if (!FileExists(tempPath))
        {
            if (Application.isEditor)
                Debug.Log(_dataFileName + " does not exist in " + tempPath);
            return false;
        }

        try
        {
            newBytes = File.ReadAllBytes(tempPath);
            if (Application.isEditor)
                Debug.Log("Loaded " + _dataFileName + " from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To Load " + _dataFileName + " from: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }

        return true;
    }

    // Load texture from application persistentDataPath
    public static bool LoadTexture2D(string _imageFileName, Ext _fileExtension, out Texture2D texture)
    {
        texture = new Texture2D(1, 1);
        string tempPath = GetPathForImages();
        tempPath = Path.Combine(tempPath, _imageFileName + GetExtension(_fileExtension));

        // Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            if (Application.isEditor)
                Debug.Log(_imageFileName + " Directory does not exist at " + tempPath);
            return false;
        }

        if (!ImageExists(tempPath))
        {
            if (Application.isEditor)
                Debug.Log(_imageFileName + " does not exist in " + tempPath);
            return false;
        }

        try
        {
            byte[] bytes = File.ReadAllBytes(tempPath);
            texture.LoadImage(bytes);
            if (Application.isEditor)
                Debug.Log("Loaded " + _imageFileName + " from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To Load " + _imageFileName + " from: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }

        return true;
    }

    public static string LoadDataJsonAsString(string dataFileName)
    {
        string tempPath = GetPathForData(); //Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + GetExtension(Ext.JSON));

        // Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            if (Application.isEditor)
            {
                Debug.Log(dataFileName + " Directory does not exist at " + tempPath);
            }
            return null;
        }

        if (!File.Exists(tempPath))
        {
            if (Application.isEditor) Debug.Log(dataFileName + " does not exist in " + tempPath);
            return null;
        }

        // Load saved Json
        byte[] newBytes = null;

        try
        {
            newBytes = File.ReadAllBytes(tempPath);
            if (Application.isEditor) Debug.Log("Loaded " + dataFileName + " from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            if (Application.isEditor)
            {
                Debug.LogWarning("Failed To Load " + dataFileName + " from: " + tempPath.Replace("/", "\\"));
                Debug.LogWarning("Error: " + e.Message);
            }
        }

        // Convert to json string
        string newData = Encoding.UTF8.GetString(newBytes);

        //Convert to Object
        //object resultValue =  JsonUtility.FromJson<T>(newData);
        return newData;
    }

    #endregion

    #region Delete

    public static bool DeleteData(string _dataFileName, Ext _fileExtension)
    {
        bool success = false;

        // Load Data
        string tempPath = GetPathForData();
        tempPath = Path.Combine(tempPath, _dataFileName + GetExtension(_fileExtension));

        // Exit if Directory or File does not exist
        if (!FileExists(tempPath))
        {
            if (Application.isEditor)
                Debug.Log(_dataFileName + " Directory does not exist to " + tempPath);
            return false;
        }

        try
        {
            File.Delete(tempPath);
            if (Application.isEditor)
                Debug.Log(_dataFileName + " deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            if (Application.isEditor)
                Debug.LogWarning("Failed To Delete " + _dataFileName + ": " + e.Message);
        }

        return success;
    }

    #endregion

    #region Check Files

    public static bool CheckIfFileExists(string _fileUrl)
    {
        return File.Exists(_fileUrl);
    }

    public static bool CheckIfDirectoryExists(string _fileUrl)
    {
        return Directory.Exists(Path.GetDirectoryName(_fileUrl));
    }

    public static bool FileExists(string _filename)
    {
        string tempPath = GetPathForData();
        tempPath = Path.Combine(tempPath, _filename);
        bool exists = System.IO.File.Exists(tempPath);
        return exists;
    }

    public static bool ImageExists(string _filename)
    {
        string tempPath = GetPathForImages();
        tempPath = Path.Combine(tempPath, _filename);
        bool exists = System.IO.File.Exists(tempPath);
        return exists;
    }

    public static bool JsonFilesExist(string[] _filenames)
    {
        int totalExists = 0;
        for (int i = 0; i < _filenames.Length; i++)
        {
            string tempPath = GetPathForData();
            Debug.LogWarning(_filenames[i] + GetExtension(Ext.JSON));
            tempPath = Path.Combine(tempPath, _filenames[i] + GetExtension(Ext.JSON));
            if (Application.isEditor)
                Debug.Log(tempPath);
            if (File.Exists(tempPath))
                totalExists++;
        }
        return totalExists == _filenames.Length;
    }

    #endregion

    #region Get file extension

    public enum Ext
    {
        TXT,
        WAV,
        XML,
        JPG,
        PNG,
        JPEG,
        MP4,
        JSON,
        NULL
    }

    public static string GetExtension(Ext xt)
    {
        if (xt == Ext.TXT)
        {
            return ".txt";
        }
        else if (xt == Ext.XML)
        {
            return ".xml";
        }
        else if (xt == Ext.JPG)
        {
            return ".jpg";
        }
        else if (xt == Ext.JPEG)
        {
            return ".jpeg";
        }
        else if (xt == Ext.PNG)
        {
            return ".png";
        }
        else if (xt == Ext.MP4)
        {
            return ".mp4";
        }
        else if (xt == Ext.JSON)
        {
            return ".json";
        }
        else if (xt == Ext.NULL)
        {
            return string.Empty;
        }

        return ".txt";
    }

    #endregion

    public static void CreatePathForData(string _dataFolderName, string _imagesFolderName)
    {
        dataFolderName = _dataFolderName;
        imagesFolderName = _imagesFolderName;
        string p = Application.platform == RuntimePlatform.WindowsPlayer ? Application.dataPath : Application.persistentDataPath;
        string dataPath = Path.Combine(p, _dataFolderName);
        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
            Directory.CreateDirectory(dataPath);
        string imagesPath = Path.Combine(dataPath, _imagesFolderName);
        if (!Directory.Exists(Path.GetDirectoryName(imagesPath)))
            Directory.CreateDirectory(imagesPath);
    }

    /// <summary>
    /// Gets the path of the data folder - multi platform.
    /// </summary>
    public static string GetPathForData()
    {
        string path = string.Empty;
        if (string.IsNullOrWhiteSpace(dataFolderName))
        {
            Debug.LogWarning("Folder Name is null!");
            return path;
        }
        string p = Application.platform == RuntimePlatform.WindowsPlayer ? Application.dataPath : Application.persistentDataPath;

        // Get folder location of current platform
        path = Path.Combine(p, dataFolderName);
        return path;
    }

    public static string GetPathForImages()
    {
        string path = string.Empty;
        if (string.IsNullOrWhiteSpace(imagesFolderName))
        {
            Debug.LogWarning("Folder Name is null!");
            return path;
        }

        // Get folder location of current platform
        path = Path.Combine(GetPathForData(), imagesFolderName);
        return path;
    }

    // Editor
    public static void OpenSaveFolder(string _dataFolderName)
    {
        bool openInsidesOfFolder = false;
        string p = Application.platform == RuntimePlatform.WindowsPlayer ? Application.dataPath : Application.persistentDataPath;
        string path = Path.Combine(p, _dataFolderName);
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(path);
        string folderPath = path.Replace(@"/", @"\");
        if (!Directory.Exists(Path.GetDirectoryName(folderPath)))
            Directory.CreateDirectory(folderPath);
        if (Directory.Exists(folderPath))
        { openInsidesOfFolder = true; }

        try { System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + folderPath); }
        catch (System.ComponentModel.Win32Exception e) { e.HelpLink = ""; }
    }
    #endregion

}
