﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.TestManagement.Client;
using MigrationTools;
using MigrationTools._EngineV1.Clients;

namespace VstsSyncMigrator.Engine.ComponentContext
{
    public class TestManagementContext
    {
        private readonly string testPlanQueryBit;
        private readonly IMigrationClient _source;
        private ITestManagementService tms;

        internal ITestManagementTeamProject Project { get; }

        public TestManagementContext(IMigrationClient source) : this(source, null)
        {
        }

        public TestManagementContext(IMigrationClient source, string testPlanQueryBit)
        {
            this.testPlanQueryBit = testPlanQueryBit;
            _source = source;
            tms = _source.GetService<ITestManagementService>();
            Project = tms.GetTeamProject(source.Config.AsTeamProjectConfig().Project);
        }

        internal List<ITestPlan> GetTestPlans()
        {
            var query = (string.IsNullOrWhiteSpace(testPlanQueryBit))
                ? "Select * From TestPlan"
                : $"Select * From TestPlan Where {testPlanQueryBit}";

            var testPlans = Project.TestPlans.Query(query);

            return testPlans.Select(tp => Project.TestPlans.Find(tp.Id)).ToList();
        }

        internal List<ITestRun> GetTestRuns()
        {
            return Project.TestRuns.Query("Select * From TestRun").ToList();
        }

        internal ITestPlan CreateTestPlan()
        {
            return Project.TestPlans.Create();
        }
    }
}