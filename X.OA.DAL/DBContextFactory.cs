using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Entity;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using X.OA.Model;

namespace X.OA.DAL
{
    public static class DBContextFactory
    {
        /// <summary>
        /// DbContext unique within a thread
        /// </summary>
        /// <typeparam name="T">Data model</typeparam>
        /// <returns>DbContext</returns>
        public static DbContext CreateDbContext<T>() where T : class, new()
        {
            // The model's description is FullName of the DbContext
            AttributeCollection attributes = TypeDescriptor.GetAttributes(typeof(T));
            string description = ((DescriptionAttribute)attributes[typeof(DescriptionAttribute)]).Description;

            Type contextType =  Assembly.Load(typeof(T).Assembly.FullName).GetType(description);
            string contextName = contextType.FullName;
            if (CallContext.GetData(contextName) == null)
            {
                CallContext.SetData(contextName, Activator.CreateInstance(contextType));
            }
            return (DbContext)CallContext.GetData(contextName);
        }
    }
}
