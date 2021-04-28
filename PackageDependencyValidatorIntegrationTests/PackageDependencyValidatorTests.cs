using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageDependencyValidator;

namespace PackageDependencyValidatorIntegrationTests
{
	[TestClass]
	public class PackageDependencyValidatorTests
	{
		[DataTestMethod]
		[DataRow("input000.txt", "output000.txt")]
		[DataRow("input001.txt", "output001.txt")]
		[DataRow("input002.txt", "output002.txt")]
		[DataRow("input003.txt", "output003.txt")]
		[DataRow("input004.txt", "output004.txt")]
		[DataRow("input005.txt", "output005.txt")]
		[DataRow("input006.txt", "output006.txt")]
		[DataRow("input007.txt", "output007.txt")]
		[DataRow("input008.txt", "output008.txt")]
		public void ValidateDependencies_File_ValidationResult(string fileName, string resultFile)
		{
			// Arrange
			var expectedStatus = GetExpectedStatus($"TestData/{resultFile}");
			using (var writer = new StringWriter())
			{
				Console.SetOut(writer);

				// Act
				Program.Main(new[] { $"TestData/{fileName}" });

				// Assert
				Assert.AreEqual(expectedStatus.Trim(), writer.ToString().Trim());
			}
		}

		private static string GetExpectedStatus(string filePath)
		{
			using (var stream = new FileStream(filePath, FileMode.Open))
			{
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}

			}
		}
	}
}
