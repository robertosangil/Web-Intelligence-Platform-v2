namespace Wip.Api.Contracts.V1;

public sealed record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
public sealed record CloudflareDailyRead(string RecordKey, string? ZoneId, string? ZoneName, string? ReportDateKey, long? TotalRequests, long? TotalBandwidthBytes, string? CollectionStatus, string? DataQualityStatus);
public sealed record GoDaddyDomainRead(string DomainKey, string? Domain, string? Status, DateTime? ExpiresAtUtc, bool? AutoRenew);
public sealed record CPanelResourceRead(string ResourceKey, string? Account, string? ResourceName, decimal? CurrentValue, decimal? LimitValue, string? Unit, string? Status);
public sealed record WpUmbrellaProjectRead(string ProjectKey, string? SiteId, string? SiteName, string? ProjectId, string? ProjectName, string? Status);
