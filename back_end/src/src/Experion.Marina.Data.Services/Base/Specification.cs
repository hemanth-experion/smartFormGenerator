using System;
using System.Data;
using System.Linq.Expressions;

namespace Experion.Marina.Data.Services
{
    /// <summary>
    /// The Specification Class.
    /// </summary>
    /// <typeparam name="T">The Generic Type</typeparam>
    public class Specification<T>
    {
        #region Fields

        /// <summary>
        /// The expression predicate.
        /// </summary>
        private readonly Expression<Func<T, bool>> expression;

        /// <summary>
        /// Function to evaluate the expression against an instance.
        /// </summary>
        private Func<T, bool> evaluateExpression;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification{T}"/> class.
        /// </summary>
        /// <param name="predicate">Expression predicate.</param>
        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.expression = predicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification{T}"/> class.
        /// </summary>
        protected Specification()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the expression predicate.
        /// </summary>
        /// <value>The expression predicate.</value>
        public virtual Expression<Func<T, bool>> Predicate
        {
            get
            {
                if (this.expression != null)
                {
                    return this.expression;
                }

                throw new InvalidExpressionException("Argument Predicate Not Overridden");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Evaluates the specification against an instance.
        /// </summary>
        /// <param name="candidate">The instance against which the specification is to be evaluated.</param>
        /// <returns>Should return <c>true</c> if the specification was satisfied by the entity, else <c>false</c>.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            if (this.evaluateExpression == null)
            {
                this.evaluateExpression = this.expression.Compile();
            }

            return this.evaluateExpression(candidate);
        }

        #endregion Methods
    }
}