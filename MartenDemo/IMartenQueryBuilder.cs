using MartenDemo.Models;

namespace MartenDemo
{
    public interface IMartenQueryBuilder
    {
        MartenData GetSingleItem(Guid id);

        List<MartenData> GetManyItems(Guid id);

        List<MartenData> GetByString(string input);

        int GetCount();

        string AddEvent(MartenData martenData, string updateText);
    }
}