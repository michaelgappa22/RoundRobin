using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace RoundRobin
{
    public class AddTeamMemberFromCount : CodeActivity
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> team { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            // Retrieve the Team lookup value
            EntityReference teamRef = team.Get(context);

            // Retrieve the IOrganizationService
            IOrganizationService service = context.GetExtension<IOrganizationServiceFactory>().CreateOrganizationService(null);

            // Retrieve the Team record using the IOrganizationService
            Entity teamRecord = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("ramcosub_countofloginusers"));

            // Access the Team record field and value
            int recordCount = teamRecord.GetAttributeValue<int>("ramcosub_countofloginusers");
            recordCount = recordCount + 1;
            teamRecord["ramcosub_countofloginusers"] = recordCount;

            // Perform the update operation
            service.Update(teamRecord);

        }
    }
    public class RemoveTeamMemberFromCount : CodeActivity
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> team { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            // Retrieve the Team lookup value
            EntityReference teamRef = team.Get(context);

            // Retrieve the IOrganizationService
            IOrganizationService service = context.GetExtension<IOrganizationServiceFactory>().CreateOrganizationService(null);

            // Retrieve the Team record using the IOrganizationService
            Entity teamRecord = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("ramcosub_countofloginusers"));

            // Access the Team record field and value
            int recordCount = teamRecord.GetAttributeValue<int>("ramcosub_countofloginusers");
            recordCount = recordCount - 1;
            teamRecord["ramcosub_countofloginusers"] = recordCount;

            // Perform the update operation
            service.Update(teamRecord);

        }
    }
    public class SetCountOfTeamMembers : CodeActivity
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> Team { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IOrganizationService service = context.GetExtension<IOrganizationServiceFactory>().CreateOrganizationService(null);
            EntityReference teamRef = Team.Get(context);

            QueryExpression query = new QueryExpression("teammembership");
            query.Criteria.AddCondition("teamid", ConditionOperator.Equal, teamRef.Id);

            EntityCollection result = service.RetrieveMultiple(query);

            Entity teamRecord = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("ramcosub_totalteammembers"));
            teamRecord["ramcosub_totalteammembers"] = result.Entities.Count;

            service.Update(teamRecord);
        }
    }

    public class GetTeamHoursOfOperations : CodeActivity
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> Team { get; set; }

        [Output("Closed Date and Time")]
        public OutArgument<DateTime> ClosedDateTime { get; set; }

        [Output("Open Date and Time")]
        public OutArgument<DateTime> OpenDateTime { get; set; }

        [Output("Count of Users Login")]
        public OutArgument<int> CountOfUsersLogin { get; set; }

        [Output("Total Members of Team")]
        public OutArgument<int> OpenTotalMembersOfTeam { get; set; }

        [Output("Round Robin Threshold")]
        public OutArgument<DateTime> RoundRobinThreshold { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            IOrganizationService service = context.GetExtension<IOrganizationServiceFactory>().CreateOrganizationService(null);
            EntityReference teamRef = Team.Get(context);
            Entity teamRecord = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("ramcosub_opendatetime","ramcosub_closedatetime", "ramcosub_countofloginusers", "ramcosub_totalteammembers", "ramcosub_roundrobinthreshold"));

            DateTime closedDateTime = teamRecord.GetAttributeValue<DateTime>("ramcosub_closedatetime");
            DateTime openDateTime = teamRecord.GetAttributeValue<DateTime>("ramcosub_opendatetime");
            DateTime getRoundRobinThreshold = teamRecord.GetAttributeValue<DateTime>("ramcosub_roundrobinthreshold");
            int countOfUsersLogin = teamRecord.GetAttributeValue<int>("ramcosub_countofloginusers");
            int openTotalMembersOfTeam = teamRecord.GetAttributeValue<int>("ramcosub_totalteammembers");
            

            ClosedDateTime.Set(context, closedDateTime);
            OpenDateTime.Set(context, openDateTime);
            CountOfUsersLogin.Set(context, countOfUsersLogin);
            OpenTotalMembersOfTeam.Set(context, openTotalMembersOfTeam);
            RoundRobinThreshold.Set(context, getRoundRobinThreshold);
        }
    }

    public class RoundRobinAssignment
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> Team { get; set; }
        [Input("Case")]
        [ReferenceTarget("incident")]
        public InArgument<EntityReference> Case { get; set; }
    }

}