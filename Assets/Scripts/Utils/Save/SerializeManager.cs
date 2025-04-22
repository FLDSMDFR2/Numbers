using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SerializeManager
{
    private static BinaryFormatter formatter = new BinaryFormatter();
    private static string mainDirectory = Application.persistentDataPath + "/SaveData";
    private static string fileExtention = ".bam";

    protected static void FilePathCheck(string path)
    {
        System.IO.FileInfo file = new System.IO.FileInfo(path);      
        if (File.Exists(path))

        {
            File.Delete(path);
        }
        else
        {
            file.Directory.Create();
        }
    }

    protected static string MainDirectoryPath()
    {
        return mainDirectory;
    }

    protected static string ConfigFilePath(SaveData obj)
    {
        return mainDirectory + "/" + obj.SaveName + fileExtention;
    }

    public static void Save(SaveData obj)
    {
        var path = ConfigFilePath(obj);
        FilePathCheck(path);

        FileStream fStream = new FileStream(path, FileMode.Create);
        try
        {
            formatter.Serialize(fStream, obj.GetSaveData());

            TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, $"Saving value to path {path}");

            fStream.Close();
        }
        catch (Exception ex)
        {
            fStream.Close();
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Error while attempting to file.");
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"Path={path}");
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"Error={ex.Message}");
        }
    }

    public static T Load<T>(SaveData obj)
    {
        var path = ConfigFilePath(obj);
        if (File.Exists(path))
        {
            FileStream fStream = new FileStream(path, FileMode.Open);
            try
            {
                T retval = (T)formatter.Deserialize(fStream);

                TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, $"Loading values from path {path}");

                fStream.Close();

                return retval;
            }
            catch (Exception ex)
            {
                fStream.Close();
                TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, "Error while attempting to file.");
                TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"Path={path}");
                TraceManager.WriteTrace(TraceChannel.Main, TraceType.error, $"Error={ex.Message}");
                return default(T);

            }
        }
        else
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "No Data to Load");
            return default(T);
        }

    }
}
