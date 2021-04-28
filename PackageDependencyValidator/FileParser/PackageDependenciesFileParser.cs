using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidator.FileParser
{
	internal class PackageDependenciesFileParser : IPackageDependenciesFileParser
	{
		private readonly IFileSystem _fileSystem;
		private readonly IPackageInformationParser _packageInformationParser;

		public PackageDependenciesFileParser(IFileSystem fileSystem, IPackageInformationParser packageInformationParser)
		{
			_fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
			_packageInformationParser = packageInformationParser ?? throw new ArgumentNullException(nameof(packageInformationParser));
		}

		/// <inheritdoc/>
		public ApplicationPackageInformation Parse(string filePath)
		{
			_ = filePath ?? throw new ArgumentNullException(nameof(filePath));
			if (!_fileSystem.File.Exists(filePath)) throw new FileNotFoundException(filePath);

			using (var stream = _fileSystem.FileStream.Create(filePath, FileMode.Open))
			{
				using (var reader = new StreamReader(stream))
				{
					var packageCountLine = reader.ReadLine();
					var packageCount = _packageInformationParser.ParsePackageCount(packageCountLine);
					var packagesToInstall = new List<PackageDetails>();

					for (var i = 0; i < packageCount; i++)
					{
						var packageLine = reader.ReadLine();
						var package = _packageInformationParser.ParsePackageInformation(packageLine);
						if (packagesToInstall.Contains(package))
						{
							continue;
						}

						packagesToInstall.Add(package);
					}

					var dependenciesCountLine = reader.ReadLine();
					if (dependenciesCountLine == null)
					{
						return new ApplicationPackageInformation(packagesToInstall, new Dictionary<PackageDetails, IList<PackageDetails>>());
					}

					var dependenciesCount = _packageInformationParser.ParsePackageCount(dependenciesCountLine);
					var packageDependencies = new Dictionary<PackageDetails, IList<PackageDetails>>();

					for (var i = 0; i < dependenciesCount; i++)
					{
						var dependenciesLine = reader.ReadLine();
						var dependencies = _packageInformationParser.ParseDependenciesInformation(dependenciesLine);
						RegisterPackageDependency(packageDependencies, dependencies.Package, dependencies.Dependencies);
					}

					return new ApplicationPackageInformation(packagesToInstall, packageDependencies);
				}
			}
		}

		private static void RegisterPackageDependency(IDictionary<PackageDetails, IList<PackageDetails>> packageDependencies, PackageDetails package, IList<PackageDetails> dependencies)
		{
			if (!packageDependencies.ContainsKey(package))
			{
				packageDependencies.Add(package, new List<PackageDetails>());
			}

			foreach (var dependency in dependencies)
			{
				if (packageDependencies[package].Contains(dependency))
				{
					continue;
				}

				packageDependencies[package].Add(dependency);
			}
		}
	}
}