using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;


namespace DataService
{
    public class Data : IData
    {
        public List<Setting> GetData(string searchField)
        {
            List<Setting> resultList = new List<Setting>();
            Setting temp = new Setting();
            string[] keysToSearch=searchField.Split(' ');
            WebClient wc = new WebClient()
            {
                Encoding = Encoding.UTF8
            };
            string response = wc.DownloadString("http://mighttrust.me/testcase/api/list/getitems");
            var json= new JavaScriptSerializer();
            List<Setting> testData = json.Deserialize<List<Setting>>(response);
            foreach(Setting item in testData)
            {
                foreach(string itemToSearch in keysToSearch)
                {
                    if (item.Key.Contains(itemToSearch))
                    {
                        if (!resultList.Contains(item))
                        {
                            resultList.Add(item);
                        }
                    }
                }
            }
            string resultToDb = json.Serialize(resultList);
            DataBase.Database.InsertLastResult(searchField, resultToDb);
            return resultList;
        }

        public List<Setting> GetLastResult()
        {
            List<Setting> result = new List<Setting>();
            result = DataBase.Database.GetLastData();
            return result;
        }
    }
}
