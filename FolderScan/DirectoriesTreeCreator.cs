using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    internal class DirectoriesTreeCreator
    {
        public List<string> CreateDirectoriesTree(List<string> fSystemEntire)
        {
            List<string> directoriesTree = new List<string>();
            int level;

            directoriesTree.Add($"-{Path.GetFullPath(@".")}");

            foreach(string fSysElem in fSystemEntire)
            {
                string[] pathElementsArr = fSysElem.Split(@"\");
                level = pathElementsArr.Length - 1;
                string indent = "--";

                for (int i = 1; i < level; i++)
                {
                    indent += "--";
                }

                directoriesTree.Add($"{indent}{pathElementsArr[^1]}");
            }

            return directoriesTree;
        }
    }
}
