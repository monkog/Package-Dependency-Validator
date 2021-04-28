using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.Model
{
	[TestClass]
	public class ApplicationPackageInformationTests
	{
		private IEnumerable<PackageDetails> _packages;
		private Dictionary<PackageDetails, IList<PackageDetails>> _dependencies;

		[TestInitialize]
		public void Initialize()
		{
			const int validVersion = 1;
			var package = new PackageDetails("package", validVersion.ToString());
			var dependency = new PackageDetails("dependency", validVersion.ToString());

			_packages = new[] { package };
			_dependencies = new Dictionary<PackageDetails, IList<PackageDetails>> { { package, new[] { dependency } } };
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullPackagesCollection_ArgumentNullException()
		{
			// Act
			_ = new ApplicationPackageInformation(null, _dependencies);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullDependenciesCollection_ArgumentNullException()
		{
			// Act
			_ = new ApplicationPackageInformation(_packages, null);
		}

		[TestMethod]
		public void Ctor_ValidArguments_PropertiesAssigned()
		{
			// Act
			var result = new ApplicationPackageInformation(_packages, _dependencies);

			// Assert
			Assert.IsNotNull(result);
			CollectionAssert.AreEquivalent(_packages.ToList(), result.PackagesToInstall.ToList());
			CollectionAssert.AreEquivalent(_dependencies.ToList(), result.PackageDependencies.ToList());
		}
	}
}