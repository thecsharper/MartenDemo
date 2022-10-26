using Marten;
using MartenDemo.Models;

namespace MartenDemo
{
    public interface IMartenQueries
    {
        MartenData QueryData(IDocumentSession session, MartenData martenData);
    }
}