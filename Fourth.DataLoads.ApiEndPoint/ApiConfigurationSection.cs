namespace Fourth.DataLoads.ApiEndPoint
{
    using System.Configuration;

    /// <remarks>
    /// Configuration settings should be implemented through a configuration section rather than app settings.
    /// This gives the benefit of strong typing, default values and better documentation.
    /// It also makes it easier to inject different configurations into tests.
    /// </remarks>
    public sealed class ApiConfigurationSection : ConfigurationSection
    {
        /// <summary> The configuration section name.</summary>
        public const string SectionName = "apiConfig";

        /// <summary> The valid options for authorization mode, i.e. check the header or just allow every request.</summary>
        public enum AuthorizationMode
        {
            Enabled,
            Disabled
        }

        /// <summary>
        /// Gets or sets the type of authorization to use.
        /// </summary>
        [ConfigurationProperty("authorizationMode", IsRequired = false, DefaultValue = AuthorizationMode.Enabled)]
        public AuthorizationMode Authorization
        {
            get
            {
                return (AuthorizationMode)this["authorizationMode"];
            }

            set
            {
                this["authorizationMode"] = value;
            }
        }

        /// <summary>
        /// Loads the section from configuration.
        /// </summary>
        /// <returns>The section in the application configuration file.</returns>
        public static ApiConfigurationSection GetConfig()
        {
            var config = ConfigurationManager.GetSection(ApiConfigurationSection.SectionName) as ApiConfigurationSection;
            if (config == null)
            {
                throw new ConfigurationErrorsException(string.Format("Could not find the configuration section \"{0}\".", ApiConfigurationSection.SectionName));
            }
            return config;
        }
    }
}