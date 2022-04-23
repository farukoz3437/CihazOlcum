using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.HelperMethods
{
    public static class CreateZipFile
    {
        public static  bool CreateZip(List<string> fileNames,string zipName,string filePath)
        {
            try
            {
                string fileName = zipName + ".zip";
                var fileList = new List<string>();

                var tempOutput= "wwwroot\\assets\\media\\zipfile\\" + fileName;

                if (System.IO.File.Exists(tempOutput))
                {
                    System.IO.File.Delete(tempOutput);
                }

                foreach (var item in fileNames)
                {
                    if (fileName != null && fileName != "")
                    {
                        fileList.Add(filePath+ item);
                    }
                }


                using (ZipOutputStream oZipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
                {
                    oZipOutputStream.SetLevel(9);
                    byte[] buffer = new byte[4096];


                    for (int i = 0; i < fileList.Count; i++)
                    {
                        ZipEntry zipEntry = new ZipEntry(Path.GetFileName(fileList[i]));
                        zipEntry.DateTime = DateTime.Now;
                        zipEntry.IsUnicodeText = true;
                        oZipOutputStream.PutNextEntry(zipEntry);

                        using (FileStream fs = System.IO.File.OpenRead(fileList[i]))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                oZipOutputStream.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }

                    }

                    oZipOutputStream.Finish();
                    oZipOutputStream.Flush();
                    oZipOutputStream.Close();

                }

                byte[] finalResut = System.IO.File.ReadAllBytes(tempOutput);


                if (finalResut == null || !finalResut.Any())
                {
                    throw new Exception(string.Format("Hiç birşey bulunamadı"));
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
