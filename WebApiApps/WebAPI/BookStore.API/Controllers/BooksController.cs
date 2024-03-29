﻿using Application.Services;
using Domain.Models.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    public class BooksController : CustomControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookService _bookService;

        public BooksController
        (
            IWebHostEnvironment hostEnvironment,
            ILogger<BooksController> logger,
            IBookService bookService
        ) : base(hostEnvironment, logger)
        {
            _logger = logger;
            _bookService = bookService;
        }

        /// <summary>
        /// Lists Book resource by pagination
        /// </summary>
        /// <param name="pageSize">Page size for pagination</param>
        /// <param name="pageToken">Current page token</param>
        /// <returns>Array of Book resource</returns>
        /// <response code="200">Returns Books queried with pagination</response>
        /// <remarks>
        /// Sample request : 
        /// 
        ///     GET books/
        ///     [
        ///         {
        ///             "id": 1,
        ///             "name" : "API Design Patterns"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "name" : "Algorithms to Live By"
        ///         }
        ///     ]
        /// 
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]
        public async Task<IActionResult> Get([FromQuery] int pageSize, [FromQuery] int pageToken)
        {
            return Ok(await _bookService.GetAsync(pageSize, pageToken));
        }

        /// <summary>
        /// Gets specific Book resource
        /// </summary>
        /// <param name="id">Id of the Book resource being queried</param>
        /// <returns>A Book resource</returns>
        /// <response code="200">Returns book queried by Id</response>
        /// <response code="404">If Book with Id not found</response>
        /// <remarks>
        /// Sample request : 
        ///     
        ///     GET books/1
        ///     {
        ///         "id": 1,
        ///         "name" : "API Design Patterns"
        ///     }
        /// 
        /// </remarks>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetById([FromRoute] int id)
        {
            var data = await _bookService.GetByIdAsync(id);
            return data != null ? Ok(data) : NotFound();
        }

        /// <summary>
        /// Creates a Book resource
        /// </summary>
        /// <param name="book">Book being created</param>
        /// <returns>Book resource with updated properties if succeeds, else BadRequest</returns>
        /// <response code="201">Returns newly created Book</response>
        /// <response code="400">If Book creation failed</response>
        /// <remarks>
        /// Sample request : 
        ///     
        ///     POST books 
        ///     201 Created
        ///     
        /// </remarks>
        [HttpPost]
        [Consumes("application/json", "text/vcard")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> Create([FromBody] Book book)
        {
            var result = await _bookService.CreateAsync(book);
            return result != null
                ? CreatedAtAction(nameof(Create), book)
                : BadRequest();
        }

        /// <summary>
        /// Updates specific Book resource
        /// </summary>
        /// <param name="id">Id of the Book resource being updated</param>
        /// <param name="book">Book resource with new property vales</param>
        /// <returns>OK result if succeeds, else BadRequest</returns>
        /// <response code="204">If Book update succeeded</response>
        /// <response code="400">If Book update failed</response>
        /// <remarks>
        /// Sample request : 
        /// 
        ///     PUT books
        ///     204 No Content
        ///     
        /// </remarks>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] Book book)
        {
            var result = await _bookService.UpdateAsync(id, book);
            return result
                ? NoContent()
                : BadRequest();
        }

        /// <summary>
        /// Updates specific Book resource partially
        /// </summary>
        /// <param name="id">Id of the Book resource being updated</param>
        /// <param name="bookPatchDoc">JSON Patch document</param>
        /// <returns>Resource previous value and new value</returns>
        /// <response code="200">If Book update succeeded</response>
        /// <response code="400">If Book update failed</response>
        /// <returns></returns>
        ///         /// <remarks>
        /// Sample request : 
        /// 
        ///     PATCH books
        ///     200 OK
        ///     
        /// </remarks>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK )]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> UpdatePartial([FromRoute] int id, [FromBody] JsonPatchDocument<Book> bookPatchDoc)
        {
            var data = await _bookService.UpdatePartialAsync(id, bookPatchDoc, ModelState);
            return data != null
             ? Ok(data)
             : BadRequest();
        }

        /// <summary>
        /// Deletes specific Book resource
        /// </summary>
        /// <param name="id">Id of the Book resource being deleted</param>
        /// <returns>OK result if succeeds, else BadRequest</returns>
        /// <response code="200">If Book deletion succeeded</response>
        /// <response code="400">If Book deletion failed</response>
        /// <remarks>
        /// Sample request : 
        ///     
        ///     DELETE books/1
        ///     200 OK
        ///     
        /// </remarks>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteById([FromRoute] int id)
        {
            var result = await _bookService.DeleteByIdAsync(id);
            return result
                ? Ok()
                : BadRequest();
        }
    }
}
