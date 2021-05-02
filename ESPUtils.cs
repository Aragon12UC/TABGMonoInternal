using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExampleAssembly {
    static class ESPUtils {
        // Most of this is straight from Quantum Cheats Rust/EFT.

        private static Texture2D drawingTex;
        private static Color lastTexColour;

        internal static bool IsOnScreen(Vector3 position) {
            return position.y > 0.01f && position.y < Screen.height - 5f && position.z > 0.01f;
        }

        internal static void DrawLine(Vector2 start, Vector2 end, Color color, float width) {
            Color oldColour = GUI.color;

            var rad2deg = 360 / (Math.PI * 2);

            Vector2 d = end - start;

            float a = (float)rad2deg * Mathf.Atan(d.y / d.x);

            if (d.x < 0)
                a += 180;

            int width2 = (int)Mathf.Ceil(width / 2);

            GUIUtility.RotateAroundPivot(a, start);

            GUI.color = color;

            GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), Texture2D.whiteTexture, ScaleMode.StretchToFill);

            GUIUtility.RotateAroundPivot(-a, start);

            GUI.color = oldColour;
        }

        internal static void CornerBox(Vector2 Head, float Width, float Height, float thickness, Color color, bool outline) {
            int num = (int)(Width / 4f);
            int num2 = num;

            if (outline) {
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);
                RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y - 1f, (float)(num + 2), 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y - 1f, 3f, (float)(num2 + 2), Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);
                RectFilled(Head.x - Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 2), Color.black);
                RectFilled(Head.x + Width / 2f - (float)num - 1f, Head.y + Height - 4f, (float)(num + 2), 3f, Color.black);
                RectFilled(Head.x + Width / 2f - 1f, Head.y + Height - (float)num2 - 4f, 3f, (float)(num2 + 3), Color.black);
            }

            RectFilled(Head.x - Width / 2f, Head.y, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y, 1f, num2, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x - Width / 2f, Head.y + Height - num2 - 3f, 1f, num2, color);
            RectFilled(Head.x + Width / 2f - num, Head.y + Height - 3f, num, 1f, color);
            RectFilled(Head.x + Width / 2f, Head.y + Height - num2 - 3f, 1f, num2 + 1, color);
        }

        internal static void DrawHealth(Vector2 pos, float health, bool center = false) {
            if (center) {
                pos -= new Vector2(26f, 0f);
            }
            pos += new Vector2(0f, 18f);
            BoxRect(new Rect(pos.x, pos.y, 52f, 5f), Color.black);
            pos += new Vector2(1f, 1f);
            Color color = Color.green;
            if (health <= 50f) {
                color = Color.yellow;
            }
            if (health <= 25f) {
                color = Color.red;
            }
            BoxRect(new Rect(pos.x, pos.y, 0.5f * health, 3f), color);
        }

        private static Color __color;
        internal static void BoxRect(Rect rect, Color color) {
            if (color != __color) {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();
                __color = color;
            }

            GUI.DrawTexture(rect, drawingTex);
        }

        private static GUIStyle __style = new GUIStyle();
        private static GUIStyle __outlineStyle = new GUIStyle();
        //private static Font tahoma = Font.CreateDynamicFontFromOSFont("Tahoma", 12);
        internal static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, FontStyle fontStyle = FontStyle.Bold, int depth = 1) {
            __style.fontSize = size;
            __style.richText = true;
            //__style.font = tahoma;
            __style.normal.textColor = color;
            __style.fontStyle = fontStyle;

            __outlineStyle.fontSize = size;
            __outlineStyle.richText = true;
            //__outlineStyle.font = tahoma;
            __outlineStyle.normal.textColor = new Color(0f, 0f, 0f, 1f);
            __outlineStyle.fontStyle = fontStyle;

            GUIContent content = new GUIContent(text);
            GUIContent content2 = new GUIContent(text);
            if (center) {
                //GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                pos.x -= __style.CalcSize(content).x / 2f;
            }
            switch (depth) {
                case 0:
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 1:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 2:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
                case 3:
                    GUI.Label(new Rect(pos.x + 1f, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x - 1f, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y - 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y + 1f, 300f, 25f), content2, __outlineStyle);
                    GUI.Label(new Rect(pos.x, pos.y, 300f, 25f), content, __style);
                    break;
            }
        }

        internal static void RectFilled(float x, float y, float width, float height, Color color) {
            if (!drawingTex)
                drawingTex = new Texture2D(1, 1);

            if (color != lastTexColour) {
                drawingTex.SetPixel(0, 0, color);
                drawingTex.Apply();

                lastTexColour = color;
            }

            GUI.DrawTexture(new Rect(x, y, width, height), drawingTex);
        }
    }
}
