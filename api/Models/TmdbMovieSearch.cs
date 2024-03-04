namespace UnmasqueradeApi.Models;

public class TmdbMovieSearch {
  public int Page { get; set; }
  public List<TmdbMovie> Results { get; set; } = new List<TmdbMovie>();
  public int TotalPages { get; set; }
  public int TotalResults { get; set; }
}