using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumCSharpFramework.utilities;
using SeleniumCSharpFramework.PageObjects;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace SeleniumCSharpFramework.test
{

    [Parallelizable(ParallelScope.Children)]
    public class SeleniumLearning : BaseClass
    {
        // run all data sets in parallel  --- [Parallelizable(ParallelScope.All)] in test level
        // run all test present under same class in parallel  ----  [Parallelizable(ParallelScope.Children)] in class level
        // run all test files present under project in parallel  --- add this on class level for each test file   [Parallelizable(ParallelScope.Self)]

        /* Run smoke test cases from command line: 
         1. Go to the project path and open cmd
         2. dotnet test SeleniumCSharpFramework.csproj --filter TestCategory=Smoke -- will run only smoke test case
         3.  dotnet test SeleniumCSharpFramework.csproj -- will run all the test cases


         */

        [Test,Category("Smoke")]
        [TestCase("rahulshettyacademy", "learning", "Consultant")]
        [TestCase("rahulshettyacademy", "learning", "Consultant")]
        [Parallelizable(ParallelScope.All)]  // To Run all data sets parallely for a test case
        public void EndToEndFlow(String username, String password, String userrole)


        {
           
            Loginpage loginpage = new Loginpage(GetDriver());
            
            string title = GetDriver().Title;
            TestContext.Progress.WriteLine("Title is :" + title);


            /* Data driven apraoch :
             * using NUNIT Test Case annotation
             * 
             *  we are not creating a new page object here for product details page , we are returning the object for ProductDetails from the login page*/
             

        ProductDetails products = loginpage.ValidLogin(username, password, userrole);
            products.addProduct();
            
        }  


        [Test, TestCaseSource(nameof(AddTestData))]
        [Parallelizable(ParallelScope.All)]  //To Run all data sets parallely for a test case
        public void EndToEndFlowwithTestDataSource(String username, String password, String userrole)


        {

            Loginpage loginpage = new Loginpage(GetDriver());

            string title = GetDriver().Title;
            TestContext.Progress.WriteLine("Title is :" + title);

            /*Make sure you right click on json file and choose  copy to output directory, so that there is not error and complied json file is available in output path*/
            /* Data driven apraoch :
             * using NUNIT TestCaseSource annotation
             */
            // we are not creating a new page object here for product details page , we are returning the object for ProductDetails from the login page
            ProductDetails products = loginpage.ValidLogin(username, password, userrole);
            products.addProduct();

            /*Extracting Data array from JSON*/

            TestContext.Progress.WriteLine(getJSONOObject().ExtractDataArray("products"));

        }


      public static IEnumerable<TestCaseData> AddTestData()
        {
            /*Taking the user data from json for the following line*/
            yield return new TestCaseData(getJSONOObject().ExtractData("username"), getJSONOObject().ExtractData("password"), getJSONOObject().ExtractData("userrole"));
            yield return new TestCaseData("rahulshettyacademy", "learning", "Consultant");
            yield return new TestCaseData("rahulshettyacademy", "learning", "Consultant");
        }
    }
}