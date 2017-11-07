using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Helpers;
using System.Web.Configuration;
using System.IO;
using System.Xml;
using WebShop.Controllers;

namespace WebShop.Models
{
    public class DAO
    {
        SqlConnection conn;
        public string message = "";


        //Intialises a connection object
        public DAO()
        {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["conStringLocal"].ConnectionString);
        }

        #region User

        //Method for inserting data to Database
        public int Insert(UserModel user, DateTime date)
        {
            //to do 
            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            string password, answer;
            cmd = new SqlCommand("uspInsertCustomerandCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@firstname", user.FirstName);
            cmd.Parameters.AddWithValue("@lastname", user.LastName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@phone", user.Phone);
            cmd.Parameters.AddWithValue("@dateregistered", date);
            cmd.Parameters.AddWithValue("@isactivated", 0);
            cmd.Parameters.AddWithValue("@lastlogin", date);
            cmd.Parameters.AddWithValue("@securityquestion", user.SecurityQuestion);
            cmd.Parameters.AddWithValue("@islogged", 1);
            cmd.Parameters.AddWithValue("@address1", user.Street1);
            cmd.Parameters.AddWithValue("@address2", user.Street2);
            cmd.Parameters.AddWithValue("@town", user.Town);
            cmd.Parameters.AddWithValue("@county", user.County);

            //Only putting in as 3 temporarily before we decide on User Roles
            cmd.Parameters.AddWithValue("@roleid", 3);

            //cart stuff
            cmd.Parameters.AddWithValue("@totalcost", 0.00);
            cmd.Parameters.AddWithValue("@orderstatusid", 1); //1 = not placed, this will change to two when the order is placed
            cmd.Parameters.AddWithValue("@deliverystatusid", 2); //2 = false for isDelivered, this will change to 1 when it is delivered
            cmd.Parameters.AddWithValue("@shippingid", 4); //this is a placeholder for not shipping method being selected, this will change at checkout


            answer = Crypto.HashPassword(user.Answer);
            password = Crypto.HashPassword(user.Password);
            message = password;
            cmd.Parameters.AddWithValue("@userpass", password);
            cmd.Parameters.AddWithValue("@securityanswer", answer);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        //Method for checking login
        public int CheckLoginAdmin(string sEmail, string sPass)
        {
            int role = 55;
            SqlCommand cmd;
            SqlDataReader reader;
            string password;
            cmd = new SqlCommand("uspCheckLoginAdmin", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", sEmail);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    password = reader["Pass"].ToString();
                    if (Crypto.VerifyHashedPassword(password, sPass))
                    {
                        role = int.Parse(reader["Role"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return role;
        }

        //Method for checking login
        public string CheckLogin(UserModel user)
        {
            string firstName = null;
            SqlCommand cmd;
            SqlDataReader reader;
            string password;
            cmd = new SqlCommand("uspCheckLogin", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@email", user.Email);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    password = reader["UserPass"].ToString();
                    if (Crypto.VerifyHashedPassword(password, user.Password))
                    {
                        firstName = reader["FirstName"].ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return firstName;
        }

        public string GetSecurityQuestion(string email)
        {

            SqlCommand cmd;
            SqlDataReader reader;
            string question = null;
            cmd = new SqlCommand("uspGetSecurityQuestion", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    question = reader["SecurityQuestion"].ToString();
                }

            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return question;
        }

        public string CheckSecurityQuestion(UserModel user)
        {
            string firstName = null;
            SqlCommand cmd;
            SqlDataReader reader;
            string answer;
            cmd = new SqlCommand("uspCheckQuestion", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@email", user.Email);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    answer = reader["QuestionAnswer"].ToString();
                    if (Crypto.VerifyHashedPassword(answer, user.Answer))
                    {
                        firstName = reader["FirstName"].ToString();
                    }


                }

            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            catch (FormatException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return firstName;
        }

        public int ResetPassword(UserModel user)
        {
            int count = 0;
            SqlCommand cmd;
            string password;
            cmd = new SqlCommand("uspChangePassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", user.Email);
            password = Crypto.HashPassword(user.Password);
            message = password;
            cmd.Parameters.AddWithValue("@userpass", password);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        #endregion

        #region Staff

        // Get all staff in a list
        internal List<StaffModel> GetAllStaff()
        {
            List<StaffModel> list = new List<StaffModel>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Staff", conn);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    StaffModel staff = new StaffModel();
                    staff.Id = int.Parse(reader["StaffID"].ToString());
                    staff.FirstName = reader["FirstName"].ToString();
                    staff.LastName = reader["LastName"].ToString();
                    staff.UserRole = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString());
                    staff.UserState = (Active)Enum.Parse(typeof(Active), reader["IsActive"].ToString());
                    staff.Email = reader["Email"].ToString();
                    list.Add(staff);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // TODO
        internal int LoginStaff(StaffLoginModel lsm)
        {
            throw new NotImplementedException();
        }
        // Get single staff details
        internal List<UpdateStaffModel> GetSingleStaff(string id)
        {
            List<UpdateStaffModel> list = new List<UpdateStaffModel>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("uspSelectSingleStaff", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UpdateStaffModel staff = new UpdateStaffModel();
                    staff.Id = int.Parse(reader["StaffID"].ToString());
                    staff.FirstName = reader["FirstName"].ToString();
                    staff.LastName = reader["LastName"].ToString();
                    staff.UserRole = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString());
                    staff.UserState = (Active)Enum.Parse(typeof(Active), reader["IsActive"].ToString());
                    staff.Email = reader["Email"].ToString();
                    list.Add(staff);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // Update single staff memeber details
        internal int UpdateSingleStaff(UpdateStaffModel staff, string id)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateSingleStaff", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fname", staff.FirstName);
            cmd.Parameters.AddWithValue("@sname", staff.LastName);
            cmd.Parameters.AddWithValue("@role", staff.UserRole);
            cmd.Parameters.AddWithValue("@email", staff.Email);
            cmd.Parameters.AddWithValue("@active", staff.UserState);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        // Update staff password
        internal int UpdateStaffPassword(PasswordResetModel pr, string id)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateStaffPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@pass", Crypto.HashPassword(pr.Password));
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Add new staff
        internal int InsertStaff(StaffModel staff)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertStaff", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fname", staff.FirstName);
            cmd.Parameters.AddWithValue("@sname", staff.LastName);
            cmd.Parameters.AddWithValue("@role", Role.Staff);
            cmd.Parameters.AddWithValue("@email", staff.Email);
            cmd.Parameters.AddWithValue("@active", Active.Yes);
            cmd.Parameters.AddWithValue("@pass", Crypto.HashPassword(staff.Password));
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        #endregion

        #region ProductAdmin

        // Get All products
        internal List<AdminProduct> GetAllProducts()
        {
            SqlDataReader reader;
            List<AdminProduct> prodList = new List<AdminProduct>();
            //List<DescriptionModel> descList = new List<DescriptionModel>();
            SqlCommand cmd = new SqlCommand("SELECT p.ProductID,p.ProdName,p.Price,p.Discount,p.SupplierID,p.CategoryId,p.ImageUrl,s.SupName FROM Product p INNER JOIN Supplier s on p.SupplierID = s.SupplierID", conn);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminProduct product = new AdminProduct();
                    //DataSet ds = new DataSet();

                    product.Id = int.Parse(reader[0].ToString());
                    product.Name = reader[1].ToString();
                    product.Price = decimal.Parse(reader[2].ToString());
                    product.Discount = decimal.Parse(reader[3].ToString());

                    //ds.ReadXml(new XmlTextReader(new StringReader(reader[4].ToString())));
                    //DataTable dt = ds.Tables[0];
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    DescriptionModel descModel = new DescriptionModel();
                    //    descModel.Heading = row["Heading"].ToString();
                    //    descModel.Description = row["Description"].ToString();
                    //    descList.Add(descModel);
                    //}
                    //product.Details = descList;

                    product.SupplierID = int.Parse(reader[4].ToString());
                    product.CategoryID = int.Parse(reader[5].ToString());
                    product.ImageUrl = reader[6].ToString();
                    product.SupplierName = reader[7].ToString();
                    prodList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return prodList;
        }
        // Add new product
        internal int InsertProduct(AdminProductModel product, string fileName)
        {
            int count = 0;
            DataSet ds = GetDetailsData(product);
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertProduct", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prodname", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@discount", product.Discount);
            cmd.Parameters.AddWithValue("@image", fileName);
            cmd.Parameters.AddWithValue("@description", ds.GetXml());
            cmd.Parameters.AddWithValue("@categoryID", product.CategoryID);
            cmd.Parameters.AddWithValue("@supplierID", product.SupplierID);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Xml 
        internal DataSet GetDetailsData(AdminProductModel product)
        {
            DataSet ds = new DataSet("Sections");
            DataTable dt = new DataTable("Section");
            DataRow row;
            dt.Columns.Add("Heading");
            dt.Columns.Add("Description");
            ds.Tables.Add(dt);

            List<DescriptionModel> list = product.Details;
            foreach (DescriptionModel description in list)
            {
                row = dt.NewRow();

                if (description.Heading == null || description.Description == null)
                {
                    row["Heading"] = " ";
                    row["Description"] = " ";
                }
                else
                {
                    row["Heading"] = description.Heading;
                    row["Description"] = description.Description;
                }
                dt.Rows.Add(row);
            }
            ds.AcceptChanges();
            return ds;
        }
        // Get Single Product Details
        internal List<DescriptionModel> GetSingleProductDetails(string id)
        {
            SqlDataReader reader;
            List<DescriptionModel> descList = new List<DescriptionModel>();
            SqlCommand cmd = new SqlCommand("uspSelectSingleProductDescription", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(new XmlTextReader(new StringReader(reader[5].ToString())));
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DescriptionModel descModel = new DescriptionModel();
                        descModel.Heading = row["Heading"].ToString();
                        descModel.Description = row["Description"].ToString();
                        descList.Add(descModel);
                    }
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return descList;
        }
        // Update Product Description
        internal int UpdateProductDescription(DescriptionModel dm, string id)
        {
            int count = 0;
            List<DescriptionModel> existingList = GetSingleProductDetails(id);
            existingList.Add(dm);
            AdminProductModel apm = new AdminProductModel();
            apm.Details = existingList;

            DataSet ds = GetDetailsData(apm);
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateSingleProductDescription", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@description", ds.GetXml());
            cmd.Parameters.AddWithValue("@id", id);


            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Get Single product
        internal List<AdminProduct> GetSingleProduct(string id)
        {
            SqlDataReader reader;
            List<AdminProduct> prodList = new List<AdminProduct>();
            //List<DescriptionModel> descList = new List<DescriptionModel>();
            SqlCommand cmd = new SqlCommand("SELECT p.ProductID,p.ProdName,p.Price,p.Discount,p.SupplierID,p.CategoryId,p.ImageUrl FROM Product p WHERE p.ProductID = " + id, conn);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminProduct product = new AdminProduct();
                    //DataSet ds = new DataSet();

                    product.Id = int.Parse(reader[0].ToString());
                    product.Name = reader[1].ToString();
                    product.Price = decimal.Parse(reader[2].ToString());
                    product.Discount = decimal.Parse(reader[3].ToString());

                    //ds.ReadXml(new XmlTextReader(new StringReader(reader[4].ToString())));
                    //DataTable dt = ds.Tables[0];
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    DescriptionModel descModel = new DescriptionModel();
                    //    descModel.Heading = row["Heading"].ToString();
                    //    descModel.Description = row["Description"].ToString(); 
                    //    descList.Add(descModel);
                    //}
                    //product.Details = descList;

                    product.SupplierID = int.Parse(reader[4].ToString());
                    product.CategoryID= int.Parse(reader[5].ToString());
                    product.ImageUrl = reader[6].ToString();
                    prodList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return prodList;
        }
        // Update product
        internal int UpdateProduct(AdminProductModel product, string fileName,string id)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateSingleProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@prodName", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@discount", product.Discount);
            cmd.Parameters.AddWithValue("@imageUrl", fileName);
            cmd.Parameters.AddWithValue("@catID", product.CategoryID);
            cmd.Parameters.AddWithValue("@supID", product.SupplierID);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }





        #endregion

        #region Products

        // New from Ivan do not edit
        public List<AdminProduct> GetAllProductsByCatID(string id)
        {
            SqlDataReader reader;
            List<AdminProduct> prodList = new List<AdminProduct>();
            List<DescriptionModel> descList = new List<DescriptionModel>();
            SqlCommand cmd = new SqlCommand("SELECT p.ProductID,p.ProdName,p.Price,p.Discount,p.ProductDescription,p.SupplierId,p.CategoryId,p.ImageUrl,s.SupName FROM Product p INNER JOIN Supplier s on p.SupplierID = s.SupplierID WHERE p.CategoryID = " + id, conn);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminProduct product = new AdminProduct();
                    DataSet ds = new DataSet();

                    product.Id = int.Parse(reader[0].ToString());
                    product.Name = reader[1].ToString();
                    product.Price = decimal.Parse(reader[2].ToString());
                    product.Discount = decimal.Parse(reader[3].ToString());

                    ds.ReadXml(new XmlTextReader(new StringReader(reader[4].ToString())));
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DescriptionModel descModel = new DescriptionModel();
                        descModel.Heading = "heading";//row["Heading"].ToString();
                        descModel.Description = "description";// row["Description"].ToString(); 
                        descList.Add(descModel);
                    }
                    product.Details = descList;
                    product.SupplierID = int.Parse(reader[5].ToString());
                    product.CategoryID = int.Parse(reader[6].ToString());
                    product.ImageUrl = reader[7].ToString();
                    product.SupplierName = reader[8].ToString();
                    prodList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return prodList;
        }

        public List<Product> ShowAllProducts()
        {
            List<Product> productList = new List<Product>();
            SqlCommand cmd;
            SqlDataReader reader;
            //Calling connection method to establish connection string
            cmd = new SqlCommand("uspAllProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ID = int.Parse(reader["ProductID"].ToString());
                    product.Name = reader["ProdName"].ToString();
                    product.Price = decimal.Parse(reader["Price"].ToString());
                    product.Discount = decimal.Parse(reader["Discount"].ToString());
                    product.CatID = int.Parse(reader["CategoryID"].ToString());
                    product.SupID = int.Parse(reader["SupplierID"].ToString());
                    product.ImageURL = reader["ImageURL"].ToString();
                    product.SupplierName = reader["SupName"].ToString();
                    productList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return productList;
        }

        public int InsertProduct(ProductModel product)
        {
            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prodname", product.Name);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@discount", product.Discount);
            cmd.Parameters.AddWithValue("@description", DBNull.Value);
            cmd.Parameters.AddWithValue("@categoryID", product.CatID);
            cmd.Parameters.AddWithValue("@supplierID", product.SupID);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public List<Product> ShowAllProductsBeginningWithParam(string blah)
        {
            //DOESN'T HAVE PARAMETERS FOR DESCRIPTION
            List<Product> list = new List<Product>();
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("upsSelectProdsByName", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productname", blah);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = reader[0].ToString(); //this reads out in the order of the stored procedure, I've updated the stored
                    product.Price = decimal.Parse(reader[1].ToString());
                    product.CatID = int.Parse(reader[2].ToString());
                    product.Discount = decimal.Parse(reader[3].ToString());
                    product.SupID = int.Parse(reader[4].ToString());
                    //product.Description = reader[5].ToString();
                    product.ImageURL = reader[6].ToString();
                    product.SupplierName = reader[8].ToString();
                    /*product.ID = int.Parse(reader[0].ToString());
                    product.Name = reader[1].ToString();
                    product.Price = decimal.Parse(reader[2].ToString());
                    product.Discount = decimal.Parse(reader[3].ToString());
                    product.CatID = int.Parse(reader[5].ToString());
                    product.SupID = int.Parse(reader[6].ToString());*/
                    /*Product.ProdName, Product.Price, Product.CategoryID, Product.Discount, Product.SupplierID, 
                     * Product.Description, ProductImage.ImageUrl, ProductImage.ImageID, Supplier.SupName
                     */
                    list.Add(product);

                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }

        #endregion

        #region Laptops

        public List<Product> ShowAllLaptops()
        {
            List<Product> laptopList = new List<Product>();
            SqlCommand cmd;
            SqlDataReader reader;
            //Calling connection method to establish connection string
            cmd = new SqlCommand("uspAllLaptops", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = reader["ProdName"].ToString();
                    product.Price = decimal.Parse(reader["Price"].ToString());
                    product.Discount = decimal.Parse(reader["Discount"].ToString());
                    product.CatID = int.Parse(reader["CategoryID"].ToString());
                    product.SupID = int.Parse(reader["SupplierID"].ToString());
                    product.ImageURL = reader["ImageURL"].ToString();
                    //product.SupplierName = reader["SupName"].ToString();
                    laptopList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return laptopList;
        }
        #endregion

        #region Tablets

        public List<Product> ShowAllTablets()
        {
            List<Product> tabletList = new List<Product>();
            SqlCommand cmd;
            SqlDataReader reader;
            //Calling connection method to establish connection string
            cmd = new SqlCommand("uspAllTablets", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = reader["ProdName"].ToString();
                    product.Price = decimal.Parse(reader["Price"].ToString());
                    product.Discount = decimal.Parse(reader["Discount"].ToString());
                    product.CatID = int.Parse(reader["CategoryID"].ToString());
                    product.SupID = int.Parse(reader["SupplierID"].ToString());
                    product.ImageURL = reader["ImageURL"].ToString();
                    product.SupplierName = reader["SupName"].ToString();
                    tabletList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return tabletList;
        }
        #endregion

        #region Phones

        public List<Product> ShowAllPhones()
        {
            List<Product> phoneList = new List<Product>();
            SqlCommand cmd;
            SqlDataReader reader;
            //Calling connection method to establish connection string
            cmd = new SqlCommand("uspAllPhones", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.Name = reader["ProdName"].ToString();
                    product.Price = decimal.Parse(reader["Price"].ToString());
                    product.Discount = decimal.Parse(reader["Discount"].ToString());
                    product.CatID = int.Parse(reader["CategoryID"].ToString());
                    product.SupID = int.Parse(reader["SupplierID"].ToString());
                    product.ImageURL = reader["ImageURL"].ToString();
                    product.SupplierName = reader["SupName"].ToString();
                    phoneList.Add(product);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return phoneList;
        }
        #endregion

        #region Supplier
        // Get all suplier namesK
        public Dictionary<int, string> GetAllSupplierNames()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT SupplierID,SupName From Supplier", conn);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminCategory category = new AdminCategory();
                    list.Add(int.Parse(reader["SupplierID"].ToString()), reader["SupName"].ToString());
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // Get all Suppliers
        public List<SupplierModel> GetAllSuppliers()
        {
            List<SupplierModel> list = new List<SupplierModel>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT * From Supplier", conn);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SupplierModel supplier = new SupplierModel();
                    supplier.Id = int.Parse(reader["SupplierId"].ToString());
                    supplier.Name = reader["SupName"].ToString();
                    supplier.Address = reader["SupAddress"].ToString();
                    supplier.Phone = reader["SupPhone"].ToString();
                    supplier.Email = reader["SupEmail"].ToString();
                    list.Add(supplier);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // Get single supplier details
        public List<SupplierModel> GetSingleSupplier(string id)
        {
            List<SupplierModel> list = new List<SupplierModel>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("uspSelectSingleSupplier", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    SupplierModel supplier = new SupplierModel();
                    supplier.Id = int.Parse(reader["SupplierID"].ToString());
                    supplier.Name = reader["SupName"].ToString();
                    supplier.Address = reader["SupAddress"].ToString();
                    supplier.Phone = reader["SupPhone"].ToString();
                    supplier.Email = reader["SupEmail"].ToString();
                    list.Add(supplier);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // Update supplier details
        internal int UpdateSupplier(SupplierModel supplier, string id)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateSingleSupplier", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@supId", id);
            cmd.Parameters.AddWithValue("@supName", supplier.Name);
            cmd.Parameters.AddWithValue("@supAddress", supplier.Address);
            cmd.Parameters.AddWithValue("@supPhone", supplier.Phone);
            cmd.Parameters.AddWithValue("@supEmail", supplier.Email);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Create a nre supplier
        internal int InsertSupplier(SupplierModel supplier)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertSupplier", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@supname", supplier.Name);
            cmd.Parameters.AddWithValue("@supaddress", supplier.Address);
            cmd.Parameters.AddWithValue("@supphone", supplier.Phone);
            cmd.Parameters.AddWithValue("@supemail", supplier.Email);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        #endregion

        #region AdminCategory

        // Get all Category names
        internal dynamic GetAllCategoryNames()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT CategoryID,CatName From Category", conn);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminCategory category = new AdminCategory();
                    list.Add(int.Parse(reader["CategoryId"].ToString()), reader["CatName"].ToString());
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        // Create new category
        public int InsertCategory(string name, string desc, string url)
        {
            SqlCommand cmd;
            //count shows the number of affected rows
            int count = 0;
            cmd = new SqlCommand("uspInsertCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@catname", name);
            cmd.Parameters.AddWithValue("@catdescription", desc);
            cmd.Parameters.AddWithValue("@imageURL", url);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Update selected category
        public int UpdateCategory(string catId, string name, string desc, string url)
        {
            SqlCommand cmd;
            //count shows the number of affected rows
            int count = 0;
            cmd = new SqlCommand("uspUpdateSingleCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@catId", catId);
            cmd.Parameters.AddWithValue("@catName", name);
            cmd.Parameters.AddWithValue("@catDesc", desc);
            cmd.Parameters.AddWithValue("@imageUrl", url);
            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }
        // Get all Categories
        public List<AdminCategory> GetAllCategories()
        {
            List<AdminCategory> list = new List<AdminCategory>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("SELECT * From Category", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminCategory category = new AdminCategory();
                    category.Id = int.Parse(reader["CategoryId"].ToString());
                    category.Name = reader["CatName"].ToString();
                    category.Description = reader["CatDescription"].ToString();
                    category.ImageUrl = reader["ImageUrl"].ToString();

                    list.Add(category);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return list;
        }
        // Get single Category
        public List<AdminCategory> GetSingleCategory(string id)
        {
            List<AdminCategory> list = new List<AdminCategory>();
            SqlDataReader reader;

            SqlCommand cmd = new SqlCommand("uspSelectSingleCategory", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdminCategory category = new AdminCategory();
                    category.Id = int.Parse(reader["CategoryId"].ToString());
                    category.Name = reader["CatName"].ToString();
                    category.Description = reader["CatDescription"].ToString();
                    category.ImageUrl = reader["ImageUrl"].ToString();

                    list.Add(category);
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return list;
        }

        #endregion

        #region Cart

        public List<Product> ShowAllProductsInCart(string email)
        {
            List<Product> list = new List<Product>();
            SqlDataReader reader;
            SqlCommand cmd = new SqlCommand("uspSelectItemsFromCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product();
                    product.ID = int.Parse(reader["ProductID"].ToString());
                    product.Name = reader["ProdName"].ToString();
                    product.Price = decimal.Parse(reader["Price"].ToString());
                    product.Discount = decimal.Parse(reader["Discount"].ToString());
                    product.CatID = int.Parse(reader["CategoryID"].ToString());
                    product.SupID = int.Parse(reader["SupplierID"].ToString());
                    product.ImageURL = reader["ImageURL"].ToString();
                    product.SupplierName = reader["SupName"].ToString();
                    product.Quantity = int.Parse(reader["Quantity"].ToString());
                    //product.Description = reader[5].ToString();                   
                    list.Add(product);

                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return list;
        }

        public int InsertProductIntoCart(string email, int productid, int quantity)
        {

            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email); //going to need to pass actual cart id
            cmd.Parameters.AddWithValue("@productID", productid);
            cmd.Parameters.AddWithValue("@quantity", quantity);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public int RemoveProductFromCart(string email, int productid)
        {

            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspDeleteCartItem", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email); //need to pass actual cart id
            cmd.Parameters.AddWithValue("@productID", productid);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public int RemoveAllProductsFromCart(string email)
        {

            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspDeleteAllCartItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email); //need to pass actual cart id

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public int AddShippingToCart(int shippingid, string email)
        {
            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateShippingOfCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@shippingID", shippingid);
            cmd.Parameters.AddWithValue("@email", email);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            return count;
        }

        public decimal CheckShippingCharges(int shippingid)
        {

            Decimal charges = 0;
            SqlCommand cmd;
            SqlDataReader reader;
            //Calling connection method to establish connection string
            cmd = new SqlCommand("uspCheckShippingCharges", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@shippingID", shippingid);
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    charges = decimal.Parse(reader["ShipCharges"].ToString());
                }
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return charges;
        }

        public int CreateNewCart(int cartid)
        {
            //Needtocreatethismethodyo
            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspInsertItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cartid", cartid); //going to need to pass actual cart id

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        } //not done yet

        public int ChangeProductQuantity(string email, int productid, int quantity)
        {
            //count shows the number of affected rows
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspIncreaseQuantityofItem", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@productID", productid);
            cmd.Parameters.AddWithValue("@quantity", quantity);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public int UpdateCartCost(string email, decimal price)
        {
            int count = 0;
            SqlCommand cmd;
            cmd = new SqlCommand("uspUpdateCartCost", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@price", price);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        #endregion

        #region Transaction
        public int AddTransaction(string transactionId, string email, DateTime date, decimal totalprice)
        {
            int count = 0;
            SqlCommand cmd;

            cmd = new SqlCommand("uspInsertUserTransaction", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@transactionid", transactionId);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@trancost", totalprice);
            cmd.Parameters.AddWithValue("@transtatus", 0); //default false bit value, will be changed to true once payment is received.            
            cmd.Parameters.AddWithValue("@trandate", date);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;

        }

        public int UpdateCartAfterSuccessfulTransaction(string email, DateTime date)
        {
            int count = 0;
            SqlCommand cmd;

            cmd = new SqlCommand("uspUpdateCartOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@orderdate", date);

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;

        }

        public int CreateCartAfterSuccessfulTransaction(string email)
        {
            int count = 0;
            SqlCommand cmd;

            cmd = new SqlCommand("uspCreateNewCart", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@totalcost", 0.00); //reverting back to zero
            cmd.Parameters.AddWithValue("@orderstatusid", 1); //1 = not placed, this will change to two when the order is placed
            cmd.Parameters.AddWithValue("@deliverystatusid", 2); //2 = false for isDelivered, this will change to 1 when it is delivered
            cmd.Parameters.AddWithValue("@shippingid", 4); //this is a placeholder for not shipping method being selected, this will change at checkout


            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                message = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return count;

        }

        #endregion


    }
}