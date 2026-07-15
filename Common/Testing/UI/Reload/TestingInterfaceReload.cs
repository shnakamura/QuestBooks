#if DEBUG
using System.Reflection;
using System.Reflection.Metadata;
using QuestBooks.Common.Testing.UI.Reload;

[assembly: MetadataUpdateHandler(typeof(TestingInterfaceReload))]

namespace QuestBooks.Common.Testing.UI.Reload;

#nullable enable

internal static class TestingInterfaceReload
{
    internal static void ClearCache(Type[]? types)
    {
        if (types == null)
        {
            return;
        }

        foreach (Type type in types)
        {
            if (type.GetCustomAttribute<TestingInterfaceReloadAttribute>() == null)
            {
                continue;
            }
            
            TestingInterfaceSystem.UserInterface.SetState(null);
        }
    }
    
    internal static void UpdateApplication(Type[]? types)
    {
        if (types == null)
        {
            return;
        }

        foreach (Type type in types)
        {
            if (type.GetCustomAttribute<TestingInterfaceReloadAttribute>() == null)
            {
                continue;
            }
            
            TestingInterfaceSystem.UserInterface.SetState(new TestingState());
        }
    }
}
#endif