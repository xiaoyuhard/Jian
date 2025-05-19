using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvData;

    // Ԥ����������ʽ��������
    private static readonly Regex CsvSplitRegex = new Regex(
        @",(?=(?:[^""\\]*(?:\\.[^""\\]*)*""(?:[^""\\]*(?:\\.[^""\\]*)*"")*[^""\\]*(?:\\.[^""\\]*)*)*[^""\\]*(?:\\.[^""\\]*)*$)",
        RegexOptions.Compiled
    );

    // Start is called before the first frame update
    void Start()
    {
        //TextAsset csvData = Resources.Load<TextAsset>("Data/test"); // ��ȡ Resources/Data/test.csv
        string[] lines = csvData.text.Split("\r\n");

        //foreach (string line in lines)
        //{
        //    string[] fields = line.Split(',');
        //    // ����ÿ�����ݣ������ɶ��������ֵ䣩
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
        //var lines = File.ReadAllLines(filePath, Encoding.UTF8); // ���� UTF-8 BOM
        var result = new string[lines.Length][];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = ParseLine(lines[i]);
        }
        return result;
    }

    private static string[] ParseLine(string line)
    {
        // �ָ��ֶ�
        string[] fields = CsvSplitRegex.Split(line);

        // �����ֶΣ��Ƴ���β���� + ����ת���ַ�
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = fields[i].Trim();
            if (fields[i].StartsWith("\"") && fields[i].EndsWith("\""))
            {
                fields[i] = fields[i]
                    .Substring(1, fields[i].Length - 2)
                    .Replace("\"\"", "\"")  // ����˫����ת��
                    .Replace("\\\"", "\""); // ����б��ת��
            }
        }
        return fields;
    }

}