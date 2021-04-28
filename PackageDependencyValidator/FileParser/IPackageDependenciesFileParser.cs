using PackageDependencyValidator.Model;

namespace PackageDependencyValidator.FileParser
{
	internal interface IPackageDependenciesFileParser
	{
		/// <summary>
		///  Parses a file with the given file name and returns a model representation of the package dependencies.
		/// </summary>
		/// <param name="filePath">Path to the text file.</param>
		/// <returns>Model representation of the file content.</returns>
		ApplicationPackageInformation Parse(string filePath);
	}
}