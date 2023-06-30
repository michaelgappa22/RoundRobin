function processDashboardLoad() {
    // Retrieve the current user ID
    var userId = Xrm.Page.context.getUserId();
  
    // Retrieve the teams the user is a member of
    var query = "/systemusers(" + userId.slice(1, -1) + ")/teammembership_association/$ref";
    Xrm.WebApi.retrieveMultipleRecords(query).then(
      function (result) {
        if (result.entities.length > 0) {
          var currentTime = new Date();
  
          // Loop through the teams
          result.entities.forEach(function (team) {
            // Retrieve the team details
            var teamId = team["teamid"];
  
            // Get the team record using Web API
            Xrm.WebApi.retrieveRecord("team", teamId, "?$select=teamid,name,new_opentime,new_closetime").then(
              function (teamRecord) {
                // Retrieve the open and close times as strings
                var openTimeStr = teamRecord["ramcosub_hoursofoperationopen"];
                var closeTimeStr = teamRecord["ramcosub_hoursofoperationclosed"];
  
                // Convert open and close times to TimeOnly objects
                var openTime = getTimeOnlyFromString(openTimeStr);
                var closeTime = getTimeOnlyFromString(closeTimeStr);
  
                // Check if the current time is within the open and close time of the team
                if (currentTime >= openTime && currentTime < closeTime) {
                  // Create a new record and add the team to the lookup field
                  var newRecord = {};
                  newRecord["ramcosub_team"] = [{ id: teamId, entityType: "team" }];
  
                  Xrm.WebApi.createRecord("ramcosub_userlogin", newRecord).then(
                    function (createdRecord) {
                      console.log("New record created with ID: " + createdRecord.id);
                    },
                    function (error) {
                      console.log("Error creating new record: " + error.message);
                    }
                  );
                }
              },
              function (error) {
                console.log("Error retrieving team record: " + error.message);
              }
            );
          });
        }
      },
      function (error) {
        console.log("Error retrieving user's teams: " + error.message);
      }
    );
  }
  
  function getTimeOnlyFromString(timeString) {
    // Split the time string into hour and minute parts
    var parts = timeString.split(":");
    var hour = parseInt(parts[0]);
    var minute = parseInt(parts[1]);
  
    // Convert the hour to 24-hour format if needed
    if (timeString.toLowerCase().indexOf("pm") > -1 && hour < 12) {
      hour += 12;
    } else if (timeString.toLowerCase().indexOf("am") > -1 && hour === 12) {
      hour = 0;
    }
  
    // Create a new Date object with the current date and the specified hour and minute
    var timeOnly = new Date();
    timeOnly.setHours(hour);
    timeOnly.setMinutes(minute);
    timeOnly.setSeconds(0);
    timeOnly.setMilliseconds(0);
  
    return timeOnly;
  }
  
  // Register the function to execute when the dashboard is loaded
  Xrm.Page.data.addOnLoad(processDashboardLoad);
  