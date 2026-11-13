namespace Wip.Persistence;

public sealed class Module
{
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string GroupKey { get; set; } = string.Empty;

    public int SortOrder { get; set; }
}
