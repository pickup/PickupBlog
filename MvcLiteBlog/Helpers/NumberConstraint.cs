// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberConstraint.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The number constraint.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// The number constraint.
    /// </summary>
    public class NumberConstraint : IRouteConstraint
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberConstraint"/> class.
        /// </summary>
        public NumberConstraint()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The match.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <param name="route">
        /// The route.
        /// </param>
        /// <param name="parameterName">
        /// The parameter name.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="routeDirection">
        /// The route direction.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool Match(
            HttpContextBase httpContext, 
            Route route, 
            string parameterName, 
            RouteValueDictionary values, 
            RouteDirection routeDirection)
        {
            try
            {
                if (parameterName == "year")
                {
                    int year = int.Parse(values["year"].ToString());

                    if (year > 1900 && year < 2100)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (parameterName == "month")
                {
                    int month = int.Parse(values["month"].ToString());

                    if (month > 0 && month <= 12)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}