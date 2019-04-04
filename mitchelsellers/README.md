# .NET Core logging Demonstration

This repository includes code samples necessary to illustrate various options and details necessary to improve the quality of .NET Core Logging.

## Prerequisites to Running

This project will create/connect to a local database for the EF Context portion of the demonstration and was built using .NET Core 2.2.  

* You will need to update the server & DB name as outlined in the appsettings.json for the database to work
* You will need to apply the migrations using "Update-Database" from the Package Manager Console to ensure all data model elements have been added

## Commit History

This repository has been structured with a commit history that allows a progression to be followed when paired with the PowerPoint presentation included inside of the repository.

Rolling the repository back to the "Baseline Project Creation" commit will start you at the baseline, where the default ASP.NET Core project template was used.  You can then progress commit-by-commit to see the progression in configuration and abilitites

## Best Practices Adherence

This is a sample project only, and does not necessarily adhere to all best practices for enterprise development.  This is a focused demonstration regarding the usage of Logging with .NET Core.  As such, please use caution when implementing any portion of this example in your own projects.
