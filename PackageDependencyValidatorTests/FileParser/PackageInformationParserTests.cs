using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.FileParser;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.FileParser
{
	[TestClass]
	public class PackageInformationParserTests
	{
		private PackageInformationParser _unitUnderTest;

		[TestInitialize]
		public void Initialize()
		{
			_unitUnderTest = new PackageInformationParser();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ParsePackageCount_NullValue_ArgumentNullException()
		{
			// Act
			_unitUnderTest.ParsePackageCount(null);
		}

		[TestMethod]
		[ExpectedException(typeof(FormatException))]
		public void ParsePackageCount_NonIntegerValue_FormatException()
		{
			// Act
			_unitUnderTest.ParsePackageCount("version");
		}

		[TestMethod]
		public void ParsePackageCount_IntegerValue_ParsedValue()
		{
			// Arrange
			const int packageCount = 2;

			// Act
			var result = _unitUnderTest.ParsePackageCount(packageCount.ToString());

			// Assert
			Assert.AreEqual(packageCount, result);
		}

		[DataTestMethod]
		[DataRow("p")]
		[DataRow("p,1,p")]
		[DataRow("p,1,p,2")]
		[ExpectedException(typeof(FormatException))]
		public void ParsePackageInformation_WrongFormat_FormatException(string value)
		{
			// Act
			_unitUnderTest.ParsePackageInformation(value);
		}

		[TestMethod]
		public void ParsePackageInformation_ValidData_DataParsed()
		{
			// Arrange
			var package = new PackageDetails("p", "1");

			// Act
			var result = _unitUnderTest.ParsePackageInformation($"{package.Name},{package.Version}");

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(package, result);
		}

		[DataTestMethod]
		[DataRow("p")]
		[DataRow("p,1")]
		[DataRow("p,1,p")]
		[DataRow("p,1,p,2,p")]
		[ExpectedException(typeof(FormatException))]
		public void ParseDependenciesInformation_WrongFormat_FormatException(string value)
		{
			// Act
			_unitUnderTest.ParseDependenciesInformation(value);
		}

		[TestMethod]
		public void ParseDependenciesInformation_PackageAndDependencyData_DataParsed()
		{
			// Arrange
			var package1 = new PackageDetails("p", "1");
			var package2 = new PackageDetails("x", "2");
			var packageDependencies = new PackageDependency(package1, package2);

			// Act
			var result = _unitUnderTest.ParseDependenciesInformation($"{package1.Name},{package1.Version},{package2.Name},{package2.Version}");

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(packageDependencies.Package, result.Package);
			CollectionAssert.AreEquivalent(packageDependencies.Dependencies.ToList(), result.Dependencies.ToList());
		}

		[TestMethod]
		public void ParseDependenciesInformation_MultipleDependencies_DataParsed()
		{
			// Arrange
			var package1 = new PackageDetails("p", "1");
			var package2 = new PackageDetails("x", "2");
			var package3 = new PackageDetails("z", "2");
			var packageDependencies = new PackageDependency(package1, package2, package3);

			// Act
			var result = _unitUnderTest.ParseDependenciesInformation($"{package1.Name},{package1.Version},{package2.Name},{package2.Version},{package3.Name},{package3.Version}");

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(packageDependencies.Package, result.Package);
			CollectionAssert.AreEquivalent(packageDependencies.Dependencies.ToList(), result.Dependencies.ToList());
		}
	}
}