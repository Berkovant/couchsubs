// SPDX-License-Identifier: AGPL-3.0-only

using CouchSubs.Core;

namespace CouchSubs.Shell.Tests;

public class SubtitleAreaTests
{
    [Fact]
    public void AdditionalAreasAreCoverOnlyAndLimitedToFourTotal()
    {
        var settings = new UserSettings();
        Assert.Empty(settings.ActiveSubtitleAreas);
        for (var i = 0; i < 5; i++) settings = settings.AddSubtitleArea();
        Assert.Equal(3, settings.SubtitleAreas.Count);
        Assert.All(settings.ActiveSubtitleAreas, area => Assert.Equal(AreaPreset.Top, area.Preset));

        var custom = new CropRegion(0.2, 0.4, 0.1, 0.9);
        settings = settings.SetSubtitleArea(1, new SubtitleArea(custom, AreaPreset.Custom));
        Assert.Equal(custom, settings.ActiveSubtitleAreas[1].Region);
        settings = settings with { Style = settings.Style with { Placement = SubtitlePlacement.Above } };
        Assert.Empty(settings.ActiveSubtitleAreas);
        settings = settings with { Style = settings.Style with { Placement = SubtitlePlacement.Cover } };
        Assert.Equal(custom, settings.ActiveSubtitleAreas[1].Region);
        settings = settings.RemoveSubtitleArea(1);
        Assert.Equal(2, settings.ActiveSubtitleAreas.Count);
        var promoted = settings.ActiveSubtitleAreas[0];
        settings = settings.RemoveSubtitleRegion(0);
        Assert.Equal(promoted.Region, settings.Region);
        Assert.Equal(promoted.Preset, settings.AreaPreset);
        Assert.Single(settings.ActiveSubtitleAreas);
    }
}
