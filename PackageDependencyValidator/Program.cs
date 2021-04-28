using System;
using System.IO.Abstractions;
using Autofac;
using PackageDependencyValidator.FileParser;
using PackageDependencyValidator.Properties;
using PackageDependencyValidator.Validators;

namespace PackageDependencyValidator
{
	public class Program
	{
		public static IContainer Container { get; set; }

		public static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			RegisterDependencies(builder);
			Container = builder.Build();

			var fileName = args[0];
			ParseFile(fileName);
		}

		public static void ParseFile(string fileName)
		{
			using (var scope = Container.BeginLifetimeScope())
			{
				var parser = scope.Resolve<IPackageDependenciesFileParser>();
				var applicationPackageInformation = parser.Parse(fileName);

				var applicationPackageValidator = scope.Resolve<IDependenciesValidator>();
				var validationStatus = applicationPackageValidator.AreDependenciesValid(applicationPackageInformation) ? Resource.PassStatus : Resource.FailStatus;
				Console.WriteLine(validationStatus);
			}
		}

		private static void RegisterDependencies(ContainerBuilder builder)
		{
			builder.RegisterType<PackageDependenciesFileParser>().As<IPackageDependenciesFileParser>();
			builder.RegisterType<PackageInformationParser>().As<IPackageInformationParser>();
			builder.RegisterType<FileSystem>().As<IFileSystem>();
			builder.RegisterType<DependenciesValidator>().As<IDependenciesValidator>();
		}
	}
}
