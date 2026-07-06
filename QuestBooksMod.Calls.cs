namespace QuestBooks;

public sealed partial class QuestBooksMod
{
    /// <summary>
    ///     Provides the commands supported by <see cref="QuestBooksMod.Call(object[])"/>.
    /// </summary>
    public static class Commands
    {
        /// <summary>
        ///     The command to mark a quest as complete.
        /// </summary>
        public const string Complete = "markcomplete";
        
        /// <summary>
        ///     The command to mark a quest as incomplete.
        /// </summary>
        public const string Incomplete  = "markincomplete";
    }
    
    public override object Call(params object[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var command = (string)args[0];
        var quest = (string)args[1];
        
        switch (command)
        {
            case Commands.Complete:
                MarkComplete(quest);
                break;
            case Commands.Incomplete:
                MarkIncomplete(quest);
                break;
            default:
                throw new ArgumentException($"Unknown command: {command}", nameof(args));
        }

        return null;
    }
}