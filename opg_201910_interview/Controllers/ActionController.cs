using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using opg_201910_interview.Models;

namespace opg_201910_interview.Controllers
{
    public class ActionController : Controller
    {
        [HttpPost]
        public JsonResult ClientList(string strClient)
        {
            string[] _arrayListA = { "shovel", "waghor", "blaze", "discus" };
            string[] _arrayListB = { "orca", "widget", "eclair", "talon" };

            if (strClient == "ClientA")
            {
                ClientMdl _CAMdl = GatherDataClient(_arrayListA, strClient, "1001");
                WriteJson(_CAMdl, null, strClient);
            }
            else if (strClient == "ClientB")
            {
                ClientMdl _CBMdl = GatherDataClient(_arrayListB, strClient, "2001");
                WriteJson(_CBMdl, null, strClient);
            }
            else
            {
                ClientsMdl _CMdl = new ClientsMdl();
                _CMdl.ClientA = GatherDataClient(_arrayListA, "ClientA", "1001");
                _CMdl.ClientB = GatherDataClient(_arrayListB, "ClientB", "2001");
                WriteJson(null, _CMdl, strClient);
            }

            return Json("Done");
        }

        public ClientMdl GatherDataClient(string[] strList, string strClient, string strClientId)
        {
            List<FilesMdl> _lstTemp = new List<FilesMdl>();
            List<FilesMdl> _lstFinal = new List<FilesMdl>();
            string[] _fileNames = Directory.GetFiles(@"UploadFiles/" + strClient);
            if (strClient.Equals("ClientA"))
            {
                foreach (string _file in _fileNames)
                {
                    string[] _infos = _file.Split("\\")[1].Split(".")[0].Split("-");
                    if (_infos.Length == 4)
                    {
                        FilesMdl _OV = new FilesMdl();
                        DateTime _dt = new DateTime(Convert.ToInt32(_infos[1]), Convert.ToInt32(_infos[2]), Convert.ToInt32(_infos[3]));
                        _OV.Word = _infos[0];
                        _OV.StartDate = _dt.ToString("yyyy-MM-dd");
                        _lstTemp.Add(_OV);
                    }
                }
            }
            else
            {
                foreach (string _file in _fileNames)
                {
                    string[] _infos = _file.Split("\\")[1].Split(".")[0].Split("_");
                    if (_infos.Length == 2)
                    {
                        FilesMdl _OV = new FilesMdl();
                        DateTime _dt = new DateTime(Convert.ToInt32(_infos[1].Substring(0, 4)), Convert.ToInt32(_infos[1].Substring(4, 2)), Convert.ToInt32(_infos[1].Substring(6, 2)));
                        _OV.Word = _infos[0];
                        _OV.StartDate = _dt.ToString("yyyy-MM-dd");
                        _lstTemp.Add(_OV);
                    }
                }
            }

            if (_lstTemp.Count() > 0)
            {
                var _query = _lstTemp.Where(x => strList.Contains(x.Word)).ToList();
                foreach (string _word in strList)
                {
                    var _getRecord = _query.Where(x => x.Word.Equals(_word)).ToList();
                    if (_getRecord.Count() > 1)
                    {
                        foreach (var _row in _getRecord.OrderBy(x => x.StartDate))
                        {
                            FilesMdl _OV = new FilesMdl();
                            _OV.Word = _row.Word;
                            _OV.StartDate = _row.StartDate;
                            _lstFinal.Add(_OV);
                        }
                    }
                    else
                    {
                        FilesMdl _OV = new FilesMdl();
                        _OV.Word = _getRecord.First().Word;
                        _OV.StartDate = _getRecord.First().StartDate;
                        _lstFinal.Add(_OV);
                    }
                }
            }

            ClientInfoMdl _CIMdl = new ClientInfoMdl();
            _CIMdl.ClientId = strClientId;
            _CIMdl.FileDirectoryPath = "UploadFiles/" + strClient;
            _CIMdl.Files = _lstFinal;

            ClientMdl _CMdl = new ClientMdl();
            _CMdl.ClientInfoMdl = _CIMdl;

            return _CMdl;
        }

        public string WriteJson(ClientMdl clientMdl, ClientsMdl clientsMdl, string strClient)
        {
            string jsonObj = strClient != "Both" ?
                Newtonsoft.Json.JsonConvert.SerializeObject(clientMdl, Newtonsoft.Json.Formatting.Indented).Replace("ClientInfoMdl", strClient) :
                Newtonsoft.Json.JsonConvert.SerializeObject(clientsMdl, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText("appsettings.json", jsonObj);
            //System.IO.File.WriteAllText("appsettings.Client.json", jsonObj); //For new Json File

            return jsonObj;
        }
    }
}
