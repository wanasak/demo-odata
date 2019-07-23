using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

public class BooksController : ODataController
{
    private readonly BookStoreContext _bookStoreContext;

    public BooksController(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext ?? throw new System.ArgumentNullException(nameof(bookStoreContext));

        if (bookStoreContext.Books.Count() == 0)
        {
            foreach (var book in DataSource.GetBooks())
            {
                bookStoreContext.Books.Add(book);
                bookStoreContext.Presses.Add(book.Press);
            }
            bookStoreContext.SaveChanges();
        }
    }

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_bookStoreContext.Books);
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        return Ok(_bookStoreContext.Books.FirstOrDefault(b => b.Id == key));
    }

    [EnableQuery]
    public IActionResult Post([FromBody]Book book)
    {
        _bookStoreContext.Books.Add(book);
        _bookStoreContext.SaveChanges();

        return Created(book);
    }
}