using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
namespace FrameworkForGenerationCRUD
{
    public class DataTemplate
    {
        public void GenerateClass(string path, string className, string[] entities)
        {
            string startupPath = path + @"\Data\" + className + "Context.cs";
            System.IO.FileInfo file = new System.IO.FileInfo(startupPath);
            file.Directory.Create(); 
            using (StreamWriter sw = new StreamWriter(startupPath, false, System.Text.Encoding.Default))
            {

                sw.WriteLine("using Microsoft.EntityFrameworkCore;");
                sw.WriteLine("using "+ className+".Models;");
                sw.WriteLine("namespace "+ className+".Data {");
                sw.WriteLine("public class " + className + "Context : DbContext{");
                sw.WriteLine(" public " + className + "Context (DbContextOptions<"+className+"Context> options) : base(options)" );
                sw.WriteLine("{");
                sw.WriteLine("}");
                foreach (string entity in entities)
                {
                    sw.WriteLine(" public DbSet<" + ToUpperFirst(entity) + "> " +  ToUpperFirst(entity) + "s { get; set; }");
                }
                sw.WriteLine("}");
                sw.WriteLine("}");
            }
        }
        static string ToUpperFirst(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }
            char[] word = fieldName.ToCharArray();
            word[0] = char.ToUpper(word[0]);
            return new string(word);
        }

    }

}