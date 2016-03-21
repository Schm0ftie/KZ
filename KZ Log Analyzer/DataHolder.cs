using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace KZLogAnalyzer.Data
{
    public class DataHolder
    {
        private List<Jump> ParsedJumps { get; set; }
        private XmlHandler oXmlHandler;
        private Jump SelectedJump, CompareJump;
        

        public DataHolder()
        {
            oXmlHandler = new XmlHandler();
        }

        public void Save(string path)
        {
            FileInfo oFileInfo = new FileInfo(path);
            if (oFileInfo.Exists)
            {
                Console.WriteLine("File already exist.");
                return;
            }

            oXmlHandler.Save(ParsedJumps, oFileInfo.FullName);
        }

        public void SetSelectedJump(Jump oJump)
        {
            SelectedJump = oJump;
        }

        public void SetComparedJump(Jump oJump)
        {
            CompareJump = oJump;
        }
    }
}
