using System;

using CMS.SettingsProvider;

/// <summary>
/// Translation services functions loader (registers translation services functions to macro resolver).
/// </summary>
[TranslationServicesModuleLoader]
public partial class CMSModuleLoader
{
    /// <summary>
    /// Attribute class ensuring correct initialization of methods in macro resolver.
    /// </summary>
    public class TranslationServicesModuleLoaderAttribute : CMSLoaderAttribute
    {
        /// <summary>
        /// Registers translation services methods.
        /// </summary>
        public override void Init()
        {
            TranslationServicesMethods.RegisterMethods();
        }
    }
}