using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrameworkForGenerationCRUD.Models;
using static FrameworkForGenerationCRUD.ModelTemplate;
using System.IO;

namespace FrameworkForGenerationCRUD.Controllers
{
    
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        
        public IActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string path, string bddrequest,string projectName)
        {
            Parser pars =new Parser();
            if(System.IO.Directory.Exists(path)){
                if(!System.IO.Directory.Exists(Path.Combine(path, projectName))){
          
          Model[] mds=pars.parseString(bddrequest);
          pars.GenerateTxt(bddrequest,projectName);

            List<string> entities=new List<string>();
            CreationOfProject pr =new CreationOfProject();
            ModelTemplate model =new ModelTemplate();
            DataTemplate data =new DataTemplate();
            pr.GenerateProject(path,projectName);
            path=path+"/"+projectName;
            foreach(Model ent in mds){
            entities.Add(ent.Name);
            model.GenerateClass(path,ent.Name,ent.Field,projectName);
            }
            data.GenerateClass(path,projectName,entities.ToArray());
            pr.rewriteMainClass(path,projectName);
            pr.rewriteConfig(path,projectName);
            pr.rewriteCsProj(path,projectName);
            pr.RestoreProject(path,projectName,entities.ToArray());
            return View();
                }
            }
            return View("Error: project exist or path doesn't exist");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
 
    }
}
