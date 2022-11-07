using Marten;
using MartenDemo.Models;

namespace MartenDemo
{
    public class MartenQueryBuilder : IMartenQueryBuilder
    {
        private readonly IDocumentSession _documentSession;

        public MartenQueryBuilder(IDocumentSession documentSession)
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

        public int GetCount()
        {
            var output = _documentSession.Query<MartenData>().ToArray();

            return output.Length;
        }

        public string AddEvent(MartenData martenData, string updateText)
        {
            var streamAction = _documentSession.Events.StartStream<MartenData>(martenData);
            _documentSession.SaveChanges();

            martenData.Text = updateText;

            // Append more events to the same stream
            _documentSession.Events.Append(martenData.Id);
            _documentSession.SaveChanges();

            var stream = _documentSession.Events.FetchStream(streamAction.Id);

            var streamId = stream.First().StreamId.ToString();

            return streamId;
        }
    }
}
