// This file is provided under The MIT License as part of RiptideNetworking.
// Copyright (c) Tom Weiland
// For additional information please see the included LICENSE.md file or view it on GitHub:
// https://github.com/RiptideNetworking/Riptide/blob/main/LICENSE.md

using Riptide.Utils;
using System;
using System.Reflection;

namespace Riptide
{
    /// <summary>The exception that is thrown when a <see cref="Message"/> does not contain enough unwritten bits to perform an operation.</summary>
    public class InsufficientCapacityException : Exception
    {
        /// <summary>The message with insufficient remaining capacity.</summary>
        public readonly Message RiptideMessage;
        /// <summary>The name of the type which could not be added to the message.</summary>
        public readonly string TypeName;
        /// <summary>The number of available bits the type requires in order to be added successfully.</summary>
        public readonly int RequiredBits;

        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance.</summary>
        public InsufficientCapacityException() { }
        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InsufficientCapacityException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InsufficientCapacityException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="message">The message with insufficient remaining capacity.</param>
        /// <param name="reserveBits">The number of bits which were attempted to be reserved.</param>
        public InsufficientCapacityException(Message message, int reserveBits) : base(GetErrorMessage(message, reserveBits))
        {
            RiptideMessage = message;
            TypeName = "reservation";
            RequiredBits = reserveBits;
        }
        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="message">The message with insufficient remaining capacity.</param>
        /// <param name="typeName">The name of the type which could not be added to the message.</param>
        /// <param name="requiredBits">The number of available bits required for the type to be added successfully.</param>
        public InsufficientCapacityException(Message message, string typeName, int requiredBits) : base(GetErrorMessage(message, typeName, requiredBits))
        {
            RiptideMessage = message;
            TypeName = typeName;
            RequiredBits = requiredBits;
        }
        /// <summary>Initializes a new <see cref="InsufficientCapacityException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="message">The message with insufficient remaining capacity.</param>
        /// <param name="arrayLength">The length of the array which could not be added to the message.</param>
        /// <param name="typeName">The name of the array's type.</param>
        /// <param name="requiredBits">The number of available bits required for a single element of the array to be added successfully.</param>
        public InsufficientCapacityException(Message message, int arrayLength, string typeName, int requiredBits) : base(GetErrorMessage(message, arrayLength, typeName, requiredBits))
        {
            RiptideMessage = message;
            TypeName = $"{typeName}[]";
            RequiredBits = requiredBits * arrayLength;
        }

        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(Message message, int reserveBits)
        {
            return $"Cannot reserve {reserveBits} {Helper.CorrectForm(reserveBits, "bit")} in a message with {message.UnwrittenBits} " +
                   $"{Helper.CorrectForm(message.UnwrittenBits, "bit")} of remaining capacity!";
        }
        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(Message message, string typeName, int requiredBits)
        {
            return $"Cannot add a value of type '{typeName}' (requires {requiredBits} {Helper.CorrectForm(requiredBits, "bit")}) to " +
                   $"a message with {message.UnwrittenBits} {Helper.CorrectForm(message.UnwrittenBits, "bit")} of remaining capacity!";
        }
        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(Message message, int arrayLength, string typeName, int requiredBits)
        {
            requiredBits *= arrayLength;
            return $"Cannot add an array of type '{typeName}[]' with {arrayLength} {Helper.CorrectForm(arrayLength, "element")} (requires {requiredBits} {Helper.CorrectForm(requiredBits, "bit")}) " +
                   $"to a message with {message.UnwrittenBits} {Helper.CorrectForm(message.UnwrittenBits, "bit")} of remaining capacity!";
        }
    }
    
    /// <summary>The exception that is thrown when a method with a <see cref="MessageHandlerAttribute"/> is not marked as <see langword="static"/>.</summary>
    public class NonStaticHandlerException : Exception
    {
        /// <summary>The type containing the handler method.</summary>
        public readonly Type DeclaringType;
        /// <summary>The name of the handler method.</summary>
        public readonly string HandlerMethodName;

        /// <summary>Initializes a new <see cref="NonStaticHandlerException"/> instance.</summary>
        public NonStaticHandlerException() { }
        /// <summary>Initializes a new <see cref="NonStaticHandlerException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NonStaticHandlerException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="NonStaticHandlerException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public NonStaticHandlerException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="NonStaticHandlerException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="declaringType">The type containing the handler method.</param>
        /// <param name="handlerMethodName">The name of the handler method.</param>
        public NonStaticHandlerException(Type declaringType, string handlerMethodName) : base(GetErrorMessage(declaringType, handlerMethodName))
        {
            DeclaringType = declaringType;
            HandlerMethodName = handlerMethodName;
        }

        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(Type declaringType, string handlerMethodName)
        {
            return $"'{declaringType.Name}.{handlerMethodName}' is an instance method, but message handler methods must be static!";
        }
    }

