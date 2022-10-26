using Marten;
using MartenDemo.Models;

namespace MartenDemo
{
    public class MartenQueries : IMartenQueries
    {

        public MartenData QueryData(IDocumentSession session, MartenData martenData)
        {
            var output = session.Query<MartenData>().First(x => x.Id == martenData.Id);

            return output;
        }
    }
}
