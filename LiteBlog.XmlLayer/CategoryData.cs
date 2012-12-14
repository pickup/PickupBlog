// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryData.cs" company="LiteBlog">
//   Copyright (c) 2012, LiteBlog. All Rights Reserved.
// </copyright>
// <summary>
//   The category data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LiteBlog.XmlLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using LiteBlog.Common;
    using LiteBlog.Common.Contracts;

    /// <summary>
    /// The category data.
    /// </summary>
    public class CategoryData : ICategoryData
    {
        #region Constants

        /// <summary>
        /// The coun t_ error.
        /// </summary>
        private const string COUNT_ERROR = "Category count is less than zero";

        /// <summary>
        /// The du p_ ca t_ error.
        /// </summary>
        private const string DUP_CAT_ERROR = "Category = {0} is already present";

        /// <summary>
        /// The forma t_ error.
        /// </summary>
        private const string FORMAT_ERROR = "Category count is not in number format";

        /// <summary>
        /// The n o_ ca t_ error.
        /// </summary>
        private const string NO_CAT_ERROR = "Category = {0} could not be found";

        /// <summary>
        /// The n o_ fil e_ error.
        /// </summary>
        private const string NO_FILE_ERROR = "Category file could not be found";

        /// <summary>
        /// The xm l_ forma t_ error.
        /// </summary>
        private const string XML_FORMAT_ERROR = "Category file is not in the right format";

        #endregion

        #region Fields

        /// <summary>
        /// The _path.
        /// </summary>
        private string _path;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryData"/> class.
        /// </summary>
        public CategoryData()
        {
            this._path = DataContext.Path + "Category.xml";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the path.
        /// </summary>
        internal static string Path
        {
            get
            {
                return DataContext.Path + "Category.xml";
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Increments the count of posts by the number
        /// Called when post is published or unpublished
        /// </summary>
        /// <param name="catID">
        /// Category ID
        /// </param>
        /// <param name="num">
        /// Count to Increase (+1/-1)
        /// </param>
        public void ChangeCount(string catID, int num)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Category") where elem.Attribute("ID").Value == catID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_CAT_ERROR, catID);
                Logger.Log(msg);
                return;
            }

            XElement catElem = qry.First<XElement>();

            try
            {
                int count = 0;
                try
                {
                    count = (int)catElem.Attribute("Count");
                }
                catch (Exception ex)
                {
                    Logger.Log(FORMAT_ERROR, ex);
                }

                count = count + num;
                if (count < 0)
                {
                    count = 0;
                    Logger.Log(COUNT_ERROR);
                }

                catElem.SetAttributeValue("Count", count);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            root.Save(this._path);
        }

        /// <summary>
        /// Delete the category using category id
        /// Called by category management
        /// </summary>
        /// <param name="catID">
        /// Category ID
        /// </param>
        public void Delete(string catID)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Category") where elem.Attribute("ID").Value == catID select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_CAT_ERROR, catID);
                Logger.Log(msg);
                return;
            }

            XElement catElem = qry.First<XElement>();
            catElem.Remove();

            root.Save(this._path);
        }

        /// <summary>
        /// Gets list of categories
        /// Called by Category user control 
        /// Called by Category Management page
        /// </summary>
        /// <returns>List of Category Object</returns>
        public List<Category> GetCategories()
        {
            XElement root = null;
            List<Category> categories = new List<Category>();

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                return categories;
            }

            try
            {
                foreach (XElement catElem in root.Elements("Category"))
                {
                    Category cat = new Category();
                    cat.CatID = catElem.Attribute("ID").Value;
                    cat.Name = catElem.Attribute("Name").Value;
                    try
                    {
                        cat.Count = (int)catElem.Attribute("Count");
                        cat.Order = (int)catElem.Attribute("Order");
                    }
                    catch (Exception ex)
                    {
                        cat.Count = 0;
                        Logger.Log(FORMAT_ERROR, ex);
                    }

                    categories.Add(cat);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            return categories;
        }

        /// <summary>
        /// Insert a category node into XML
        /// Called when a new category is added in category management
        /// </summary>
        /// <param name="category">
        /// The Category
        /// </param>
        public void Insert(Category category)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Category")
                      where elem.Attribute("ID").Value == category.CatID
                      select elem;

            if (qry.Count<XElement>() > 0)
            {
                string msg = string.Format(DUP_CAT_ERROR, category.CatID);
                Logger.Log(msg);
                throw new ApplicationException(msg);
            }

            try
            {
                XElement catElem = new XElement(
                    "Category", 
                    new XAttribute("ID", category.CatID), 
                    new XAttribute("Name", category.Name), 
                    new XAttribute("Count", category.Count), 
                    new XAttribute("Order", category.Order));

                int count = root.Elements("Category").Count<XElement>();

                // Usually order is zero
                if (category.Order == 0 || category.Order > count)
                {
                    category.Order = count + 1;
                    root.Add(catElem);
                }
                else
                {
                    var qry2 = from elem in root.Elements("Category")
                               where (int)elem.Attribute("Order") > category.Order
                               select elem;

                    if (qry2.Count<XElement>() > 0)
                    {
                        // This is a rare case (does not happen)
                        XElement refElem = qry2.First<XElement>();
                        refElem.AddBeforeSelf(catElem);
                    }
                }

                root.Save(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }
        }

        /// <summary>
        /// Updates the category node given the old category id
        /// Called by category management
        /// </summary>
        /// <param name="id">
        /// Old ID
        /// </param>
        /// <param name="category">
        /// The Category
        /// </param>
        public void Update(string id, Category category)
        {
            XElement root = null;

            try
            {
                root = XElement.Load(this._path);
            }
            catch (Exception ex)
            {
                Logger.Log(NO_FILE_ERROR, ex);
                throw new ApplicationException(NO_FILE_ERROR, ex);
            }

            var qry = from elem in root.Elements("Category") where elem.Attribute("ID").Value == id select elem;

            if (qry.Count<XElement>() == 0)
            {
                string msg = string.Format(NO_CAT_ERROR, id);
                Logger.Log(msg);
                return;
            }

            XElement catElem = qry.First<XElement>();

            try
            {
                catElem.SetAttributeValue("ID", category.CatID);
                catElem.SetAttributeValue("Name", category.Name);
                catElem.SetAttributeValue("Order", category.Order);

                // does not update count
            }
            catch (Exception ex)
            {
                Logger.Log(XML_FORMAT_ERROR, ex);
                throw new ApplicationException(XML_FORMAT_ERROR, ex);
            }

            root.Save(this._path);
        }

        #endregion
    }
}