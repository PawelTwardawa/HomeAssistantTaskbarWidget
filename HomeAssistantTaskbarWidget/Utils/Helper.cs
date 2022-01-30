using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HomeAssistantTaskbarWidget.Utils
{
    public class Helper
    {
        //https://stackoverflow.com/questions/39874172/dynamic-string-interpolation
        public static string ReplaceTemplate(string template, object obj)
        {
            var matchResult = Regex.Replace(template, @"{(.+?)}",
                match =>
                {
                    var param = Expression.Parameter(obj.GetType(), obj.GetType().Name);
                    if (IsList(obj))
                        param = Expression.Parameter(obj.GetType(), @"entities");
                    var e = DynamicExpressionParser.ParseLambda(new[] { param }, null, match.Groups[1].Value);
                    return (e.Compile().DynamicInvoke(obj) ?? "").ToString();
                });

            //add new line to text
            return string.Format(matchResult.Replace(@"\n", "{0}"), Environment.NewLine);
        }


        public static int TextWidth(string text, float fontSize = 10, string fontFamily = "Arial")
        {
            var font = new Font(fontFamily, fontSize);
            var size = TextRenderer.MeasureText(text, font);

            return size.Width;
        }

        public static Color HexToColor(string colorHex)
        {
            var colorConventer = new ColorConverter();
            return (Color)colorConventer.ConvertFromString(colorHex);
        }

        //https://stackoverflow.com/questions/17190204/check-if-object-is-dictionary-or-list
        private static bool IsList(object obj)
        {
            if (obj == null) return false;
            return obj is IList &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
    }
}
