using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.Model
{
	[TestClass]
	public class PackageDetailsTests
	{
		private const string ValidName = "package";
		private const int ValidVersion = 1;

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullName_ArgumentNullException()
		{
			// Act
			_ = new PackageDetails(null, ValidVersion.ToString());
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("\t")]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_WhitespaceName_ArgumentException(string name)
		{
			// Act
			_ = new PackageDetails(name, ValidVersion.ToString());
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("\t")]
		[DataRow("version")]
		[ExpectedException(typeof(FormatException))]
		public void Ctor_InvalidVersion_FormatException(string version)
		{
			// Act
			_ = new PackageDetails(ValidName, version);
		}

		[DataTestMethod]
		[DataRow("-1")]
		[DataRow("0")]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_VersionLessThan1_ArgumentException(string version)
		{
			// Act
			_ = new PackageDetails(ValidName, version);
		}

		[TestMethod]
		public void Ctor_ValidArguments_PropertiesAssigned()
		{
			// Act
			var result = new PackageDetails(ValidName, ValidVersion.ToString());

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(ValidName, result.Name);
			Assert.AreEqual(ValidVersion, result.Version);
		}

		[TestMethod]
		public void Equals_OtherType_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion.ToString());
			var other = new OtherPackageDetails(ValidName, ValidVersion.ToString());

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_OtherName_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion.ToString());
			var other = new PackageDetails("other package", ValidVersion.ToString());

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_OtherVersion_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion.ToString());
			var other = new PackageDetails(ValidName, (ValidVersion + 1).ToString());

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_SameNameAndVersion_True()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion.ToString());
			var other = new PackageDetails(ValidName, ValidVersion.ToString());

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		private class OtherPackageDetails
		{
			private string Name { get; }

			private int Version { get; }

			public OtherPackageDetails(string name, string version)
			{
				Name = name;
				Version = int.Parse(version);
			}
		}
	}
}