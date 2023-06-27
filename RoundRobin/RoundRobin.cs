﻿using System;
using System.Activities;
using System.Linq;
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

    public class RoundRobinAssignment : CodeActivity
    {
        [Input("Team")]
        [ReferenceTarget("team")]
        public InArgument<EntityReference> TeamRecord { get; set; }

        [Input("System View")]
        [ReferenceTarget("savedquery")]
        public InArgument<EntityReference> SystemView { get; set; }

        [Input("Case")]
        [ReferenceTarget("new_case")]
        public InArgument<EntityReference> CaseRecord { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
                IOrganizationService service = serviceFactory.CreateOrganizationService(null);
                // get Team Record, System View, and Created Case Record
                EntityReference teamRef = TeamRecord.Get(context);
                EntityReference savedView = SystemView.Get(context);
                EntityReference caseRecord = CaseRecord.Get(context);

                EntityReference teamQueue = GetTeamQueue(teamRef, service);
                EntityCollection queueItems = GetQueueItems(savedView, service);
                EntityCollection teamMembers = GetTeamMembers(teamRef, service);
                Entity teamRecord = GetLastUserName(teamRef, service);
                // retrieves EntityReference object
                EntityReference lastAssignedUser = teamRecord.GetAttributeValue<EntityReference>("ramcosub_lastuserassigned");
                AssignQueueItemToUser(queueItems, teamMembers, lastAssignedUser, service);
                CreateCaseQueueItem(caseRecord, teamQueue, lastAssignedUser, service);
            }

            catch (Exception ex)
            {
                // Handle or log the exception here
                throw new InvalidPluginExecutionException("An error occurred: " + ex.Message);
            }
        }

        private static EntityCollection GetQueueItems(EntityReference savedView, IOrganizationService service)
        {
            ColumnSet columns = new ColumnSet("name", "fetchxml");
            Entity systemView = service.Retrieve("savedquery", savedView.Id, columns);
            string fetchXml = systemView.GetAttributeValue<string>("fetchxml");
            EntityCollection results = service.RetrieveMultiple(new FetchExpression(fetchXml));
            return results;
        }

        private static EntityCollection GetTeamMembers(EntityReference teamRef, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("ramcosub_userlogin");
            query.Criteria.AddCondition("ramcosub_team", ConditionOperator.Equal, teamRef.Id);
            query.Criteria.AddCondition("createdon", ConditionOperator.OnOrAfter, DateTime.Today);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            query.ColumnSet.AddColumn("ramcosub_user");
            EntityCollection teamMembers = service.RetrieveMultiple(query);
            return teamMembers;
        }

        private static void CreateCaseQueueItem(EntityReference caseRecord, EntityReference teamQueue, EntityReference lastAssignedUser, IOrganizationService service)
        {
            Entity queueItem = new Entity("queueitem");
            queueItem["objecttypecode"] = new OptionSetValue(112);  // Set the object type code of the entity being queued
            queueItem["objectid"] = new EntityReference(caseRecord.LogicalName, caseRecord.Id);  // Set the entity reference of the record being queued
            queueItem["queueid"] = new EntityReference("queue", teamQueue.Id);  // Set the entity reference of the target queue
            queueItem["workerid"] = lastAssignedUser;
            service.Create(queueItem);
        }

        private static Entity GetLastUserName(EntityReference teamRef, IOrganizationService service)
        {
            Entity teamRecord = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("ramcosub_lastuserassigned"));
            return teamRecord;
        }

        private static EntityReference GetTeamQueue(EntityReference teamRef, IOrganizationService service)
        {
            Entity team = service.Retrieve(teamRef.LogicalName, teamRef.Id, new ColumnSet("queueid"));
            EntityReference teamQueue = team.GetAttributeValue<EntityReference>("queueid");
            return teamQueue;
        }
        private static void AssignQueueItemToUser(EntityCollection queueItems, EntityCollection teamMembers, EntityReference lastAssignedUser, IOrganizationService service)
        {
            int teamMemberIndex = 0;
            if (lastAssignedUser == null)
            {


                foreach (Entity queueItem in queueItems.Entities)
                {
                    if (teamMemberIndex >= teamMembers.Entities.Count)
                        teamMemberIndex = 0;
                    EntityReference userRecordRef = new EntityReference("systemuser", teamMembers.Entities[teamMemberIndex].Id);
                    queueItem["workerid"] = userRecordRef;
                    service.Update(queueItem);

                    teamMemberIndex++;
                }
            }
            else
            {
                for (int i = 0; i < teamMembers.Entities.Count; i++)
                {
                    if (teamMembers.Entities[i].GetAttributeValue<EntityReference>("ramcosub_user").Name == lastAssignedUser.Name)
                    {
                        teamMemberIndex = i;
                    }
                }
                foreach (Entity queueItem in queueItems.Entities)
                {
                    if (teamMemberIndex >= teamMembers.Entities.Count)
                        teamMemberIndex = 0;

                    EntityReference userRecordRef = new EntityReference("systemuser", teamMembers.Entities[teamMemberIndex].GetAttributeValue<EntityReference>("ramcosub_user").Id);
                    queueItem["workerid"] = userRecordRef;
                    service.Update(queueItem);

                    teamMemberIndex++;
                }
            }
        }
    }

}