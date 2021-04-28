using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
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
						packagesToInstall.Add(package);
					}

					var dependenciesCount = _packageInformationParser.ParsePackageCount(packageCountLine);
					var packageDependencies = new Dictionary<PackageDetails, IList<PackageDetails>>();

					for (var i = 0; i < dependenciesCount; i++)
					{
						var dependenciesLine = reader.ReadLine();
						var dependencies = _packageInformationParser.ParseDependenciesInformation(dependenciesLine);
						RegisterPackageDependency(packageDependencies, dependencies.Package, dependencies.Dependency);
					}

					return new ApplicationPackageInformation(packagesToInstall, packageDependencies);
				}
			}
		}

		private static void RegisterPackageDependency(IDictionary<PackageDetails, IList<PackageDetails>> packageDependencies, PackageDetails package, PackageDetails dependency)
		{
			if (!packageDependencies.ContainsKey(package))
			{
				packageDependencies.Add(package, new List<PackageDetails>());
			}

			packageDependencies[package].Add(dependency);
		}
	}
}