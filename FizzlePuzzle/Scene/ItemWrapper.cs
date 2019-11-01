using FizzlePuzzle.Core;
using FizzlePuzzle.Utility;

namespace FizzlePuzzle.Scene
{
    internal class ItemWrapper : FizzleBehaviour
    {
        internal FizzleItem WrappedItem => GetComponentInChildren<FizzleItem>();

        internal void Generate(FizzleJson data)
        {
            WrappedItem.Generate(data);
        }

        internal static ItemWrapper GetWrapper(object item)
        {
            return (item as FizzleBehaviour)?.GetComponentInParent<ItemWrapper>();
        }
    }
}
