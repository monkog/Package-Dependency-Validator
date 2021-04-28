using PackageDependencyValidator.Model;

namespace PackageDependencyValidator.Validators
{
	public interface IDependenciesValidator
	{
		/// <summary>
		/// Determines whether the dependencies are valid for the provided package setup.
		/// </summary>
		/// <param name="packageInformation">Model of package dependencies in the system.</param>
		/// <returns>True if the setup is valid, false otherwise.</returns>
		bool AreDependenciesValid(ApplicationPackageInformation packageInformation);
	}
}