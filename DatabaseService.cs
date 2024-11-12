using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LearningApp
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.ExecuteAsync("CREATE TABLE IF NOT EXISTS Words (Text TEXT PRIMARY KEY)").Wait();
        }

        public Task<int> AddWordAsync(string word)
        {
            return _database.ExecuteAsync("INSERT OR IGNORE INTO Words (Text) VALUES (?)", word);
        }

        public Task<string> GetWordAsync(string word)
        {
            return _database.ExecuteScalarAsync<string>("SELECT Text FROM Words WHERE Text = ?", word);
        }

        async public Task<List<Word>> GetWordsAsync()
        {
            var query = "SELECT Text FROM Words";  // Query to fetch all rows
            //await _database.OpenAsync();  // Open the connection if not open already

            // Use Dapper to execute the query and map the results directly to a collection
            var words = await _database.QueryAsync<Word>(query);

            return words;  // Return the collection of words
        }
    }
}
