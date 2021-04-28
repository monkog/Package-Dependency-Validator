using Autofac;

namespace PackageDependencyValidator
{
	public class Program
	{
		public static IContainer Container { get; set; }

		static void Main(string[] args)
		{
			var builder = new ContainerBuilder();
			Container = builder.Build();
		}
	}
}
