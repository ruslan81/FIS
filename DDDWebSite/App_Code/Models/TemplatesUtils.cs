using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;


public class TemplatesUtils
{

    public static string generateHTML(string pathTemplate, Dictionary<String, String> dict)
    {
        StreamReader streamReader = new StreamReader(pathTemplate);
        string template = streamReader.ReadToEnd();
        streamReader.Close();

        foreach (KeyValuePair<string, string> kvp in dict)
        {
            template = template.Replace("<" + kvp.Key + ">", kvp.Value);
        }

        return template;
    }
}
