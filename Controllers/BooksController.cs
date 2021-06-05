using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        /// <summary>
        /// Returns all the books
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();
        }

        //[HttpGet("{idBook}")]
        //public async Task<ActionResult<Book>> GetBooks([FromRoute]  int idBook)
        //{
        //    return await _bookRepository.Get(id);
        //}

        [HttpGet]
        public async Task<ActionResult<Book>> GetBook([FromQuery] int idBook)
        {
            return await _bookRepository.Get(idBook);
        }

        /// <summary>
        /// Posts a book
        /// </summary>
        /// <param name="book">Object to be sent. Contains title, author, description</param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult<Book>> PostBooks([FromBody] Book book)
        {
            var newBook = await _bookRepository.Create(book);
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
        }

        [HttpPut]
        public async Task<ActionResult> PutBooks(int id, [FromBody] Book book)
        {
            if(id != book.Id)
            {
                return BadRequest();
            }

            await _bookRepository.Update(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if (bookToDelete == null)
                return NotFound();

            await _bookRepository.Delete(bookToDelete.Id);
            return NoContent();
        }
    }
}
