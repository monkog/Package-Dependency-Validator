using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.FileParser;
using PackageDependencyValidator.Model;
using Rhino.Mocks;

namespace PackageDependencyValidatorTests.FileParser
{
	[TestClass]
	public class PackageDependenciesFileParserTests
	{
		private IFileSystem _fileSystem;
		private IPackageInformationParser _packageInformationParser;

		private PackageDependenciesFileParser _unitUnderTest;
		private const string FilePath = "filepath";
		private const string FileContent = "content";

		[TestInitialize]
		public void Initialize()
		{
			var fileData = new MockFileData(FileContent);
			var files = new Dictionary<string, MockFileData> { { FilePath, fileData } };
			_fileSystem = new MockFileSystem(files);
			_packageInformationParser = MockRepository.GenerateMock<IPackageInformationParser>();

			_unitUnderTest = new PackageDependenciesFileParser(_fileSystem, _packageInformationParser);
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

			_packageInformationParser.Stub(p => p.ParsePackageCount(Arg<string>.Is.Anything)).Return(1);
			_packageInformationParser.Stub(p => p.ParsePackageInformation(Arg<string>.Is.Anything)).Return(package);
			_packageInformationParser.Stub(p => p.ParseDependenciesInformation(Arg<string>.Is.Anything)).Return(packageDependency);

			// Act
			var result = _unitUnderTest.Parse(FilePath);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(package, result.PackagesToInstall.Single());
			Assert.AreEqual(package, result.PackageDependencies.Single().Key);
			Assert.AreEqual(dependency, result.PackageDependencies.Single().Value.Single());
		}
	}
}