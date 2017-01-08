using Newtonsoft.Json;
using System;
using System.Reflection;

namespace X.OA.Common.Utility
{
    public static class LazyLoadUtility
    {
        /// <summary>
        /// Convert lazy load type to normal type.
        /// </summary>
        /// <typeparam name="T">The type to convert</typeparam>
        /// <param name="lazyObject">Lazy load object</param>
        /// <returns></returns>
        public static T LazyLoadConvertReflect<T>(object lazyObject) where T : class, new()
        {
            T entity = new T();
            Type convertType = typeof(T);
            PropertyInfo[] lazyProps = lazyObject.GetType().GetProperties();
            foreach (PropertyInfo lazyProp in lazyProps)
            {
                if (lazyProp.CanWrite)
                {
                    object lazyData = lazyProp.GetValue(lazyObject);
                    if (lazyData != null)
                        convertType.GetProperty(lazyProp.Name).SetValue(entity, lazyData);
                }
            }
            return entity;
        }

        /// <summary>
        /// Convert lazy load type to normal type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lazyObject"></param>
        /// <returns></returns>
        public static T LazyLoadConvert<T>(object lazyObject) where T : class, new()
        {
            string serializedObject = JsonConvert.SerializeObject(lazyObject, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}
