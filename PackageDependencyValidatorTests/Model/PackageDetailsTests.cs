using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator.Model;

namespace PackageDependencyValidatorTests.Model
{
	[TestClass]
	public class PackageDetailsTests
	{
		private const string ValidName = "package";
		private const string ValidVersion = "Beta-1";

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullName_ArgumentNullException()
		{
			// Act
			_ = new PackageDetails(null, ValidVersion);
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("\t")]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_WhitespaceName_ArgumentException(string name)
		{
			// Act
			_ = new PackageDetails(name, ValidVersion);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullVersion_ArgumentNullException()
		{
			// Act
			_ = new PackageDetails(ValidName, null);
		}

		[DataTestMethod]
		[DataRow("")]
		[DataRow(" ")]
		[DataRow("\t")]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_WhitespaceVersion_ArgumentException(string version)
		{
			// Act
			_ = new PackageDetails(ValidName, version);
		}

		[TestMethod]
		public void Ctor_ValidArguments_PropertiesAssigned()
		{
			// Act
			var result = new PackageDetails(ValidName, ValidVersion);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(ValidName, result.Name);
			Assert.AreEqual(ValidVersion, result.Version);
		}

		[TestMethod]
		public void Equals_OtherType_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new OtherPackageDetails(ValidName, ValidVersion);

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_OtherName_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new PackageDetails("other package", ValidVersion);

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_OtherVersion_False()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new PackageDetails(ValidName, "Beta-2");

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void Equals_SameNameAndVersion_True()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new PackageDetails(ValidName, ValidVersion);

			// Act
			var result = unitUnderTest.Equals(other);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void GetHashCode_SameObject_SameHashCode()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);

			// Act
			var hash = unitUnderTest.GetHashCode();
			var sameHash = unitUnderTest.GetHashCode();

			// Assert
			Assert.AreEqual(hash, sameHash);
		}

		[TestMethod]
		public void GetHashCode_DifferentObjectsSameValues_SameHashCode()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new PackageDetails(ValidName, ValidVersion);

			// Act
			var hash = unitUnderTest.GetHashCode();
			var otherHash = other.GetHashCode();

			// Assert
			Assert.AreEqual(hash, otherHash);
		}

		[TestMethod]
		public void GetHashCode_DifferentObjectsValues_DifferentHashCode()
		{
			// Arrange
			var unitUnderTest = new PackageDetails(ValidName, ValidVersion);
			var other = new PackageDetails("package-1", ValidVersion);

			// Act
			var hash = unitUnderTest.GetHashCode();
			var otherHash = other.GetHashCode();

			// Assert
			Assert.AreNotEqual(hash, otherHash);
		}

		private class OtherPackageDetails
		{
			private string Name { get; }

			private string Version { get; }

			public OtherPackageDetails(string name, string version)
			{
				Name = name;
				Version = version;
			}
		}
	}
}