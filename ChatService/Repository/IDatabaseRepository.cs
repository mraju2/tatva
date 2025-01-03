using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatService.Repositories
{
    public interface IDatabaseRepository
    {
        Task<List<Dictionary<string, object>>> ExecuteQueryAsync(string sqlQuery);
    }
}
