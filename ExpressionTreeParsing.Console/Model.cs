using System.Collections.Generic;

namespace ExpressionTreeParsing.Console
{
    public class Model
    {
        public string StringProperty { get; set; }

        public int Int32Property { get; set; }

        public IEnumerable<Model> SubModels { get; set; }
    }
}
