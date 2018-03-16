using System;
using System.Collections.Generic;

namespace Vega.Models.Library.Dto {
    public class AuthorForCreationDto {

        public AuthorForCreationDto () {
            Books = new List<BookForCreationDto> ();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Genre { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

        public ICollection<BookForCreationDto> Books { get; set; }

    }
}