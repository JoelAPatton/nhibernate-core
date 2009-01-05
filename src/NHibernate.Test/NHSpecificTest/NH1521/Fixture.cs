using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace NHibernate.Test.NHSpecificTest.NH1521
{
	[TestFixture]
	public class Fixture
	{
		private static void AssertThatCheckOnTableExistenceIsCorrect(Configuration configuration)
		{
			var su = new SchemaExport(configuration);
			var sb = new StringBuilder(500);
			su.Execute(x => sb.AppendLine(x), false, false, true);
			string script = sb.ToString();
			Assert.That(script, Text.Contains("if exists (select * from dbo.sysobjects where id = object_id(N'nhibernate.dbo.Aclass') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"));
		}

		[Test]
		public void TestForClassWithDefaultSchema()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithNothing.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "nhibernate");
			cfg.SetProperty(Environment.DefaultSchema, "dbo");
			AssertThatCheckOnTableExistenceIsCorrect(cfg);
		}

		[Test]
		public void WithDefaultValuesInMapping()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithDefault.hbm.xml", GetType().Assembly);
			AssertThatCheckOnTableExistenceIsCorrect(cfg);
		}

		[Test]
		public void WithSpecificValuesInMapping()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithSpecific.hbm.xml", GetType().Assembly);
			AssertThatCheckOnTableExistenceIsCorrect(cfg);
		}

		[Test]
		public void WithDefaultValuesInConfigurationPriorityToMapping()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithDefault.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "somethingDifferent");
			cfg.SetProperty(Environment.DefaultSchema, "somethingDifferent");
			AssertThatCheckOnTableExistenceIsCorrect(cfg);
		}
	}

	public class Aclass
	{
		public int Id { get; set; }
	}
}