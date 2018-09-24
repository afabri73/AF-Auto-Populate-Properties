using AF.AutoPopulateProperties.Constants;
using umbraco.businesslogic;
using umbraco.interfaces;

namespace AF.AutoPopulateProperties.Applications
{
    /// <summary>
    /// Application(APPConstants.Application.Alias, APPConstants.Application.Name, APPConstants.Application.Icon, 50)
    /// HkApplication
    /// </summary>
    [Application(APPConstants.Application.Alias, APPConstants.Application.Name, APPConstants.Application.Icon, 50)]
    public class APPApplication : IApplication
    { }
}