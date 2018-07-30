using System.Threading.Tasks;
using MediatR;

namespace IQSoft.PersonList.Server.Extensions
{
    public static class TaskExtensions
    {
        public static Task Void(this Task<Unit> task)
        {
            return task;
        }
    }
}