using PublicBonds.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicBonds.Application.DTOs.Response
{
    public class IndexerResponseDto
    {
        public string Name { get; set; }

        public static IndexerResponseDto IndexerResponseDtoFromIndexer(Indexer indexer)
        {
            return new IndexerResponseDto { Name = indexer.IndexerName };
        }
    }
}
