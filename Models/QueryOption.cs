using System.Linq.Expressions;

namespace Mexican_Restaurant.Models
{
    public class QueryOption<T> where T : class
    {
        public Expression<Func<T, object>> OrderBy { get; set; } = null!;
        public Expression<Func<T, bool>> Where { get; set; } = null!;
        public string[] includes = Array.Empty<string>();
        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        public string[] GetInclude() => includes;
        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
    }
}