using System.Collections.Generic;
using System.Linq;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidator.Validators
{
	public class DependenciesValidator : IDependenciesValidator
	{
		/// <inheritdoc/>
		public bool AreDependenciesValid(ApplicationPackageInformation packageInformation)
		{
			var packagesToInstall = packageInformation.PackagesToInstall.Distinct().ToList();
			var distinctPackages = new Dictionary<string, string>();

			for (var i = 0; i < packagesToInstall.Count; i++)
			{
				var packageToInstall = packagesToInstall.ElementAt(i);
				if (!distinctPackages.ContainsKey(packageToInstall.Name))
				{
					distinctPackages.Add(packageToInstall.Name, packageToInstall.Version);
				}
				else
				{
					var existingVersion = distinctPackages[packageToInstall.Name];
					if (existingVersion != packageToInstall.Version)
					{
						return false;
					}
				}

				if (!packageInformation.PackageDependencies.ContainsKey(packageToInstall))
				{
					continue;
				}

				var dependencies = packageInformation.PackageDependencies[packageToInstall];
				foreach (var dependency in dependencies)
				{
					if (packagesToInstall.Contains(dependency))
					{
						continue;
					}

					packagesToInstall.Add(dependency);
				}
			}

			return true;
		}
	}
}