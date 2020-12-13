using System;
using System.IO;
using System.Reflection;

namespace Utilities
{
    public class UtilityHelper
    {
        /// <summary>
        /// Read the content of the Embedded Resource file.
        /// </summary>
        /// <param name="resource">Location/Path of the Embedded Resource</param>
        /// <returns>return content of the Embedded Resource file as <see cref="System.String"/>.</returns>
        public static string GetEmbeddedResource(string resource)
        {
            string content;
            //Retrieve the embedded resource file and convert to stream.
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream stream = assembly.GetManifestResourceStream(resource);

            if (stream == null)
                throw new ArgumentException("Resource Location/Path is not valid or Resource is not Embedded Resource.", nameof(resource));

            // Read the content of the embedded file, then close it.
            using (StreamReader streamReader = new StreamReader(stream))
            {
                content = streamReader.ReadToEnd();
            }

            return content;
        }
    }
}
