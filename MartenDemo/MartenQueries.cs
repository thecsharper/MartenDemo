using Marten;
using MartenDemo.Models;

namespace MartenDemo
{
    public class MartenQueries : IMartenQueries
    {
        private readonly IDocumentSession _documentSession;

        public MartenQueries(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public MartenData QueryData(Guid id)
        {
            var output = _documentSession.Query<MartenData>().First(x => x.Id == id);

            return output;
        }
    }
}
