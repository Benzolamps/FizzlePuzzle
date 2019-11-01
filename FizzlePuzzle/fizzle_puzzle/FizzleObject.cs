using FizzlePuzzle.Core;
using FizzlePuzzle.Extension;
using FizzlePuzzle.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace fizzle_puzzle
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class FizzleObject
    {
        internal readonly FizzleBehaviour __item;
        private readonly ItemWrapper __item_wrapper;
        private readonly MeshRenderer[] __mesh_renderer;

        public string name { get; }

        public bool __eq__(object obj)
        {
            FizzleObject fizzleObject = obj as FizzleObject;
            if (fizzleObject != null)
            {
                return fizzleObject.__item == __item;
            }
            return false;
        }

        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        internal FizzleObject(string name, Type type)
        {
            this.name = name;
            if ((__item = FizzleScene.FindObject(type, name) as FizzleBehaviour) == null)
            {
                throwException();
            }
            if (__check_item == null)
            {
                throwException();
            }
            __item_wrapper = __item?.GetComponentInParent<ItemWrapper>();
            __mesh_renderer = __item_wrapper?.GetComponentsInChildren<MeshRenderer>();
        }

        protected abstract object __check_item { get; }

        private void throwException()
        {
            throw new FizzleException("Cannot find `" + GetType().Name + "` object named `" + name + "`");
        }

        public void destroy()
        {
            Object.DestroyImmediate(__item_wrapper.gameObject);
        }

        public void disappear()
        {
            __item_wrapper.gameObject.SetActive(false);
        }

        public void appear()
        {
            __item_wrapper.gameObject.SetActive(true);
        }

        public void hide()
        {
            MeshRenderer[] meshRenderer = __mesh_renderer;
            meshRenderer?.ForEach(item => item.enabled = false);
        }

        public void show()
        {
            MeshRenderer[] meshRenderer = __mesh_renderer;
            meshRenderer?.ForEach(item => item.enabled = true);
        }

        public string __str__()
        {
            return "`" + GetType().Name + "` object `" + name + "`";
        }

        internal static R __convert<R>(object obj) where R : FizzleObject
        {
            ConstructorInfo constructor = typeof(R).GetConstructor(new[] {typeof(string)});
            return constructor == null ? null : constructor.Invoke(new object[] {ItemWrapper.GetWrapper(obj).name}) as R;
        }

        public static IEnumerable<T> get_objects_by_type<T>() where T : FizzleObject
        {
            IEnumerable<ItemWrapper> allItems = FizzleScene.GetAllItems();
            if (typeof(T).IsAbstract)
            {
                yield break;
            }
            foreach (ItemWrapper itemWrapper in allItems)
            {
                ConstructorInfo constructor = typeof(T).GetConstructor(new[] {typeof(string)});
                T obj;
                try
                {
                    obj = constructor == null ? null : constructor.Invoke(new object[] {itemWrapper.name}) as T;
                }
                catch (Exception)
                {
                    continue;
                }

                yield return obj;
            }
        }
    }
}
