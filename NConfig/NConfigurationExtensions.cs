using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NConfig
{
    /// <summary>
    /// Provides a set of static extension methods for INConfiguration interface.
    /// </summary>
    public static class NConfigurationExtensions
    {
        /// <summary>
        /// Enables NConfig files inheritance: 
        /// for instance, "Prod.Basisprod.Basic.config" file will inherite values from "Basisprod.Basic.config" 
        /// and file "Basisprod.Basic.config"  - from "Basic.config" an so on.
        /// </summary>
        /// <param name="configuration">The INConfiguration instance.</param>
        /// <returns></returns>
        public static INConfiguration EnableFilesInheritance(this INConfiguration configuration)
        {
            IList<string> fileNames = configuration.FileNames;
            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] parentFileNames = GetParentFileNames(fileNames[i]);
                foreach (string item in parentFileNames)
                {
                    if (fileNames.IndexOf(item) == -1)
                    {
                        fileNames.Insert(++i, item);
                    }
                }
            }

            return configuration;
        }

        private static string[] GetParentFileNames(string fileName)
        {
            string directoryName = Path.GetDirectoryName(fileName);
            string extension = Path.GetExtension(fileName);
            string[] array = Path.GetFileNameWithoutExtension(fileName).Split('.');
            if (array.Length < 2)
            {
                return new string[0];
            }

            string[] array2 = new string[array.Length - 1];
            for (int i = 1; i < array.Length; i++)
            {
                array2[i - 1] = Path.Combine(directoryName, string.Join(".", array.Skip(i)) + extension);
            }

            return array2;
        }
    }
}
