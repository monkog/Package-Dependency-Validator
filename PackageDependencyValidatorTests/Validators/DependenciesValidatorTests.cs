using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.Model;
using PackageDependencyValidator.Validators;

namespace PackageDependencyValidatorTests.Validators
{
	[TestClass]
	public class DependenciesValidatorTests
	{
		private DependenciesValidator _unitUnderTest;
		private PackageDetails _package;
		private PackageDetails _dependency;

		[TestInitialize]
		public void Initialize()
		{
			_package = new PackageDetails("package", "1");
			_dependency = new PackageDetails("dependency", "2");
			_unitUnderTest = new DependenciesValidator();
		}

		[TestMethod]
		public void AreDependenciesValid_DuplicatedPackagesNoDependencies_True()
		{
			// Arrange
			var packageInformation = new ApplicationPackageInformation(new[] { _package }, new Dictionary<PackageDetails, IList<PackageDetails>>());

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AreDependenciesValid_NoDependencies_True()
		{
			// Arrange
			var packageInformation = new ApplicationPackageInformation(new[] { _package, _package }, new Dictionary<PackageDetails, IList<PackageDetails>>());

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AreDependenciesValid_NoDependenciesSamePackageInTwoVersions_False()
		{
			// Arrange
			var differentVersion = new PackageDetails(_package.Name, "different-version");
			var packageInformation = new ApplicationPackageInformation(new[] { _package, differentVersion }, new Dictionary<PackageDetails, IList<PackageDetails>>());

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void AreDependenciesValid_PackageSameAsItsDependency_False()
		{
			// Arrange
			var dependencies = new Dictionary<PackageDetails, IList<PackageDetails>> { { _package, new List<PackageDetails> { _package } } };
			var packageInformation = new ApplicationPackageInformation(new[] { _package }, dependencies);

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void AreDependenciesValid_PackageWithDependencies_True()
		{
			// Arrange
			var dependencies = new Dictionary<PackageDetails, IList<PackageDetails>> { { _package, new List<PackageDetails> { _dependency } } };
			var packageInformation = new ApplicationPackageInformation(new[] { _package }, dependencies);

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AreDependenciesValid_RedundantDependencies_True()
		{
			// Arrange
			var redundantDependency = new PackageDetails("not-used", "not-used");
			var dependencies = new Dictionary<PackageDetails, IList<PackageDetails>>
			{
				{ _package, new List<PackageDetails> { _dependency } },
				{ redundantDependency, new List<PackageDetails> { _package, _dependency } }
			};
			var packageInformation = new ApplicationPackageInformation(new[] { _package }, dependencies);

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AreDependenciesValid_MultiplePackagesWithTheSameDependency_True()
		{
			// Arrange
			var somePackage = new PackageDetails("some-package", "some-package");
			var dependencies = new Dictionary<PackageDetails, IList<PackageDetails>>
			{
				{ _package, new List<PackageDetails> { _dependency } },
				{ somePackage, new List<PackageDetails> { _package, _dependency } }
			};
			var packageInformation = new ApplicationPackageInformation(new[] { _package }, dependencies);

			// Act
			var result = _unitUnderTest.AreDependenciesValid(packageInformation);

			// Assert
			Assert.IsTrue(result);
		}
	}
}