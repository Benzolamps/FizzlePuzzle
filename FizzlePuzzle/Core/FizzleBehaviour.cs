﻿using System;
using System.Linq;
 using UnityEngine;
using Object = UnityEngine.Object;

namespace FizzlePuzzle.Core
{
    internal abstract class FizzleBehaviour : MonoBehaviour
    {
        internal event FizzleEvent awake = () => { };

        protected virtual void Awake()
        {
            awake();
        }

        internal event FizzleEvent start = () => { };

        protected virtual void Start()
        {
            start();
        }

        internal event FizzleEvent update = () => { };

        protected virtual void Update()
        {
            update();
        }

        internal event FizzleEvent fixedUpdate = () => { };

        protected virtual void FixedUpdate()
        {
            fixedUpdate();
        }

        internal event FizzleEvent lateUpdate = () => { };

        protected virtual void LateUpdate()
        {
            lateUpdate();
        }

        internal event FizzleEvent destroy = () => { };

        protected virtual void OnDestroy()
        {
            destroy();
        }

        internal event FizzleEvent disable = () => { };

        protected virtual void OnDisable()
        {
            disable();
        }

        internal event FizzleEvent enable = () => { };

        protected virtual void OnEnable()
        {
            enable();
        }

        protected virtual void OnInspector()
        {
        }

        private void OnValidate()
        {
            OnInspector();
        }

        private void Reset()
        {
            OnInspector();
        }

        internal static T Spawn<T>(T original, Transform parent = null, string name = null) where T : Object
        {
            T obj = Instantiate(original, parent);
            obj.name = name ?? typeof(T).Name;
            return obj;
        }

        internal static GameObject Spawn(GameObject original, Transform parent, string name = null)
        {
            return Spawn<GameObject>(original, parent, name);
        }

        internal static T Spawn<T>(Transform parent, string name = null) where T : Component
        {
            return Spawn(parent, name).AddComponent<T>();
        }

        internal static GameObject Spawn(Transform parent, string name = null)
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            primitive.name = name ?? "New Object";
            primitive.transform.SetParent(parent);
            DestroyImmediate(primitive.GetComponent<BoxCollider>());
            DestroyImmediate(primitive.GetComponent<MeshRenderer>());
            DestroyImmediate(primitive.GetComponent<MeshFilter>());
            return primitive;
        }

        internal static T GetObjectByName<T>(string name)
        {
            try
            {
                return FindObjectsOfType<GameObject>().Single(obj => obj.name == name).GetComponent<T>();
            }
            catch (InvalidOperationException)
            {
                return default(T);
            }
        }

        internal event FizzleEvent<Collision> collisionEnter = _ => { };

        protected virtual void OnCollisionEnter(Collision other)
        {
            collisionEnter(other);
        }

        internal event FizzleEvent<Collision> collisionExit = _ => { };

        protected virtual void OnCollisionExit(Collision other)
        {
            collisionExit(other);
        }

        internal event FizzleEvent<Collision> collisionStay = _ => { };

        protected virtual void OnCollisionStay(Collision other)
        {
            collisionStay(other);
        }

        internal event FizzleEvent<Collider> triggerEnter = _ => { };

        protected virtual void OnTriggerEnter(Collider other)
        {
            triggerEnter(other);
        }

        internal event FizzleEvent<Collider> triggerExit = _ => { };

        protected virtual void OnTriggerExit(Collider other)
        {
            triggerExit(other);
        }

        internal event FizzleEvent<Collider> triggerStay = _ => { };

        protected virtual void OnTriggerStay(Collider other)
        {
            triggerStay(other);
        }
    }
}
