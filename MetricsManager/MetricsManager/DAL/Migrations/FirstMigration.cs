using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Migrations
{    
    public static class Ex
    {
        public static IFluentSyntax CreateTableIfNotExists(
            this MigrationBase self, 
            string tableName, 
            Func<ICreateTableWithColumnOrSchemaOrDescriptionSyntax, 
                IFluentSyntax> constructTableFunction,
            string schemaName = "dbo")
        {
            if (!self.Schema.Schema(schemaName).Table(tableName).Exists())
            {
                return constructTableFunction(self.Create.Table(tableName));
            }
            else
            {
                return null;
            }
        }
    }

    [Migration(1)]
    public class FirstMigration : Migration
    {        
        public override void Up()
        {
            //create
            string[] tabNames = {
                "cpumetrics",
                "dotnetmetrics",
                "hddmetrics",
                "networkmetrics",
                "rammetrics"
                };

            foreach(string name in tabNames)
            {
                this.CreateTableIfNotExists(
                    name, 
                    table => table
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("AgentId").AsInt32()
                    .WithColumn("Value").AsInt32()
                    .WithColumn("Time").AsInt64());
            }

            this.CreateTableIfNotExists(
                "agents",
                table => table
                .WithColumn("AgentId").AsInt32().PrimaryKey().Identity()
                .WithColumn("AgentAddress").AsString().Unique());
        }

        public override void Down()
        {
            //delete
            string[] tabNames = {
                "cpumetrics",
                "dotnetmetrics",
                "hddmetrics",
                "networkmetrics",
                "rammetrics"
                };

            foreach (string name in tabNames)
            {
                Delete.Table(name);
            }
        }
    }
}