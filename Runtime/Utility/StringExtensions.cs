using UnityEngine;
namespace ProxyBasics
{
    public static class StringExtensions
    {
        public static string DisplayWithColor(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
    }
}

