using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QuestBooks.Utilities;

#nullable enable
public class JsonTypeResolverFix : ISerializationBinder
{
    /* -----------------------------------
     *
     * This is the EXACT same as the normal serialization binder,
     * except it calls Assembly.Load() all the time.
     *
     * Using the default logic does not work due to duplicate ASMs.
     *
     * ----------------------------------- */
    public static JsonSerializerSettings Settings => new()
    {
        TypeNameHandling = TypeNameHandling.All,
        SerializationBinder = new JsonTypeResolverFix(),
        PreserveReferencesHandling = PreserveReferencesHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        MaxDepth = null
    };

    private readonly ThreadSafeStore<StructMultiKey<string?, string>, Type> _typeCache;

    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonTypeResolverFix" /> class.
    /// </summary>
    public JsonTypeResolverFix()
    {
        _typeCache = new ThreadSafeStore<StructMultiKey<string?, string>, Type>(GetTypeFromTypeNameKey);
    }

    private Type GetTypeFromTypeNameKey(StructMultiKey<string?, string> typeNameKey)
    {
        var assemblyName = typeNameKey.Value1;
        var typeName = typeNameKey.Value2;

        if (assemblyName != null)
        {
            var assembly = Assembly.Load(assemblyName) ?? throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, assemblyName));
            var type = assembly.GetType(typeName);

            if (type == null)
            {
                // if generic type, try manually parsing the type arguments for the case of dynamically loaded assemblies
                // example generic typeName format: System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]
                if (typeName.Contains('`'))
                {
                    try
                    {
                        type = GetGenericTypeFromTypeName(typeName, assembly);
                    }
                    catch (Exception ex)
                    {
                        throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName), ex);
                    }
                }

                if (type == null)
                {
                    throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
                }
            }

            return type;
        }

        return Type.GetType(typeName)!;
    }

    private Type? GetGenericTypeFromTypeName(string typeName, Assembly assembly)
    {
        Type? type = null;
        var openBracketIndex = typeName.IndexOf('[');

        if (openBracketIndex >= 0)
        {
            var genericTypeDefName = typeName.Substring(0, openBracketIndex);
            var genericTypeDef = assembly.GetType(genericTypeDefName);

            if (genericTypeDef != null)
            {
                var genericTypeArguments = new List<Type>();
                var scope = 0;
                var typeArgStartIndex = 0;
                var endIndex = typeName.Length - 1;

                for (var i = openBracketIndex + 1; i < endIndex; ++i)
                {
                    var current = typeName[i];

                    switch (current)
                    {
                        case '[':
                            if (scope == 0)
                            {
                                typeArgStartIndex = i + 1;
                            }

                            ++scope;
                            break;
                        case ']':
                            --scope;

                            if (scope == 0)
                            {
                                var typeArgAssemblyQualifiedName = typeName.Substring(typeArgStartIndex, i - typeArgStartIndex);

                                var typeNameKey = SplitFullyQualifiedTypeName(typeArgAssemblyQualifiedName);
                                genericTypeArguments.Add(GetTypeByName(typeNameKey));
                            }

                            break;
                    }
                }

                type = genericTypeDef.MakeGenericType(genericTypeArguments.ToArray());
            }
        }

        return type;
    }

    private Type GetTypeByName(StructMultiKey<string?, string> typeNameKey) => _typeCache.Get(typeNameKey)!;

    /// <summary>
    ///     When overridden in a derived class, controls the binding of a serialized object to a type.
    /// </summary>
    /// <param name="assemblyName">Specifies the <see cref="Assembly" /> name of the serialized object.</param>
    /// <param name="typeName">Specifies the <see cref="System.Type" /> name of the serialized object.</param>
    /// <returns>
    ///     The type of the object the formatter creates a new instance of.
    /// </returns>
    public Type BindToType(string? assemblyName, string typeName) => GetTypeByName(new StructMultiKey<string?, string>(assemblyName, typeName));

    /// <summary>
    ///     When overridden in a derived class, controls the binding of a serialized object to a type.
    /// </summary>
    /// <param name="serializedType">The type of the object the formatter creates a new instance of.</param>
    /// <param name="assemblyName">Specifies the <see cref="Assembly" /> name of the serialized object.</param>
    /// <param name="typeName">Specifies the <see cref="System.Type" /> name of the serialized object.</param>
    public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
    {
        assemblyName = serializedType.GetTypeInfo().Assembly.FullName;
        typeName = serializedType.FullName;
    }

    #region Normally Internal Types

    private class ThreadSafeStore<TKey, TValue> where TKey : notnull
    {
        private readonly ConcurrentDictionary<TKey, TValue> _concurrentStore;

        private readonly Func<TKey, TValue> _creator;

        public ThreadSafeStore(Func<TKey, TValue> creator)
        {
            ArgumentNullException.ThrowIfNull(creator);

            _creator = creator;
            _concurrentStore = new ConcurrentDictionary<TKey, TValue>();
        }

        public TValue? Get(TKey key) => _concurrentStore.GetOrAdd(key, _creator);
    }

    private readonly struct StructMultiKey<T1, T2>(T1 v1, T2 v2) : IEquatable<StructMultiKey<T1, T2>>
    {
        public readonly T1 Value1 = v1;
        public readonly T2 Value2 = v2;

        public override int GetHashCode() => (Value1?.GetHashCode() ?? 0) ^ (Value2?.GetHashCode() ?? 0);

        public override bool Equals(object? obj)
        {
            if (obj is not StructMultiKey<T1, T2> key)
            {
                return false;
            }

            return Equals(key);
        }

        public bool Equals(StructMultiKey<T1, T2> other) => Equals(Value1, other.Value1) && Equals(Value2, other.Value2);
    }

    private static StructMultiKey<string?, string> SplitFullyQualifiedTypeName(string fullyQualifiedTypeName)
    {
        var assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);

        string typeName;
        string? assemblyName;

        if (assemblyDelimiterIndex != null)
        {
            typeName = fullyQualifiedTypeName[..assemblyDelimiterIndex.GetValueOrDefault()];
            assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.GetValueOrDefault() + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.GetValueOrDefault() - 1);
        }
        else
        {
            typeName = fullyQualifiedTypeName;
            assemblyName = null;
        }

        return new StructMultiKey<string?, string>(assemblyName, typeName);
    }

    private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
    {
        // we need to get the first comma following all surrounded in brackets because of generic types
        // e.g. System.Collections.Generic.Dictionary`2[[System.String, mscorlib,Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        var scope = 0;

        for (var i = 0; i < fullyQualifiedTypeName.Length; i++)
        {
            var current = fullyQualifiedTypeName[i];

            switch (current)
            {
                case '[':
                    scope++;
                    break;
                case ']':
                    scope--;
                    break;
                case ',':
                    if (scope == 0)
                    {
                        return i;
                    }

                    break;
            }
        }

        return null;
    }

    #endregion
}