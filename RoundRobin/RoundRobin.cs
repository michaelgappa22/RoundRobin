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

}