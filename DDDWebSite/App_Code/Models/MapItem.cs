using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Элемент, хранящий пару ключ-значение 
/// </summary>
public class MapItem
{
    public String Key { get; set; }
    public String Value { get; set; }

    public MapItem()
    {
    }

	public MapItem(String Key, String Value)
	{
        this.Key = Key;
        this.Value = Value;
	}
}