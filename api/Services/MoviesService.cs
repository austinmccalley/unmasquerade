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
  private readonly TMDBService _tmdbService;
  private readonly TMDbClient _tmdbClient;

  public MoviesService(IOptions<UnmasqueradeDatabaseSettings> settings, IOptions<TmdbSettings> tmdbSettings, TMDBService tMDBService)
  {
    var client = new MongoClient(settings.Value.ConnectionString);
    var database = client.GetDatabase(settings.Value.DatabaseName);
    _moviesCollection = database.GetCollection<Movie>(settings.Value.MoviesCollectionName);
    _tmdbClient = new TMDbClient(tmdbSettings.Value.ApiKey);

    _tmdbService = tMDBService;
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
    // Check if the movie is already in the database
    Movie? movieInDatabase = await _moviesCollection.Find(m => movie.TMDBId == m.TMDBId && movie.Title == m.Title ).FirstOrDefaultAsync();

    if (movieInDatabase != null)
    {
      return movieInDatabase;
    }

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
    else
    {
      // If we don't have the movie in the database, we will search for it in the TMDB API
      List<TmdbMovie> tmdbMovies = await _tmdbService.SearchMoviesAsync(title);

      // Convert the TmdbMovies to Movies
      foreach (TmdbMovie tmdbMovie in tmdbMovies)
      {
        Movie movie = new Movie(tmdbMovie);
        movies.Add(movie);
      }

      // Save the movies in the database
      for (int i = 0; i < movies.Count; i++)
      {
        movies[i] = await CreateMovieAsync(movies[i]);
      }

      return movies;
    }


  }

}