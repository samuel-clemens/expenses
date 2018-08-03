using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Expenses.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Expenses.Api.Users
{
    public static class AddUser
    {
        [FunctionName("AddUser")]
        public static HttpResponseMessage Run(
            [HttpTrigger( 
                AuthorizationLevel.Function,
                "get", 
                "post", 
                Route = "users/add/{login}/{password}")
            ]HttpRequestMessage req, 
            string login,
            string password,
            [Table("ExpensesApp")]ICollector<User> outTable,
            [Table("ExpensesApp", "user_{login}", "{password}")] User entity,
            TraceWriter log)
        {
            log.Info("Request to AddUser");

            if (login == null)
            {
                log.Info("AddUser response: BadRequest - login is null");
                return req.CreateResponse(
                    statusCode: HttpStatusCode.BadRequest,
                    value: "Please pass a login on the query string or in the request body");

            }
            if (password == null)
            {
                log.Info("AddUser response: BadRequest - password is null");
                return req.CreateResponse(
                    statusCode: HttpStatusCode.BadRequest,
                    value: "Please pass a password on the query string or in the request body");
            }
            if (entity != null)
            {
                log.Info($"AddUser response: BadRequest - entity with PK=user_{login} and RK={password} already exists");
                return req.CreateResponse(
                    statusCode: HttpStatusCode.BadRequest,
                    value: "User with given login and password already exists"
                    );
            }

            outTable.Add(new User()
            {
                PartitionKey = $"user_{login}",
                RowKey = password,
                Login = login,
                Password = password
            });

            log.Info($"AddUser response: Created - entity with PK=user_{login} and RK={password}");
            return req.CreateResponse(HttpStatusCode.Created);
        }
    }
}
