using System.Diagnostics;

namespace QuestBooks.Common.Testing.UI.Reload;

[Conditional("DEBUG")]
[AttributeUsage(AttributeTargets.Class)]
public sealed class TestingInterfaceReloadAttribute : Attribute;