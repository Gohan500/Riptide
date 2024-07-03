// This file is provided under The MIT License as part of RiptideNetworking.
// Copyright (c) Tom Weiland
// For additional information please see the included LICENSE.md file or view it on GitHub:
// https://github.com/RiptideNetworking/Riptide/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Text;

namespace Riptide
{
    /// <summary>Registers the Serializable for automatic resolving</summary>
    /// <remarks>
    ///     <para>
    ///         The Class or struct that this attribute is on <b>MUST</b> inherit from <see cref="IMessageSerializable"/>. 
    ///         Otherwise this attribute will do nothing and the serializable will not be registered.
    ///     </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class SerializableDataAttribute : Attribute
    {
        /// <summary> the ID of this serializable </summary>
        public readonly ushort SerializableId;

        /// <summary> Initializes a new instance of the <see cref="SerializableDataAttribute"/> class with the <paramref name="serializableId"/> value. </summary>
        /// <param name="serializableId"> the ID of this serializable </param>
        /// <remarks>
        ///     <para>
        ///         This class or struct will be ignored if it doesn't inherit from the <see cref="IMessageSerializable"/> class.
        ///     </para>
        /// </remarks>
        public SerializableDataAttribute(ushort serializableId)
        {
            SerializableId = serializableId;
        }
    }

}
