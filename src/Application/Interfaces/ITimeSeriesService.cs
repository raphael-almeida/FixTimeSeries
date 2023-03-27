namespace Application.Interfaces;

/// <summary>
/// Interface for the TimeSeriesService
/// </summary>
public interface ITimeSeriesService
{
    /// <summary>
    /// Runs the TimeSeriesService and orchestrates application modules
    /// </summary>
    void Run(string path);
}
