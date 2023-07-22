﻿using Elastic.Clients.Elasticsearch;

namespace ElasticSearch.Web.ECommerce.Repositories;

public class ECommerceRepository
{
	private readonly ElasticsearchClient _elasticsearchClient;
	private const string INDEX_NAME = "kibana_sample_data_ecommerce";

	public ECommerceRepository(ElasticsearchClient elasticsearchClient)
	{
		_elasticsearchClient = elasticsearchClient;
	}

	public async Task<Tuple<List<ECommercee>, long>> SearchAsync(ECommerceSearchViewModel searchViewModel, int pageIndex, int pageSize)
	{
		List<Action<QueryDescriptor<ECommercee>>> listQuery = new();

		if (!string.IsNullOrWhiteSpace(searchViewModel.Category))
		{
			listQuery.Add(query => query
									.Match(match => match
										.Field(field => field.Category)
											.Query(searchViewModel.Category)));
		}

		if (!string.IsNullOrWhiteSpace(searchViewModel.CustomerFullName))
		{
			listQuery.Add(query => query
									.Match(match => match
										.Field(field => field.CustomerFullName)
											.Query(searchViewModel.CustomerFullName)));
		}

		if (searchViewModel.OrderDateStart.HasValue)
		{
			listQuery.Add(query => query
									.Range(range => range
										.DateRange(dateRange => dateRange
											.Field(field => field.OrderDate)
												.Gte(searchViewModel.OrderDateStart.Value))));
		}

		if (searchViewModel.OrderDateEnd.HasValue)
		{
			listQuery.Add(query => query
									.Range(range => range
										.DateRange(dateRange => dateRange
											.Field(field => field.OrderDate)
												.Lte(searchViewModel.OrderDateEnd.Value))));
		}

		if (!string.IsNullOrWhiteSpace(searchViewModel.Gender))
		{
			listQuery.Add(query => query
							.Term(term => term
								.Field(field => field)
									.Value(searchViewModel.Gender)));
		}

		var result = await _elasticsearchClient.SearchAsync<ECommercee>(search => 
																			search.Index(INDEX_NAME)
																					.Size(pageSize)
																						.From((pageIndex - 1) * pageSize)
																							.Query(query => query
																								.Bool(b => b
																									.Must(listQuery.ToArray()))));

		foreach (var hit in result.Hits)
		{
			hit.Source.Id = hit.Id;
		}

		return (result.Documents.ToList(), result.Total);
	}
}
