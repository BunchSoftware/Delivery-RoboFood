using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;


public class DataStream : IDisposable
{
    private FileStream stream;

    public void Serialize(string path, object data)
    {
        if (data == null)
            return;
        if (IsCreateFileSave(path) == false)
            stream = File.Create(path);
        else
            stream = File.Open(path, FileMode.Open);

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, data);

        stream.Close();
    }
    public T Deserialize<T>(string path)
    {
        if (CountSymbolInFile(path) == 0)
            return default(T);

        stream = File.Open(path, FileMode.Open);

        BinaryFormatter formatter = new BinaryFormatter();
        T data = (T)formatter.Deserialize(stream);
        stream.Close();
        return data;
    }

    public void Delete(string path)
    {
        if (IsCreateFileSave(path) == false)
            return;
        else
            File.Delete(path);
    }

    public void DeleteAll(string path)
    {
        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }

    }

    public int CountSymbolInFile(string path)
    {
        if (IsCreateFileSave(path) == false)
            return 0;

        return File.ReadAllText(path).Length;
    }

    public bool IsCreateFileSave(string path)
    {
        return File.Exists(path);
    }

    public void Dispose()
    {
        stream.Close();
    }

    ~DataStream() 
    { 
    Dispose();
    }
}
