using UnityEngine;

namespace FizzlePuzzle.Extension
{
    internal static class ColorExtension
    {
        internal static Color Add(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            return new Color(color.r + r ?? 0.0F, color.g + g ?? 0.0F, color.b + b ?? 0.0F, color.a + a ?? 0.0F);
        }

        internal static Color Sub(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            return new Color(color.r - r ?? 0.0F, color.g - g ?? 0.0F, color.b - b ?? 0.0F, color.a - a ?? 0.0F);
        }

        internal static Color Replace(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
        {
            return new Color(r ?? color.r, g ?? color.g, b ?? color.b, a ?? color.a);
        }
    }
}