using System.Web.Http;
using Nereus.Models;

namespace Nereus.Controllers
{
   public class QueriesController : ApiController
   {
      private readonly NereusDb _database;

      public QueriesController(NereusDb database)
      {
         _database = database;
      }

      // DELETE api/queries/5
      [HttpDelete]
      public void Delete(int id)
      {
         _database.ProjectQueries.Remove(_database.ProjectQueries.Find(id));
         _database.SaveChanges();
      }
   }
}
