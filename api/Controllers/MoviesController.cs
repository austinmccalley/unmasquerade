using Microsoft.AspNetCore.Mvc;
using UnmasqueradeApi.Models;
using UnmasqueradeApi.Services;

namespace UnmasqueradeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{

  private readonly MoviesService _moviesService;

  public MoviesController(MoviesService moviesService)
  {
    _moviesService = moviesService;
  }

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<Movie>> GetMovieAsync(string id)
  {
    var movie = await _moviesService.GetMovieAsync(id);
    if (movie == null)
    {
      return NotFound();
    }
    return movie;
  }

  [HttpGet]
  public async Task<ActionResult<List<Movie>>> GetMoviesAsync()
  {
    return await _moviesService.GetMoviesAsync();
  }

  [HttpPost]
  public async Task<ActionResult<Movie>> CreateMovieAsync(Movie movie)
  {
    await _moviesService.CreateMovieAsync(movie);
    return CreatedAtAction(nameof(GetMovieAsync), new { id = movie.Id }, movie);
  }

  [HttpPut("{id:length(24)}")]
  public async Task<IActionResult> UpdateMovieAsync(string id, Movie movieIn)
  {
    var movie = await _moviesService.GetMovieAsync(id);
    if (movie == null)
    {
      return NotFound();
    }
    await _moviesService.UpdateMovieAsync(id, movieIn);
    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> DeleteMovieAsync(string id)
  {
    var movie = await _moviesService.GetMovieAsync(id);
    if (movie == null)
    {
      return NotFound();
    }
    await _moviesService.DeleteMovieAsync(id);
    return NoContent();
  }


}