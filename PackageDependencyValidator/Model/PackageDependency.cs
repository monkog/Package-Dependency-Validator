using System;
using System.Collections.Generic;

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
		/// Definition of a package dependencies.
		/// </summary>
		public IList<PackageDetails> Dependencies { get; }

		public PackageDependency(PackageDetails package, params PackageDetails[] dependencies)
		{
			Package = package?? throw new ArgumentNullException(nameof(package));
			Dependencies = dependencies?? throw new ArgumentNullException(nameof(dependencies));
		}
	}
}