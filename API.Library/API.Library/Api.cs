using RestSharp;
using Todo.API.Client.Models;

namespace API.Library
{
    public class Api : IApi
    {
        private readonly IRestClient client;
        private IRestRequest request;

        public Api(string baseUrl = "http://localhost:20885")
        {
            client = new RestClient(baseUrl);
            client.ClearHandlers();
        }

        public IRestResponse RegisterUser(User register)
        {
            request = new RestRequest
            {
                Resource = "/api/auth/register"
            };
            request.AddJsonBody(register);
            return client.Execute(request, Method.POST);
        }

        public IRestResponse AuthenticateUser(User register)
        {
            request = new RestRequest
            {
                Resource = "/api/auth"
            };
            request.AddJsonBody(register);
            return client.Execute(request, Method.POST);
        }

        public IRestResponse GetUser(long id)
        {
            request = new RestRequest
            {
                Resource = $"/api/users/{id}"
            };
            return client.Execute(request, Method.GET);
        }

        public IRestResponse GetUsers(string token)
        {
            request = new RestRequest
            {
                Resource = $"/api/users"
            };
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.GET);
        }

        public IRestResponse CreateToDo(ToDo todo, string token)
        {
            request = new RestRequest
            {
                Resource = "/api/todos"
            };
            request.AddJsonBody(todo);
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.POST);
        }

        public IRestResponse GetToDos(string token)
        {
            request = new RestRequest
            {
                Resource = "/api/todos"
            };
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.GET);
        }

        public IRestResponse GetToDo(long id, string token)
        {
            request = new RestRequest
            {
                Resource = $"/api/todos/{id}"
            };
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.GET);
        }

        public IRestResponse DeleteToDo(long id, string token)
        {
            request = new RestRequest
            {
                Resource = $"/api/todos/{id}"
            };
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.DELETE);
        }

        public IRestResponse UpdateToDo(ToDo todo, long id, string token)
        {
            request = new RestRequest
            {
                Resource = $"/api/todos/{id}"
            };
            request.AddJsonBody(todo);
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.PUT);
        }

        public IRestResponse CompleteToDo(long id, string token, bool isComplete)
        {
            request = new RestRequest
            {
                Resource = $"/api/todos/{id}/IsComplete"
            };
            request.AddJsonBody(new { Value = isComplete });
            request.AddHeader("Authorization", $"bearer {token}");
            return client.Execute(request, Method.PUT);
        }
    }
}
