using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace I18N
{
    public class JsonLocalizer
    {
        private readonly Dictionary<string, JsonLocalization[]> _localization
            = new Dictionary<string, JsonLocalization[]>();

        public JsonLocalizer(bool useBase = true, Dictionary<Type, string> additionalPaths = null)
        {
            if (useBase)
                PopulateLocalization("Resources");

            if (additionalPaths == null) return;
            foreach (var additional in additionalPaths)
            {
                var codeBase = additional.Key.Assembly.CodeBase;
                var uri = new UriBuilder(codeBase);
                var data = Uri.UnescapeDataString(uri.Path);
                var path = Path.GetDirectoryName(data);
                var fullPath = Path.Combine(path, additional.Value);
                PopulateLocalization(fullPath);
            }
        }

        /// <summary>
        /// resource:key:culture
        /// resource is the resource name
        /// key is the key you're looking for
        /// culture is optional
        /// </summary>
        /// <param name="key"></param>
        public string this[string key] => GetString(key);


        private void PopulateLocalization(string path)
        {
            foreach (var resource in Directory.GetFiles(path, "*.json", SearchOption.AllDirectories))
            {
                try
                {
                    var fileInfo = new FileInfo(resource);
                    var fileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf('.'));
                    var loc = JsonConvert.DeserializeObject<JsonLocalization[]>(File.ReadAllText(resource));
                    _localization.Add(fileName, loc);
                }
                catch (ArgumentException e)
                {
                    throw new I18NException($"Resource {resource} was already added, check your files.", e);
                }
                catch (Exception ex)
                {
                    throw new I18NException("Something wrong is not right, check inner exception", ex);
                }
            }
        }

        private string GetString(string query)
        {
            try
            {
                string culture = null;

                var split = query.Split(':');
                var resource = split[0];
                var key = split[1];
                if (split.Length > 2)
                    culture = split[2];

                culture = culture ?? CultureInfo.CurrentCulture.Name;

                return _localization
                    .Single(l => l.Key == resource)
                    .Value.Single(x => x.Key == key)
                    .LocalizedValues[culture];
            }
            catch (Exception ex)
            {
                throw new I18NException($"Couldn't find key: {query}", ex);
            }
        }
    }
}