using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UnmasqueradeApi.Models;
  public class Movie
  {

    public Movie(TmdbMovie tmdbMovie)
    {
      Title = tmdbMovie.Title;
      TMDBId = tmdbMovie.Id.ToString();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Title { get; set; } = null!;
    public string TMDBId { get; set; } = null!;
  }