using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper
{
    /// <summary>
    /// 将十六进制颜色代码 ‌FFDB00‌ 转换为 RGB 颜色值
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        // 移除可能的 "#" 符号
        hex = hex.Replace("#", "").Trim();

        // 将十六进制转为十进制
        float r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

        return new Color(r, g, b, 1f); // Alpha 默认为 1
    }
}
