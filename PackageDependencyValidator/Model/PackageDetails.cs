using System;
using PackageDependencyValidator.Properties;

namespace PackageDependencyValidator.Model
{
	/// <summary>
	/// Describes a single package in the system.
	/// </summary>
	public class PackageDetails
	{
		/// <summary>
		/// Name of the package.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Version of the package.
		/// </summary>
		public string Version { get; }

		public PackageDetails(string name, string version)
		{
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Version = version ?? throw new ArgumentNullException(nameof(version));

			if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException(Resource.EmptyPackageName);
			if (string.IsNullOrWhiteSpace(Version)) throw new ArgumentException(Resource.EmptyPackageVersion);
		}

		public override bool Equals(object obj)
		{
			return obj is PackageDetails other && Name == other.Name && Version == other.Version;
		}
	}
}