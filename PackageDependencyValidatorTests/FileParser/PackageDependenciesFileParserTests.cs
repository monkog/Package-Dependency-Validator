using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PackageDependencyValidator.FileParser;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.FileParser
{
	[TestClass]
	public class PackageDependenciesFileParserTests
	{
		private IFileSystem _fileSystem;
		private Mock<IPackageInformationParser> _packageInformationParser;

		private PackageDependenciesFileParser _unitUnderTest;
		private const string FilePath = "filepath";
		private const string FileContent = "1\nP1,42\n1\nP1,42,P2,Beta-1";

		[TestInitialize]
		public void Initialize()
		{
			var fileData = new MockFileData(FileContent);
			var files = new Dictionary<string, MockFileData> { { FilePath, fileData } };
			_fileSystem = new MockFileSystem(files);
			_packageInformationParser = new Mock<IPackageInformationParser>();

			_unitUnderTest = new PackageDependenciesFileParser(_fileSystem, _packageInformationParser.Object);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Parse_NullFilePath_ArgumentNullException()
		{
			// Act
			_ = _unitUnderTest.Parse(null);
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void Parse_FileNotFound_FileNotFoundException()
		{
			// Act
			_ = _unitUnderTest.Parse("unknown-file");
		}

		[TestMethod]
		public void Parse_ValidFile_ApplicationPackageInformation()
		{
			// Arrange
			var package = new PackageDetails("package", "1");
			var dependency = new PackageDetails("dependency", "1");
			var packageDependency = new PackageDependency(package, dependency);

			_packageInformationParser.Setup(p => p.ParsePackageCount(It.IsAny<string>())).Returns(1);
			_packageInformationParser.Setup(p => p.ParsePackageInformation(It.IsAny<string>())).Returns(package);
			_packageInformationParser.Setup(p => p.ParseDependenciesInformation(It.IsAny<string>())).Returns(packageDependency);

			// Act
			var result = _unitUnderTest.Parse(FilePath);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(package, result.PackagesToInstall.Single());
			Assert.AreEqual(package, result.PackageDependencies.Single().Key);
			Assert.AreEqual(dependency, result.PackageDependencies.Single().Value.Single());
		}

		[TestMethod]
		public void Parse_DuplicatedPackageAndDependency_DistinctPackageAndDependency()
		{
			// Arrange
			var package = new PackageDetails("package", "1");
			var dependency = new PackageDetails("dependency", "1");
			var packageDependency = new PackageDependency(package, dependency);

			_packageInformationParser.Setup(p => p.ParsePackageCount(It.IsAny<string>())).Returns(2);
			_packageInformationParser.Setup(p => p.ParsePackageInformation(It.IsAny<string>())).Returns(package);
			_packageInformationParser.Setup(p => p.ParseDependenciesInformation(It.IsAny<string>())).Returns(packageDependency);

			// Act
			var result = _unitUnderTest.Parse(FilePath);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(package, result.PackagesToInstall.Single());
			Assert.AreEqual(package, result.PackageDependencies.Single().Key);
			Assert.AreEqual(dependency, result.PackageDependencies.Single().Value.Single());
		}

		[TestMethod]
		public void Parse_NoDependencies_OnlyPackages()
		{
			// Arrange
			var package = new PackageDetails("package", "1");

			_packageInformationParser.SetupSequence(p => p.ParsePackageCount(It.IsAny<string>())).Returns(1).Returns(0);
			_packageInformationParser.Setup(p => p.ParsePackageInformation(It.IsAny<string>())).Returns(package);

			// Act
			var result = _unitUnderTest.Parse(FilePath);

			// Assert
			Assert.AreEqual(package, result.PackagesToInstall.Single());
			Assert.AreEqual(0, result.PackageDependencies.Count);
		}
	}
}