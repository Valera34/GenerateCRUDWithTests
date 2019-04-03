using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace FrameworkForGenerationCRUD
{
    public class Parser
    {
        public Model[] parseString(string all)
        {
        List<Model> result = new List<Model>();
        all = all.Replace(" ", String.Empty);
        string[] lines = all.Split("@");
        foreach (string str in lines){
      //  Console.WriteLine(str);
        string begin ="Entity:";
        string end ="(";
        Model model= new Model();
        model.Name=ModelTemplate.ToUpperFirst(getStr(str,begin,end));
        begin="(";
        end=";";
        string fields=getStr(str,begin,end);
        model.Field=getFields(fields).ToArray();
        result.Add(model);
        }
        return result.ToArray();
        }
        public List<Field> getFields(string fields){
            string str="";
            List<Field> array = new List<Field>();
            Field f=new Field();
            foreach (char c in fields)
        {
            if(c.Equals(':')){
            f.Name=ModelTemplate.ToUpperFirst(str);
            str="";
            }
            else if(c.Equals(',')|| c.Equals(')')){
            
             if (str=="string"){
             f.Type=Types.String;
             }
            else if(str=="integer"){
             f.Type=Types.Int;
            }
            else if(str=="double"){
             f.Type=Types.Double;  
            }
             else if(str=="bool"){
             f.Type=Types.Bool;  
             }
             else if(str=="char"){
             f.Type=Types.Char;
             }
            else{
             f.Type=Types.String;
            }
            
            array.Add(f);
            f=new Field();
            str="";
            }
            else{
            str+=c;
            }
        }
            return array;
        }

        public string getStr(string str,string begin,string end){
        if (str.Contains(begin) && str.Contains(end))
        {
        int Start = str.IndexOf(begin, 0) + begin.Length;
        int End = str.IndexOf(end, Start);
        return str.Substring(Start, End - Start);
        }
        return "";
        }

        public void GenerateTxt(string str,string pr)
        {
            string path = System.IO.Directory.GetCurrentDirectory()+@"\Info.txt";
            string project = System.IO.Directory.GetCurrentDirectory()+@"\ProjectName.txt";
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            file.Directory.Create(); 
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(str);
            }
            System.IO.FileInfo projectName = new System.IO.FileInfo(project);
            file.Directory.Create(); 
            using (StreamWriter sw = new StreamWriter(project, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(pr);
            }
        }

    }
   public class Model{
        public string Name { get; set; }
        public Field[] Field { get; set; }
        public Model(string name, Field[] field)
        {
            this.Name = name;
            this.Field = field;
        }

        public Model()
        {
        }
    }

}