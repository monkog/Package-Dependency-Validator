using System;
using System.Collections.Generic;
using System.Linq;
using PackageDependencyValidator.Model;
using PackageDependencyValidator.Properties;

namespace PackageDependencyValidator.FileParser
{
	internal class PackageInformationParser : IPackageInformationParser
	{
		/// <inheritdoc/>
		public int ParsePackageCount(string value)
		{
			_ = value ?? throw new ArgumentNullException(nameof(value));

			if (!int.TryParse(value, out var packageCount))
			{
				throw new FormatException(string.Format(Resource.InvalidPackageNumber, value));
			}

			return packageCount;
		}

		/// <inheritdoc/>
		public PackageDetails ParsePackageInformation(string value)
		{
			return ParsePackageInformationLine(value, true).Single();
		}

		/// <inheritdoc/>
		public PackageDependency ParseDependenciesInformation(string value)
		{
			var packages = ParsePackageInformationLine(value, false).ToArray();
			return new PackageDependency(packages[0], packages.Skip(1).ToArray());
		}

		private static IEnumerable<PackageDetails> ParsePackageInformationLine(string value, bool isSinglePackage)
		{
			const char separator = ',';
			var lineParts = value.Split(separator);
			if (!HasValidNumberOfElements(lineParts, isSinglePackage)) throw new FormatException(string.Format(Resource.InvalidPackageDescription, value));
			var packagesCount = lineParts.Length / 2;

			var packageInformation = new List<PackageDetails>();
			for (var i = 0; i < packagesCount; i++)
			{
				var packageName = lineParts[2 * i];
				var packageVersion = lineParts[2 * i + 1];

				packageInformation.Add(new PackageDetails(packageName, packageVersion));
			}

			return packageInformation;
		}

		private static bool HasValidNumberOfElements(IReadOnlyCollection<string> collection, bool isSinglePackage)
		{
			return isSinglePackage ? collection.Count == 2 : collection.Count > 2 && collection.Count % 2 == 0;
		}
	}
}