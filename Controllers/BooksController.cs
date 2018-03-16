using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vega.Models.Library;
using Vega.Models.Library.Dto;
using Vega.Services.Library;

namespace Vega.Controllers {

    [Route ("api/authors/{authorId}/books")]
    public class BooksController : Controller {
        private ILibraryRepository _libraryRepository;

        public BooksController (ILibraryRepository libraryRepository) {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetBooksByAuthor (Guid authorId) {
            if (!_libraryRepository.AuthorExists (authorId)) {
                return NotFound ();
            }
            var booksForAuthorFromRepo = _libraryRepository.GetBooksForAuthor (authorId);

            var booksForAuthor = Mapper.Map<IEnumerable<BookDto>> (booksForAuthorFromRepo);

            return Ok (booksForAuthor);
        }

        [HttpGet ("{bookId}", Name = "GetBook4Author")]
        public IActionResult GetBookByAuthor (Guid authorId, Guid bookId) {

            if (!_libraryRepository.AuthorExists (authorId)) return NotFound ();

            var bookForAuthorFromRepo = _libraryRepository.GetBookForAuthor (authorId, bookId);

            if (bookForAuthorFromRepo == null) return NotFound ();

            var bookForAuthor = Mapper.Map<BookDto> (bookForAuthorFromRepo);
            return Ok (bookForAuthor);

        }

        [HttpPost]
        public IActionResult CreateBookForAuthor (Guid authorId, [FromBody] BookForCreationDto book) {

            if (book == null) return BadRequest ();

            if (!_libraryRepository.AuthorExists (authorId)) return NotFound ();

            var bookEntity = Mapper.Map<Book> (book);

            _libraryRepository.AddBookForAuthor (authorId, bookEntity);

            if (!_libraryRepository.Save ()) throw new Exception ($"Fail to create book for {authorId}");

            var bookToReturn = Mapper.Map<BookDto> (bookEntity);

            // create a uri in returned header, the parameters must be exact with GetBookByAuthor
            // otherwise it won't work
            return CreatedAtRoute ("GetBook4Author", new { authorId = authorId, bookId = bookToReturn.Id }, bookToReturn);
        }
    }
}