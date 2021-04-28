using System;

namespace PackageDependencyValidator.Model
{
	/// <summary>
	/// Describes a package dependency.
	/// </summary>
	public class PackageDependency
	{
		/// <summary>
		/// Definition of a package.
		/// </summary>
		public PackageDetails Package { get; }

		/// <summary>
		/// Definition of a package dependency.
		/// </summary>
		public PackageDetails Dependency { get; }

		public PackageDependency(PackageDetails package, PackageDetails dependency)
		{
			Package = package?? throw new ArgumentNullException(nameof(package));
			Dependency = dependency?? throw new ArgumentNullException(nameof(dependency));
		}
	}
}