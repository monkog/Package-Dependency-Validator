using PackageDependencyValidator.Model;

namespace PackageDependencyValidator.FileParser
{
	public interface IPackageInformationParser
	{
		/// <summary>
		/// Parses the package count.
		/// </summary>
		/// <param name="value">String value to parse.</param>
		/// <returns>Package count.</returns>
		int ParsePackageCount(string value);

		/// <summary>
		/// Parses the package information form the provided input.
		/// </summary>
		/// <param name="value">String value to parse.</param>
		/// <returns>Model representation of the package.</returns>
		PackageDetails ParsePackageInformation(string value);

		/// <summary>
		/// Parses the dependencies for a package.
		/// </summary>
		/// <param name="value">String value to parse.</param>
		/// <returns>Description of a package dependency.</returns>
		PackageDependency ParseDependenciesInformation(string value);
	}
}