    /// <summary>The exception that is thrown when a method with a <see cref="MessageHandlerAttribute"/> does not have an acceptable message handler method signature (either <see cref="Server.MessageHandler"/> or <see cref="Client.MessageHandler"/>).</summary>
    public class InvalidHandlerSignatureException : Exception
    {
        /// <summary>The type containing the handler method.</summary>
        public readonly Type DeclaringType;
        /// <summary>The name of the handler method.</summary>
        public readonly string HandlerMethodName;

        /// <summary>Initializes a new <see cref="InvalidHandlerSignatureException"/> instance.</summary>
        public InvalidHandlerSignatureException() { }
        /// <summary>Initializes a new <see cref="InvalidHandlerSignatureException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidHandlerSignatureException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="InvalidHandlerSignatureException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidHandlerSignatureException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="InvalidHandlerSignatureException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="declaringType">The type containing the handler method.</param>
        /// <param name="handlerMethodName">The name of the handler method.</param>
        public InvalidHandlerSignatureException(Type declaringType, string handlerMethodName) : base(GetErrorMessage(declaringType, handlerMethodName))
        {
            DeclaringType = declaringType;
            HandlerMethodName = handlerMethodName;
        }

        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(Type declaringType, string handlerMethodName)
        {
            return $"'{declaringType.Name}.{handlerMethodName}' doesn't match any acceptable message handler method signatures! Server message handler methods should have a 'ushort' and a '{nameof(Riptide.Message)}' parameter, while client message handler methods should only have a '{nameof(Riptide.Message)}' parameter.";
        }
    }

    /// <summary>The exception that is thrown when multiple methods with <see cref="MessageHandlerAttribute"/>s are set to handle messages with the same ID <i>and</i> have the same method signature.</summary>
    public class DuplicateHandlerException : Exception
    {
        /// <summary>The message ID with multiple handler methods.</summary>
        public readonly ushort Id;
        /// <summary>The type containing the first handler method.</summary>
        public readonly Type DeclaringType1;
        /// <summary>The name of the first handler method.</summary>
        public readonly string HandlerMethodName1;
        /// <summary>The type containing the second handler method.</summary>
        public readonly Type DeclaringType2;
        /// <summary>The name of the second handler method.</summary>
        public readonly string HandlerMethodName2;

        /// <summary>Initializes a new <see cref="DuplicateHandlerException"/> instance with a specified error message.</summary>
        public DuplicateHandlerException() { }
        /// <summary>Initializes a new <see cref="DuplicateHandlerException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DuplicateHandlerException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="DuplicateHandlerException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public DuplicateHandlerException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="DuplicateHandlerException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="id">The message ID with multiple handler methods.</param>
        /// <param name="method1">The first handler method's info.</param>
        /// <param name="method2">The second handler method's info.</param>
        public DuplicateHandlerException(ushort id, MethodInfo method1, MethodInfo method2) : base(GetErrorMessage(id, method1, method2))
        {
            Id = id;
            DeclaringType1 = method1.DeclaringType;
            HandlerMethodName1 = method1.Name;
            DeclaringType2 = method2.DeclaringType;
            HandlerMethodName2 = method2.Name;
        }

        /// <summary>Constructs the error message from the given information.</summary>
        /// <returns>The error message.</returns>
        private static string GetErrorMessage(ushort id, MethodInfo method1, MethodInfo method2)
        {
            return $"Message handler methods '{method1.DeclaringType.Name}.{method1.Name}' and '{method2.DeclaringType.Name}.{method2.Name}' are both set to handle messages with ID {id}! Only one handler method is allowed per message ID!";
        }
    }

    /// <summary>The Exception that is thrown when a Type that has the <see cref="SerializableDataAttribute"/>, does not inherit from <see cref="IMessageSerializable"/>.</summary>
    public class SerializableInheritanceException : Exception
    {
        /// <summary>The Type which this exception is about.</summary>
        public readonly Type DeclaringType;

        /// <summary>Initializes a new <see cref="SerializableInheritanceException"/> instance with a specified error message.</summary>
        public SerializableInheritanceException() { }
        /// <summary>Initializes a new <see cref="SerializableInheritanceException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public SerializableInheritanceException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="SerializableInheritanceException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public SerializableInheritanceException(string message, Exception inner) : base(message, inner) { }

        /// <summary>Initializes a new <see cref="SerializableInheritanceException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="type">The Type which this exception is about.</param>
        public SerializableInheritanceException(Type type) : base(GetErrorMessage(type))
        {
            DeclaringType = type;
        }

        private static string GetErrorMessage(Type type)
        {
            return $"Type '{type.Name}' has the Riptide Serializable Attribute but does not inherit from IMessageSerializable";
        }
    }

