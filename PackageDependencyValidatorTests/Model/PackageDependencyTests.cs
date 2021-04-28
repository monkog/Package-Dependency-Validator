using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.Model
{
	[TestClass]
	public class PackageDependencyTests
	{
		private PackageDetails _package;
		private PackageDetails _dependency;

		[TestInitialize]
		public void Initialize()
		{
			const int validVersion = 1;
			_package = new PackageDetails("package", validVersion.ToString());
			_dependency = new PackageDetails("dependency", validVersion.ToString());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullPackage_ArgumentNullException()
		{
			// Act
			_ = new PackageDependency(null, _dependency);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullDependency_ArgumentNullException()
		{
			// Act
			_ = new PackageDependency(_package, null);
		}

		[TestMethod]
		public void Ctor_ValidArguments_PropertiesAssigned()
		{
			// Act
			var result = new PackageDependency(_package, _dependency);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(_package, result.Package);
			Assert.AreEqual(_dependency, result.Dependency);
		}
	}
}