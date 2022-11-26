using RentACarAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACarAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainer, string fileName); //storage service deki metodun birebir karşılığı bu  hangi concreate temsil ediyorsaak ondaki hasfile metodu bununla çağırmış olacağız. delegateler bir refereans türlüdür biza bir referens sağlıyor. yani nesne oluşturabiliyoruz
       protected  async Task<string> FileRenameAsync(string path, string fileName,HasFile hasfilemethod, bool first = true) // sadece kalıtımsal olarak erişilsin.
        {
            try
            {
                string newFileName = await Task.Run<string>(async () =>
                {
                    string extension = Path.GetExtension(fileName);

                    string newFileName = string.Empty;


                    if (first)
                    {
                        string oldName = Path.GetFileNameWithoutExtension(fileName);
                        newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";

                    }
                    else
                    {
                        newFileName = fileName;
                        int index = newFileName.IndexOf("-");
                        if (index == -1)
                        {
                            newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
                        }
                        else
                        {
                            int lastIndex = 0;
                            while (true)
                            {
                                lastIndex = index;
                                index = newFileName.IndexOf("-", index + 1);
                                if (index == -1)
                                {
                                    index = lastIndex;
                                    break;
                                }
                            }



                            int indexNo = newFileName.IndexOf(".");
                            string fileNo = newFileName.Substring(index + 1, indexNo - index - 1);
                            if (int.TryParse(fileNo, out int _fileNO))
                            {
                                _fileNO++;
                                newFileName = newFileName.Remove(index + 1, indexNo - index - 1).Insert(index + 1, _fileNO.ToString());

                            }
                            else
                            {
                                newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
                            }

                        }
                    }




                    //if (File.Exists($"{path}\\{newFileName}"))
                    if(hasfilemethod(path,newFileName))
                        return await FileRenameAsync(path, newFileName,hasfilemethod, false);
                    else
                        return newFileName;

                });
                return newFileName;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
