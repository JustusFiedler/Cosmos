﻿using System.Windows.Media;

namespace Cosmos.Classes
{
    public static class CColor
    {
        public enum Theme
        {
            Default,
            
            Teal,
            Red,
            Blue,
            HKS41,
            HKS44,
            Yellow,
            Green,

            Black,
            White,
            Gray,
            DarkGrey,
            Disabled,
        }


        public static Brush GetColorBrush(Theme theme)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(GetColorString(theme)));
        }

        public static string GetColorString(Theme theme)
        {
            switch (theme)
            {
                case Theme.Default:
                    return "#FF111111";
                    
                case Theme.Teal:
                    return "#FF2c736b";
                case Theme.Red:
                    return "#FFCB2030";
                case Theme.Blue:
                    return "#FF007ACC";
                case Theme.HKS41:
                    return "#FF00305d";
                case Theme.HKS44:
                    return "#FF006ab2";
                case Theme.Yellow:
                    return "#FFE7AD00";
                case Theme.Green:
                    return "#FF11A244";

                case Theme.Black:
                    return "#FF111111";
                case Theme.White:
                    return "#FFEEEEEE";
                case Theme.Gray:
                    return "#FF999999";
                case Theme.DarkGrey:
                    return "#FF1D1D1D";
                case Theme.Disabled:
                    return "#67aaaaaa";
                default:
                    return string.Empty;
            }
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
