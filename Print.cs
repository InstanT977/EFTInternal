using System;
using UnityEngine;

namespace EscapeFromGod
{
    public class Print
    {
        private static class D
        {
            public static TextAnchor defaultAlignment = TextAnchor.UpperLeft;
            public static Rect rectA = Rect.zero;
            public static GUIContent tGUIContent = GUIContent.none;
            public static GUIStyle style = new GUIStyle();
            public static Color backupColor = new Color();
            public static Matrix4x4 matrix = new Matrix4x4();
            public static float floatA;
            public static int intA;

            public static Vector2 vectorA = Vector2.zero;
            public static Vector2 vector1x1 = new Vector2(1, 1);
        }
        public class Menu
        {
            public static void Button(string name, ref bool switcher)
            {
                switcher = GUILayout.Button(name);
            }
            public static void Label(string name, bool center = false)
            {
                if (center)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.Label(name);
                if (center)
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
            public static void Label(string name) { GUILayout.Label(name); }
            public static void Label(string name1, string name2) { GUILayout.Label(name1 + name2); }
            public static void Label(string name1, string name2, string name3) { GUILayout.Label(name1 + name2 + name3); }
            public static void Label(string name1, string name2, string name3, string name4) { GUILayout.Label(name1 + name2 + name3 + name4); }
            public static class Slider
            {
                public static class Horizontal
                {
                    public static void Int(ref int value, float min = 0f, float max = 1000f, string name = "", bool showValue = true)
                    {
                        if (name != "")
                            Menu.Label(name + ": " + value.ToString());
                        value = (int)GUILayout.HorizontalSlider(value, min, max);
                        if (showValue)
                        {
                            GUILayout.BeginHorizontal();
                            D.defaultAlignment = GUI.skin.label.alignment;
                            GUILayout.Label(min.ToString());
                            GUI.skin.label.alignment = TextAnchor.UpperRight;
                            GUILayout.Label(max.ToString());
                            GUI.skin.label.alignment = D.defaultAlignment;
                            GUILayout.EndHorizontal();
                        }
                    }
                    public static void Float(ref float value, float min = 0f, float max = 1000f, string name = "", bool round = false, bool showValue = true)
                    {
                        if (name != "")
                            Menu.Label(name + ": " + value.ToString());
                        if (round)
                            value = (float)(int)value;
                        value = GUILayout.HorizontalSlider(value, min, max);
                        if (showValue)
                        {
                            GUILayout.BeginHorizontal();
                            D.defaultAlignment = GUI.skin.label.alignment;
                            GUILayout.Label(min.ToString());
                            GUI.skin.label.alignment = TextAnchor.UpperRight;
                            GUILayout.Label(max.ToString());
                            GUI.skin.label.alignment = D.defaultAlignment;
                            GUILayout.EndHorizontal();
                        }
                    }
                }
            }
            public static class TextBox
            {
                public static void Int(ref int value) { value = Int32.Parse(GUILayout.TextField(value.ToString())); }
                public static void Long(ref long value) { value = Int64.Parse(GUILayout.TextField(value.ToString())); }
                public static void String(ref string value) { value = GUILayout.TextField(value); }
                public static void Float(ref float value) { value = float.Parse(GUILayout.TextField(value.ToString())); }
            }
            public static void Checkbox(string name, ref bool switcher) { switcher = GUILayout.Toggle(switcher, name); }

        }

        #region Color Codes
        public enum colorCodeEnum
        {
            White,
            Black,
            Grey,
            Yellow,
            Red,
            Blue,
            Cyan,
            Green,
            Pink,
            Purple,
            Orange,
            DarkGreen
        }

        public static Color colorCode(colorCodeEnum ColorCode)
        {
            switch (ColorCode)
            {
                case colorCodeEnum.Black:
                    return Color.black;
                case colorCodeEnum.White:
                    return Color.white;
                case colorCodeEnum.Blue:
                    return Color.blue;
                case colorCodeEnum.Cyan:
                    return Color.cyan;
                case colorCodeEnum.Grey:
                    return Color.gray;
                case colorCodeEnum.Yellow:
                    return Color.yellow;
                case colorCodeEnum.Red:
                    return Color.red;
                case colorCodeEnum.Green:
                    return Color.green;
                case colorCodeEnum.Purple:
                    return Color.magenta;
                case colorCodeEnum.Pink:
                    return new Color(232, 0, 254);
                case colorCodeEnum.Orange:
                    return new Color(1.0f, 0.64f, 0.0f);
                case colorCodeEnum.DarkGreen:
                    return new Color(0f, 0.39f, 0.0f);

                default:
                    return Color.black;
            }
        }

        #endregion

        private const colorCodeEnum black = colorCodeEnum.Black;
        private static Texture2D texture;

        //public static void DrawFont(Vector2 Point, string txt, Color color)
        //{
        //    DrawOutline(Point, txt, Color.black, color, 1f);
        //}
        public static void DrawFont(Vector2 position, string text, colorCodeEnum outColor = colorCodeEnum.Black, colorCodeEnum inColor = colorCodeEnum.Black, float size = 1f)
        {
            D.style.alignment = TextAnchor.UpperCenter;
            D.backupColor = GUI.color;
            D.vectorA = D.style.CalcSize(new GUIContent(text));
            D.rectA = new Rect(position.x, position.y, D.vectorA.x + 10f, D.vectorA.y + 10f);

            D.style.normal.textColor = colorCode(outColor);
            GUI.color = colorCode(outColor);

            D.rectA.x -= (size * 0.5f);
            GUI.Label(D.rectA, text, D.style);

            D.rectA.x += size;
            GUI.Label(D.rectA, text, D.style);

            D.rectA.x -= (size * 0.5f);
            D.rectA.y -= (size * 0.5f);
            GUI.Label(D.rectA, text, D.style);

            D.rectA.y += size;
            GUI.Label(D.rectA, text, D.style);

            D.rectA.y -= (size * 0.5f);
            D.style.normal.textColor = colorCode(inColor);
            GUI.color = D.backupColor;
            GUI.Label(D.rectA, text, D.style);
        }
        public static void DrawLine(Vector2 pointA, Vector2 pointB, float width, colorCodeEnum color)
        {
            DrawLineStretched(pointA, pointB, (int)width, color);
            if (!false)
                return;
        }
        public static void DrawLineStretched(Vector2 lineStart, Vector2 lineEnd, int thickness, colorCodeEnum color)
        {
            if (!texture) { texture = new Texture2D(1, 1); texture.filterMode = FilterMode.Point; }
            D.matrix = GUI.matrix;
            D.backupColor = GUI.color;
            GUI.color = colorCode(color);
            D.vectorA = lineEnd - lineStart;
            D.floatA = (float)(180f / Math.PI * Mathf.Atan(D.vectorA.y / D.vectorA.x));
            if (D.vectorA.x < 0f)
            {
                D.floatA += 180f;
            }
            if (thickness < 1)
            {
                thickness = 1;
            }
            D.intA = (int)Mathf.Ceil((float)(thickness / 2));
            GUIUtility.RotateAroundPivot(D.floatA, lineStart);
            GUI.DrawTexture(new Rect(lineStart.x, lineStart.y - (float)D.intA, D.vectorA.magnitude, (float)thickness), texture);
            GUIUtility.RotateAroundPivot(-D.floatA, lineStart);

            GUI.color = D.backupColor;
            GUI.matrix = D.matrix;
        }
    }
}

