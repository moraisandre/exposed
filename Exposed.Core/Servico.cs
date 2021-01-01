using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Exposed.Core
{
    class Orfaos
    {
        //private string arquivo;
        //private string projeto;

        public string arquivo
        {
            get { return arquivo; }
            set { arquivo = value; }
        }

        public string projeto
        {
            get { return projeto; }
            set { projeto = value; }
        }

    }
    class Servico
    {
        public List<FileInfo> buscaCsproj(string caminho)
        {
            DirectoryInfo dirCaminho = new DirectoryInfo(caminho);
            List<FileInfo> lstCsproj = new List<FileInfo>();


            foreach (FileInfo file in dirCaminho.GetFiles("*.csproj"))
            {
                lstCsproj.Add(file);
            }

            return lstCsproj;
        }

        public List<Orfaos> percorreArquivos(string caminho, string extensoes)
        {
            List<Orfaos> lstOrfao = new List<Orfaos>();
            var lstcsproj = buscaCsproj(caminho);
            List<string> lstExt = extensoes.Split(',').ToList();


            foreach (var csp in lstcsproj)
            {
                DirectoryInfo dirCsproj = new DirectoryInfo(csp.DirectoryName);

                foreach (string v in lstExt)
                {
                    foreach (FileInfo f in dirCsproj.GetFiles(v, SearchOption.AllDirectories))
                    {
                        Orfaos arquivo = new Orfaos();
                        string arq = f.FullName.Replace(csp.DirectoryName, "");
                        string csproj = csp.Name.Replace(".csproj", "");

                        arquivo.arquivo = arq;
                        arquivo.projeto = csproj;

                        lstOrfao.Add(arquivo);

                    }
                }
            }


            return lstOrfao;
        }
        public List<Orfaos> percorreProj(List<FileInfo> csproj, List<Orfaos> arquivos)
        {
            List<Orfaos> lstComparacao = new List<Orfaos>();
            List<Orfaos> lstOraos = new List<Orfaos>();


            foreach (var file in csproj)
            {

                FileStream arq = new FileStream(file.FullName, FileMode.Create);
                XmlReader xread = XmlReader.Create(arq);

                string projeto = string.Empty;
                string arquivo = string.Empty;

                //List<String> find = new List<string>();

                while (xread.Read())
                {
                    if ((xread.Name == "Compile") && (xread.NodeType == XmlNodeType.Element))
                    {
                        Orfaos orf = new Orfaos();

                        orf.arquivo = xread.GetAttribute("Include");
                        orf.projeto = xread.ReadInnerXml();

                        lstComparacao.Add(orf);
                    }
                    if ((xread.Name == "Content") && (xread.NodeType == XmlNodeType.Element))
                    {
                        Orfaos orf = new Orfaos();

                        orf.arquivo = xread.GetAttribute("Include");
                        orf.projeto = xread.ReadInnerXml();

                        lstComparacao.Add(orf);
                    }

                }

            }


            return null;
        }
    }
}
