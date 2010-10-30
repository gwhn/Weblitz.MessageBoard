using System;
using System.Collections.Generic;
using System.Diagnostics;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace Weblitz.MessageBoard.Tests.Integration
{
    [TestFixture]
    public class NHibernateConfigurationTests
    {
        private Configuration _config;
        private List<string> _scripts;

        [Test]
        public void DatabaseConfiguration()
        {
            new Story("database configuration")
                .InOrderTo("validate database schema")
                .AsA("developer")
                .IWant("to export database schema")

                        .WithScenario("valid nhibernate configuration and class maps")
                            .Given(NhibernateConfigurationFile)
                                .And(_AssemblyWithClassMapsEmbedded, "Weblitz.MessageBoard.Infrastructure")
                            .When(SchemaScriptsAreExported)
                            .Then(_ExecuteScriptToDrop_Table, true, "Forum")
                                .And(_ExecuteScriptToDrop_Table, true, "Entry")
                                .And(_ExecuteScriptToDrop_Table, true, "Topic")
                                .And(_ExecuteScriptToDrop_Table, true, "Post")
                                .And(_ExecuteScriptToDrop_Table, true, "Attachment")
                                .And(_ExecuteScriptToCreate_Table, true, "Forum")
                                .And(_ExecuteScriptToCreate_Table, true, "Entry")
                                .And(_ExecuteScriptToCreate_Table, true, "Topic")
                                .And(_ExecuteScriptToCreate_Table, true, "Post")
                                .And(_ExecuteScriptToCreate_Table, true, "Attachment")
                .Execute();
        }

        private void NhibernateConfigurationFile()
        {
            _config = new Configuration().Configure();
        }

        private void _AssemblyWithClassMapsEmbedded(string assembly)
        {
            _config.AddAssembly(assembly);
        }

        private void SchemaScriptsAreExported()
        {
            _scripts = new List<string>();
            var schema = new SchemaExport(_config);
            schema.Execute(_scripts.Add, true, false);
        }

        private void _ExecuteScriptToDrop_Table([BooleanParameterFormat("should", "should not")]bool execute, string table)
        {
            if (execute)
            {
                Assert.IsTrue(_scripts.Exists(m => m.Contains(string.Format("drop table {0}", table))));
            }
            else
            {
                Assert.IsFalse(_scripts.Exists(m => m.Contains(string.Format("drop table {0}", table))));                
            }
        }

        private void _ExecuteScriptToCreate_Table([BooleanParameterFormat("should", "should not")]bool execute, string table)
        {
            if (execute)
            {
                Assert.IsTrue(_scripts.Exists(m => m.Contains(string.Format("create table {0}", table))));
            }
            else
            {
                Assert.IsFalse(_scripts.Exists(m => m.Contains(string.Format("create table {0}", table))));
            }
        }
    }
}