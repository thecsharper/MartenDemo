using Marten;
using MartenDemo.Models;

namespace MartenDemo
{
    public interface IMartenQueries
    {
        MartenData QueryData(Guid id);
    }
}