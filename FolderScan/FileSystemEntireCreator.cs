using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderScan
{
    internal class FileSystemEntireCreator
    {
        public List<string> GetFSystemEntire(string startPath)
        {
            List<string> fSysElemsList = new List<string>();

            try
            {
                string[] folders = Directory.GetDirectories(startPath);
                foreach(string folder in folders)
                {
                    fSysElemsList.Add(folder);
                    fSysElemsList.AddRange(GetFSystemEntire(folder));
                }

                string[] files = Directory.GetFiles(startPath);
                foreach(string file in files)
                {
                    fSysElemsList.Add(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fSysElemsList;
        }
    }
}
