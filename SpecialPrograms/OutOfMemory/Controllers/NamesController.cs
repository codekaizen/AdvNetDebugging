using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using OutOfMemory.Models;

namespace OutOfMemory.Controllers
{
    [RoutePrefix("api/names")]
    public class NamesController : ApiController
    {
        private static readonly ConcurrentDictionary<Name, Int32> _guesses = new ConcurrentDictionary<Name, Int32>();

        // GET api/values
        [Route("")]
        public IEnumerable<KeyValuePair<Name, Int32>> Get()
        {
            var names = _guesses.Keys.ToList();
            foreach (var name in names)
            {
                Int32 count;

                if (_guesses.TryGetValue(name, out count))
                {
                    yield return new KeyValuePair<Name, Int32>(name, count);
                }
            }
        }

        // GET api/values/5
        [Route("{name}")]
        public Int32 Get(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return 0;
            }

            var key = parseNameFromString(name);
            Int32 count;
            return _guesses.TryGetValue(key, out count) ? count : 0;
        }

        // POST api/values
        [Route("{name}")]
        public void Post([FromUri]String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var key = parseNameFromString(name);
            _guesses.AddOrUpdate(key, 1, (k, c) => c + 1);
        }

        private static Name parseNameFromString(String name)
        {
            var nameParts = name.Split(',', ' ', '\t')
                .Select(n => n.Trim(',', ' ', '\t'))
                .Where(n => !String.IsNullOrWhiteSpace(n))
                .ToList();

            return nameParts.Count == 0 
                ? null 
                : new Name(nameParts.Count > 1 ? nameParts[1] : default(String), nameParts[0]);
        }
    }
}
