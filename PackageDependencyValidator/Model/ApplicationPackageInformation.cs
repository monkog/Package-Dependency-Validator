using System;
using System.Collections.Generic;

namespace PackageDependencyValidator.Model
{
	/// <summary>
	/// System package dependencies model.
	/// </summary>
	public class ApplicationPackageInformation
	{
		/// <summary>
		/// Collection of packages to install.
		/// </summary>
		public IEnumerable<PackageDetails> PackagesToInstall { get; }

		/// <summary>
		/// Collection of packages together with their dependencies.
		/// </summary>
		public Dictionary<PackageDetails, IList<PackageDetails>> PackageDependencies { get; }

		public ApplicationPackageInformation(IEnumerable<PackageDetails> packagesToInstall, Dictionary<PackageDetails, IList<PackageDetails>> packageDependencies)
		{
			PackagesToInstall = packagesToInstall?? throw new ArgumentNullException(nameof(packagesToInstall));
			PackageDependencies = packageDependencies?? throw new ArgumentNullException(nameof(packageDependencies));
		}
	}
}