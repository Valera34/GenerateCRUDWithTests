D:
 cd D:/Files/ProjectThatWork
 dotnet restore
dotnet aspnet-codegenerator controller -name CustomerController -m Customer -dc ProjectThatWorkContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
dotnet aspnet-codegenerator controller -name UserController -m User -dc ProjectThatWorkContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
dotnet aspnet-codegenerator controller -name MeController -m Me -dc ProjectThatWorkContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
 dotnet add package SpecFlow --version 3.0.150-beta
 dotnet add package NUnit3TestAdapter --version 3.13.0
 dotnet add package SpecFlow.Tools.MsBuild.Generation --version 3.0.150-beta
 dotnet add package Microsoft.NET.Test.Sdk --version 16.0.1
 dotnet add package T4 --version 2.0.1
 dotnet add package T5.TextTransform.Tool --version 1.1.0
 dotnet add package SpecFlow.NUnit --version 3.0.150-beta
 move C:\Users\mac\Documents\GitHub\FrameworkForGenerationCRUD\GenerateFeatures.feature D:/Files/ProjectThatWork
 dotnet ef migrations add CreateDatabase2 -v
 dotnet ef database update
 dotnet restore
 move C:\Users\mac\Documents\GitHub\FrameworkForGenerationCRUD\GenerateTests.cs D:/Files/ProjectThatWork
 Exit
