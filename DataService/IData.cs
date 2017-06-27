using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DataService
{
    [ServiceContract]
    public interface IData
    {

        //Method to get data by filtering
        [OperationContract]
        List<Setting> GetData(string searchField);

        //Method to get last data from DB
        [OperationContract]
        List<Setting> GetLastResult();

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class Setting
    {
        string _key;
        string _value;
        string _outputValue;

        [DataMember]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DataMember]
        public string OutputValue
        {
            get { return _outputValue= _key + " : " + _value;  }
            set { }
        }
    }
}
