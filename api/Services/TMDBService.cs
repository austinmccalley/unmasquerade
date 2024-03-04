using Microsoft.Extensions.Options;
using RestSharp;
using UnmasqueradeApi.Core;
using UnmasqueradeApi.Models;
using Newtonsoft.Json;

namespace UnmasqueradeApi.Services;

public class TMDBService
{

  private readonly string _apiKey;

  public TMDBService(IOptions<TmdbSettings> tmdbSettings)
  {
    var apiKey = tmdbSettings.Value.ApiKey;

    if (apiKey == null)
    {
      throw new ArgumentNullException("TMDB API Key is not set");
    }

    _apiKey = apiKey;
  }

  public async Task<TmdbMovie> GetMovieAsync(string id)
  {
    // var options = new RestClientOptions("https://api.themoviedb.org/3/movie/movie_id?language=en-US");
    var options = new RestClientOptions("https://api.themoviedb.org/3/movie/" + id + "?language=en-US");
    var client = new RestClient(options);
    var request = new RestRequest("");
    request.AddHeader("accept", "application/json");
    request.AddHeader("Authorization", "Bearer " + _apiKey);
    var response = await client.GetAsync<TmdbMovie>(request);

    if (response == null)
    {
      throw new Exception("TMDB API returned null");
    }

    return response;
  }

  public async Task<List<TmdbMovie>> SearchMoviesAsync(string title)
  {

    RestClientOptions options = new RestClientOptions("https://api.themoviedb.org/3/search/movie?language=en-US");
    RestClient client = new RestClient(options);
    RestRequest request = new RestRequest("");
    request.AddHeader("accept", "application/json");
    request.AddHeader("Authorization", "Bearer " + _apiKey);
    request.AddQueryParameter("query", title);
    RestResponse response = await client.GetAsync(request);

    if (response.Content == null)
    {
      throw new Exception("TMDB API returned null");
    }

    // Serialize response.content to be an object with the property of results: List<TmdbMovie>
    TmdbMovieSearch? tmdbMovies = JsonConvert.DeserializeObject<TmdbMovieSearch>(response.Content);

    if (tmdbMovies == null)
    {
      throw new Exception("TMDB API returned null");
    }

    return tmdbMovies.Results;
  }



}