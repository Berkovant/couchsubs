// SPDX-License-Identifier: AGPL-3.0-only

using CouchSubs.Core;

namespace CouchSubs.Shell;

/// <summary>Her ek altyazı hattının gösterimini ana satırdan ve diğer bölgelerden ayrı tutar.</summary>
internal sealed class SubtitleAreaSender(IRegionSender sender, int number) : ISubtitleSender
{
    private readonly string id = $"subtitle-area-{number}";

    public void SendSubtitle(string text, int durationMs = 15000, TextBox? box = null, bool paragraphs = false, bool replaces = false) =>
        sender.SendRegion(id, text, box);

    public void Clear() => sender.SendRegion(id, "", null);
}
