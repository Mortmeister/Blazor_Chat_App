using System.Text.RegularExpressions;

namespace Campus_Chat;

/// <summary>Small helpers for how things look. No data access and no rules.</summary>
public static class UiHelpers
{
    private static readonly string[] Palette = ["#FF735C", "#6456E8", "#13A58D", "#E7A830", "#2A90D9"];

    /// <summary>A stable accent colour for a thread, so the same thread always looks the same.</summary>
    public static string Accent(Guid id) => Palette[id.ToByteArray()[0] % Palette.Length];

    /// <summary>Up to two letters from a name, for example "Ola Nordmann" gives "ON".</summary>
    /// <returns>"?" when the name is empty.</returns>
    public static string Initials(string? name)
    {
        var parts = (name ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length switch
        {
            0 => "?",
            1 => parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant(),
            _ => $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
        };
    }

    /// <summary>The avatar background: the user's chosen colour, or grey.</summary>
    /// <remarks>The value ends up in a style attribute, so only a hex colour or a plain colour name is accepted.
    /// Anything else, such as "red; position: fixed", falls back to grey.</remarks>
    public static string AvatarBackground(string? colour)
    {
        var value = colour?.Trim() ?? "";
        var safe = Regex.IsMatch(value, "^#[0-9a-fA-F]{3,8}$") || Regex.IsMatch(value, "^[a-zA-Z]{3,20}$");
        return safe ? value : "#6C757D";
    }
}