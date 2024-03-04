using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UnmasqueradeApi.Core;
using UnmasqueradeApi.Models;

namespace UnmasqueradeApi.Services;

public class MoviesService {
  private readonly IMongoCollection<Movie> _moviesCollection;

  public MoviesService(IOptions<UnmasqueradeDatabaseSettings> settings)
  {
    var client = new MongoClient(settings.Value.ConnectionString);
    var database = client.GetDatabase(settings.Value.DatabaseName);
    _moviesCollection = database.GetCollection<Movie>(settings.Value.MoviesCollectionName);
  }

  public async Task<List<Movie>> GetMoviesAsync()
  {
    return await _moviesCollection.Find(movie => true).ToListAsync();
  }

  public async Task<Movie?> GetMovieAsync(string id)
  {
    return await _moviesCollection.Find(movie => movie.Id == id).FirstOrDefaultAsync();
  }

  public async Task<Movie> CreateMovieAsync(Movie movie)
  {
    await _moviesCollection.InsertOneAsync(movie);
    return movie;
  }

  public async Task UpdateMovieAsync(string id, Movie movieIn)
  {
    await _moviesCollection.ReplaceOneAsync(movie => movie.Id == id, movieIn);
  }

  public async Task DeleteMovieAsync(string id)
  {
    await _moviesCollection.DeleteOneAsync(movie => movie.Id == id);
  }

}