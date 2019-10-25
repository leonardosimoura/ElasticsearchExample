using Bogus;
using Nest;
using System;
using System.Linq;
using Xunit;

namespace ElasticSearchExample
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var node = new Uri("http://192.168.99.102:9200/");
            var settings = new ConnectionSettings(node);
            settings.ThrowExceptions();
            settings.EnableDebugMode();
            var client = new ElasticClient(settings);          

            var searchResponse = client.Search<Produto>(w =>                     
                                    w.Index("produto")
                                    .Query(q => q.SimpleQueryString(t => 
                                                            t.Fields(new Field("nome").And("categoria"))
                                                            .Query("laticinios"))
                                    )
                                );

            var groupResult = client.Search<Produto>(w =>
                   w.Index("produto")
                   .From(0)
                   .Take(0)
                   .Aggregations(agg => agg.Average("avg", avg => avg.Field(p => p.Preco)))
                   );

        }
    }

    public class Produto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public decimal Preco { get; set; }
        public decimal QtdeEstoque { get; set; }
    }
}
