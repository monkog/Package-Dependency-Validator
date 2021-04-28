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
		public int Version { get; }

		public PackageDetails(string name, string version)
		{
			_ = name ?? throw new ArgumentNullException(nameof(name));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(Resource.EmptyPackageName);

			if (!int.TryParse(version, out var parsedVersion))
			{
				throw new FormatException(string.Format(Resource.WrongPackageVersionFormat, parsedVersion));
			}

			if (parsedVersion < 1) throw new ArgumentException(string.Format(Resource.InvalidPackageVersion, version));

			Name = name;
			Version = parsedVersion;
		}

		public override bool Equals(object obj)
		{
			return obj is PackageDetails other && Name == other.Name && Version == other.Version;
		}
	}
}