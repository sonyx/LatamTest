using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TestLatam.Models;

namespace TestLatam.Controllers
{
    public class BooksController : ApiController
    {
        static IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
        }
        
        public IEnumerable<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }
        
        public Book GetBook(int id)
        {
            Book book = _repository.Get(id);
            if (book == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return book;
        }
        public HttpResponseMessage PostBook(Book book)
        {
            book = _repository.Add(book);
            var response = Request.CreateResponse<Book>(HttpStatusCode.Created, book);
            string uri = Url.Route(null, new { id = book.Id });
            response.Headers.Location = new Uri(Request.RequestUri, uri);
            return response;
        }

        public void PutBook(int id, Book book)
        {
            book.Id = id;
            if (!_repository.Update(book))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        public HttpResponseMessage DeleteBook(int id)
        {
            _repository.Remove(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}