namespace Mbiza.NinetyOne.TopScorers.Application.Commom
{
    public static class Validations
    {
        static string[] allowedExtensions = [".csv"];

        /// <summary>
        /// Determines whether the specified file name has an allowed file extension.
        /// </summary>
        /// <remarks>The comparison is case-insensitive. Only the file extension is evaluated; the file
        /// does not need to exist.</remarks>
        /// <param name="fileName">The name of the file to check, including its extension. Cannot be null.</param>
        /// <returns>true if the file name has an allowed extension; otherwise, false.</returns>
        public static bool IsValidExtension(string fileName)
        {
            string? fileExtension = Path.GetExtension(fileName)?.ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
