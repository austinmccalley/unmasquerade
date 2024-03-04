namespace UnmasqueradeApi.Models;

/// <summary>
/// Represents a movie from the TMDb (The Movie Database) API.
/// </summary>
public class TmdbMovie
{

  public bool Adult { get; set; }
  public string? BackdropPath { get; set; }
  public string? BelongsToCollection { get; set; }
  public int Budget { get; set; }
  public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();

  public string? Homepage { get; set; }

  public int Id { get; set; }
  public string? ImdbId { get; set; }
  public string? OriginalLanguage { get; set; }
  public string? OriginalTitle { get; set; }
  public string? Overview { get; set; }
  public double? Popularity { get; set; }
  public string? PosterPath { get; set; }

  public List<TmdbProductionCompany> ProductionCompanies { get; set; } = new List<TmdbProductionCompany>();

  public List<TmdbProductionCountry> ProductionCountries { get; set; } = new List<TmdbProductionCountry>();

  public string? ReleaseDate { get; set; }
  public int? Revenue { get; set; }
  public int? Runtime { get; set; }
  public List<TmdbSpokenLanguage> SpokenLanguages { get; set; } = new List<TmdbSpokenLanguage>();
  public string? Status { get; set; }
  public string? Tagline { get; set; }
  public string Title { get; set; } = null!;
  public bool Video { get; set; }
  public double? VoteAverage { get; set; }
  public int VoteCount { get; set; }

  public class TmdbProductionCountry
  {
    public string? Iso3166_1 { get; set; }
    public string Name { get; set; } = null!;
  }

  public class TmdbProductionCompany
  {
    public int Id { get; set; }
    public string? LogoPath { get; set; }
    public string Name { get; set; } = null!;
    public string? OriginCountry { get; set; }
  }

  public class TmdbSpokenLanguage
  {
    public string? EnglishName { get; set; }
    public string? Iso639_1 { get; set; }
    public string Name { get; set; } = null!;
  }
}