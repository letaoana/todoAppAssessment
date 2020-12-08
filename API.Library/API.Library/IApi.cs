using RestSharp;
using Todo.API.Client.Models;

namespace API.Library
{
    public interface IApi
    {
        IRestResponse AuthenticateUser(User register);
        IRestResponse CompleteToDo(long id, string token, bool isComplete);
        IRestResponse CreateToDo(ToDo todo, string token);
        IRestResponse GetToDo(long id, string token);
        IRestResponse DeleteToDo(long id, string token);
        IRestResponse GetToDos(string token);
        IRestResponse GetUser(long id);
        IRestResponse GetUsers(string token);
        IRestResponse RegisterUser(User register);
        IRestResponse UpdateToDo(ToDo todo, long id, string token);
    }
}