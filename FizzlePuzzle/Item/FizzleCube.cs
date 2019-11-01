using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using FizzlePuzzle.UI;
using FizzlePuzzle.Utility;

namespace FizzlePuzzle.Item
{
    internal class FizzleCube : BasicCube, FizzleItem
    {
        public void Generate(FizzleJson data)
        {
            FizzleJson fizzleJson = data["size"];
            m_Width = fizzleJson[0].ToObject<float>();
            m_Height = fizzleJson[1].ToObject<float>();
            m_Length = fizzleJson[2].ToObject<float>();
            m_RepeatTarget = data.GetOrDefault("repeat-target", m_RepeatTarget);
            string defaultValue = ((FizzleColor) m_Color).ToString();
            m_Color = (FizzleColor) data.GetOrDefault("color", defaultValue);
        }
    }
}
