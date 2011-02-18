namespace Cradiator.Config
{
    /// <summary>
    /// settings for a certain 'view' or group of pojects that belong together
    /// or somenone wants to see in 1 screen/ view
    /// </summary>
    // ReSharper disable UnusedMemberInSuper.Global
    public interface IViewSettings
    {
        string URL { get; set; }
        string SkinName { get; set; }
        string ProjectNameRegEx { get; set; }
        string CategoryRegEx { get; set; }
        string ServerNameRegEx { get; set; }
        string ViewName { get; set; }
        bool ShowOnlyBroken { get; set; }
        bool ShowServerName { get; set; }

        //extended settings
        bool ShowOutOfDate { get; set; }
        int OutOfDateDifferenceInMinutes { get; set; }

    }
    // ReSharper restore UnusedMemberInSuper.Global
}