using EstateViewWebAPIServer.Utilities;
using System.Linq.Expressions;

namespace EstateViewWebAPIServer.Applications
{
    public class BaseApplication
    {
        private readonly Dictionary<string, object> propertyValues;
        public BaseApplication()
        {
            this.propertyValues = new Dictionary<string, object>();
        }

        protected T GetValue<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = ExpressionHelper.GetName(propertyExpression);
            object value;

            if(this.propertyValues.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        protected void SetValue<T>(Expression<Func<T>> propertyExpression, T value)
        {
            string propertyName = ExpressionHelper.GetName(propertyExpression);
            this.propertyValues[propertyName] = value;
        }
    }
}
