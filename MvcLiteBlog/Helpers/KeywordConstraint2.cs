// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeywordConstraint2.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The keyword constraint 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MvcLiteBlog.Helpers
{
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// The keyword constraint 2.
    /// </summary>
    public class KeywordConstraint2 : IRouteConstraint
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeywordConstraint2"/> class.
        /// </summary>
        public KeywordConstraint2()
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
                if (parameterName == "id")
                {
                    string id = values["id"].ToString();

                    switch (id.ToLower())
                    {
                        case "compose":
                        case "manage":
                        case "managedraft":
                        case "delete":
                        case "deletedraft":
                        case "getcomments":
                        case "page":
                        case "postcontrol":
                        case "correct":
                        case "save":
                            return false;
                        default:
                            return true;
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