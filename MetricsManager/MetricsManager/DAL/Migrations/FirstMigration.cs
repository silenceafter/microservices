using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Migrations
{
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
                Delete.Table(name);
                Create.Table(name)
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("AgentId").AsInt32()
                    .WithColumn("Value").AsInt32()
                    .WithColumn("Time").AsInt64();
            }
            Delete.Table("agents");
            Create.Table("agents")
                .WithColumn("AgentId").AsInt32().PrimaryKey().Identity()
                .WithColumn("AgentAddress").AsString().Unique();
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