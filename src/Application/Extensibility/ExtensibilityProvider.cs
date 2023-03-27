using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Application.Interfaces;

namespace Application.Extensibility;

/// <summary>
/// This class is responsible for providing Dependency Injection capabilities.
/// </summary>
public class ExtensibilityProvider
{
    /// <summary>
    /// This property is used to import the ITimeSeriesService implementation.
    /// </summary>
    [Import(typeof(ITimeSeriesService))]
    private ITimeSeriesService _timeSeriesService;

    /// <summary>
    /// This property is used to expose the ITimeSeriesService implementation.
    /// </summary>
    public ITimeSeriesService TimeSeriesService => _timeSeriesService;

    /// <summary>
    /// This constructor is responsible for initializing the CompositionContainer.
    /// </summary>
    public ExtensibilityProvider()
    {
        try
        {
            // An aggregate catalog that combines multiple catalogs.
            var catalog = new AggregateCatalog();
            // Adds all the parts found in the same assembly as the Program class.
            catalog.Catalogs.Add(new DirectoryCatalog(AppContext.BaseDirectory));
            // Create the CompositionContainer with the parts in the catalog.
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
        catch (CompositionException compositionException)
        {
            Console.WriteLine(compositionException.ToString());
        }
    }
}
