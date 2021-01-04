using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

namespace Exposed.Core
{
    class Orfaos
    {
        public string Arquivo
        {
            get { return Arquivo; }
            set { Arquivo = value; }
        }

        public string Projeto
        {
            get { return Projeto; }
            set { Projeto = value; }
        }

    }
    class Servico
    {
        public List<FileInfo> ListaCSProj(string caminho)
        {
            return new DirectoryInfo(caminho).GetFiles("*.csproj").ToList();
        }

        public List<Orfaos> ObtemArquivosOrfaos(string caminho, string extensoes)
        {
            var arquivosOrfaos = new List<Orfaos>();

            ListaCSProj(caminho).ForEach(file =>
            {
                extensoes.Split(',').ToList().ForEach(ext =>
                {
                    new DirectoryInfo(file.DirectoryName).GetFiles(ext, SearchOption.AllDirectories).ToList().ForEach(fileInfo =>
                    {
                        arquivosOrfaos.Add(new Orfaos
                        {
                            Arquivo = fileInfo.FullName.Replace(file.DirectoryName, ""),
                            Projeto = file.Name.Replace(".csproj", "")
                        });
                    });
                });
            });

            return arquivosOrfaos;
        }

        public List<Orfaos> PercorreProj(List<FileInfo> csproj, List<Orfaos> arquivos, List<string> extensoes)
        {
            List<Orfaos> lstComparacao = new List<Orfaos>();
            List<Orfaos> lstOraos = new List<Orfaos>();

            foreach (string ext in extensoes)
            {
                foreach (var file in csproj)
                {

                    FileStream arq = new FileStream(file.FullName, FileMode.Create);
                    XmlReader xread = XmlReader.Create(arq);

                    string projeto = string.Empty;
                    string arquivo = string.Empty;

                    //List<String> find = new List<string>();

                    while (xread.Read())
                    {
                        if ((xread.Name == "Compile" || xread.Name == "Content") && (xread.NodeType == XmlNodeType.Element) && xread.GetAttribute("Include").Contains(ext))
                        {
                            Orfaos orf = new Orfaos
                            {
                                Arquivo = xread.GetAttribute("Include"),
                                Projeto = xread.ReadInnerXml()
                            };

                            lstComparacao.Add(orf);
                        }
                    }

                }
            }

            return null;
        }
    }
}
