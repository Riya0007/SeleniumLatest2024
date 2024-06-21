using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCSharpFramework.utilities
{
    public class JSONReader
    {

        public JSONReader()
        {

        }


        public string ExtractData(string token)

        {

            /*ReadAllText methods under File class, will read all the text from JSON and return on string at run time*/
            String myJSONString = File.ReadAllText("utilities/testData.json");

            /*This string will be parsed with help of JToken class and parse method and it will return parse object*/
            var JSONObject = JToken.Parse(myJSONString);

            /*Select token method with token name will return value which has string return type*/
            return (JSONObject.SelectToken(token).Value<string>());



        }

        public string[] ExtractDataArray(string token)
        {

          string myJSON =  File.ReadAllText("utilities/testData.json");
           var JSONOBJ = JToken.Parse(myJSON);

            /*While fetching array from JSON, first SelectTokens and Values will be used , then it will be converted into list.
             Then it will be converted into the array.
            Reurn type of the method will be array of string
             
             */

            List<String> productlist = JSONOBJ.SelectTokens(token).Values<string>().ToList();
           return productlist.ToArray();


        }




    }
}