    /// <summary>The Exception that is thrown when multple classes with <see cref="SerializableDataAttribute"/>s that have the same ID.</summary>
    public class DuplicateSerializableIdException : Exception
    {
        /// <summary>The ID with multiple types.</summary>
        public readonly ushort Id;
        /// <summary>The first type with the <see cref="SerializableDataAttribute"/>.</summary>
        public readonly Type DeclaringType1;
        /// <summary>The second type with the <see cref="SerializableDataAttribute"/>.</summary>
        public readonly Type DeclaringType2;

        /// <summary>Initializes a new <see cref="DuplicateSerializableIdException"/> instance with a specified error message.</summary>
        public DuplicateSerializableIdException() { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableIdException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DuplicateSerializableIdException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableIdException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public DuplicateSerializableIdException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableIdException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="id">The ID with multiple types.</param>
        /// <param name="declaringType1">The first type with the <see cref="SerializableDataAttribute"/>.</param>
        /// <param name="declaringType2">The second type with the <see cref="SerializableDataAttribute"/>.</param>
        public DuplicateSerializableIdException(ushort id, Type declaringType1, Type declaringType2) : base(GetErrorMessage(id, declaringType1, declaringType2))
        {
            Id = id;
            DeclaringType1 = declaringType1;
            DeclaringType2 = declaringType2;
        }

        private static string GetErrorMessage(ushort id, Type declaringType1, Type declaringType2)
        {
            return $"Types '{declaringType1.Name}' and '{declaringType2.Name}' both have declared the same ID {id}! Only one type is allowed per ID.";
        }
    }
    
    /// <summary>The Exception that is thrown when multple classes with <see cref="SerializableDataAttribute"/>s that have the same ID.</summary>
    public class DuplicateSerializableException : Exception
    {
        /// <summary>The Type that has multiple ids</summary>
        public readonly Type DeclaringType;
        /// <summary>The first id</summary>
        public readonly ushort Id1;
        /// <summary>The second id</summary>
        public readonly ushort Id2;

        /// <summary>Initializes a new <see cref="DuplicateSerializableException"/> instance with a specified error message.</summary>
        public DuplicateSerializableException() { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public DuplicateSerializableException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public DuplicateSerializableException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="DuplicateSerializableException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="declaringType">The Type that has multiple ids</param>
        /// <param name="id1">The first id</param>
        /// <param name="id2">The second id</param>
        public DuplicateSerializableException(Type declaringType, ushort id1, ushort id2) : base(GetErrorMessage(declaringType, id1, id2))
        {
            DeclaringType = declaringType;
            Id1 = id1;
            Id2 = id2;
        }

        private static string GetErrorMessage(Type declaringType, ushort id1, ushort id2)
        {
            return $"Id '{id1}' and '{id2}' both are declared on the same Type {declaringType.Name}. One Type can only have one Id.";
        }
    }

    /// <summary>The Exception that is thrown when an invalid Id is used in the lookup</summary>
    public class InvalidSerializableIdException : Exception
    {
        /// <summary>The invalid Id</summary>
        public readonly ushort Id;

        /// <summary>Initializes a new <see cref="InvalidSerializableIdException"/> instance with a specified error message.</summary>
        public InvalidSerializableIdException() { }
        /// <summary>Initializes a new <see cref="InvalidSerializableIdException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidSerializableIdException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="InvalidSerializableIdException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidSerializableIdException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="InvalidSerializableIdException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="id">The invalid Id</param>
        public InvalidSerializableIdException(ushort id) : base(GetErrorMessage(id))
        {
            Id = id;
        }

        private static string GetErrorMessage(ushort id)
        {
            return $"The ID {id} is not currently registered with a serializable.";
        }
    }
    
    /// <summary>The Exception that is thrown when an unknown serializable is used</summary>
    public class InvalidSerializableException : Exception
    {
        /// <summary>The invalid Type</summary>
        public readonly Type Type;

        /// <summary>Initializes a new <see cref="InvalidSerializableException"/> instance with a specified error message.</summary>
        public InvalidSerializableException() { }
        /// <summary>Initializes a new <see cref="InvalidSerializableException"/> instance with a specified error message.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidSerializableException(string message) : base(message) { }
        /// <summary>Initializes a new <see cref="InvalidSerializableException"/> instance with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. If <paramref name="inner"/> is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidSerializableException(string message, Exception inner) : base(message, inner) { }
        /// <summary>Initializes a new <see cref="InvalidSerializableException"/> instance and constructs an error message from the given information.</summary>
        /// <param name="type">The invalid Type</param>
        public InvalidSerializableException(Type type) : base(GetErrorMessage(type))
        {
            Type = type;
        }

        private static string GetErrorMessage(Type type)
        {
            return $"The Type '{type.Name}' is not a registered serializable.";
        }
    }
}
