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
    }
    // ReSharper restore UnusedMemberInSuper.Global
}