using System;
using System.Collections.Generic;

public class Whiteboard
{
    private Dictionary<String, Object> _data;

    public Whiteboard()
    {
        _data = new Dictionary<string, object>();
    }

    public T Get<T>(String key)
    {
        return (T) _data[key];
    }

    public void Set(String key, Object obj)
    {
        _data[key] = obj;
    }
}