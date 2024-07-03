// This file is provided under The MIT License as part of RiptideNetworking.
// Copyright (c) Tom Weiland
// For additional information please see the included LICENSE.md file or view it on GitHub:
// https://github.com/RiptideNetworking/Riptide/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Riptide
{
    internal class MessageSerializableManager
    {
        private static Dictionary<ushort, Type> serializables;
        private static Dictionary<Type, ushort> ids;

        internal static Type GetSerializable(ushort id)
        {
            if (!serializables.TryGetValue(id, out Type t))
                throw new InvalidSerializableIdException(id);

            return t;
        }
        internal static ushort GetId(Type t)
        {
            if (!ids.TryGetValue(t, out ushort id))
                throw new InvalidSerializableException(t);

            return id;
        }

        internal static void FindSerializables()
        {
            if (serializables != null)
                return;

            serializables = new Dictionary<ushort, Type>();
            ids = new Dictionary<Type, ushort>();

            string thisAssemblyName = Assembly.GetExecutingAssembly().GetName().FullName;
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a
                    .GetReferencedAssemblies()
                    .Any(r => r.FullName == thisAssemblyName))
                .SelectMany(a => a.GetTypes())
                .Where(c => c.GetCustomAttributes(typeof(SerializableDataAttribute), false).Length > 0)
                .ToArray();

            foreach (Type t in types)
            {
                if (!typeof(IMessageSerializable).IsAssignableFrom(t))
                    throw new SerializableInheritanceException(t);

                SerializableDataAttribute serializableAttribute = t.GetCustomAttribute<SerializableDataAttribute>();

                if(serializables.ContainsKey(serializableAttribute.SerializableId))
                {
                    Type other = serializables[serializableAttribute.SerializableId];
                    throw new DuplicateSerializableIdException(serializableAttribute.SerializableId, other, t);
                }
                else if (ids.ContainsKey(t))
                {
                    ushort other = ids[t];
                    throw new DuplicateSerializableException(t, other, serializableAttribute.SerializableId);
                }

                serializables.Add(serializableAttribute.SerializableId, t);
                ids.Add(t, serializableAttribute.SerializableId);
            }
        }
    }
}
