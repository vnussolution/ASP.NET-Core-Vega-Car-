using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vega.Helpers;
using Vega.Models.Library;
using Vega.Models.Library.Dto;
using Vega.Services.Library;

namespace Vega.Controllers {

    [Route ("api/authors")]
    public class AuthorsController : Controller {

        private ILibraryRepository _libraryRepository;
        public AuthorsController (ILibraryRepository libraryRepository) {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors () {
            var authorsFromRepo = _libraryRepository.GetAuthors ();

            var authors = Mapper.Map<IEnumerable<AuthorDto>> (authorsFromRepo);

            return Ok (authors);
        }

        [HttpGet ("{id}", Name = "GetVegaAuthor")]
        public IActionResult GetAuthor (Guid id) {
            var authorsFromRepo = _libraryRepository.GetAuthor (id);

            if (authorsFromRepo == null) return NotFound ();

            var author = Mapper.Map<AuthorDto> (authorsFromRepo);
            return Ok (author);

        }

        [HttpPost]
        public IActionResult CreateAuthor ([FromBody] AuthorForCreationDto author) {
            if (author == null) return BadRequest ();

            var authorEntity = Mapper.Map<Author> (author);

            _libraryRepository.AddAuthor (authorEntity);
            if (!_libraryRepository.Save ()) {

                //Startup.cs will catch this.
                //advantages of doing this way are : handle error in one place,  
                //disadvantage: performance
                throw new Exception ("Creating an author failed on save");

                // disadvantage: handle error in many places
                //return StatusCode (500, "A problem with handling your request.");
            }
            var authorToReturn = Mapper.Map<AuthorDto> (authorEntity);
            return CreatedAtRoute ("GetVegaAuthor", new { id = authorToReturn.Id }, authorToReturn);
        }

        // this is to handle when user pass id in api to create author
        [HttpPost ("{id}")]
        public IActionResult BlockAuthorCreation (Guid id) {
            if (_libraryRepository.AuthorExists (id)) return new StatusCodeResult (StatusCodes.Status409Conflict);
            return NotFound ();
        }
    }
}