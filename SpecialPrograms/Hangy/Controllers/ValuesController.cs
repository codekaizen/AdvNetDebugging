using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;

namespace Hangy.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        private static readonly Object _valueSync = new Object();
        private static Int32 _value;

        // GET: api/values
        [Route("")]
        public Int32 Get()
        {
            lock (_valueSync)
            {
                return _value;
            }
        }

        // GET api/values/5
        [Route("{value:int}")]
        public Int32 Get(Int32 value)
        {
            lock (_valueSync)
            {
                while (_value != value)
                {
                    Thread.Sleep(250);
                }

                return _value;
            }
        }

        // POST api/values
        [Route("{value:int}")]
        public void Post(Int32 value)
        {
            lock (_valueSync)
            {
                _value = value;
            }
        }
    }
}
