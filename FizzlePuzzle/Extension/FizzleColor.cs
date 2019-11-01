using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using UnityEngine;

namespace FizzlePuzzle.Extension
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ParameterHidesMember")]
    public struct FizzleColor
    {
        internal byte r;

        internal byte g;

        internal byte b;

        internal byte a;

        internal FizzleColor(int r, int g, int b, int a)
        {
            this.r = (byte) r;
            this.g = (byte) g;
            this.b = (byte) b;
            this.a = (byte) a;
        }

        [SuppressMessage("ReSharper", "ArrangeThisQualifier")]
        internal FizzleColor(int r, int g, int b)
        {
            this.r = (byte) r;
            this.g = (byte) g;
            this.b = (byte) b;
            this.a = 255;
        }

        public static implicit operator FizzleColor(string color0)
        {
            return ColorTranslator.FromHtml(color0);
        }

        public static implicit operator string(FizzleColor color0)
        {
            return color0.ToString();
        }

        public override string ToString()
        {
            return ColorTranslator.ToHtml(this);
        }

        public static implicit operator System.Drawing.Color(FizzleColor color0)
        {
            return System.Drawing.Color.FromArgb(color0.a, color0.r, color0.g, color0.b);
        }

        public static implicit operator FizzleColor(System.Drawing.Color color0)
        {
            return new FizzleColor(color0.R, color0.G, color0.B, color0.A);
        }

        public static implicit operator UnityEngine.Color(FizzleColor color0)
        {
            return new Color32(color0.r, color0.g, color0.b, color0.a);
        }

        public static implicit operator FizzleColor(UnityEngine.Color color0)
        {
            Color32 color = color0;
            return new FizzleColor(color.r, color.g, color.b, color.a);
        }

        internal FizzleColor Add(int? r = null, int? g = null, int? b = null, int? a = null)
        {
            this.r += (byte) (r ?? 0);
            this.g += (byte) (g ?? 0);
            this.b += (byte) (b ?? 0);
            this.a += (byte) (a ?? 0);
            return this;
        }

        internal FizzleColor Sub(int? r = null, int? g = null, int? b = null, int? a = null)
        {
            this.r -= (byte) (r ?? 0);
            this.g -= (byte) (g ?? 0);
            this.b -= (byte) (b ?? 0);
            this.a -= (byte) (a ?? 0);
            return this;
        }

        internal FizzleColor Replace(int? r = null, int? g = null, int? b = null, int? a = null)
        {
            this.r = (byte) (r ?? this.r);
            this.g = (byte) (g ?? this.g);
            this.b = (byte) (b ?? this.b);
            this.a = (byte) (a ?? this.a);
            return this;
        }
    }
}
