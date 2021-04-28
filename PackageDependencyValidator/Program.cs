using System.IO.Abstractions;
using Autofac;
using PackageDependencyValidator.FileParser;

namespace PackageDependencyValidator
{
	public class Program
	{
		public static IContainer Container { get; set; }

		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<PackageDependenciesFileParser>().As<IPackageDependenciesFileParser>();
			builder.RegisterType<PackageInformationParser>().As<IPackageInformationParser>();
			builder.RegisterType<FileSystem>().As<IFileSystem>();
			Container = builder.Build();
		}
	}
}
