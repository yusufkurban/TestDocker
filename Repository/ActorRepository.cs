using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MicroKubernete.Database;
using MicroKubernete.Model;

namespace MicroKubernete.Repository
{
   public class ActorRepository : IRepository<Actor>
   {
      private string connectionString;
      private readonly StoreDataContext _db;
      public ActorRepository(IConfiguration configuration)
      {
         connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
      }

      internal IDbConnection Connection
      {
         get
         {
            return new NpgsqlConnection(connectionString);
         }
      }

      public int Add(Actor item)
      {
         int id = -1;
         using (IDbConnection dbConnection = Connection)
         {
            dbConnection.Open();
            id=dbConnection.Query<int>(@"INSERT INTO actor(first_name,last_name,last_update) VALUES(@first_name,@last_name,@last_update) RETURNING actor_id", new { first_name = item.first_name,last_name=item.last_name,last_update=item.last_update }).SingleOrDefault();
         }
         return Convert.ToInt32(id);
      }

      public IEnumerable<Actor> FindAll()
      {
         using (IDbConnection dbConnection = Connection)
         {
            dbConnection.Open();
            return dbConnection.Query<Actor>("SELECT * FROM actor");
         }
      }

      public Actor FindByID(int id)
      {
         using (IDbConnection dbConnection = Connection)
         {
            dbConnection.Open();
            return dbConnection.Query<Actor>("SELECT * FROM actor WHERE actor_id = @Id", new { Id = id }).FirstOrDefault();
         }
      }

      public void Remove(int id)
      {
         using (IDbConnection dbConnection = Connection)
         {
            dbConnection.Open();
            dbConnection.Execute("DELETE FROM actor WHERE actor_id=@Id", new { Id = id });
         }
      }

      public void Update(Actor item)
      {
         using (IDbConnection dbConnection = Connection)
         {
            dbConnection.Open();
            dbConnection.Query("UPDATE actor SET first_name = @first_name,  last_name  = @last_name, last_update= @last_update WHERE actor_id = @Id", item);
         }
      }
   }
}
