using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvData;

    // 预编译正则表达式提升性能
    private static readonly Regex CsvSplitRegex = new Regex(
        @",(?=(?:[^""\\]*(?:\\.[^""\\]*)*""(?:[^""\\]*(?:\\.[^""\\]*)*"")*[^""\\]*(?:\\.[^""\\]*)*)*[^""\\]*(?:\\.[^""\\]*)*$)",
        RegexOptions.Compiled
    );

    // Start is called before the first frame update
    void Start()
    {
        //TextAsset csvData = Resources.Load<TextAsset>("Data/test"); // 读取 Resources/Data/test.csv
        string[] lines = csvData.text.Split("\r\n");

        //foreach (string line in lines)
        //{
        //    string[] fields = line.Split(',');
        //    // 处理每行数据（如生成对象或存入字典）
        //}

        var result = ReadCSV(lines);

        foreach (var row in result)
        {
            print(string.Join(" | ", row));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string[][] ReadCSV(string[] lines)
    {
        //var lines = File.ReadAllLines(filePath, Encoding.UTF8); // 处理 UTF-8 BOM
        var result = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = ParseLine(lines[i]);
        }
        return result;
    }

    private static string[] ParseLine(string line)
    {
        // 分割字段
        string[] fields = CsvSplitRegex.Split(line);

        // 处理字段：移除首尾引号 + 处理转义字符
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i].Trim();
            if (fields[i].StartsWith("\"") && fields[i].EndsWith("\""))
            {
                fields[i] = fields[i]
                    .Substring(1, fields[i].Length - 2)
                    .Replace("\"\"", "\"")  // 处理双引号转义
                    .Replace("\\\"", "\""); // 处理反斜杠转义
            }
        }
        return fields;
    }

}