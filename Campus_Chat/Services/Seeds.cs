namespace Campus_Chat.Services;

/// <summary>Fixed IDs for seed data, so the user store and chat store agree on who is who.</summary>
public static class SeedIds
{
    public static readonly Guid Ola = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid Kari = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid Lecturer = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid Auditor = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static readonly Guid GeneralThread = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static readonly Guid PrivateThread = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
}