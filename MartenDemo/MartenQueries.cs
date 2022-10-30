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

        public MartenData GetSingleItem(Guid id)
        {
            var output = _documentSession.Query<MartenData>().First(x => x.Id == id);

            return output;
        }

        public List<MartenData> GetManyItems(Guid id)
        {
            var output = _documentSession.Query<MartenData>().Where(x => x.Id == id).ToList();

            return output;
        }

        public List<MartenData> GetByString(string input)
        {
            var output = _documentSession.Query<MartenData>().Where(x => x.Text!.Contains(input)).ToList();

            return output;
        }
    }
}
