using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FrameworkForGenerationCRUD
{
    public class CreationOfProject
    {
        public void GenerateProject(string path, string projectName)
        {
            File.WriteAllText("myrun.bat", scriptForCreation(path, projectName));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C myrun.bat ";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        public void RestoreProject(string path, string projectName, string[] entities)
        {
            File.WriteAllText("restore.bat", scriptForDatabase(path, projectName, entities));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C restore.bat ";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        public string scriptForCreation(string path, string projectName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" dotnet tt GenerateFeatures.tt "+ Environment.NewLine);
            sb.Append(" dotnet tt GenerateTests.tt "+ Environment.NewLine);
            sb.Append(" cd " + path + Environment.NewLine);
            sb.Append(" mkdir " + projectName + Environment.NewLine);
            sb.Append(" cd " + path + @"\" + projectName + Environment.NewLine);
            sb.Append(" dotnet new mvc" + Environment.NewLine);
            sb.Append(" Exit" + Environment.NewLine);

            return sb.ToString();
        }
        public string scriptForDatabase(string path, string projectName, string[] entities)
        {
            string mainPath = System.IO.Directory.GetCurrentDirectory();
            StringBuilder sb = new StringBuilder();
            sb.Append(" cd " + path + Environment.NewLine);
            sb.Append(" dotnet restore" + Environment.NewLine);
            foreach (string entity in entities)
            {
                sb.Append("dotnet aspnet-codegenerator controller -name " + entity + "Controller -m " + entity +
                " -dc " + projectName + "Context --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries" + Environment.NewLine);
            }
            sb.Append(" dotnet add package SpecFlow --version 3.0.150-beta" + Environment.NewLine);
            sb.Append(" dotnet add package NUnit3TestAdapter --version 3.13.0" + Environment.NewLine);
            sb.Append(" dotnet add package SpecFlow.Tools.MsBuild.Generation --version 3.0.150-beta" + Environment.NewLine);
            sb.Append(" dotnet add package Microsoft.NET.Test.Sdk --version 16.0.1" + Environment.NewLine);
            sb.Append(" dotnet add package T4 --version 2.0.1" + Environment.NewLine);
            sb.Append(" dotnet add package T5.TextTransform.Tool --version 1.1.0" + Environment.NewLine);
            sb.Append(" dotnet add package SpecFlow.NUnit --version 3.0.150-beta" + Environment.NewLine);
            sb.Append(@" move "+mainPath+ @"\GenerateFeatures.feature " +path  + Environment.NewLine);
            sb.Append(" dotnet ef migrations add CreateDatabase2 -v" + Environment.NewLine);
            sb.Append(" dotnet ef database update" + Environment.NewLine);
            sb.Append(" dotnet restore" + Environment.NewLine);
            sb.Append(@" move "+mainPath+ @"\GenerateTests.cs " +path  + Environment.NewLine);
            sb.Append(" Exit" + Environment.NewLine);

            return sb.ToString();
        }
        public void rewriteMainClass(string path, string projectName)
        {
            var allLines = File.ReadAllLines(path + @"\Startup.cs").ToList();
            List<string> l = new List<string>();
            l.AddRange(allLines);
            var counter = 2;
            l.Insert(counter, "using Microsoft.EntityFrameworkCore;");
            l.Insert(counter, "using " + projectName + ".Data;");
            foreach (string str in allLines)
            {
                if (str == "        public void ConfigureServices(IServiceCollection services)")
                {
                    l.Insert(counter + 2, "          services.AddDbContext<" + projectName + "Context>(options =>");
                    l.Insert(counter + 3, "          options.UseSqlite(Configuration.GetConnectionString(\"DefaultConnection\")));");
                }
                counter++;
            }
            File.WriteAllLines(path + @"\Startup.cs", l.ToArray());
        }
        public void rewriteConfig(string path, string projectName)
        {
            var allLines = File.ReadAllLines(path + @"\appsettings.json").ToList();
            List<string> l = new List<string>();
            l.AddRange(allLines);
            l.Insert(1, "\"ConnectionStrings\": {");
            l.Insert(2, " \"DefaultConnection\": \"Filename=" + path + @"/" + projectName.ToLower() + ".db\"");
            l.Insert(3, "},");
            File.WriteAllLines(path + @"\appsettings.json", l.ToArray());
        }

        public void rewriteCsProj(string path, string projectName)
        {
            var allLines = File.ReadAllLines(path + @"\" + projectName + ".csproj").ToList();
            List<string> l = new List<string>();
            l.AddRange(allLines);
            var counter = 0;
            foreach (string str in allLines)
            {
                if (str == "  <PropertyGroup>")
                {
                    counter += 2;
                    l.Insert(counter, "   <GenerateProgramFile>false</GenerateProgramFile>");
                    counter += 2;
                    l.Insert(counter++, "   <PropertyGroup>");
                    l.Insert(counter++, "   <SpecFlowTasksPath>../packages/specflow/3.0.150-beta/tools/specflow.exe</SpecFlowTasksPath>");
                    l.Insert(counter++, "   </PropertyGroup>");
                    l.Insert(counter++, "   <Import Project=\"../packages/specflow/3.0.150-beta/tools/TechTalk.SpecFlow.tasks\" Condition=\"Exists('../packages/specflow/3.0.150-beta/tools/TechTalk.SpecFlow.tasks')\" />");
                    l.Insert(counter++, "   <Import Project=\"../packages/specflow/3.0.150-beta/tools/TechTalk.SpecFlow.targets\" Condition=\"Exists('../packages/specflow/3.0.150-beta/tools/TechTalk.SpecFlow.targets')\" />");
                    l.Insert(counter++, "    <Target Name=\"AfterUpdateFeatureFilesInProject\">");
                    l.Insert(counter++, "    <Move SourceFiles=\"@(SpecFlowGeneratedFiles)\"  OverwriteReadOnlyFiles=\"true\" />");
                    l.Insert(counter++, "    <ItemGroup>");
                    l.Insert(counter++, "    <Compile Include=\"*.cs\">");
                    l.Insert(counter++, "    <Visible>true</Visible>");
                    l.Insert(counter++, "    </Compile>");
                    l.Insert(counter++, "    </ItemGroup>");
                    l.Insert(counter++, "    </Target>");
                    l.Insert(counter++, "    <ItemGroup>");
                    l.Insert(counter++, "    <DotNetCliToolReference Include=\"T5.TextTransform.Tool\" Version=\"1.1.0-*\" />");
                    l.Insert(counter++, "    <TextTemplate Include=\"*.tt\" />");
                    l.Insert(counter++, "    <Generated Include=\"*.cs\" />");
                    l.Insert(counter++, "    </ItemGroup>");
                }
                if (str == "  <ItemGroup>")
                {
                    counter -= 2;
                    l.Insert(counter, "   <DotNetCliToolReference Include=\"Microsoft.EntityFrameworkCore.Tools.DotNet\" Version=\"2.0.2\" />");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.EntityFrameworkCore.Sqlite\" Version=\"2.1.1\"/>");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.VisualStudio.Web.CodeGeneration.Design\" Version=\"2.1.4\" />");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.Composition\" Version=\"1.0.30\" ExcludeAssets=\"All\" />");
                    l.Insert(counter++, "   <PackageReference Include=\"System.Composition\" Version=\"1.2.0\" />");
                    l.Insert(counter++, "   <DotNetCliToolReference Include=\"Microsoft.VisualStudio.Web.CodeGeneration.Tools\" Version=\"2.0.4\" />");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.CodeAnalysis.Common\" Version=\"2.8.2\" />");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.AspNetCore.All\" Version=\"2.1.2\" />  ");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.CodeAnalysis.CSharp\" Version=\"2.8.2\" />  ");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.CodeAnalysis.CSharp.Workspaces\" Version=\"2.8.2\" />   ");
                    l.Insert(counter++, "   <PackageReference Include=\"Microsoft.CodeAnalysis.Workspaces.Common\" Version=\"2.8.2\" />   ");
                }
                counter++;
            }
            File.WriteAllLines(path + @"\" + projectName + ".csproj", l.ToArray());
        }


    }

}