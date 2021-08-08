using System.Linq.Expressions;

namespace Simplify.Storage.MongoDb.Builders
{
    public sealed class FieldDefinitionBuilder<T>
    {
        public static readonly FieldDefinitionBuilder<T> Instance = new FieldDefinitionBuilder<T>();

        private FieldDefinitionBuilder() { }

        public FieldDefinition<T, TResult> Build<TResult>(Expression<Func<T, TResult>> expression) => new ExpressionFieldDefinition<T, TResult>(expression);

        public FieldDefinition<T, TResult> Build<TResult>(string name) => new StringFieldDefinition<T, TResult>(name);
    }
}