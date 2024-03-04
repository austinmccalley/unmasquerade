using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using UnmasqueradeApi.Core;
using UnmasqueradeApi.Models;

namespace UnmasqueradeApi.Services;

public class MoviesService
{
  private readonly IMongoCollection<Movie> _moviesCollection;
  private readonly TMDbClient _tmdbClient;

  public MoviesService(IOptions<UnmasqueradeDatabaseSettings> settings, IOptions<TmdbSettings> tmdbSettings)
  {
    var client = new MongoClient(settings.Value.ConnectionString);
    var database = client.GetDatabase(settings.Value.DatabaseName);
    _moviesCollection = database.GetCollection<Movie>(settings.Value.MoviesCollectionName);

    _tmdbClient = new TMDbClient(tmdbSettings.Value.ApiKey);
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

  public async Task<List<Movie>> SearchMoviesAsync(string title)
  {
    // First look if we have the movie in the database
    List<Movie> movies = new List<Movie>();

    // Do a general search of the movie in the database
    movies = await _moviesCollection.Find(movie => movie.Title.Contains(title)).ToListAsync();

    if (movies.Count > 0)
    {
      return movies;
    }

    // If we don't have the movie in the database, we will search for it in the TMDB API
    SearchContainer<SearchMovie> results = await _tmdbClient.SearchMovieAsync(title);

    foreach (SearchMovie result in results.Results)
    {
      Movie movie = new Movie
      {
        Title = result.Title,
        TMDBId = result.Id.ToString()
      };
      movies.Add(movie);
    }

    // Save the movies in the database
    foreach (Movie movie in movies)
    {
      await _moviesCollection.InsertOneAsync(movie);
    }

    return movies;
  }

}