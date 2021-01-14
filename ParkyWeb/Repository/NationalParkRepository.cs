using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>,INationalParkRepository
    {
        private readonly IHttpClientFactory httpFactory;

        public NationalParkRepository(IHttpClientFactory httpFactory) : base (httpFactory)
        {
            this.httpFactory = httpFactory;
        }
    }
}
