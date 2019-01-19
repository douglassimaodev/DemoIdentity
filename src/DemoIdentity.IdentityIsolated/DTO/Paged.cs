using System.Collections.Generic;

namespace DemoIdentity.IdentityIsolated.DTO
{
    public class Paged<T> where T : class
    {
        public IEnumerable<T> List { get; set; }
        public int TotalCount { get; set; }
    }
}
